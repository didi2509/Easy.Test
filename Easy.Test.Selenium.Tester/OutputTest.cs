using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Easy.Test.Selenium.Base;
using Easy.Test.Selenium.Enums;
using OpenQA.Selenium;
using Easy.Test.Selenium.Extensions;

namespace Easy.Test.Selenium.Tester
{
 
    [TestClass]
    public class OutputTest : SeleniumUnitTestBase<string, double>
    {
        /// <summary>
        /// URL de onde será iniciado
        /// </summary>
        private const string TestURL = "url";


        #region TestMethods
        [TestMethod]
        public void OutputTestChromeTest()
        {
            base.ExecuteTest(TypeTestBrowser.Chrome, true);
        }
        #endregion

        #region Test Implementation

        public override void DoTest()
        {
            var input = this.GetTestInput();
            this.SetTestOupput(999);
        }

        #endregion

        #region Actions

        /// <summary>
        /// Executa as ações do teste
        /// </summary>
        /// <param name="driverNavigator"></param>
        private void ExecuteTestSteps(IWebDriver driverNavigator)
        {


        }

        #endregion

        #region Asserts

        /// <summary>
        /// Asserts para o teste da inclusão
        /// </summary>
        private void ExecuteTestAsserts(IWebDriver webDriver)
        {
        }

        #endregion
    }
}
