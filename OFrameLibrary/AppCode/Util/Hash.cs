using System;
using System.Security.Cryptography;
using System.Text;

namespace OFrameLibrary.Util
{
    public class Hash : IDisposable
    {
        HashAlgorithm mCryptoService;

        public void Dispose()
        {
            if (mCryptoService != null)
            {
                mCryptoService.Dispose();
                mCryptoService = null;
            }
        }

        public Hash()
        {
            mCryptoService = new SHA1Managed();
        }

        public Hash(HashServiceProvider serviceProvider)
        {
            switch (serviceProvider)
            {
                case HashServiceProvider.MD5:
                    mCryptoService = new MD5CryptoServiceProvider();
                    break;

                case HashServiceProvider.SHA1:
                    mCryptoService = new SHA1Managed();
                    break;

                case HashServiceProvider.SHA256:
                    mCryptoService = new SHA256Managed();
                    break;

                case HashServiceProvider.SHA384:
                    mCryptoService = new SHA384Managed();
                    break;

                case HashServiceProvider.SHA512:
                    mCryptoService = new SHA512Managed();
                    break;
            }
        }

        public Hash(string serviceProviderName)
        {
            mCryptoService = (HashAlgorithm)CryptoConfig.CreateFromName(serviceProviderName.ToUpper());
        }

        public virtual string Encrypt(string plainText)
        {
            var cryptoByte = mCryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText + Salt));

            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
        }

        public string Salt
        {
            get;
            set;
        }
    }
}