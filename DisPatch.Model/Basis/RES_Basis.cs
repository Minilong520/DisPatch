using DisPatch.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Basis
{
    public class RES_Basis
    {
        /// <summary>
        /// 是否调用成功
        /// </summary>
        public bool isSucess { get; set; } = true;

        /// <summary>
        /// 调用代码
        /// </summary>
        public ReqCode code { get; set; } = ReqCode.success;

        /// <summary>
        /// 调用异常信息
        /// </summary>
        public string message { get; set; } = "";

        /// <summary>
        /// 调用结果
        /// </summary>
        public dynamic content { get; set;}
    }
}
