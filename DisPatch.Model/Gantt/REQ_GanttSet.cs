using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Gantt
{
    public class REQ_GanttSet : REQ_Basis
    {
        /// <summary>
        /// 待保存的甘特信息
        /// </summary>
        public List<DTO_GanttInfo> ganttInfo { get; set; }
    }
}
