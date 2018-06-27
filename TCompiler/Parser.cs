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

        public Action Parse()
        {
            return Expression.Lambda<Action>(TopLevelParser()).Compile();

        }

        public Expression TopLevelParser()
        {
            return DivOpParser();
        }

        Expression DivOpParser()
        {
            var checkPoint = ts.NowIndex;
            var expr1 = TermParser();
            if (expr1 != null)
            {
                while (ts.Size>ts.NowIndex && ts.Get().Str == "/")
                {
                    ts.Next();
                    var expr2 = TermParser();
                    if (expr2 == null)
                    {
                        ts.Rollback(checkPoint);
                        return null;
                    }
                    expr1 = Expression.Divide(expr1, expr2);
                }
                return expr1;
            }
            ts.Rollback(checkPoint);
            return null;
        }

        Expression TermParser()
        {
            var expr = PrintParser();
            if (expr != null) return expr;
            expr = NumParser();
            if (expr != null) return expr;
            return null;
        }

        Expression PrintParser()
        {
            var checkPoint = ts.NowIndex;
            if(ts.Get().Str == "print")
            {
                ts.Next();
                if (ts.Get().Str == "(")
                {
                    ts.Next();
                    var expr = TopLevelParser();
                    if (ts.Get().Str == ")")
                    {
                        ts.Next();
                        return Expression.Call(typeof(buildInFunc).GetMethod("Print"),expr);
                    }
                }
            }
            ts.Rollback(checkPoint);
            return null;
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
