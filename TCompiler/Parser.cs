using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
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

        public Result<int, string> Parse(MethodBuilder mbuilder)
        {
            return TopLevelParser().Bind(expr => {
                Expression.Lambda<Action>(expr).CompileToMethod(mbuilder);
                return Result<int, string>.Ok(0);
            });
        }

        public Result<Expression,string> TopLevelParser()
        {
            return MinusOpParser();
        }

        Result<Expression, string> MinusOpParser()
        {
            return AddOpParser().Bind(expr => MinusOpParser2(expr));
        }

        Result<Expression, string> MinusOpParser2(Expression expr1)
        {
            var checkPoint = ts.NowIndex;

            return
                (
                    (
                       ts.Get("error:+ or -?")
                        .Bind(TokenStrEqual("-", "error:+?"))
                        .okFlag
                    ) ?
                        ts.Next("+の後に式がありません")
                        .Bind(_ => AddOpParser())
                        .ErrBind(_ => ErrParseResult("+の後に式がありません"))
                        .Bind(expr2 => {
                            return MinusOpParser2(Expression.Add(expr1,Expression.Negate(expr2)));
                        })
                    :
                        OkParseResult(expr1)
                ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> AddOpParser()
        {
            return MulOpParser().Bind(expr => AddOpParser2(expr));
        }

        Result<Expression, string> AddOpParser2(Expression expr1)
        {
            var checkPoint = ts.NowIndex;

            return
                (
                    (
                       ts.Get("error:+ or -?")
                        .Bind(TokenStrEqual("+", "error:+?"))
                        .okFlag
                    ) ?
                        ts.Next("+の後に式がありません")
                        .Bind(_ => MulOpParser())
                        .ErrBind(_ => ErrParseResult("+の後に式がありません"))
                        .Bind(expr2 => {
                            return AddOpParser2(Expression.Add(expr1, expr2));
                        })
                    :
                        OkParseResult(expr1)
                ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> MulOpParser()
        {
            return DivOpParser().Bind(expr => MulOpParser2(expr));
        }

        Result<Expression, string> MulOpParser2(Expression expr1)
        {
            var checkPoint = ts.NowIndex;

            return
                (
                    (
                       ts.Get("error:*?")
                        .Bind(TokenStrEqual("*", "error:*?"))
                        .okFlag
                    ) ?
                        ts.Next("*の後に式がありません")
                        .Bind(_ => DivOpParser())
                        .ErrBind(_ => ErrParseResult("*の後に式がありません"))
                        .Bind(expr2 => {
                            return MulOpParser2(Expression.Multiply(expr1, expr2));
                        })
                    :
                        OkParseResult(expr1)
                ).ErrBind(Rollback<Expression>(checkPoint));
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
