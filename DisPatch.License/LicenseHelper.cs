using DisPatch.Common.Utils;
using DisPatch.Model.Interfaces;
using System.Text;
using System.Xml;

namespace DisPatch.License
{
    public class LicenseHelper : ILicenseHelper
    {
        public void GetLicenseInfo(string licenseFile, ref string data)
        {
            FileStream fss = new FileStream(licenseFile, FileMode.Open, FileAccess.Read);
            long n = fss.Length;
            byte[] b = new byte[n];
            int index1, index2;
            index2 = 0;
            index1 = fss.ReadByte();
            while (index1 != -1) { b[index2++] = Convert.ToByte(index1); index1 = fss.ReadByte(); }
            fss.Close();
            string strLicenses = Encoding.UTF8.GetString(b);
            strLicenses = strLicenses.Substring(1, strLicenses.Length - 1);
            strLicenses = DesUtil.DecryptString(strLicenses);
            strLicenses = AesUtil.AesDecrypt(strLicenses);

            XmlDocument xmlDosc = new XmlDocument();
            xmlDosc.LoadXml(strLicenses);
            data = xmlDosc.ChildNodes[1].ChildNodes[0].InnerText + "," + xmlDosc.ChildNodes[1].ChildNodes[1].InnerText + "," + xmlDosc.ChildNodes[1].ChildNodes[2].InnerText;
        }
    }
}