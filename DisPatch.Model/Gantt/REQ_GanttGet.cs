using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Gantt
{
    public class REQ_GanttGet : REQ_Basis
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string equipmentNo { get; set; }

        /// <summary>
        /// 生产批号
        /// </summary>
        public string lotNo { get; set; }

        /// <summary>
        /// 开始时间（yyyy-MM-dd）
        /// </summary>
        public string dispStartTime { get; set; }

        /// <summary>
        /// 结束时间（yyyy-MM-dd）
        /// </summary>
        public string dispEndTime { get; set; }
    }
}
