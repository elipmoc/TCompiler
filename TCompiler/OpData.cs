using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    using OpFunc = Func<Expression, Expression, Expression>;

    class OpData
    {
        public string Op { get; private set; }
        public OpFunc OpMake { get; private set; }
        public OpData(string op,OpFunc opMake)
        {
            Op = op;
            OpMake = opMake;
        }
    }
}
