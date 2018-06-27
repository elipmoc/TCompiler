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


        Result<Expression,string> OkParseResult(Expression expr)
        {
            return Result<Expression, string>.Ok(expr);
        }

        Result<Expression, string> ErrParseResult(string str)
        {
            return Result<Expression, string>.Err(str);
        }

        Result<Token,string>.BindFunc<Token> TokenStrEqual(string str,string errMsg)
        {
            return (Token token) =>
                  token.Str == str ?
                      Result<Token, string>.Ok(token) :
                      Result<Token, string>.Err(errMsg);
        }

        Result<Token, string>.BindFunc<Token> TokenTypeEqual(TokenType tt, string errMsg)
        {
            return (Token token) =>
                  token.TokenType == tt ?
                      Result<Token, string>.Ok(token) :
                      Result<Token, string>.Err(errMsg);
        }

        Result<T,string>.ErrBindFunc<string> Rollback<T>(int checkPoint)
        {
            return (errorMsg) =>
            {
                ts.Rollback(checkPoint);
                return Result<T,string>.Err(errorMsg);
            };
        }

        public Result<Action,string> Parse()
        {
            return TopLevelParser().Bind(expr => {
                return Result<Action,string>.Ok(Expression.Lambda<Action>(expr).Compile());
            });

        }

        public Result<Expression,string> TopLevelParser()
        {
            return DivOpParser();
        }

        Result<Expression,string> DivOpParser()
        {
            return TermParser().Bind(expr => DivOpParser2(expr));
        }

        Result<Expression, string> DivOpParser2(Expression expr1)
        {
            var checkPoint = ts.NowIndex;

            return
                (
                    (
                       ts.Get("error:/?")
                        .Bind(TokenStrEqual("/", "error:/?"))
                        .okFlag
                    ) ?
                        ts.Next("/の後に式がありません")
                        .Bind(_ => TermParser())
                        .ErrBind(_ => ErrParseResult("/の後に式がありません"))
                        .Bind(expr2 => {
                            return DivOpParser2(Expression.Divide(expr1, expr2));
                        })
                    :
                        OkParseResult(expr1)
                ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression,string> TermParser()
        {
            return NumParser()
                .ErrBind((e)=>PrintParser());
        }

        Result<Expression,string> PrintParser()
        {
            var checkPoint = ts.NowIndex;
            return ts.Get("error:print?")
            .Bind(TokenStrEqual("print", "error:print?"))
            .Bind(_ =>
                ts.Next("error:printに(がありません")
                .Bind(TokenStrEqual("(", "error:printに(がありません"))
                .Bind(__ => ts.Next("error:printに式がありません"))
                .Bind(__ => TopLevelParser())
                .Bind(expr =>
                    ts.Get("error:printに)がありません")
                    .Bind(TokenStrEqual(")", "error:printに)がありません"))
                    .Bind(___ =>
                    {
                        ts.Next("eof");
                        return OkParseResult(Expression.Call(typeof(buildInFunc).GetMethod("Print"), expr));
                    })
                )
            ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression,string> NumParser()
        {
           return ts.Get("error:Num?")
            .Bind(TokenTypeEqual(TokenType.Num, "error:Num?"))
            .Bind(token =>
            {
                ts.Next("eof");
                return OkParseResult(Expression.Constant(token.GetNum()));
            });
        }
    }
}
