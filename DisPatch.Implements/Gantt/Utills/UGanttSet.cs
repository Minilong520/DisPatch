using DisPatch.Model.Basis;
using DisPatch.Model.Gantt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.Gantt.Utills
{
    public static class UGanttSet
    {
        public static RES_GanttSet SetGanttInfo(REQ_GanttSet ganttInfo)
        {

            if (ganttInfo.ganttInfo != null && ganttInfo.ganttInfo.Count > 0)
            {
                StringBuilder sbSQL = new StringBuilder();
                List<string> strSqls = new List<string>();
                foreach (DTO_GanttInfo item in ganttInfo.ganttInfo)
                {
                    if (item.isChange)
                    {
                        sbSQL.Clear();
                        sbSQL.AppendFormat(@" UPDATE TBLWIPDISPATCHSTATE
                                              SET EquipmentNo = '{0}',DispStartTime = '{1}',DispEndTime = '{2}'
                                              WHERE EquipmentNo = '{3}' AND LotNo = '{4}' AND OPNo = '{5}' AND WorkDate = '{6}' ",
                                              item.equipmentNo, item.start, item.end,
                                              item.equipmentNoOld, item.lotNo, item.opNo, item.workDate);
                        strSqls.Add(sbSQL.ToString());
                    }
                }

                return new RES_GanttSet() { strSqls = strSqls };
            }
            else
            {
                throw new Tip_Basis("甘特信息为空，无法修改！");
            }
        }
    }
}
