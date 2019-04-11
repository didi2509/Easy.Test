using OpenQA.Selenium;
using Easy.Test.Selenium.Enums;

namespace Easy.Test.SeleniumTest.Interfaces
{
    /// <summary>
    /// Interface para manipular a criação do driver de teste
    /// </summary>
    public interface ISeleniumDriverCreator
    {
        /// <summary>
        /// Retorna o Driver de acordo com o navegador
        /// </summary>
        /// <param name="typeTestBrowser"></param>
        /// <returns></returns>
        IWebDriver CreateDriver(TypeTestBrowser typeTestBrowser);
      
    }
}
