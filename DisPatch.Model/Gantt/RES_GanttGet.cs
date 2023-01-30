using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Gantt
{
    public class RES_GanttGet
    {
        /// <summary>
        /// 查询出的甘特信息
        /// </summary>
        public List<DTO_GanttInfo> ganttData { get; set; }

        /// <summary>
        /// 设备集合
        /// </summary>
        public List<string> equipmentNoList { get; set; }
    }
}
