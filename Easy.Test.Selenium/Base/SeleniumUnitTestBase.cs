using Easy.Test.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Easy.Test.Selenium.Commom.Log;
using Easy.Test.Selenium.Enums;
using Easy.Test.Selenium.Interfaces;

namespace Easy.Test.Selenium.Base
{

    #region [SeleniumUintTestBase]

    /// <summary>
    /// Classe base para a o teste com o Selenium
    /// </summary>
    public abstract class SeleniumUnitTestBase<TInput, TOutput> : SeleniumInputOutputTestBase<TInput, TOutput>, ITestContext
    {

        #region [Properties]

        /// <summary>
        /// Cria o driver do selenium
        /// </summary>
        private  ISeleniumDriverCreator _seleniumDriverCreator { get; set; }

        /// <summary>
        /// Driver navigator fornecido ao DoTest
        /// </summary>
        public IWebDriver DriverNavigator { get { return this._driverNavigator; } }

        /// <summary>
        /// Driver navigator fornecido ao DoTest
        /// </summary>
        private IWebDriver _driverNavigator { get; set; }

        /// <summary>
        /// Contexto do teste
        /// </summary>
        public TestContext Context { get { return this._context; } }

        /// <summary>
        /// Contexto do teste
        /// </summary>
        private TestContext _context { get; set; }


        #endregion

        #region Construtores

        /// <summary>
        /// Constrói uma nova instância da classe permitindo a inserção do driverCreator e do input
        /// </summary>
        /// <param name="ISeleniumDriverCreator"></param>
        /// <param name="input"></param>
        public SeleniumUnitTestBase(ISeleniumDriverCreator ISeleniumDriverCreator, TInput input)
        {
            this._seleniumDriverCreator = ISeleniumDriverCreator;
            SetTestInput(input);
        }

        /// <summary>
        /// Constrói uma nova instância da classe utilizando o valor padrão para o driverCreator, permitindo que o input seja fornecido
        /// </summary>
        /// <param name="input">Input a ser fornecido para o teste</param>
        public SeleniumUnitTestBase(TInput input) : this(new DefaultSeleniumDriverCreator(), input)
        {

        }

        /// <summary>
        /// Constrói uma nova instância da classe utilizando os valores padrão para o driverCreator e input
        /// </summary>
        public SeleniumUnitTestBase(bool isSetDefaultInput = true)
        {
            this._seleniumDriverCreator = new DefaultSeleniumDriverCreator();
        }


        #endregion;

        #region [Test]

        /// <summary>
        /// Método que irá conter o teste
        /// </summary>
        /// <param name="driverNavigator">Navegador utilizado</param>
        public abstract void DoTest();

        /// <summary>
        /// Método que irá efetuar o Assert dos testes
        /// </summary>
        public virtual void DoAsserts() { }

        /// <summary>
        /// Executado apos os asserts passarem
        /// </summary>
        public virtual void DoTestPass() { }

        /// <summary>
        /// Executa o teste
        /// </summary>
        internal void Execute()
        {
            this.DoTest();
            this.DoAsserts();
            this.DoTestPass();
        }

        /// <summary>
        /// Executa o teste de acordo com o navegador utilizado, basendo-se no que está implementado em DoTest
        /// </summary>
        /// <param name="typeDriver"></param>
        /// <param name="dispose"></param>
        /// <returns>Retorna o log do teste</returns>
        public List<KeyValuePair<string, string>> ExecuteTest(TypeTestBrowser typeDriver, bool dispose = true)
        {
            //Criacao do navegador
            if (this.DriverNavigator == null)
                this._driverNavigator = _seleniumDriverCreator.CreateDriver(typeDriver);

            //Criacao do contexto
            if (this.Context == null)
                this._context = new TestContext();

            //Log padrão do selenium
            List<KeyValuePair<string, string>> logs = null;

            try
            {
                this.Execute();
            }
            catch
            {
                throw;
            }
            finally
            {
                //O dispose fecha o Browser
                if (DriverNavigator != null)
                {
                    logs = this.GetLogs(DriverNavigator);

                    if (dispose)
                        DriverNavigator.Dispose();
                }
            }

            return logs;
        }


        #region [LOG]

        /// <summary>
        /// Método responsável por gerar o log do teste
        /// </summary>
        /// <param name="driverNavigator"></param>
        /// <returns></returns>
        public virtual List<KeyValuePair<string, string>> GetLogs(IWebDriver driverNavigator)
        {
            return EasySeleniumLog.GetLogs(driverNavigator);
        }


        #endregion

