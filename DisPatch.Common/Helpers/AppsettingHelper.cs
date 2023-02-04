using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Common.Helpers
{
    public class AppsettingHelper
    {
        private static IConfiguration _config;

        public AppsettingHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        /// <summary>
        /// 读取指定节点的字符串
        /// </summary>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public static string ReadAppSettings(params string[] sessions)
        {
            try
            {
                if (sessions.Any())
                {
                    return _config[string.Join(":", sessions)];
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
    }
}
