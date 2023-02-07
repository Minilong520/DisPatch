using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.GanttWF
{
    public class REQ_WFGanttSet : REQ_Basis
    {
        /// <summary>
        /// 待保存的甘特信息
        /// </summary>
        public List<DTO_WFGanttInfo> ganttInfo { get; set; }
    }
}
