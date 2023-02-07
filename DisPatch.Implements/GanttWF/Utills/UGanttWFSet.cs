using DisPatch.Model.Basis;
using DisPatch.Model.GanttWF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.GanttWF.Utills
{
    public static class UGanttWFSet
    {
        public static RES_WFGanttSet SetGanttInfo(REQ_WFGanttSet ganttInfo)
        {

            if (ganttInfo.ganttInfo != null && ganttInfo.ganttInfo.Count > 0)
            {
                StringBuilder sbSQL = new StringBuilder();
                List<string> strSqls = new List<string>();
                foreach (DTO_WFGanttInfo item in ganttInfo.ganttInfo)
                {
                    if (item.isChange)
                    {
                        sbSQL.Clear();
                        sbSQL.AppendFormat(@" UPDATE MOCTA 
                                              SET TA009 = '{0}',TA010 = '{1}',TA021 = '{2}'
                                              WHERE TA033 = '{4}' ",
                                              item.workCenterOld,
                                              Convert.ToDateTime(item.start).ToString("yyyyMMdd"),
                                              Convert.ToDateTime(item.end).ToString("yyyyMMdd"),
                                              item.lotNo);
                        strSqls.Add(sbSQL.ToString());

                        sbSQL.Clear();
                        sbSQL.AppendFormat(@" UPDATE PURTD 
                                              SET TD012 = '{0}'
                                              WHERE TD024 = '{1}' ",
                                              Convert.ToDateTime(item.end).AddDays(-1).ToString("yyyyMMdd"),
                                              item.lotNo);
                        strSqls.Add(sbSQL.ToString());

                        sbSQL.Clear();
                        sbSQL.AppendFormat(@" UPDATE PURTB 
                                              SET TD012 = '{0}'
                                              WHERE TD024 = '{1}' ",
                                              Convert.ToDateTime(item.end).AddDays(-1).ToString("yyyyMMdd"),
                                              item.lotNo);
                        strSqls.Add(sbSQL.ToString());
                    }
                }

                return new RES_WFGanttSet() { strSqls = strSqls };
            }
            else
            {
                throw new ExceptionTip_Basis("甘特信息为空，无法修改！");
            }
        }
    }
}
