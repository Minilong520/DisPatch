using DisPatch.Common.Authentication;
using DisPatch.Implements.GanttWF.Services;
using DisPatch.Model.Basis;
using DisPatch.Model.GanttWF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisPatch.Api.WFControllers
{
    [Authorize(AuthenticationSchemes = DisPatchAuthOptions.Scheme)]
    [ApiController]
    [Route("[controller]")]
    public class WFGanttController : Controller
    {
        private readonly ILogger<WFGanttController> _logger;
        public WFGanttController(ILogger<WFGanttController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取甘特图信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetGantt")]
        public RES_Basis Get(REQ_WFGanttGet reqGanttGet)
        {
            return new SGanttWFGet().Execute(reqGanttGet);
        }

        /// <summary>
        /// 修改甘特图信息
        /// </summary>
        [HttpPost]
        [Route("SetGantt")]
        public RES_Basis Set(REQ_WFGanttSet reqGanttSet)
        {
            return new SGanttWFSet().Execute(reqGanttSet);
        }
    }
}
