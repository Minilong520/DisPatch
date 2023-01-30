using DisPatch.DB.Factories;
using DisPatch.Model.Enum;
using DisPatch.Model.Gantt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.Gantt.Utills
{
    public static class UGanttGet
    {
        public static RES_GanttGet GetGanttInfo(REQ_GanttGet ganttInfo)
        {
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Clear();
            sbSQL.AppendFormat(@"SELECT EquipmentNo,EquipmentNo as EquipmentNoOld,LotNo,OPNo,WorkDate,
                                        LotNo+'('+OPNo+')' AS job,DispStartTime as start,DispEndTime as start
                                 FROM TBLWIPDISPATCHSTATE
                                 WHERE WORKDATE > GETDATE() - 5 ");

            if (!string.IsNullOrEmpty(ganttInfo.equipmentNo))
                sbSQL.AppendFormat(" AND EQUIPMENTNO LIKE '%{0}%' ", ganttInfo.equipmentNo);

            if (!string.IsNullOrEmpty(ganttInfo.lotNo))
                sbSQL.AppendFormat(" AND LOTNO LIKE '%{0}%' ", ganttInfo.lotNo);

            //if (!string.IsNullOrEmpty(ganttInfo.OPNo))
            //    sbSQL.AppendFormat(" AND OPNo LIKE '%{0}%' ", ganttInfo.OPNo);

            if (!string.IsNullOrEmpty(ganttInfo.dispStartTime))
                sbSQL.AppendFormat(" AND DispStartTime >= '{0}' ", ganttInfo.dispStartTime);

            if (!string.IsNullOrEmpty(ganttInfo.dispEndTime))
                sbSQL.AppendFormat(" AND DispEndTime <= '{0}' ", ganttInfo.dispEndTime);

            sbSQL.AppendFormat(" order by EquipmentNo ");

            var repo = DBFactory.GetRepository((DBType)Enum.Parse(typeof(DBType), System.Configuration.ConfigurationManager.AppSettings["GanttConnectType"].ToString()));

            List<DTO_GanttInfo> result = repo.QueryRecords<DTO_GanttInfo>(sbSQL.ToString()).ToList();
            List<string> strEqp = new List<string>();

            if (result != null && result.Count > 0)
            {
                string equipmentNo = "";
                int sid = 0;
                int y = -1;

                foreach (DTO_GanttInfo item in result)
                {
                    sid += 1;

                    // 不同设备，表示不同行
                    if (item.equipmentNo != equipmentNo)
                        y += 1;

                    if (!strEqp.Contains(item.equipmentNo))
                        strEqp.Add(item.equipmentNo);

                    item.y = y;
                    item.uid = sid;
                    equipmentNo = item.equipmentNo;

                    item.start = Convert.ToDateTime(item.start).ToString("yyyy-MM-dd") + "T08:00:00";
                    item.end = item.end == null ?
                        Convert.ToDateTime(item.start).AddDays(1).ToString("yyyy-MM-dd") + "T08:00:00" :
                        Convert.ToDateTime(item.end).ToString("yyyy-MM-dd") + "T08:00:00";
                }
            }

            result = result.OrderBy(_ => _.uid).ToList();
            return new RES_GanttGet() { ganttData = result, equipmentNoList = strEqp };
        }
    }
}
