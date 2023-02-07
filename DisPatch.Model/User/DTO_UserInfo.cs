using DisPatch.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.User
{
    public class DTO_UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userid { get; set; }        

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }     

        /// <summary>
        /// 权限类型
        /// Admin
        /// </summary>
        public AccessType permissions { get; set; } = AccessType.User;
    }
}
