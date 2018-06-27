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
            var num_int = new Regex(@"^\d+");
            var character = new Regex(@"^(?:').(?:')");
            var string_ = new Regex("^\"[^\"]*\"");
            var symbol = new Regex(@"^((return)|(>>=)|(\.\.)|(\.)|(@)|(new)|(int)|(in)|(let)|(unit)|(void)|(string)|(double)|(bool)|(do)|(->)|(::)|(==)|(<=)|(>=)|(!=)|(\+\+)|[<>\[\]:\?\+\-%\*\/{}\(\)=,])");
            var Identifier = new Regex(@"^([a-z]|[A-Z]|[0-9]|_)+");
            Match match;
            while (str.Length != 0)
            {
                if ((match = space.Match(str)).Success) ;
                else if ((match = comment.Match(str)).Success) ;
                else if ((match = num_int.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.Num));
                else if ((match = character.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.Charcter));
                else if ((match = string_.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.String));
                else if ((match = symbol.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.symbol));
                else if ((match = Identifier.Match(str)).Success)
                    tokenlist.Add(new Token(match.Value, TokenType.Identifier));
                else
                    return null;
                str = str.Substring(match.Length);
            }

            return new TokenStream(tokenlist);
        }
    }
}
