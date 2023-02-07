using DisPatch.Common.Authentication;
using DisPatch.Common.Helpers;
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
            // 1.登录检查授权
            AuthorizeHelper.CheckAuthorize(userLogin.userNo, userLogin.autoLogin);

            // 2.验证账密
            if (userLogin.userNo.StartsWith(DisPatchAuthOptions.UserNo))
            {
                if (userLogin.userNo == userLogin.password)
                {
                    string inSecretCode = $"{userLogin.userNo}:{userLogin.password}";
                    var bytes = Encoding.GetEncoding("utf-8").GetBytes(inSecretCode);
                    string token = $"{DisPatchAuthOptions.Scheme} {Convert.ToBase64String(bytes)}";

                    // 3.保存登录信息
                    if (!userLogin.autoLogin)
                        AuthorizeHelper.SetUserAuthorize(token, userLogin.userNo);

                    return new RES_Login()
                    {
                        token = token,
                        userInfo = new DTO_UserInfo()
                        {
                            userid = userLogin.userNo,
                            //avatar = "<SmileTwoTone />",
                            name = userLogin.userNo,

                            permissions = Model.Enum.AccessType.Admin
                        }
                    };
                }
                else
                {
                    throw new ExceptionTip_Basis("用户登录密码异常！");
                }
            }
            else
            {
                throw new ExceptionTip_Basis($"不存在用户【{userLogin.userNo}】");
            }
        }
    }
}
