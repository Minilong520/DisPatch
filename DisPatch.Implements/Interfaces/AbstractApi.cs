using DisPatch.Common.Helpers;
using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.Interfaces
{
    public abstract class AbstractApi
    {
        protected RES_Basis resContent { get; set; }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        public virtual dynamic Execute(dynamic reqInfo)
        {
            try
            {
                BeforeExecute(reqInfo);
                resContent = ExecuteImplement(reqInfo, resContent);
                AfterExecute(reqInfo);
            }

            catch (ExceptionTip_Basis t)
            {
                resContent.code = Model.Enum.ReqCode.tip;
                resContent.isSuccess = false;
                TipExecute(reqInfo, t);
            }
            catch (Exception e)
            {
                resContent.code = Model.Enum.ReqCode.error;
                resContent.isSuccess = false;
                ExceptionExecute(reqInfo, e);
            }
            return resContent;
        }

        /// <summary>
        /// 调用API前
        /// </summary>
        /// <param name="task"></param>
        /// <param name="quartzInfo"></param>
        /// <param name="taskHistory"></param>
        public virtual void BeforeExecute(dynamic reqInfo)
        {
            // 检查授权
            // AuthorizeHelper.CheckAuthorize(reqInfo);

            // 初始化结果
            resContent = new RES_Basis()
            {
                isSuccess = true,
                code = Model.Enum.ReqCode.success
            };
        }

        /// <summary>
        /// 业务代码（必须重写）
        /// </summary>
        /// <param name="task"></param>
        /// <param name="quartzInfo"></param>
        /// <param name="taskHistory"></param>
        public abstract RES_Basis ExecuteImplement(dynamic reqInfo, RES_Basis resContent);

        /// <summary>
        /// 调用API后
        /// </summary>
        /// <param name="task"></param>
        /// <param name="quartzInfo"></param>
        /// <param name="taskHistory"></param>
        public virtual void AfterExecute(dynamic reqInfo)
        {
            if (resContent.content == null)
                resContent.content = new Object();
        }

        /// <summary>
        /// 调用API异常
        /// </summary>
        /// <param name="reqInfo"></param>
        /// <param name="e"></param>
        public virtual void ExceptionExecute(dynamic reqInfo, Exception e)
        {
            resContent.message = e.Message;
        }

        /// <summary>
        /// 调用API提示
        /// </summary>
        /// <param name="reqInfo"></param>
        /// <param name="e"></param>
        public virtual void TipExecute(dynamic reqInfo, Exception e)
        {
            resContent.message = ((ExceptionTip_Basis)e).Message;
        }
    }
}
