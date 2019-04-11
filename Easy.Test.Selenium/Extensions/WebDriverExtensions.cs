using Easy.Test.SeleniumTest;
using Easy.Test.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Easy.Test.Selenium.Base;
using Easy.Test.Selenium.Enums;

namespace Easy.Test.Selenium.Extensions
{
    public static class WebDriverExtensions
    {
        #region [Javascript]

        /// <summary>
        /// Evnia código javascript para ser executado na página
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="script">script a ser executado</param>
        public static object ExecuteScript(this IWebDriver webDriver, string script)
        {
            return ((IJavaScriptExecutor)webDriver).ExecuteScript(script);
        }

        private static string EasyExecuteScript(this IWebDriver diver, string script, bool isAsync, bool autoConcatReturn = true, params object[] args)
        {
            if (diver == null) throw new NullReferenceException("Favor informar um objeto não nulo para o parâmetro diver");

            object obj =
                isAsync
                ? ((IJavaScriptExecutor)diver).ExecuteAsyncScript(autoConcatReturn ? $"return {script}" : script, args)
                : ((IJavaScriptExecutor)diver).ExecuteScript(autoConcatReturn ? $"return {script}" : script, args);

            return obj != null ? obj.ToString() : String.Empty;
        }

        public static string ExecuteScriptAndReturn(this IWebDriver diver, string script, bool autoConcatReturn = true, params object[] args)
        {
            return EasyExecuteScript(diver, script, false, autoConcatReturn, args);
        }

        public static string ExecuteScriptAndReturnAsync(this IWebDriver diver, string script, bool autoConcatReturn = true, params object[] args)
        {
            return EasyExecuteScript(diver, script, true, autoConcatReturn, args);
        }


        /// <summary>
        /// Executa o comando Trigger do jQuery para disparar um evento via Script, se necessário espera um tempo informado antes de disparar o comando
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector">Seletor do objeto no qual será acionado o evento</param>
        /// <param name="waitTime">Tempo a ser esperado antes de executar a ação</param>
        /// <param name="triggerEvent">Evento do objeto a ser disparado</param>
        public static void JQueryTriggerBySelector(this IWebDriver webDriver, string selector, int maxMilliSecondsWaitTime = 300, TriggerEvent triggerEvent = TriggerEvent.Click)
        {
            if (maxMilliSecondsWaitTime > 0) Thread.Sleep(maxMilliSecondsWaitTime);

            string executeEvent = $"$(\"{selector}\").trigger('{ SeleniumUtils.GetEventName(triggerEvent) }')";
            ((IJavaScriptExecutor)webDriver).ExecuteScript(executeEvent);
        }


        /// <summary>
        /// Dispara o evento fornecido em uma linha da grid, de acordo com seu ID
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="gridID">ID da grid, sem o #</param>
        /// <param name="elementIndex">Indice do elemento, por padrao sera o primeiro</param>
        /// <param name="maxMilliSecondsWaitTime"></param>
        /// <param name="triggerEvent">Evento a ser disparado em cima da linha, por padrao sera o doubleClick</param>
        public static void JQueryGridHelperTriggerBySelector(this IWebDriver webDriver, string gridID, int elementIndex = 1, int maxMilliSecondsWaitTime = 300, TriggerEvent triggerEvent = TriggerEvent.DoubleClick)
        {
            JQueryTriggerBySelector(webDriver, $"#{gridID} tr[data-tabela='{gridID}']:nth-child({elementIndex})", maxMilliSecondsWaitTime, triggerEvent);
        }


        #region [wait]

        /// <summary>
        /// Aguarda até que o elemento esteja visível ou invisível de acordo com seu seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector">Seletor</param>
        /// <param name="isInvertVisible">Se true aguarda té que o elemento esteja INVISÍVEL, por padrão ele espera até que esteja VISÍVEL</param>
        /// <param name="maxMilliSecondsWaitTime">Timeout para a esperar o elemento ficar visível/invisível</param>
        private static IWebDriver WaitVisibleBySelectorType(this IWebDriver webDriver, string selector, bool isXPath = false, bool isInvertVisible = false, int maxMilliSecondsWaitTime = 30000)
        {
            SeleniumUtils.WaitCondition(() =>
            {
                string typeFind = isXPath ? "$x" : "$";
                string strVisible = webDriver.ExecuteScriptAndReturn($"{typeFind}('{selector}').is(':visible');");
                return string.Compare(strVisible, isInvertVisible ? "false" : "true", true) == 0;
            }, maxMilliSecondsWaitTime);

            return webDriver;
        }

