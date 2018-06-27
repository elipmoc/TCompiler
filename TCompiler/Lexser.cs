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
        static public TokenStream Lexicalanalysis(string str)
        {
            var tokenlist = new List<Token>();
            var comment = new Regex(@"\/\*[^(\*\/)]*\*\/");
            var space = new Regex(@"^[ \t]+");
            var num = new Regex(@"^\d+");
            var identifier = new Regex(@"^[a-z]+");
            var symbol = new Regex(@"^(if|then|else|\(|\))");
            var op = new Regex(@"^(\+|-|/|\*|=|==)");
            Match match;
            while (str.Length != 0)
            {
                if ((match = space.Match(str)).Success) ;
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
                    return null;
                str = str.Substring(match.Length);
            }

            return new TokenStream(tokenlist);
        }
    }
}
