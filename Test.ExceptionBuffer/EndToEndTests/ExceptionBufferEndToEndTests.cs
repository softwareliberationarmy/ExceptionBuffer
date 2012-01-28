using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AUIM.ExceptionHandling;
using System.Threading;
using AUIM.ExceptionHandling;

namespace ExceptionBufferTest
{
    /// <summary>
    /// Summary description for ExceptionBufferEndToEndTests
    /// </summary>
    [TestClass]
    public class ExceptionBufferEndToEndTests
    {
        [TestMethod]
        public void E2E_MultipleExceptionsOfSameTypeThrownTooManyTimes_ProducesOnlyOneException()
        {
            //NOTE:  no asserts because if this test fails it should throw an exception anyway
            ExceptionBuffer buffer = new ExceptionBuffer();
            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.GetType() == typeof(DivideByZeroException)),
                    FrequencyAllowed = new Frequency(9, TimeSpan.FromSeconds(10.0))
                });

            int i = 1;
            try
            {
                for (; i <= 10; i++)
                {
                    Thread.Sleep(100);
                    buffer.HandleException(new DivideByZeroException());
                }
                Assert.Fail("Should have thrown 10th exception");
            }
            catch (DivideByZeroException ex)
            {
                Assert.AreEqual(10, i);
            }
        }

        [TestMethod]
        public void E2E_MultipleExceptionsOfSameTypeThrownNotEnough_ProducesNoException()
        {
            //NOTE:  no asserts because if this test fails it should throw an exception anyway
            ExceptionBuffer buffer = new ExceptionBuffer();
            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.GetType() == typeof(DivideByZeroException)),
                    FrequencyAllowed = new Frequency(9, TimeSpan.FromSeconds(15.0))
                });

            int i = 1;
            for (; i <= 8; i++)
            {
                Thread.Sleep(100);
                buffer.HandleException(new DivideByZeroException());
            }
        }

        [TestMethod]
        public void E2E_MultipleExceptionsOfSameTypeThrownEnoughButTooInfrequently_ProducesNoException()
        {
            //NOTE:  no asserts because if this test fails it should throw an exception anyway
            ExceptionBuffer buffer = new ExceptionBuffer();
            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.GetType() == typeof(DivideByZeroException)),
                    FrequencyAllowed = new Frequency(9, TimeSpan.FromSeconds(0.1))
                });

            int i = 1;
            for (; i <= 10; i++)
            {
                Thread.Sleep(100);
                buffer.HandleException(new DivideByZeroException());
            }
        }

        [TestMethod]
        public void E2E_MultipleExceptionsOfSameType_WhenReachesLimitAndThrowsException_StartsOverAgain()
        {
            ExceptionBuffer buffer = new ExceptionBuffer();
            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.GetType() == typeof(DivideByZeroException)),
                    FrequencyAllowed = new Frequency(9, TimeSpan.FromSeconds(10.0))
                });

            int i = 1;
            for (int j = 0; j < 5; j++)
            {
                try
                {
                    for (; i <= 10; i++)
                    {
                        Thread.Sleep(100);
                        buffer.HandleException(new DivideByZeroException());
                    }
                    Assert.Fail("Should have thrown 10th exception");
                }
                catch (DivideByZeroException ex)
                {
                    Assert.AreEqual(10, i);
                }

                i = 1;
            }
        }

        [TestMethod]
        public void E2E_WhenNoCriteriaMatches_ExceptionGetsThrown()
        {
            ExceptionBuffer buffer = new ExceptionBuffer();
            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.GetType() == typeof(DivideByZeroException)),
                    FrequencyAllowed = new Frequency(9, TimeSpan.FromSeconds(10.0))
                });

            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.GetType() == typeof(NotFiniteNumberException)),
                    FrequencyAllowed = new Frequency(10, TimeSpan.FromSeconds(15.0))
                });

            int i = 1;
            for (int j = 0; j < 5; j++)
            {
                try
                {
                    for (; i <= 10; i++)
                    {
                        Thread.Sleep(100);
                        buffer.HandleException(new DivideByZeroException());
                    }
                    Assert.Fail("Should have thrown 10th exception");
                }
                catch (DivideByZeroException ex)
                {
                    Assert.AreEqual(10, i);
                }

                i = 1;
            }
        }

        [TestMethod]
        public void E2E_WhenMultipleCriteriaMatches_AddedToEachCriteriaCount()
        {
            ExceptionBuffer buffer = new ExceptionBuffer();
            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.Message.Contains("Marco")),
                    FrequencyAllowed = new Frequency(6, TimeSpan.FromSeconds(15.0))
                });

            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.Message.Contains("Polo")),
                    FrequencyAllowed = new Frequency(7, TimeSpan.FromSeconds(15.0))
                });

            List<string> messagesThrown = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                try
                {
                    buffer.HandleException(new ApplicationException(string.Format("Marco Polo {0}", i)));
                }
                catch (ApplicationException appEx)
                {
                    messagesThrown.Add(appEx.Message);
                }
            }

            Assert.AreEqual(2, messagesThrown.Count);
            Assert.IsTrue(messagesThrown.Contains("Marco Polo 6"));
            Assert.IsTrue(messagesThrown.Contains("Marco Polo 7"));
        }

        [TestMethod]
        public void E2E_CriteriaWithTimespanZero_ThrowsNthExceptionRegardlessOfTime()
        {
            ExceptionBuffer buffer = new ExceptionBuffer();
            buffer.Add(
                new ExceptionBufferingCriteria()
                {
                    ExceptionCriteria = (ex => ex.GetType() == typeof(MulticastNotSupportedException)),
                    FrequencyAllowed = new Frequency(10, TimeSpan.Zero)
                });

            List<string> messagesThrown = new List<string>();

            int thrownCount = 0;

            for (int i = 1; i <= 55; i++)
            {
                Thread.Sleep(new Random().Next(100));
                try
                {
                    buffer.HandleException(new MulticastNotSupportedException());
                }
                catch (MulticastNotSupportedException)
                {
                    Assert.AreEqual(0, i % 11);
                    thrownCount++;
                }
            }

            Assert.AreEqual(5, thrownCount);
        }
    }
}
