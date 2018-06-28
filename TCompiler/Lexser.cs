using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TCompiler
{
    class Lexser
    {
        //字句解析実行
        static public Result<TokenStream,string> Lexicalanalysis(string str)
        {
            var tokenlist = new List<Token>();
            var comment = new Regex(@"\/\*[^(\*\/)]*\*\/");
            var skip = new Regex(@"^[ \t\n\r]+");
            var num = new Regex(@"^\d+");
            var identifier = new Regex(@"^[a-z]+");
            var symbol = new Regex(@"^(print|goto|@|if|then|else|\(|\))");
            var op = new Regex(@"^(\+|-|/|\*|==|=)");
            Match match;
            while (str.Length != 0)
            {
                if ((match = skip.Match(str)).Success) ;
                else if ((match = comment.Match(str)).Success) ;
                else if ((match = num.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.Num));
                else if ((match = symbol.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.Other));
                else if ((match = identifier.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.Id));
                else if ((match = op.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.Op));
                else
                    return Result<TokenStream,string>.Err("字句解析エラー");
                str = str.Substring(match.Length);
            }

            return Result<TokenStream,string>.Ok(new TokenStream(tokenlist));
        }
    }
}
