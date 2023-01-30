using DisPatch.Implements.Gantt.Utills;
using DisPatch.Implements.Interfaces;
using DisPatch.Model.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Implements.Gantt.Services
{
    public class SGanttGet : AbstractApi
    {
        public override RES_Basis ExecuteImplement(dynamic reqInfo, RES_Basis resContent)
        {
            resContent.content = UGanttGet.GetGanttInfo(reqInfo);
            return resContent;
        }
    }
}
