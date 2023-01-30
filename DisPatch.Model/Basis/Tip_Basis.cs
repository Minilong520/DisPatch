using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Basis
{
    public class Tip_Basis : Exception
    {
        public int code;
        public new string Message;
        public Exception original_exception;

        public Tip_Basis(string msg)
        {
            Message = msg;
        }
    }
}