        /// <summary>
        /// Espera até o retorno de jsReturn == value
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="jsReturn">script q irá retornar um valor</param>
        /// <param name="value">valor a ser comparado</param>
        /// <param name="maxMilliSecondsWaitTime"></param>
        /// <param name="autoConcatReturn">se true coloca de forma automatica a palavra return antes do script</param>
        public static IWebDriver WaitUntil(this IWebDriver webDriver, string jsReturn, string value, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            SeleniumUtils.WaitCondition(() =>
            {
                string strValue = webDriver.ExecuteScriptAndReturn(jsReturn, autoConcatReturn);
                return string.Compare(strValue, value, true) == 0;
            }, maxMilliSecondsWaitTime);

            return webDriver;
        }

        /// <summary>
        /// Espera até o retorno de jsReturn == value
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="jsReturn">script q irá retornar um valor</param>
        /// <param name="value">valor a ser comparado</param>
        /// <param name="maxMilliSecondsWaitTime"></param>
        /// <param name="autoConcatReturn">se true coloca de forma automatica a palavra return antes do script</param>
        public static IWebDriver WaitUntil(this IWebDriver webDriver, string jsReturn, Func<string,bool> validateFunction, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            SeleniumUtils.WaitCondition(() =>
            {
                string strValue = webDriver.ExecuteScriptAndReturn(jsReturn, autoConcatReturn);
                return validateFunction(strValue);
            }, maxMilliSecondsWaitTime);

            return webDriver;
        }

        /// <summary>
        /// Espera até o retorno de jsReturn == value
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="jsReturn">script q irá retornar um valor</param>
        /// <param name="value">valor a ser comparado</param>
        /// <param name="maxMilliSecondsWaitTime"></param>
        /// <param name="autoConcatReturn">se true coloca de forma automatica a palavra return antes do script</param>
        public static IWebDriver WaitUntil(this IWebDriver webDriver, Func<bool> validateFunction, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            SeleniumUtils.WaitCondition(validateFunction, maxMilliSecondsWaitTime);

            return webDriver;
        }

        #region [enabled]

