using DisPatch.Common.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.Common.Utils
{
    /// <summary>
    /// AES加密 解密
    /// </summary>
    public static class AesUtil
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key = DisPatchAuthOptions.Scheme)
        {
            if (string.IsNullOrEmpty(str)) { return null; };
            if (IsBase64(str)) { return str; }

            key = checkKey(key);
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key = DisPatchAuthOptions.Scheme)
        {
            if (string.IsNullOrEmpty(str)) return null;

            if (!IsBase64(str)) { return str; }
            key = checkKey(key);
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        private static char[] base64CodeArray = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4',  '5', '6', '7', '8', '9', '+', '/', '='
        };
        /// <summary>
        /// 检查需要解密的内容
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static bool IsBase64(string base64Str)
        {
            //string strRegex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$";
            //byte[] bytes = null;
            if (string.IsNullOrEmpty(base64Str))
                return false;
            else
            {
                if (base64Str.Contains(","))
                    base64Str = base64Str.Split(',')[1];
                if (base64Str.Length % 4 != 0)
                    return false;
                if (base64Str.Any(c => !base64CodeArray.Contains(c)))
                    return false;
            }
            try
            {
                //bytes = Convert.FromBase64String(base64Str);
                return true;
            }
            catch (FormatException)
            {
                return false;
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

            while (key.Length < 16)
            {
                key += key;
            }

            return key.Substring(0, 16);
        }

        /// <summary>
        /// 随机密钥
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string randomStr(int i)
        {
            Random rd = new Random(i);
            int a = rd.Next(0, 64);
            return base64CodeArray[a].ToString();
        }
    }
}
