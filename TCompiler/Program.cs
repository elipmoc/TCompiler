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
                            .Bind(tokenStream=> 
                                new Parser(tokenStream).Parse().Bind(action=> {
                                    action();
                                    return Result<int, string>.Ok(0);
                                })
                            ).ErrBind(errmsg =>
                            {
                                Console.WriteLine(errmsg);
                                return Result<int, string>.Err(errmsg);
                            });
                    }
                    break;
                case "-v":
                case "-V":
                case "--Version":
                case "--version":
                case "-version":
                case "-Version":
                    Console.WriteLine("TCompiler version 0.0.2");
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
                               return new Parser(tokenStream).Parse(methodBuilder).Bind(
                                   _ =>
                                   {
                                       var toPath = args.Length < 4 ? "" : args[3] + "\\";
                                       typeBuilder.CreateType();
                                       assemblyBuilder.SetEntryPoint(methodBuilder);
                                       if (File.Exists(toPath + args[2] + ".exe")) File.Delete(toPath + args[2] + ".exe");
                                       assemblyBuilder.Save(args[2] + ".exe");
                                       File.Move(args[2] + ".exe", toPath+ args[2] + ".exe");
                                       var tcEXE = Assembly.GetExecutingAssembly().Location;
                                       if (toPath != "")
                                       {
                                           if (File.Exists(toPath + "tc.exe")) File.Delete(toPath + "tc.exe");
                                           File.Copy(tcEXE, toPath + "tc.exe");
                                       }
                                       Console.WriteLine("build:" + toPath);
                                       return Result<int, string>.Ok(0);
                                   });
                           }).ErrBind(errmsg => { Console.WriteLine(errmsg); return Result<int, string>.Err(errmsg); });
                    break;
                default:
                    Console.WriteLine("ha?");
                    break;
            }
        }
    }
}
