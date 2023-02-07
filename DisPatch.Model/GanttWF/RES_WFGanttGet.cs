using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.GanttWF
{
    public class RES_WFGanttGet
    {
        /// <summary>
        /// 查询出的甘特信息
        /// </summary>
        public List<DTO_WFGanttInfo> ganttData { get; set; }

        /// <summary>
        /// 工作中心集合
        /// </summary>
        public List<string> workCenterList { get; set; }
    }
}
