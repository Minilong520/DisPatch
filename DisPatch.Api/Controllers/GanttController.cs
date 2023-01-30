using DisPatch.Implements.Gantt.Services;
using DisPatch.Model.Basis;
using DisPatch.Model.Gantt;
using Microsoft.AspNetCore.Mvc;

namespace DisPatch.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GanttController : Controller
    {
        private readonly ILogger<GanttController> _logger;
        public GanttController(ILogger<GanttController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// 获取甘特图信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetGantt")]
        public RES_Basis Get(REQ_GanttGet reqGanttGet)
        {
            return new SGanttGet().Execute(reqGanttGet);
        }

        /// <summary>
        /// 修改甘特图信息
        /// </summary>
        [HttpPost]
        [Route("SetGantt")]
        public RES_Basis Set(REQ_GanttSet reqGanttSet)
        {
            return new SGanttSet().Execute(reqGanttSet);
        }
    }
}
