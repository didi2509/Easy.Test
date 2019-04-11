using Easy.Test.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using Easy.Test.Selenium.Interfaces;

namespace Easy.Test.SeleniumTest
{
    /// <summary>
    /// Implementa a interface ICustomWebElement
    /// </summary>
    public class CustomWebElement : ICustomWebElement
    {
        #region propriedades

        /// <summary>
        /// IWebElement manipulado internamente para suprir as necessidades da interface
        /// </summary>
        protected IWebElement webelement { get; set; }

        #endregion

        #region construtores

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webelement"></param>
        public CustomWebElement(IWebElement webelement)
        {
            this.webelement = webelement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoadElement">Função responsável por carregar o IWebElement</param>
        /// <param name="isLoadelement">Indica se o elemento interno deverá ser carregado com a função fornecida em LoadElement</param>
        public CustomWebElement(Func<IWebElement> LoadElement, bool isLoadelement = true)
        {
            this.Reload = LoadElement;

            if (isLoadelement)
                this.webelement = LoadElement();
        }

        #endregion

        #region Interface IWebElement

        public bool Displayed => this.webelement.Displayed;

        public bool Enabled => this.webelement.Enabled;

        public Point Location => this.webelement.Location;

        public Func<IWebElement> _Reload
        {
            get;
            set;
        }

        public bool Selected => this.webelement.Selected;

        public Size Size => this.webelement.Size;

        public string TagName => this.webelement.TagName;

        public string Text => this.webelement.Text;

        public string GetProperty(string propertyName) => this.webelement.GetProperty(propertyName);

        public void Clear() => this.webelement.Clear();

        public void Click() => this.webelement.Click();

        public IWebElement FindElement(By by) => this.webelement.FindElement(by);

        public ReadOnlyCollection<IWebElement> FindElements(By by) => this.webelement.FindElements(by);

        public string GetAttribute(string attributeName) => this.webelement.GetAttribute(attributeName);

        public string GetCssValue(string propertyName) => this.webelement.GetCssValue(propertyName);

        public void SendKeys(string text) => this.webelement.SendKeys(text);

        public void Submit() => this.webelement.Submit();

        #endregion

        #region Conversores

        /// <summary>
        /// Converte o objeto para o tipo SelectElement
        /// </summary>
        /// <returns></returns>
        public SelectElement ConvertToSelectElement()
        {
            return new SelectElement(this.webelement);
        }

        #endregion

        #region ICustomWebElement

        /// <summary>
        /// Método resposável por carregar elemento
        /// </summary>
        public Func<IWebElement> Reload
        {
            get
            {
                this.webelement = _Reload();
                return () => this.webelement;
            }

            set { _Reload = value; }
        }

        /// <summary>
        /// Indica se o elemento foi encontrado
        /// </summary>
        public bool IsFound => this.webelement != null;

        /// <summary>
        /// Retorna  o Value do elemento (se o mesmo possuir)
        /// </summary>
        public string Value => this.webelement.GetAttribute("value");

        /// <summary>
        /// Verifica se o texto do elemento contem o texto fornecido (desconsiderando o case e os espaços)
        /// </summary>
        /// <param name="textToNormalize"></param>
        /// <returns></returns>
        public bool NormalizedTextContains(string textToNormalize) => this.Normalize(this.Text).Contains(Normalize(textToNormalize));

        /// <summary>
        /// Verifica se o value do elemento contem o texto fornecido (desconsiderando o case e os espaços)
        /// </summary>
        /// <param name="textToNormalize"></param>
        /// <returns></returns>
        public bool NormalizedValueContains(string textToNormalize) => this.Normalize(this.Value).Contains(Normalize(textToNormalize));

        #endregion

        #region Auxiliares

        /// <summary>
        /// Remove os espaços de uma string e deixa os caractéres em caixa alta
        /// </summary>
        /// <param name="strToNormalize"></param>
        /// <returns></returns>
        private string Normalize(string strToNormalize) => String.IsNullOrEmpty(strToNormalize) ? String.Empty : strToNormalize.ToUpper().Replace(" ", "");

        #endregion
    }
}