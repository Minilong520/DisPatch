using DisPatch.Implements.Interfaces;
using DisPatch.Implements.User.Utils;
using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.User.Services
{
    public class SLogin : AbstractApi
    {
        public override RES_Basis ExecuteImplement(dynamic reqInfo, RES_Basis resContent)
        {
            resContent.content = ULogin.Login(reqInfo);
            return resContent;
        }
    }
}
