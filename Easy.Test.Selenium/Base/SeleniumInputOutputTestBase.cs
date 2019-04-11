using Easy.Test.SeleniumTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Test.Selenium.Base
{
    public abstract class SeleniumInputOutputTestBase<TInput, TOutput> : ITestInputBehavior<TInput>, ITestOutputBehavior<TOutput>
    {

        #region [Properties]

        /// <summary>
        /// Dados de entrada para o teste
        /// </summary>
        private TInput TestInput { get; set; }

        /// <summary>
        /// Retorno do teste
        /// </summary>
        private TOutput TestOutput { get; set; }

        /// <summary>
        /// Indica se o teste é a continuação de outro teste
        /// </summary>
        internal bool IsContinuous { get; set; }

        #endregion

        /// <summary>
        /// Retorna a entrada padrão do teste
        /// </summary>
        /// <returns></returns>
        public virtual TInput GetDefaultTestInput() => default(TInput);

        /// <summary>
        /// Retorna o input do teste
        /// </summary>
        /// <returns></returns>
        public virtual TInput GetTestInput() => this.IsContinuous ? this.TestInput : this.GetDefaultTestInput();

        /// <summary>
        /// Seta a entrada do teste
        /// </summary>
        /// <param name="testInput"></param>
        internal virtual void SetTestInput(TInput testInput) { this.TestInput = testInput; }

        /// <summary>
        /// Retorna a saída do teste
        /// </summary>
        /// <returns></returns>
        public virtual TOutput GetTestOutput() => this.TestOutput;

        /// <summary>
        /// Seta o valor de resultado do teste
        /// </summary>
        /// <param name="output"></param>
        public virtual void SetTestOupput(TOutput output) { this.TestOutput = output; }
    }
}
