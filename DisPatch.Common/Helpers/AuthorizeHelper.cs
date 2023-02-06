using DisPatch.Common.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Common.Helpers
{
    public static class AuthorizeHelper
    {
        private static string AuthenticationType = AppsettingHelper.ReadAppSettings("Authentication", "Type") ?? "RGlzUGF0Y2hBdXRoIERC";
        private static LicenseHelper licenseHelper = new LicenseHelper();

        /// <summary>
        /// 检查授权（登录使用）
        /// </summary>
        public static void CheckAuthorize(string userNo)
        {
            if (licenseHelper == null)
                licenseHelper = new LicenseHelper();

            // 限制服务器MAC地址 + 时间限制
            string mac = WindowsUtil.GetMacAddress();
            if (mac != licenseHelper.mac)
            {
                throw new Exception("当前服务器暂未取得授权！");
            }

            if (DateTime.Now > licenseHelper.datetime)
            {
                throw new Exception($"当前服务器授权时间【{licenseHelper.datetime.ToString("yyyy-MM-dd")}】到期！");
            }

            var userCache = CacheUtil.GetCache(userNo);
            if (userCache != null)
            {
                throw new Exception($"当前用户【{userNo}】已登录！");
            }
        }

        /// <summary>
        /// 检查用户授权（授权API使用）
        /// </summary>
        /// <param name="reqInfo"></param>
        public static void CheckUserAuthorize(string token, string[] authVal)
        {
            // 用户必须已登录
            var userCache = CacheUtil.GetCache(authVal[0]);
            if (userCache != null)
            {
                if (userCache != token)
                {
                    throw new Exception($"当前用户【{authVal[0]}】token异常！");
                }
            }
            else
            {
                throw new Exception($"当前用户【{authVal[0]}】未登录！");
            }
        }

        /// <summary>
        /// 保存用户授权（登录使用）
        /// </summary>
        /// <param name="reqInfo"></param>
        public static void SetUserAuthorize(string token, string userNo)
        {
            var userCache = CacheUtil.GetCache(DisPatchAuthOptions.Scheme);
            if (userCache == null)
            {
                CacheUtil.SetCache(DisPatchAuthOptions.Scheme, 1);
            }
            else
            {
                if (licenseHelper == null)
                    licenseHelper = new LicenseHelper();

                if (Convert.ToInt32(userCache) >= licenseHelper.count)
                {
                    throw new Exception($"当前服务器用户数【{licenseHelper.count}】超限！");
                }

                CacheUtil.SetCache(DisPatchAuthOptions.Scheme, Convert.ToInt32(userCache) + 1);
            }

            CacheUtil.SetCache(userNo, token);
        }
    }
}
