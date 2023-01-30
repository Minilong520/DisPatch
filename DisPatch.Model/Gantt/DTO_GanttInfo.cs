using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Gantt
{
    public class DTO_GanttInfo
    {
        /// <summary>
        /// 设备编号（主键 - Y轴）
        /// </summary>
        public string equipmentNo { get; set; }

        /// <summary>
        /// 设备编号（主键 - 记录）
        /// </summary>
        public string equipmentNoOld { get; set; }

        /// <summary>
        /// 生产批号（主键）
        /// </summary>
        public string lotNo { get; set; }

        /// <summary>
        /// 作业站（主键）
        /// </summary>
        public string opNo { get; set; }

        /// <summary>
        /// 工作日期（主键）
        /// </summary>
        public DateTime workDate { get; set; }

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
