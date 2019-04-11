using Easy.Test.SeleniumTest;
using Easy.Test.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Test.Selenium.Extensions
{
    public static class WebElementExtensions
    {
        /// <summary>
        /// auxiliar
        /// </summary>
        private static object _new = new object();

        #region [Waitable]

        /// <summary>
        /// Dispara o evento click de um elemento, caso o mesmo ainda não esteja renderizado a função irá agaurdar 30 segundos (até que o mesmo esteja disponível)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isWaitVisibleAndEnabled">Se true, antes de se invocar o click será verificado se o objeto se encontra visível e habilitado</param>
        public static void WaitClick(this ICustomWebElement element, bool isWaitVisibleAndEnabled = false)
        {
            if (isWaitVisibleAndEnabled)
            {
                element.WaitEnabled();
                element.WaitVisible();
            }

            SeleniumUtils.Wait<Object>(() =>
            {
                int count = 0;

                try
                {
                    count++;
                    element.Click();
                    return _new;
                }
                catch
                {
                    if (count == 30) throw;
                    Thread.Sleep(1000);
                }

                return _new;
            }, String.Empty);
        }


        /// <summary>
        /// Preenche o conteúdo texto de um elemento, caso o mesmo ainda não esteja renderizado a função irá agaurdar 30 segundos (até que o mesmo esteja disponível)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text">Texto a ser inserido no elemento</param>
        /// <param name="isWaitVisibleAndEnabled">Se true, antes de se invocar o Sendkeys será verificado se o objeto se encontra visível e habilitado</param>
        public static void WaitSendKeys(this ICustomWebElement element, object text, bool isWaitVisibleAndEnabled = false)
        {
            if (isWaitVisibleAndEnabled)
            {
                element.WaitEnabled();
                element.WaitVisible();
            }

            SeleniumUtils.Wait<Object>(() =>
            {
                int count = 0;

                try
                {
                    count++;
                    element.SendKeys(text.ToString());
                    return _new;
                }
                catch (NullReferenceException nullReferenceException)
                {
                    throw new Exception("O parâmetro text não possui valor", nullReferenceException);
                }
                catch
                {
                    if (count == 30) throw;
                    Thread.Sleep(1000);
                }

                return _new;
            }, String.Empty);
        }


        /// <summary>
        /// Busca um elemento dentro de outro pelo attribute Name, esperando até que o mesmo se encontre pronto (renderizado)
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindByName(this ICustomWebElement customWebElement, string name)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => customWebElement.FindElement(By.Name(name)), name);
            });

            return customWebelement;
        }



        /// <summary>
        /// Busca um elemento dentro de outro pelo seletor, esperando até que o mesmo se encontre pronto (renderizado) 
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindBySelector(this ICustomWebElement customWebElement, string selector)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => customWebElement.FindElement(By.CssSelector(selector)), selector);
            });

            return customWebelement;
        }



        /// <summary>
        /// Busca um elemento dentro de outro pelo attribute ID, esperando até que o mesmo se encontre pronto (renderizado)
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindById(this ICustomWebElement customWebElement, string id)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => customWebElement.FindElement(By.Id(id)), id);
            });

            return customWebelement;
        }


        /// <summary>
        /// Busca um elemento dentro de outro pelo attribute ID, esperando até que o mesmo se encontre pronto (renderizado)
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindByXPath(this ICustomWebElement customWebElement, string xpath)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => customWebElement.FindElement(By.XPath(xpath)), xpath);
            });

            return customWebelement;
        }

        /// <summary>
        /// Aguarda até que um elemento esteja VISÍVEL  (timeout com 30 segundos)
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="isInvertInvisible">Inverte a lógica, fazendo que o sistema aguarde até que o mesmo esteja INVISÍVEL</param>
        /// <returns></returns>
        public static ICustomWebElement WaitVisible(this ICustomWebElement customWebElement, bool isInvertInvisible = false, int totalAttempts = 30)
        {
            return WaitCondition(customWebElement, () => isInvertInvisible ? customWebElement.Displayed : !customWebElement.Displayed, "Visible", totalAttempts);
        }




        /// <summary>
        /// Aguarda até que um elemento esteja HABILITADO  (timeout com 30 segundos)
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="isInvertEnabled">Inverte a lógica, fazendo que o sistema aguarde até que o mesmo esteja DESABILITADO</param>
        /// <returns></returns>
        public static ICustomWebElement WaitEnabled(this ICustomWebElement customWebElement, bool isInvertEnabled = false, int totalAttempts = 30)
        {
            return WaitCondition(customWebElement, () => isInvertEnabled ? customWebElement.Enabled : !customWebElement.Enabled, "Enabled", totalAttempts);
        }

        /// <summary>
        /// Aguarda até que um elemento esteja SELECIONADO  (timeout com 30 segundos)
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="isInvertEnabled">Inverte a lógica, fazendo que o sistema aguarde até que o mesmo NÃO esteja SELECIONADO</param>
        /// <returns></returns>
        public static ICustomWebElement WaitSelected(this ICustomWebElement customWebElement, bool isInvertSelected = false, int totalAttempts = 30)
        {
            return WaitCondition(customWebElement, () => isInvertSelected ? customWebElement.Selected : !customWebElement.Selected, "Selected", totalAttempts);
        }

        #endregion [Waitables]

        #region [Utils]

        /// <summary>
        /// Recupera o valor de um atributo até que o mesmo exista e seja igual aos valores fornecidos em values (timeout com 30 segundos)
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="strAttribute"></param>
        /// <param name="waitTime"></param>
        /// <param name="totalAttempts"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static ICustomWebElement WaitAttributeValue(this ICustomWebElement customWebElement, string strAttribute, int waitTime = 330, int totalAttempts = 30, params string[] values)
        {
            if (customWebElement == null)
                throw new Exception("O elemento não pode ser nulo");

            //Coloca todos os valores maiúsculos
            for (int i = 0; i < values.Length; i++)
                values[i] = values[i].ToUpper();

            int cont = 0;

            string attributeValue = String.Empty;

            bool isWrongValue = true;

            while (isWrongValue)
            {
                customWebElement.Reload();

                //Verifica se o elemento está nulo

                //Se o elemento não estiver nulo verifica se a propriedade a ser consultada possui o valor fornecido como parâmetro (value)
                attributeValue = customWebElement.GetAttribute(strAttribute);
                isWrongValue = String.IsNullOrEmpty(attributeValue) || !values.Contains(attributeValue.ToUpper());

                if (isWrongValue)
                {
                    Thread.Sleep(waitTime);
                    cont++;

                    if (cont > totalAttempts)
                        throw new Exception($"O valor {attributeValue} não foi atribuído para o atributo {strAttribute} em um período de {waitTime * totalAttempts} milisegundos");

                }
            }



            return customWebElement;
        }

        /// <summary>
        /// Obtém um elemento até que a propriedade fornecida satisfaça a condição de saída do loop
        /// </summary>
        /// <param name="customWebElement"></param>
        /// <param name="isTrue"></param>
        /// <returns></returns>
        public static ICustomWebElement WaitCondition(this ICustomWebElement customWebElement, Func<bool> isTrue, string valorEsperadoError, int totalAttempts = 30)
        {
            if (customWebElement == null)
                throw new Exception("O elemento não pode ser nulo");

            int waitTime = 330,
                cont = 0;

            while (isTrue())
            {
                customWebElement.Reload();

                Thread.Sleep(waitTime);
                cont++;

                if (cont > totalAttempts)
                    throw new Exception($"O valor da propriedade não foi setado para {valorEsperadoError} em um período de {waitTime * totalAttempts} milisegundos");
            }

            return customWebElement;
        }

        #endregion
    }
}