using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.GanttWF
{
    public class DTO_WFGanttInfo
    {
        /// <summary>
        /// 工作中心（主键 - Y轴）
        /// </summary>
        public string workCenter { get; set; }

        /// <summary>
        /// 工作中心（主键 - 记录）
        /// </summary>
        public string workCenterOld { get; set; }

        /// <summary>
        /// 计划批号（主键）
        /// </summary>
        public string lotNo { get; set; }

        /// <summary>
        /// 执行任务（各甘特标记）
        /// </summary>
        public string job { get; set; }

        /// <summary>
        /// 开始时间（X轴）
        /// </summary>
        public string start { get; set; }

        /// <summary>
        /// 结束时间（X轴）
        /// </summary>
        public string end { get; set; }

        /// <summary>
        /// 是否变化
        /// </summary>
        public bool isChange { get; set; } = false;

        /// <summary>
        /// ID主键
        /// </summary>
        public int uid { get; set; }

        /// <summary>
        /// 在第几行
        /// </summary>
        public int y { get; set; }
    }
}
