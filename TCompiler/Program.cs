using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) return;
            switch (args[0]){
                case "-lex":
                    {
                        var result = Lexser.Lexicalanalysis(new StreamReader(args[1]).ReadToEnd());
                        result.Match(
                            (tokenStream) =>
                            {
                                for (var i = 0; i < tokenStream.Size; i++)
                                    tokenStream[i].DebugPrint();
                            }
                       );
                    }
                    break;
                case "-run":
                    {
                        Lexser.Lexicalanalysis(new StreamReader(args[1]).ReadToEnd())
                            .Bind(tokenStream=> {
                                new Parser(tokenStream).Parse().VoidMatch(
                                    (action) => { action(); },
                                    (errmsg) => { Console.WriteLine(errmsg); }
                                    );
                                return Result<int, string>.Ok(0);
                            });
                    }
                    break;
                case "-v":
                case "-V":
                case "--Version":
                case "--version":
                case "-version":
                case "-Version":
                    Console.WriteLine("TCompiler version 0.0.1");
                    break;
                case "-h":
                    Console.WriteLine("コンパイル");
                    Console.WriteLine("tc -o <コンパイル対象のファイル名> <コンパイル後の名前>");
                    Console.WriteLine();
                    Console.WriteLine("バージョン情報");
                    Console.WriteLine("tc -v or -V or --version or --Version -version -Version");
                    break;
                case "-o":
                    Lexser.Lexicalanalysis(new StreamReader(args[1]).ReadToEnd())
                           .Bind(tokenStream => {
                               var appDomain = AppDomain.CurrentDomain;
                               var assemblyBuilder =
                                   appDomain.DefineDynamicAssembly(
                                       new AssemblyName(args[2]),
                                       AssemblyBuilderAccess.RunAndSave
                                   );
                               var moduleBuilder = assemblyBuilder.DefineDynamicModule(args[2]+".exe");
                               var typeBuilder = moduleBuilder.DefineType("Program", TypeAttributes.Class);
                               var methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Static);
                               methodBuilder.SetParameters(typeof(string[]));
                               new Parser(tokenStream).Parse(methodBuilder).VoidMatch(
                                   (_) => {
                                       typeBuilder.CreateType();
                                       assemblyBuilder.SetEntryPoint(methodBuilder);
                                       assemblyBuilder.Save(args[2]+".exe");
                                   },
                                   (errmsg) => { Console.WriteLine(errmsg); }
                                   );
                               return Result<int, string>.Ok(0);
                           });
                    break;
                default:
                    Console.WriteLine("ha?");
                    break;
            }
        }
    }
}
