using DisPatch.Common.Helpers;
using DisPatch.DB.Factories;
using DisPatch.Model.Enum;
using DisPatch.Model.GanttWF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.GanttWF.Utills
{
    public static class UGanttWFGet
    {
        public static RES_WFGanttGet GetGanttInfo(REQ_WFGanttGet ganttInfo)
        {
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.AppendFormat(@"SELECT TA021 AS workCenter,TA021 AS workCenterOld,TA033 AS lotNo,
                                        TA033 AS job,
                                        TA009 AS start,TA010 as 'end'
                                 FROM MOCTA 
                                 WHERE TA021 IS NOT NULL AND TA021 <> '' AND TA010 IS NOT NULL AND TA010 <> ''");

            if (!string.IsNullOrEmpty(ganttInfo.workCenter))
                sbSQL.AppendFormat(" AND TA021 LIKE '%{0}%' ", ganttInfo.workCenter);

            if (!string.IsNullOrEmpty(ganttInfo.lotNo))
                sbSQL.AppendFormat(" AND TA033 LIKE '%{0}%' ", ganttInfo.lotNo);

            if (!string.IsNullOrEmpty(ganttInfo.dispStartTime))
                sbSQL.AppendFormat(" AND TA009 > '{0}' ", Convert.ToDateTime(ganttInfo.dispStartTime).ToString("yyyyMMdd"));

            if (!string.IsNullOrEmpty(ganttInfo.dispEndTime))
                sbSQL.AppendFormat(" AND TA010 < '{0}' ", Convert.ToDateTime(ganttInfo.dispEndTime).ToString("yyyyMMdd"));

            sbSQL.AppendFormat(" order by workCenter ");

            string dbType = AppsettingHelper.ReadAppSettings("DB", "Type") ?? "sqlserver";
            var repo = DBFactory.GetRepository((DBType)Enum.Parse(typeof(DBType), dbType.ToLower()));

            List<DTO_WFGanttInfo> result = repo.QueryRecords<DTO_WFGanttInfo>(sbSQL.ToString()).ToList();
            List<string> strEqp = new List<string>();

            if (result != null && result.Count > 0)
            {
                string workCenter = "";
                int sid = 0;
                int y = -1;

                foreach (DTO_WFGanttInfo item in result)
                {
                    sid += 1;

                    // 不同工作中心，表示不同行
                    if (item.workCenter != workCenter)
                        y += 1;

                    if (!strEqp.Contains(item.workCenter))
                        strEqp.Add(item.workCenter);

                    item.y = y;
                    item.uid = sid;
                    workCenter = item.workCenter;

                    item.start = DateTime.ParseExact(item.start.Trim(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd") + "T08:00:00";
                    item.end = item.end == null ?
                        DateTime.ParseExact(item.start.Trim(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd") + "T20:00:00" :
                        DateTime.ParseExact(item.end.Trim(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd") + "T20:00:00";
                }
            }

            result = result.OrderBy(_ => _.uid).ToList();
            return new RES_WFGanttGet() { ganttData = result, workCenterList = strEqp };
        }
    }
}