        public static IWebDriver WaitEnabledById(this IWebDriver webDriver, string id, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitEnabledBySelector($"#{id}", isInvertEnabled, maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitEnabledByName(this IWebDriver webDriver, string name, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitEnabledBySelector($"name=\"{name}\"", isInvertEnabled, maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitEnabledBySelector(this IWebDriver webDriver, string selector, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitEnabledByElement($"'{selector}'", isInvertEnabled , maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitEnabledByElement(this IWebDriver webDriver, string element, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitUntil($"$({element}).is(':disabled');", isInvertEnabled ? "true" : "false", maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitEnabledByxPath(this IWebDriver webDriver, string path, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitUntil($"$x('{path}').is(':disabled');", isInvertEnabled ? "true" : "false", maxMilliSecondsWaitTime, autoConcatReturn);
        }

        #endregion [enabled]

        #region [element]

        public static IWebDriver WaitElementById(this IWebDriver webDriver, string id, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitEnabledBySelector($"#{id}", isInvertEnabled, maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitElementByName(this IWebDriver webDriver, string name, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitElementBySelector($"name=\"{name}\"", isInvertEnabled, maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitElementBySelector(this IWebDriver webDriver, string selector, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitEnabledByElement($"'{selector}'", isInvertEnabled, maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitElementBy(this IWebDriver webDriver, string elementUnderjQuery, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitUntil($"$({elementUnderjQuery}).length > 0;", isInvertEnabled ? "false" : "true", maxMilliSecondsWaitTime, autoConcatReturn);
        }

        public static IWebDriver WaitElementByxPath(this IWebDriver webDriver, string path, bool isInvertEnabled = false, int maxMilliSecondsWaitTime = 30000, bool autoConcatReturn = true)
        {
            return webDriver.WaitUntil($"$x('{path}').length > 0;", isInvertEnabled ? "false" : "true", maxMilliSecondsWaitTime, autoConcatReturn);
        }

        #endregion [element]

        #region [visible]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="element">Elemento - $('#id') ou $('#id')[0]</param>
        /// <param name="isInvertVisible"></param>
        /// <param name="maxMilliSecondsWaitTime"></param>
        public static IWebDriver WaitVisibleByjQueryElement(this IWebDriver webDriver, string element, bool isInvertVisible = false, int maxMilliSecondsWaitTime = 30000)
        {
            SeleniumUtils.WaitCondition(() =>
                     {
                         string strVisible = webDriver.ExecuteScriptAndReturn($"$({element}).is(':visible');");
                         return string.Compare(strVisible, isInvertVisible ? "false" : "true", true) == 0;
                     }, maxMilliSecondsWaitTime);

            return webDriver;
        }

        /// <summary>
        /// Aguarda até que o elemento esteja visível ou invisível de acordo com seu seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector">Seletor</param>
        /// <param name="isInvertVisible">Se true aguarda té que o elemento esteja INVISÍVEL, por padrão ele espera até que esteja VISÍVEL</param>
        /// <param name="maxMilliSecondsWaitTime">Timeout para a esperar o elemento ficar visível/invisível</param>
        public static IWebDriver WaitVisibleBySelector(this IWebDriver webDriver, string selector, bool isInvertVisible = false, int maxMilliSecondsWaitTime = 30000)
        {
            return WaitVisibleBySelectorType(webDriver, selector, false, isInvertVisible, maxMilliSecondsWaitTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="xPah"></param>
        /// <param name="isInvertVisible"></param>
        /// <param name="maxMilliSecondsWaitTime"></param>
        public static IWebDriver WaitVisibleByXPath(this IWebDriver webDriver, string xPath, bool isInvertVisible = false, int maxMilliSecondsWaitTime = 30000)
        {
            return WaitVisibleBySelectorType(webDriver, xPath, true, isInvertVisible, maxMilliSecondsWaitTime);
        }

        /// <summary>
        /// Aguarda até que o elemento esteja visível ou invisível de acordo com seu seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector">Seletor</param>
        /// <param name="isInvertVisible">Se true aguarda té que o elemento esteja INVISÍVEL, por padrão ele espera até que esteja VISÍVEL</param>
        /// <param name="maxMilliSecondsWaitTime">Timeout para a esperar o elemento ficar visível/invisível</param>
        public static IWebDriver WaitVisibleById(this IWebDriver webDriver, string id, bool isInvertVisible = false, int maxMilliSecondsWaitTime = 30000, Action<string> debugMethod = null)
        {
            return webDriver.WaitVisibleBySelector($"#{id}", isInvertVisible, maxMilliSecondsWaitTime);
        }

        /// <summary>
        /// Aguarda até que o elemento esteja visível ou invisível de acordo com seu seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector">Seletor</param>
        /// <param name="isInvertVisible">Se true aguarda té que o elemento esteja INVISÍVEL, por padrão ele espera até que esteja VISÍVEL</param>
        /// <param name="maxMilliSecondsWaitTime">Timeout para a esperar o elemento ficar visível/invisível</param>
        public static IWebDriver WaitVisibleByName(this IWebDriver webDriver, string name, bool isInvertVisible = false, int maxMilliSecondsWaitTime = 30000, Action<string> debugMethod = null)
        {
            return webDriver.WaitVisibleBySelector($"[name=\"{name}\"]", isInvertVisible, maxMilliSecondsWaitTime);
        }

        #endregion [visible]

        #endregion [wait]

        #endregion

        #region [Methods]

        #region [Find]

        /// <summary>
        /// Busca uma coleção de elementos através do attribute name
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElementsByName(this IWebDriver webDriver, string name) => webDriver.FindElements(By.Name(name));


        /// <summary>
        /// Busca uma coleção de elementos através do seletor informado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElementsBySelector(this IWebDriver webDriver, string selector) =>
             webDriver.FindElements(By.CssSelector(selector));


        /// <summary>
        /// Busca um elemento através do attribute name
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementByName(this IWebDriver webDriver, string name) =>
             new CustomWebElement(() => webDriver.FindElement(By.Name(name)));


        /// <summary>
        /// Busca um elementos através de seu id (lembrando que o mesmo deve ser único na página)
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementByID(this IWebDriver webDriver, string id) =>
             new CustomWebElement(() => webDriver.FindElement(By.Id(id)));

        /// <summary>
        /// Busca um elemento através de seu seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementBySelector(this IWebDriver webDriver, string selector) =>
            new CustomWebElement(() => webDriver.FindElement(By.CssSelector(selector)));


        /// <summary>
        /// Busca um elemento através de seu XPath
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementByXPath(this IWebDriver webDriver, string xpath) =>
            new CustomWebElement(() => webDriver.FindElement(By.XPath(xpath)));


        #endregion

        #endregion

        #region [Wait Methods]

        #region [Find]

        /// <summary>
        /// Busca um elemento através de seu attribute name, aguardando até que o mesmo seja renderizado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindByName(this IWebDriver webDriver, string name, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.Name(name)), name, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        /// <summary>
        /// Busca um elemento através de seu attribute name, aguardando até que o mesmo seja renderizado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindByXPath(this IWebDriver webDriver, string xpath, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.XPath(xpath)), xpath, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        /// <summary>
        /// Busca um elemento através de seu seletor, aguardando até que o mesmo seja renderizado com um tiemout de 30 segundos
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindBySelector(this IWebDriver webDriver, string selector, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.CssSelector(selector)), selector, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }


        /// <summary>
        /// Busca uma lista de elementos através de um seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> WaitFindElementsBySelector(this IWebDriver webDriver, string selector, int maxMilliSecondsWaitTime = 30000)
        {
            return SeleniumUtils.Wait(() => webDriver.FindElements(By.CssSelector(selector)), selector, maxMilliSecondsWaitTime);
        }


        /// <summary>
        /// Busca um elemento em uma lista através de seu seletor e índice, aguardando até que o mesmo seja renderizado com um tiemout de 30 segundos
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <param name="indice">Índice do elemento a ser retornado</param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindElementsBySelector(this IWebDriver webDriver, string selector, int indice, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() =>
                {
                    var list = webDriver.FindElements(By.CssSelector(selector));

                    if (list.Count < indice)
                    {
                        return null;
                    }
                    else
                    {
                        return list[indice];
                    }
                }, selector, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        /// <summary>
        /// Encontra um elemento através de seu ID (lembrando que o ID deverá ser único na página) e aguarda até que o mesmo tenha sido renderizado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="id"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindById(this IWebDriver webDriver, string id, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.Id(id)), id, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        #endregion

        #region [Page]

        /// <summary>
        /// Navega até uma URL, aguardando até que a URL do browser tenha sido de fato trocada
        /// </summary>
        /// <param name="web"></param>
        /// <param name="url"></param>
        /// <param name="contains">Indica a URL fonecida deverá fazer apenas parte da URL carregada</param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que a url tenha sido trocada</param>
        /// <returns></returns>
        public static void WaitGoToUrl(this IWebDriver web, string url, bool contains = true, int maxMilliSecondsWaitTime = 30000)
        {
            web.Navigate().GoToUrl(url);
            web.WaitPage(url, contains, maxMilliSecondsWaitTime);
        }

        /// <summary>
        /// Aguarda até ´que a URL do Browser tenha sido trocada pela URL informada
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="url"></param>
        /// <param name="contains">Indica se sua URL fornecida deverá fazer apenas parte da URL carregada</param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static void WaitPage(this IWebDriver webDriver, string url, bool contains = false, int maxMilliSecondsWaitTime = 30000)
        {
            if (String.IsNullOrEmpty(url)) return;

            string webDriverURL = webDriver.Url.ToUpper();
            url = url.ToUpper();

            SeleniumUtils.Wait<object>(() =>
            {
                if ((contains && webDriverURL.Contains(url))
                     || (webDriverURL.ToUpper() == url)
                )
                {
                    return new object();
                }
                else
                {
                    return null;
                }
            }, url, maxMilliSecondsWaitTime);
        }

        #endregion

        #endregion
    }
}