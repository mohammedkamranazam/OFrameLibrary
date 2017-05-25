using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OFrameLibrary.Util
{
    public static class EncodeMD5
    {
        public static string Encode(string originalPassword)
        {
            var originalBytes = Encoding.Default.GetBytes(originalPassword);
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                var encodedBytes = md5.ComputeHash(originalBytes);

                return Regex.Replace(BitConverter.ToString(encodedBytes), "-", string.Empty);
            }
        }
    }
}
