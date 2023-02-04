using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Common.Helpers
{
    public class LicenseHelper
    {
        public string mac { get; }

        public DateTime datetime { get; }

        public int count { get; }

        public LicenseHelper()
        {
            mac = "E45E37C63F09";
            datetime = DateTime.Now.AddDays(100);
            count = 9999;
        }
    }
}
