using System;
using System.Collections.Generic;
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
