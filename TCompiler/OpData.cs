using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    using OpFunc = Func<Expression, Expression, Expression>;

    class OpDataList
    {
        public string[] OpList
        {
            get
            {
               return opDataList.Select(x => x.Op).ToArray();
            }
        }

        private List<OpData> opDataList;

        public Expression OpMake(string op,Expression expr1,Expression expr2)
        {
            return opDataList.Find(x => x.Op == op).OpMake(expr1, expr2);
        }

        public OpDataList(params OpData[] opDataList)
        {
            this.opDataList = opDataList.ToList();
        }

    }

    class OpData
    {
        public readonly string Op; 
        public readonly OpFunc OpMake;
        public OpData(string op,OpFunc opFunc)
        {
            this.Op = op;
            this.OpMake= opFunc;
        }
    }
}
