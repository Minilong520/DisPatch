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
    public class ULoginOut
    {
        public static RES_Login LoginOut(REQ_Login userLogin)
        {
            CacheUtil.SetCache(userLogin.userNo, null);

            var userCountCache = CacheUtil.GetCache(DisPatchAuthOptions.Scheme);
            if (userCountCache != null)
            {
                CacheUtil.SetCache(DisPatchAuthOptions.Scheme, Convert.ToInt32(userCountCache) - 1);
            }

            return new RES_Login();
        }
    }
}
