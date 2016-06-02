using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OFrameLibrary.Util
{
    public class EncodeMD5
    {
        public static string Encode(string originalPassword)
        {
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return Regex.Replace(BitConverter.ToString(encodedBytes), "-", string.Empty);
        }
    }
}