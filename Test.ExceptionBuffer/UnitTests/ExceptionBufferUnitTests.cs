using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AUIM.ExceptionHandling;
using ExceptionBufferTest.HelpersFakesStubsAndMocks;

namespace ExceptionBufferTest.UnitTests
{
    /// <summary>
    /// Summary description for ExceptionBufferUnitTests
    /// </summary>
    [TestClass]
    public class ExceptionBufferUnitTests
    {
        [TestMethod]
        public void U_ExceptionBuffer_AddCriteria_IncrementsCountTo1()
        {
            //arrange
            FakeExceptionBuffer buffer = new FakeExceptionBuffer();

            //act
            buffer.Add(new ExceptionBufferingCriteria()
            {
                ExceptionCriteria = (ex => ex.Message == "Hello"),
                FrequencyAllowed = new Frequency(17, TimeSpan.FromDays(1.0))
            });

            //assert
            Assert.AreEqual(1, buffer.ExceptionCriteriaSets.Count);
        }

        [TestMethod]
        public void U_ExceptionBuffer_HandleException_ThrowsExceptionWhenNoCriteriaExist()
        {
            //arrange
            FakeExceptionBuffer buffer = new FakeExceptionBuffer();
            ArgumentNullException nullEx = new ArgumentNullException("BadParam");

            //act
            try
            {
                buffer.HandleException(nullEx);
                Assert.Fail("Should have thrown ArgumentNullException");
            }
            catch (ArgumentNullException argEx)
            {
                //assert
                Assert.AreEqual(nullEx.ParamName, argEx.ParamName);
            }

        }

    }
}
