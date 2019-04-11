using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Test.Selenium
{
    public class TestParameter<TInput, Toutput>
    {
        public TestParameter(TInput input, Toutput output)
        {
            this.Input = input;
            this.Output = output;
        }

        public TInput Input;
        public Toutput Output;
    }
}
