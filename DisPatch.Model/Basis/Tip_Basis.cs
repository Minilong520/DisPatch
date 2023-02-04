using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Basis
{
    public class ExceptionTip_Basis : Exception
    {
        public int code;
        public new string Message;
        public Exception original_exception;

        public ExceptionTip_Basis(string msg)
        {
            Message = msg;
        }
    }
}
