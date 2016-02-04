using System;
using System.Security.Cryptography;
using System.Text;

namespace MT.Common.Utility.ConvertExt
{
    /// <summary>
    /// 文本相关
    /// </summary>
    public class TextToBase64String
    {
        public static string Sha256(string plainText)
        {
            SHA256Managed _sha256 = new SHA256Managed();
            byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            return Convert.ToBase64String(_cipherText);
        }
    }
}
