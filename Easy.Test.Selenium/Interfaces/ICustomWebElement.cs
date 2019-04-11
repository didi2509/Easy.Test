using OpenQA.Selenium;
using System;
using Easy.Test.Selenium.Interfaces;

namespace Easy.Test.SeleniumTest.Interfaces
{
    /// <summary>
    /// Interface para Webelements customizados, herda da interface IWebElement
    /// </summary>
    public interface ICustomWebElement : IWebElement, IElementConvertable
    {
        /// <summary>
        /// Função responsável por encontrar o elemento
        /// </summary>
        Func<IWebElement> Reload { get; set; }

        /// <summary>
        /// Indica se o elemento foi encontrado
        /// </summary>
        bool IsFound { get; }

        /// <summary>
        /// Retorna o valor do atribute Value do elemento (caso possua)
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Realiza um contains no text (sem espaço e sem caixa alta)
        /// </summary>
        /// <param name="textToNormalize">Texto a ser comparado, o método remove os espaços e coloca tudo em caixa alta</param>
        /// <returns></returns>
        bool NormalizedTextContains(string textToNormalize);


        /// <summary>
        /// Realiza um contains no value (sem espaço e sem caixa alta)
        /// </summary>
        /// <param name="textToNormalize">Texto a ser comparado, o método remove os espaços e coloca tudo em caixa alta</param>
        /// <returns></returns>
        bool NormalizedValueContains(string textToNormalize);


    }
}
