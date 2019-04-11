using Easy.Test.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Easy.Test.Selenium.Enums;

namespace Easy.Test.Selenium.Base
{
    /// <summary>
    /// Base para a criação de drivers
    /// </summary>
    public abstract class SeleniumDriverCreatorBase : ISeleniumDriverCreator
    {
        /// <summary>
        /// Caminho padrão dos drivers
        /// </summary>
        public virtual string GetCommomDriverPath()
        {
            return @"C:\SeleniumDrivers\";
        }

        /// <summary>
        /// Retorna o caminho completo do driver de acordo com o navegador
        /// </summary>
        /// <param name="typeTestBrowser"></param>
        /// <returns></returns>
        public virtual string GetDriver(TypeTestBrowser typeTestBrowser)
        {
            string CommomPath = GetCommomDriverPath();

            switch (typeTestBrowser)
            {
                case TypeTestBrowser.Edge: return string.Format("{0}{1}", CommomPath, "Edge");
                case TypeTestBrowser.Chrome: return string.Format("{0}{1}", CommomPath, "Chrome");
                case TypeTestBrowser.FireFox: return string.Format("{0}{1}", CommomPath, "FireFox");
                case TypeTestBrowser.Phantom: return string.Format("{0}{1}", CommomPath, "Phantom");
                case TypeTestBrowser.IE: return string.Format("{0}{1}", CommomPath, "IE");
                default: throw new System.Exception("Favor informar o tipo de Driver a ser criado");
            }
        }

        /// <summary>
        /// Retorna o Driver de acordo com o navegador
        /// </summary>
        /// <param name="typeTestBrowser"></param>
        /// <returns></returns>
        public virtual IWebDriver CreateDriver(TypeTestBrowser typeTestBrowser)
        {
            switch (typeTestBrowser)
            {
                case TypeTestBrowser.FireFox: return new FirefoxDriver(FirefoxDriverService.CreateDefaultService(GetDriver(typeTestBrowser), "geckodriver.exe"));
                case TypeTestBrowser.Chrome: return new ChromeDriver(GetDriver(typeTestBrowser));
                case TypeTestBrowser.Edge: return new EdgeDriver(GetDriver(typeTestBrowser));
                case TypeTestBrowser.IE: return new InternetExplorerDriver(GetDriver(typeTestBrowser));
             //   case TypeTestBrowser.Phantom: return new PhantomJSDriver(GetDriver(typeTestBrowser));
                default: return null;
            }
        }
    }
}
