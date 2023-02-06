using DisPatch.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Common.Helpers
{
    public class LicenseHelper
    {
        /// <summary>
        /// mac地址
        /// </summary>
        public string mac { get; } = "";

        /// <summary>
        /// 授权时间
        /// </summary>
        public DateTime datetime { get; } = DateTime.Now.AddDays(-1);

        /// <summary>
        /// 授权用户数
        /// </summary>
        public int count { get; } = -1;

        public LicenseHelper()
        {
            try
            {
                string licenseFile = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "license.lic");
                if (!File.Exists(licenseFile))
                    throw new Exception("缺少授权文件！");

                string licenseDll = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "DisPatch.License.dll");
                if (!File.Exists(licenseDll))
                    throw new Exception("授权环境异常！");

                var ass = Assembly.LoadFile(licenseDll);
                var type = ass.GetTypes().Where(_ => _.IsPublic && _.IsClass && _.GetInterfaces().Contains(typeof(ILicenseHelper))).FirstOrDefault();
                var exportInstance = Activator.CreateInstance(type) as ILicenseHelper;
                string data = string.Empty;
                exportInstance.GetLicenseInfo(licenseFile, ref data);

                if (!string.IsNullOrEmpty(data))
                {
                    string[] licenseData = data.Split(',');
                    mac = licenseData[0];
                    datetime = Convert.ToDateTime(licenseData[1]);
                    count = Convert.ToInt32(licenseData[2]);
                }
                else
                {
                    throw new Exception("授权文件异常！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"授权异常：【{ex.Message}】");
            }
        }
    }


}
