using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Basis
{
    public class REQ_Basis
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string userNo { get; set; }

        /// <summary>
        /// 口令
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool isLogin { get; set; } = false;
    }
}
