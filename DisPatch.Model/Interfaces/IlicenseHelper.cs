using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Model.Interfaces
{
    public interface ILicenseHelper
    {
        public void GetLicenseInfo(string licenseFile, ref string data);
    }
}
