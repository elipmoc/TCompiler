using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCompiler
{
    class Result<T,F>
    {
        public delegate void OkFunc(T ok);
        public delegate void ErrFunc(F err);
        T ok;
        F err;
        bool okFlag;

        public static Result<T,F> Err(F err)
        {
            return new Result<T, F>(err);
        }
        public static Result<T, F> Ok(T ok)
        {
            return new Result<T, F>(ok);
        }
        private Result(T ok)
        {
            this.ok = ok;
            okFlag = true;
        }

        private Result(F err)
        {
            this.err = err;
            okFlag = false;
        }

        public void Match(OkFunc okF,ErrFunc errF)
        {
            if (okFlag)
                okF(ok);
            else
                errF(err);
        }
    }

}