        #endregion

        #region [TestContinue]

        /// <summary>
        /// Continua com o teste fornecido em TypeTest, fornecendo para o mesmo o valor padrão de sua entrada
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <typeparam name="InputTest"></typeparam>
        /// <typeparam name="TOutputTest"></typeparam>
        /// <returns></returns>
        public TypeTest ContinueWithDefaultInput<TypeTest, InputTest, TOutputTest>() where TypeTest : SeleniumUnitTestBase<InputTest, TOutputTest>, new()
        {
            return ConfigureContinuousTest<TypeTest, InputTest, TOutputTest>();
        }


        /// <summary>
        /// Continua com o teste fornecido em TypeTest, fornecendo para o mesmo o valor contindo no parâmetro input
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <typeparam name="TInputTest"></typeparam>
        /// <typeparam name="TOutputTest"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public TypeTest ContinueWith<TypeTest, TInputTest, TOutputTest>(TInputTest input) where TypeTest : SeleniumUnitTestBase<TInputTest, TOutputTest>, new()
        {
            return ConfigureContinuousTest<TypeTest, TInputTest, TOutputTest>((s) => input);
        }


        /// <summary>
        /// Continua com o teste fornecido em TypeTest, fornecendo para o valor de entrada a saída do teste no qual ContinueWith foi aplicado 
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <typeparam name="TInputTest"></typeparam>
        /// <typeparam name="TOutputTest"></typeparam>
        /// <returns></returns>
        public TypeTest ContinueWith<TypeTest, TInputTest, TOutputTest>() where TypeTest : SeleniumUnitTestBase<TOutput, TOutputTest>, new()
        {
            return ConfigureContinuousTest<TypeTest, TOutput, TOutputTest>((s) => GetTestOutput());
        }


        /// <summary>
        /// Continua com o teste fornecido em TypeTest, fornecendo para o valor de entrada a saída do teste no qual ContinueWith foi aplicado (quando o teste fornecido tem os mesmos tipos de entrada e saída que o último)
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <returns></returns>

        public TypeTest ContinueWith<TypeTest>() where TypeTest : SeleniumUnitTestBase<TOutput, TOutput>, new()
        {
            return ConfigureContinuousTest<TypeTest, TOutput, TOutput>((s) => GetTestOutput());
        }

        /// <summary>
        /// Continua com o teste fornecido em TypeTest, fornecendo para o valor de entrada o resultado de returnInputTest(p) 
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <typeparam name="TInputTest"></typeparam>
        /// <typeparam name="TOutputTest"></typeparam>
        /// <param name="returnInputTest"></param>
        /// <returns></returns>
        public TypeTest ContinueWith<TypeTest, TInputTest, TOutputTest>(Func<SeleniumUnitTestBase<TInput, TOutput>, TInputTest> returnInputTest) where TypeTest : SeleniumUnitTestBase<TInputTest, TOutputTest>, new()
        {
            return ConfigureContinuousTest<TypeTest, TInputTest, TOutputTest>(returnInputTest);
        }


        /// <summary>
        /// Configura uma continuação para um teste
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <typeparam name="TInputTest"></typeparam>
        /// <typeparam name="TOutputTest"></typeparam>
        /// <param name="returnInputTest"></param>
        /// <returns></returns>
        private TypeTest ConfigureContinuousTest<TypeTest, TInputTest, TOutputTest>(Func<SeleniumUnitTestBase<TInput, TOutput>, TInputTest> returnInputTest = null) where TypeTest : SeleniumUnitTestBase<TInputTest, TOutputTest>, new()
        {
            TypeTest continueTest = new TypeTest();

            //Indica que o teste é continuo
            continueTest.IsContinuous = true;

            continueTest._context = this.Context;
            continueTest._driverNavigator = this.DriverNavigator;

            //Se a entrada (input) para o novo teste não for fornecida o mesmo assumirá o valor contido em GetDefaultTestInput() do novo teste criado
            TInputTest input = returnInputTest == null ? continueTest.GetTestInput() : returnInputTest(this);

            continueTest.SetTestInput(input);

            //Executa o teste
            continueTest.Execute();

            //Log de parâmetros
            if (this.Context != null)
                this.Context.AddParameter<TypeTest>(input, continueTest.GetTestOutput());

            return continueTest;
        }

        #endregion

        #region [Commom]

        /// <summary>
        /// Executa o método Thread.Sleep, fornecendo os millisegundos a serem aguardados
        /// </summary>
        /// <param name="milliseconds"></param>
        public void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        #endregion
    }


    #endregion

}
