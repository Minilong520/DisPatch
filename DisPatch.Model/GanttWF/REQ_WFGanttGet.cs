using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.GanttWF
{
    public class REQ_WFGanttGet : REQ_Basis
    {
        /// <summary>
        /// 工作中心
        /// </summary>
        public string workCenter { get; set; }

        /// <summary>
        /// 计划批号
        /// </summary>
        public string lotNo { get; set; }

        /// <summary>
        /// 预计开工日（yyyy-MM-dd）
        /// </summary>
        public string dispStartTime { get; set; }

        /// <summary>
        /// 预计完工日（yyyy-MM-dd）
        /// </summary>
        public string dispEndTime { get; set; }
    }
}
