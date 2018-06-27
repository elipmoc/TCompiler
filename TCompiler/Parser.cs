using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    class Parser
    {
        TokenStream ts;
        public Parser(TokenStream ts)
        {
            this.ts = ts;
        }

        public Action TopLevelParser()
        {
            return Expression.Lambda<Action>(PrintParser()).Compile();
        }

        Expression PrintParser()
        {
          return  Expression.Call(typeof(buildInFunc).GetMethod("Print"),NumParser());
        }

        Expression NumParser()
        {
            var token = ts.Get();
            if (token.TokenType== TokenType.Num)
            {
                ts.Next();
                return Expression.Constant(token.GetNum());
            }
            return null;
        }
    }
}
