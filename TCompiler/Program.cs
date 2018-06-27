using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                        var tokenStream = Lexser.Lexicalanalysis(new StreamReader(args[1]).ReadToEnd());
                        if (tokenStream != null)
                        {
                            for (var i = 0; i < tokenStream.Size; i++)
                                tokenStream[i].DebugPrint();
                        }
                    }
                    break;
                case "-run":
                    {
                        var tokenStream = Lexser.Lexicalanalysis(new StreamReader(args[1]).ReadToEnd());
                        if (tokenStream != null)
                        {
                            new Parser(tokenStream).TopLevelParser()();
                        }
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
                    Console.WriteLine("tc <コンパイル対象のファイル名> <コンパイル後の名前>");
                    Console.WriteLine();
                    Console.WriteLine("バージョン情報");
                    Console.WriteLine("tc -v or -V or --version or --Version -version -Version");
                    break;
                default:
                    Console.WriteLine("ha?");
                    break;
            }
        }
    }
}
