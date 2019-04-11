using Easy.Test.SeleniumTest.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Easy.Test.Selenium.Enums;

namespace Easy.Test.Selenium.Extensions
{
    internal static class SeleniumUtils
    {
        /// <summary>
        /// Tenta buscar um elemento de forma a esperar até que o mesmo esteja disponível
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">Metodo que retorna o elemento</param>
        /// <param name="waitTime">Tempo a ser esperado para se efetuar uma nova tentativa de buscar o elemento, em milisegundos</param>
        /// <param name="totalAttempts">Total máxima de tentativas de buscar o elemento</param>
        /// <returns></returns>
        internal static T Wait<T>(Func<T> method, string elementSearch, int maxMilliSecondsWaitTime = 30000)
        {
            int cont = 0;

            T element = default(T);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (element == null)
            {
                try
                {
                    element = method();
                }
                catch { }

                if (element == null)
                {
                    Thread.Sleep(350);
                    cont++;

                    //ToDo - permitir que a mensagem seja modificada pois nem sempre a mesma se remete a um elemento
                    if (timer.ElapsedMilliseconds > maxMilliSecondsWaitTime)
                        throw new Exception($"Elemento {elementSearch} não encontrado");
                }
            }

            timer.Stop();

            return element;
        }



        /// <summary>
        /// Espera até que uma condição seja satisfeita
        /// </summary>
        /// <param name="method">metodo que irá retornar um boleano</param>
        /// <param name="maxMilliSecondsWaitTime">total máxima de tentativas de satisfazer a condição</param>
        internal static void WaitCondition(Func<bool> method,  int maxMilliSecondsWaitTime = 30000)
        {
            int cont = 0;

            bool isTrue =false;

            Stopwatch timer = new Stopwatch();
            timer.Start();

            StringBuilder sbException = new StringBuilder();

            while (!isTrue)
            {
                try
                {
                    isTrue = method();
                }
                catch(Exception ex)
                {
                    sbException.AppendLine($"{ex.Message}{(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
                }

                if (!isTrue)
                {
                    Thread.Sleep(350);
                    cont++;

                    //ToDo - permitir que a mensagem seja modificada pois nem sempre a mesma se remete a um elemento
                    if (timer.ElapsedMilliseconds > maxMilliSecondsWaitTime)
                        throw new Exception($"A condição não foi satisfeita em um período de {maxMilliSecondsWaitTime} milisegundos {sbException.ToString()}");
                }
            }

            timer.Stop();
        }

        /// <summary>
        /// Retorna o nome do evento a ser disparado de acordo com o TriggerEvent informado
        /// </summary>
        /// <param name="triggerEvent"></param>
        /// <returns></returns>
        internal static string GetEventName(TriggerEvent triggerEvent)
        {
            switch (triggerEvent)
            {
                case TriggerEvent.Click: return "click";
                case TriggerEvent.DoubleClick: return "dblclick";

                case TriggerEvent.MouseEnter: return "mouseenter";
                case TriggerEvent.MouseLeave: return "mouseleave";
                case TriggerEvent.MouseDown: return "mousedown";
                case TriggerEvent.MouseUp: return "mouseup";

                case TriggerEvent.KeyPress: return "keypress";
                case TriggerEvent.KeyDown: return "keydown";
                case TriggerEvent.KeyUp: return "keyup";

                case TriggerEvent.Submit: return "submit";

                case TriggerEvent.Focus: return "focus";
                case TriggerEvent.Blur: return "blur";

                default: return "click";
            }
        }
    }
}
