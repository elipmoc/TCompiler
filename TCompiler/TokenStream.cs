using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    class TokenStream
    {
        private readonly List<Token> tokenlist = new List<Token>();
        public int NowIndex { get; private set; }

        //次のトークンへインデックスを進める
        public Result<Token,string> Next(string errorMsg) {
            NowIndex++;
            if (Size <= NowIndex)
                return Result<Token, string>.Err(errorMsg);
            return Result<Token, string>.Ok(tokenlist[NowIndex]);
        }

        //前のトークンへインデックスを戻す
        public void Prev() { NowIndex--; }

        //任意インデックスにロールバック
        public void Rollback(int index)
        {
            NowIndex = index;
        }

        //nowindexがさすトークンを得る
        internal Result<Token,string> Get(string errorMsg) {
            if (Size <= NowIndex)
                return Result<Token, string>.Err(errorMsg);
            return Result<Token, string>.Ok(tokenlist[NowIndex]);
        }

        internal Token this[int index]
        {
            get { return tokenlist[index]; }
        }

        public int Size { get { return tokenlist.Count; } }

        internal TokenStream(List<Token> tokenlist)
        {
            NowIndex = 0;
            this.tokenlist = tokenlist;
        }
    }
}
