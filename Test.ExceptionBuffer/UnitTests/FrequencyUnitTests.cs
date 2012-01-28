using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AUIM.ExceptionHandling;

namespace ExceptionBufferTest.UnitTests
{

    [TestClass]
    public class FrequencyUnitTests
    {
        [TestMethod]
        public void U_Frequency_DoesntAllowNegativeNumberOfTimes()
        {
            try
            {
                Frequency freq = new Frequency(-1, TimeSpan.Zero);
                Assert.Fail("Expected negative NumberOfTimes to fail");
            }
            catch (ArgumentOutOfRangeException argEx)
            {
                Assert.AreEqual("NumberOfTimes", argEx.ParamName); 
            }
        }

        [TestMethod]
        public void U_Frequency_DoesntAllowZeroNumberOfTimes()
        {
            try
            {
                Frequency freq = new Frequency(0, TimeSpan.Zero);
                Assert.Fail("Expected negative NumberOfTimes to fail");
            }
            catch (ArgumentOutOfRangeException argEx)
            {
                Assert.AreEqual("NumberOfTimes", argEx.ParamName);
            }
        }






        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

    }
}
