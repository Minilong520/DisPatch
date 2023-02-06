using DisPatch.Common.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Common.Utils
{
    public class DesUtil
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string EncryptString(string str, string sKey = DisPatchAuthOptions.Scheme)
        {
            byte[] inputByteArray = Encoding.Default.GetBytes(str);
            sKey = checkKey(sKey);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(sKey);// 秘钥
                des.IV = Encoding.ASCII.GetBytes(sKey);// 初始化向量
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                    }

                    var retB = Convert.ToBase64String(ms.ToArray());
                    return retB;
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DecryptString(string pToDecrypt, string sKey = DisPatchAuthOptions.Scheme)
        {            
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            sKey = checkKey(sKey);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {

                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        // 如果两次秘钥不一样，这一步可能会引发异常
                        cs.FlushFinalBlock();
                    }
                    return Encoding.Default.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 检查密钥
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string checkKey(string key)
        {
            //if (key.Length < 16)
            //{
            //    for (int i = key.Length; i < 16; i++)
            //    {
            //        key += randomStr(i);
            //    }
            //}
            //else
            //{
            //    key = key.Substring(0, 16);
            //}
            //return key;

            while (key.Length < 8)
            {
                key += key;
            }

            return key.Substring(0, 8);
        }
    }
}
