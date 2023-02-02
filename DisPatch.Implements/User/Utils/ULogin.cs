using DisPatch.Model.Basis;
using DisPatch.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.User.Utils
{
    public class ULogin
    {
        public static RES_Login Login(REQ_Login userLogin)
        {
            if (userLogin.userNo == "DS")
            {
                if (userLogin.password == "DS")
                    return new RES_Login()
                    {
                        token = "DS,DS",
                        userInfo = new DTO_UserInfo()
                        {
                            permissions = Model.Enum.AccessType.Admin
                        }                        
                    };
                else
                    throw new Tip_Basis("用户登录密码异常！");
            }
            else
            {
                throw new Tip_Basis($"不存在用户【{userLogin.userNo}】");
            }
        }
    }
}
