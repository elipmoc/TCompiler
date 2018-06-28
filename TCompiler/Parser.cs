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
        VariableTable vt= new VariableTable();
        LabelTable lt = new LabelTable();

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
            return TopLevelBlockParser().Bind(expr => {
                return Result<Action,string>.Ok(Expression.Lambda<Action>(expr).Compile());
            });
        }

        public Result<int, string> Parse(MethodBuilder mbuilder)
        {
            return TopLevelBlockParser().Bind(expr => {
                Expression.Lambda<Action>(expr).CompileToMethod(mbuilder);
                return Result<int, string>.Ok(0);
            });
        }

        Result<Expression, string> TopLevelBlockParser()
        {
            return TopLevelListParser().Bind(exprList => OkParseResult(Expression.Block(vt.GetNowNestParamList(), exprList)));
        }

        Result<List<Expression>, string> TopLevelListParser()
        {
            return TopLevelParser().Bind(expr =>
            {
                var list = new List<Expression>();
                list.Add(expr);
                if (ts.Size <= ts.NowIndex)
                    return Result<List<Expression>, string>.Ok(list);
                return TopLevelListParser().Bind(exprList =>
                {
                    list.AddRange(exprList);
                    return Result<List<Expression>, string>.Ok(list);
                });

            });
        }


        Result<Expression,string> TopLevelParser()
        {
            return AssignOpParser().ErrBind(e=>LabelParser()).ErrBind(e=>GotoParser());
        }

        Result<Expression, string> AssignOpParser()
        {
            var checkPoint = ts.NowIndex;

            return
                ts.Get("error:Assign?")
                .Bind(TokenTypeEqual(TokenType.Id, "error:Assign?"))
                .Match<Result<Expression,string>>(
                    token =>
                        (
                            ts.Next("error:=がないのでは？")
                            .Bind(TokenStrEqual("=", "error:=がないのでは？"))
                            .Bind(_ => ts.Next("error:=の右に式がありません"))
                            .Match<Result<Expression,string>>(
                                _=>(
                                    TopLevelParser()
                                    .Bind(expr => {
                                        vt.Register(token.Str, Expression.Parameter(Type.GetType("System.Int32"),token.Str));
                                        return OkParseResult(Expression.Assign(vt.FindNowNest(token.Str), expr));
                                    })
                                ),
                                _=> 
                                {
                                    ts.Prev();
                                    return EqualOpParser();
                                }

                            )
                            
                         ),
                    _=> EqualOpParser()
                ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> LabelParser()
        {
            var checkPoint = ts.NowIndex;

            return ts.Get("error:label?")
             .Bind(TokenStrEqual("@", "error:label?"))
             .Bind(token => ts.Next("error:label?"))
             .Bind(TokenTypeEqual(TokenType.Id,"@の後に識別子がありません"))
             .Bind(token =>
             {
                 ts.Next("eof");
                 return OkParseResult(Expression.Label(lt.Find(token.Str)));
             }
             ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> GotoParser()
        {
            var checkPoint = ts.NowIndex;

            return ts.Get("error:goto?")
             .Bind(TokenStrEqual("goto", "error:goto?"))
             .Bind(token => ts.Next("error:goto?"))
             .Bind(TokenTypeEqual(TokenType.Id, "gotoの後に識別子がありません"))
             .Bind(token =>
             {
                 ts.Next("eof");
                 return OkParseResult(Expression.Block( Expression.Goto(lt.Find(token.Str)),Expression.Constant(1)));
             }
             ).ErrBind(Rollback<Expression>(checkPoint));
        }


        Result<Expression, string> EqualOpParser()
        {
            return MinusOpParser().Bind(expr => EqualOpParser2(expr));
        }

        Result<Expression, string> EqualOpParser2(Expression expr1)
        {
            var checkPoint = ts.NowIndex;

            return
                (
                    (
                       ts.Get("error:==?")
                        .Bind(TokenStrEqual("==", "error:==?"))
                        .okFlag
                    ) ?
                        ts.Next("==の後に式がありません")
                        .Bind(_ => MinusOpParser())
                        .ErrBind(_ => ErrParseResult("==の後に式がありません"))
                        .Bind(expr2 => {
                            return EqualOpParser2( Expression.Condition(Expression.Equal(expr1, expr2),Expression.Constant(1),Expression.Constant(0)));
                        })
                    :
                        OkParseResult(expr1)
                ).ErrBind(Rollback<Expression>(checkPoint));
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
                       ts.Get("error:-?")
                        .Bind(TokenStrEqual("-", "error:-?"))
                        .okFlag
                    ) ?
                        ts.Next("-の後に式がありません")
                        .Bind(_ => AddOpParser())
                        .ErrBind(_ => ErrParseResult("-の後に式がありません"))
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
                       ts.Get("error:+?")
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
            return
                PrintParser()
                .ErrBind(e=>NumParser())
                .ErrBind(e=>IdParser())
                .ErrBind(e=>IfParser());
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

        Result<Expression, string> IfParser()
        {
            var checkPoint = ts.NowIndex;

            return ts.Get("error:If?")
             .Bind(TokenStrEqual("if", "error:If?"))
             .Bind(token => ts.Next("eof"))
             .Bind(_ => TopLevelParser())
             .Bind(conExpr =>
                ts.Get("ifの後にthenがありません").Bind(TokenStrEqual("then", "ifの後にthenがありません"))
                .Bind(_ => ts.Next("thenの後に式がありません"))
                .Bind(_ => TopLevelParser())
                .Bind(trueExpr =>
                    ts.Get("elseがありません").Bind(TokenStrEqual("else", "elseがありません"))
                    .Bind(_ => ts.Next("elseの後に式がありません"))
                    .Bind(_ => TopLevelParser())
                    .Bind(falseExpr =>
                       OkParseResult(Expression.Condition(Expression.NotEqual(conExpr, Expression.Constant(0)), trueExpr, falseExpr))
                    )
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

        Result<Expression, string> IdParser()
        {
            return ts.Get("error:Id?")
             .Bind(TokenTypeEqual(TokenType.Id, "error:Id?"))
             .Bind(token =>
             {
                 ts.Next("eof");
                 return OkParseResult(vt.FindNowNest(token.Str));
             });
        }
    }
}
