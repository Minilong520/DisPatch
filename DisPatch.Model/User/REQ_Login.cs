using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.User
{
    public class REQ_Login : REQ_Basis
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }        
    }
}
