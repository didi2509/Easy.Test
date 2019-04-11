using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Easy.Test.Selenium.Base;
using Easy.Test.Selenium.Enums;
using OpenQA.Selenium;
using Easy.Test.Selenium.Extensions;

namespace Easy.Test.Selenium.Tester
{

    [TestClass]
    public class TestGroupStarter : SeleniumUnitTestBase<EmptyData, EmptyData>
    {
        /// <summary>
        /// URL de onde será iniciado
        /// </summary>
        private const string TestURL = "http://sgaphomolog.Easy.mg.gov.br/Documento";


        #region TestMethods
        [TestMethod]
        public void TestGroupStarterChromeTest()
        {
            base.ExecuteTest(TypeTestBrowser.Chrome, true);
        }
        #endregion

        #region Test Implementation

        public override void DoTest()
        {
            this.ContinueWithDefaultInput<InputOutputTest,string,string>()
                .ContinueWith<OutputTest, string, double>()
                .ContinueWith<CommomTest,EmptyData,EmptyData>(new EmptyData())
                .ContinueWith<OutputTest,string,double>("input");
         
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
            //webDriver.WaitFindBySelector(".btn[value='Novo Documento']").WaitClick();
        }

        #endregion
    }
}
