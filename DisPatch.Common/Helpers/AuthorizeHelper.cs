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
        public static void CheckAuthorize(string userNo, bool autoLogin)
        {
            checkLicense();

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
                //if (!autoLogin)
                //    throw new Exception($"当前用户【{userNo}】已登录！");
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
                if (userCache.ToString() != token)
                {
                    CacheUtil.SetCache(authVal[0], null);

                    var userCountCache = CacheUtil.GetCache(DisPatchAuthOptions.Scheme);
                    if (userCountCache != null)
                    {
                        CacheUtil.SetCache(DisPatchAuthOptions.Scheme, Convert.ToInt32(userCountCache) - 1);
                    }

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
                checkLicense();

                if (Convert.ToInt32(userCache) >= licenseHelper.count)
                {
                    throw new Exception($"当前服务器用户数【{licenseHelper.count}】超限！");
                }

                CacheUtil.SetCache(DisPatchAuthOptions.Scheme, Convert.ToInt32(userCache) + 1);
            }

            CacheUtil.SetCache(userNo, token);
        }

        /// <summary>
        /// 检查授权文件
        /// </summary>
        private static void checkLicense()
        {
            // 实例化
            if (licenseHelper == null)
                licenseHelper = new LicenseHelper();

            string licenseFile = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "license.lic");
            if (!File.Exists(licenseFile))
                throw new Exception("缺少授权文件！");

            licenseHelper.lastWrite = new FileInfo(licenseFile).LastWriteTime;

            var licenseLastWrite = CacheUtil.GetCache("licenseLastWrite");
            if (licenseLastWrite == null)
            {
                CacheUtil.SetCache("licenseLastWrite", licenseHelper.lastWrite);
            }
            else
            {
                if (Convert.ToDateTime(licenseLastWrite) < licenseHelper.lastWrite)
                {
                    // 取最新授权
                    licenseHelper = new LicenseHelper();
                }
            }
        }
    }
}
