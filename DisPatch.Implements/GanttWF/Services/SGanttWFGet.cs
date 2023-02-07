using DisPatch.Implements.GanttWF.Utills;
using DisPatch.Implements.Interfaces;
using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.GanttWF.Services
{
    public class SGanttWFGet : AbstractApi
    {
        public override RES_Basis ExecuteImplement(dynamic reqInfo, RES_Basis resContent)
        {
            resContent.content = UGanttWFGet.GetGanttInfo(reqInfo);
            return resContent;
        }
    }
}
