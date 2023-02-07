

using DisPatch.Common.Utils;
using Newtonsoft.Json;
using System;
using System.Management;
using System.Text;
using System.Xml;
using System.Xml.Linq;


string macLocal = "";
try
{
    string strMac = string.Empty;
    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
    ManagementObjectCollection moc = mc.GetInstances();
    foreach (ManagementObject mo in moc)
    {
        if ((bool)mo["IPEnabled"] == true)
        {
            strMac = mo["MacAddress"].ToString();
        }
    }
    moc = null;
    mc = null;
    macLocal = strMac.Replace(":", "");
}
catch (Exception)
{

}

string FilePath = @"D:\Temp\license.lic";
string XMLPath = @"D:\Temp\license.xml";

Console.WriteLine("*-*-*-*-*-*-*-*-*-*-DisPatch授权文件生成启动-*-*-*-*-*-*-*-*-*-*");
Console.WriteLine($"请输入授权服务器MAC地址[{macLocal}]：");
string mac = Console.ReadLine().ToString();
Console.WriteLine("请输入授权天数：");
string date = Console.ReadLine().ToString();
Console.WriteLine("请输入授权用户数：");
string count = Console.ReadLine().ToString();

XElement xElement = new XElement(new XElement("LicenseValue", new XElement("mac", mac), new XElement("date", DateTime.Now.AddDays(Convert.ToInt32(date)).ToString("yyyy-MM-dd")), new XElement("count", count)));
XmlWriterSettings settings = new XmlWriterSettings();
settings.Encoding = new UTF8Encoding(false);
settings.Indent = true;
XmlWriter xw = XmlWriter.Create(XMLPath, settings);
xElement.Save(xw);
xw.Flush();
xw.Close();
XmlDocument xmlDoc = new XmlDocument();
xmlDoc.Load(XMLPath);
MemoryStream stream = new MemoryStream();
XmlTextWriter writer = new XmlTextWriter(stream, null);
writer.Formatting = System.Xml.Formatting.Indented;
xmlDoc.Save(writer);
StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
stream.Position = 0;
string strLicense = sr.ReadToEnd();
sr.Close();
stream.Close();
strLicense = AesUtil.AesEncrypt(strLicense);
strLicense = DesUtil.EncryptString(strLicense);

using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
{
    using (StreamWriter streamWriter = new StreamWriter(fs, Encoding.UTF8))
    {
        streamWriter.Write(strLicense);
    }
}