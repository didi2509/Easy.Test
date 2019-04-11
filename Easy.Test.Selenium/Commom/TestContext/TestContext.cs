using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Test.Selenium.Base;

namespace Easy.Test.Selenium
{
    public class TestContext
    {

        #region [Constructor]

        /// <summary>
        /// Construtor padrão
        /// </summary>
        internal TestContext()
        {

        }

        #endregion

        #region [Properties]

        private Dictionary<string, List<TestParameter<object, object>>> _parametersDictionary { get; set; }

        /// <summary>
        /// Dicionário de parâmetros de entrada e saída utilizados durante o contexto Dictionary<nomeClasse<valorParâmetroIn,valorParametroOut>>
        /// </summary>
        private Dictionary<string, List<TestParameter<object, object>>> parametersDictionary
        {
            get { return _parametersDictionary ?? (_parametersDictionary = new Dictionary<string, List<TestParameter<object, object>>>()); }
            set { this._parametersDictionary = value; }
        }

        #endregion

        #region [Metodos]


        /// <summary>
        /// Retorna o parâmetro do teste de acordo com seu tipo
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="Toutput"></typeparam>
        /// <param name="index">Se o tipo foi chamado durante o teste mais de uma vez</param>
        /// <returns></returns>
        public TestParameter<TInput, Toutput> GetParameter<TypeTest, TInput, Toutput>(int index = 0) where TypeTest : SeleniumUnitTestBase<TInput, Toutput>
        {
            TestParameter<object, object> tuple = parametersDictionary[typeof(TypeTest).Name][index];
            return new TestParameter<TInput, Toutput>((TInput)tuple.Input, (Toutput)tuple.Output);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeTest"></typeparam>
        /// <param name="input"></param>
        /// <param name="output"></param>
        internal void AddParameter<TypeTest>(object input, object output)
        {
            string key = typeof(TypeTest).Name;

            if (this.parametersDictionary.Where(w => String.Compare(w.Key, key, true) == 0).Count() > 0)
                this.parametersDictionary[key].Add(new TestParameter<object, object>(input, output));
            else
                this.parametersDictionary.Add(key, new List<TestParameter<object, object>>() { new TestParameter<object, object>(input, output) });
        }

        #endregion

    }


}
