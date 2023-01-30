using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Enum
{
    /// <summary>
    /// API调用代码
    /// </summary>
    public enum ReqCode
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        success = 0,
        /// <summary>
        /// 执行异常
        /// </summary>
        error = 1,
        /// <summary>
        /// 卡控提示
        /// </summary>
        tip = 2
    }
}
