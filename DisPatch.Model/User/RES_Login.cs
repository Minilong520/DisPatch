using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.User
{
    public class RES_Login
    {
        public DTO_UserInfo userInfo { get; set; }

        /// <summary>
        /// 返回的 key-token
        /// </summary>
        public string token { get; set; }

    }
}
