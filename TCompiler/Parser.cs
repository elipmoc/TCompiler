﻿using System;
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
        List<OpDataList> opListList = new List<OpDataList>();


        private Expression NumberToBool(Expression expr)
        {
            return Expression.Condition(Expression.Equal(expr, Expression.Constant(0)), Expression.Constant(false), Expression.Constant(true));
        }

        private Expression BoolToNumber(Expression expr)
        {
            return Expression.Condition(expr, Expression.Constant(1), Expression.Constant(0));
        }

        public Parser(TokenStream ts)
        {
            this.ts = ts;
            opListList.Add(
                new OpDataList(
                    new OpData("||", (expr1,expr2)=>BoolToNumber(Expression.OrElse(NumberToBool(expr1), NumberToBool(expr2))))
                ));
            opListList.Add(new OpDataList(new OpData("&&", (expr1,expr2)=>BoolToNumber(Expression.AndAlso(NumberToBool(expr1),NumberToBool(expr2))) )));
            opListList.Add(new OpDataList(new OpData("|", (expr1, expr2) => Expression.And(expr1, expr2))));
            opListList.Add(new OpDataList(new OpData("^", (expr1, expr2) => Expression.ExclusiveOr(expr1, expr2))));
            opListList.Add(new OpDataList(new OpData("|", (expr1, expr2) => Expression.Or(expr1, expr2))));
            opListList.Add(new OpDataList(
                new OpData("==", (expr1,expr2)=>BoolToNumber(Expression.Equal(expr1, expr2)) ),
                new OpData("!=", (expr1, expr2)=>BoolToNumber(Expression.NotEqual(expr1, expr2)) )
            ));
            opListList.Add(new OpDataList(
                new OpData("<", (expr1, expr2) => BoolToNumber(Expression.LessThan(expr1, expr2))),
                new OpData(">", (expr1, expr2) => BoolToNumber(Expression.GreaterThan(expr1, expr2))),
                new OpData("<=", (expr1, expr2) => BoolToNumber(Expression.LessThanOrEqual(expr1, expr2))),
                new OpData(">=", (expr1, expr2) => BoolToNumber(Expression.GreaterThanOrEqual(expr1, expr2)))
            ));
            opListList.Add(new OpDataList(
                new OpData("<<", (expr1, expr2) => Expression.LeftShift(expr1, expr2)),
                new OpData(">>", (expr1, expr2) => Expression.RightShift(expr1, expr2))
            ));
            opListList.Add(new OpDataList(
                new OpData("+", (expr1, expr2) => Expression.Add(expr1, expr2)),
                new OpData("-", (expr1, expr2) => Expression.Add(expr1, Expression.Negate(expr2)))
            ));
            opListList.Add(new OpDataList(
                new OpData("*", (expr1, expr2) => Expression.Multiply(expr1, expr2)),
                new OpData("%", (expr1, expr2) => Expression.Modulo(expr1, expr2))
            ));
            opListList.Add(new OpDataList(new OpData("/", (expr1, expr2) => Expression.Divide(expr1, expr2))));
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

        Result<Token, string>.BindFunc<Token> TokenStrEquals(string[] strList, string errMsg)
        {
            return (Token token) =>
            {
                foreach (var str in strList)
                {
                    if (token.Str == str)
                        return Result<Token, string>.Ok(token);
                }
                return Result<Token, string>.Err(errMsg);

            };
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
            var endExpr1 = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("続行するには何かキーを押してください"));
            var endExpr2 = Expression.Call(typeof(Console).GetMethod("ReadKey",new Type[] { }));
            return TopLevelListParser().Bind(exprList => {
                exprList.Add(endExpr1);
                exprList.Add(endExpr2);
                return OkParseResult(Expression.Block(vt.GetNowNestParamList(), exprList));
                }
            );
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
            return 
                AssignOpParser()
                .ErrBind(e=>LabelParser().ErrBind(ee => ErrParseResult(e + ee)))
                .ErrBind(e=>GotoParser().ErrBind(ee => ErrParseResult(e + ee)));
        }

        Result<Expression, string> AssignOpParser()
        {
            var checkPoint = ts.NowIndex;

            return
                ts.Get("")
                .Bind(TokenTypeEqual(TokenType.Id, ""))
                .Match<Result<Expression,string>>(
                    token =>
                        (
                            ts.Next("")
                            .Bind(TokenStrEqual("=", ""))
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
                                    return OpParser();
                                }

                            )
                            
                         ),
                    _=> OpParser()
                ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> LabelParser()
        {
            var checkPoint = ts.NowIndex;

            return ts.Get("")
             .Bind(TokenStrEqual("@", ""))
             .Bind(token => ts.Next("error:@の後に識別子がありません"))
             .Bind(TokenTypeEqual(TokenType.Id,"error:@の後に識別子がありません"))
             .Bind(token =>
             {
                 ts.Next("");
                 return OkParseResult(Expression.Label(lt.Find(token.Str)));
             }
             ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> GotoParser()
        {
            var checkPoint = ts.NowIndex;

            return ts.Get("")
             .Bind(TokenStrEqual("goto", ""))
             .Bind(token => ts.Next("error:gotoの後に識別子がありません"))
             .Bind(TokenTypeEqual(TokenType.Id, "error:gotoの後に識別子がありません"))
             .Bind(token =>
             {
                 ts.Next("");
                 return OkParseResult(Expression.Block( Expression.Goto(lt.Find(token.Str)),Expression.Constant(1)));
             }
             ).ErrBind(Rollback<Expression>(checkPoint));
        }
        
        Result<Expression,string> OpParser(int index=-1)
        {
            index++;
            if (opListList.Count()>index)
            {
                return OpParser(index).Bind(expr => OpParser2(index,opListList[index], expr));
            }else
            {
                return TermParser();
            }
        }

        Result<Expression, string> OpParser2(int index,OpDataList opDataList,Expression expr1)
        {
            var checkPoint = ts.NowIndex;

            return
                (
                       ts.Get("")
                        .Bind(TokenStrEquals(opDataList.OpList, ""))
                        .Match(
                           (token) => 
                                ts.Next($"error:演算子の後に式がありません")
                                .Bind(_ => OpParser(index))
                                .ErrBind(_ => ErrParseResult($"error:演算子の後に式がありません"))
                                .Bind(expr2 => 
                                    OpParser2(index, opDataList, opDataList.OpMake(token.Str, expr1, expr2))
                                ), 
                           (err)=>OkParseResult(expr1)
                         )
                ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression,string> TermParser()
        {
            return
                PrintParser()
                .ErrBind(e=>ScanParser().ErrBind(ee=>ErrParseResult(e+ee)))
                .ErrBind(e=>NumParser().ErrBind(ee => ErrParseResult(e + ee)))
                .ErrBind(e=>IdParser().ErrBind(ee => ErrParseResult(e + ee)))
                .ErrBind(e=>IfParser().ErrBind(ee => ErrParseResult(e + ee)))
                .ErrBind(e=>ParenParser().ErrBind(ee => ErrParseResult(e + ee)));
        }

        Result<Expression,string> ParenParser()
        {
            return
                ts.Get("")
                .Bind(TokenStrEqual("(", ""))
                .Bind(_ => ts.Next("error:(の後に式がありません"))
                .Bind(_ => TopLevelParser())
                .Bind(expr =>
                {
                    return
                        ts.Get("error: 式の後に)がありません")
                        .Bind(TokenStrEqual(")", "error: (の後に)がありません"))
                        .Bind(_ => { ts.Next(""); return OkParseResult(expr); });
                });
        }

        Result<Expression,string> PrintParser()
        {
            var checkPoint = ts.NowIndex;
            return ts.Get("")
            .Bind(TokenStrEqual("print", ""))
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
                        ts.Next("");
                        return OkParseResult(Expression.Call(typeof(buildInFunc).GetMethod("Print"), expr));
                    })
                )
            ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> ScanParser()
        {
            var checkPoint = ts.NowIndex;
            return ts.Get("")
            .Bind(TokenStrEqual("scan", ""))
            .Bind(_ =>
                ts.Next("error:scanに(がありません")
                .Bind(TokenStrEqual("(", "error:scanに(がありません"))
                .Bind(__ =>
                    ts.Next("error:scanに)がありません")
                    .Bind(TokenStrEqual(")", "error:scanに)がありません"))
                    .Bind(___ =>
                    {
                        ts.Next("");
                        return OkParseResult(Expression.Call(typeof(buildInFunc).GetMethod("Scan")));
                    })
                )
            ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression, string> IfParser()
        {
            var checkPoint = ts.NowIndex;

            return ts.Get("")
             .Bind(TokenStrEqual("if", ""))
             .Bind(token => ts.Next(""))
             .Bind(_ => TopLevelParser())
             .Bind(conExpr =>
                ts.Get("error:ifの後にthenがありません").Bind(TokenStrEqual("then", "error:ifの後にthenがありません"))
                .Bind(_ => ts.Next("error:thenの後に式がありません"))
                .Bind(_ => TopLevelParser())
                .Bind(trueExpr =>
                    ts.Get("error:elseがありません").Bind(TokenStrEqual("else", "error:elseがありません"))
                    .Bind(_ => ts.Next("error:elseの後に式がありません"))
                    .Bind(_ => TopLevelParser())
                    .Bind(falseExpr =>
                       OkParseResult(Expression.Condition(Expression.NotEqual(conExpr, Expression.Constant(0)), trueExpr, falseExpr))
                    )
                )
             ).ErrBind(Rollback<Expression>(checkPoint));
        }

        Result<Expression,string> NumParser()
        {
           return ts.Get("")
            .Bind(TokenTypeEqual(TokenType.Num, ""))
            .Bind(token =>
            {
                ts.Next("");
                return OkParseResult(Expression.Constant(token.GetNum()));
            });
        }

        Result<Expression, string> IdParser()
        {
            return ts.Get("")
             .Bind(TokenTypeEqual(TokenType.Id, ""))
             .Bind(token =>
             {
                 ts.Next("");
                 var v=vt.FindNowNest(token.Str);
                 if (v == null)
                 {
                     vt.Register(token.Str, Expression.Parameter(Type.GetType("System.Int32"), token.Str));
                     return OkParseResult(Expression.Assign(vt.FindNowNest(token.Str), Expression.Constant(0)));
                 }
                 return OkParseResult(v);
             });
        }
    }
}
