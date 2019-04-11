using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Test.Selenium.Commom.Log
{
    internal static class EasySeleniumLog
    {
        /// <summary>
        /// Método responsável por gerar o log do teste
        /// </summary>
        /// <param name="driverNavigator"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetLogs(IWebDriver driverNavigator)
        {
            List<KeyValuePair<string, string>> logs = new List<KeyValuePair<string, string>>();

            try
            {
                //O dispose fecha o Browser
                if (driverNavigator != null)
                {
                    IOptions options = driverNavigator.Manage();

                    if (options.Logs != null && options.Logs.AvailableLogTypes != null)
                    {
                        options.Logs.AvailableLogTypes.ToList().ForEach(logType =>
                        {
                            options.Logs.GetLog(logType).ToList().ForEach(log => logs.Add(new KeyValuePair<string, string>(logType, log.Message)));
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logs.Add(new KeyValuePair<string, string>("Problemas ao gerar o log", ex.Message));
            }

            return logs;
        }
    }
}
