using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OFrameLibrary.Util
{
    public class SymCryptography : IDisposable
    {
        SymCryptographyServiceProvider mAlgorithm;
        SymmetricAlgorithm mCryptoService;
        string mKey = string.Empty;
        string mSalt = string.Empty;

        public void Dispose()
        {
            if (mCryptoService != null)
            {
                mCryptoService.Dispose();
                mCryptoService = null;
            }
        }

        public SymCryptography()
        {
            mCryptoService = new RijndaelManaged();
            mCryptoService.Mode = CipherMode.CBC;
            mAlgorithm = SymCryptographyServiceProvider.Rijndael;
        }

        public SymCryptography(SymCryptographyServiceProvider serviceProvider)
        {
            switch (serviceProvider)
            {
                case SymCryptographyServiceProvider.Rijndael:
                    mCryptoService = new RijndaelManaged();
                    mAlgorithm = SymCryptographyServiceProvider.Rijndael;
                    break;

                case SymCryptographyServiceProvider.RC2:
                    mCryptoService = new RC2CryptoServiceProvider();
                    mAlgorithm = SymCryptographyServiceProvider.RC2;
                    break;

                case SymCryptographyServiceProvider.DES:
                    mCryptoService = new DESCryptoServiceProvider();
                    mAlgorithm = SymCryptographyServiceProvider.DES;
                    break;

                case SymCryptographyServiceProvider.TripleDES:
                    mCryptoService = new TripleDESCryptoServiceProvider();
                    mAlgorithm = SymCryptographyServiceProvider.TripleDES;
                    break;
            }
            mCryptoService.Mode = CipherMode.CBC;
        }

        public SymCryptography(string serviceProviderName)
        {
            try
            {
                switch (serviceProviderName.ToLower())
                {
                    case "rijndael":
                        serviceProviderName = "Rijndael";
                        mAlgorithm = SymCryptographyServiceProvider.Rijndael;
                        break;

                    case "rc2":
                        serviceProviderName = "RC2";
                        mAlgorithm = SymCryptographyServiceProvider.RC2;
                        break;

                    case "des":
                        serviceProviderName = "DES";
                        mAlgorithm = SymCryptographyServiceProvider.DES;
                        break;

                    case "tripledes":
                        serviceProviderName = "TripleDES";
                        mAlgorithm = SymCryptographyServiceProvider.TripleDES;
                        break;
                }

                mCryptoService = (SymmetricAlgorithm)CryptoConfig.CreateFromName(serviceProviderName);
                mCryptoService.Mode = CipherMode.CBC;
            }
            catch
            {
                throw;
            }
        }

        public string Key
        {
            get
            {
                return mKey;
            }
            set
            {
                mKey = value;
            }
        }

        public string Salt
        {
            get
            {
                return mSalt;
            }
            set
            {
                mSalt = value;
            }
        }

        void SetLegalIV()
        {
            switch (mAlgorithm)
            {
                case SymCryptographyServiceProvider.Rijndael:
                    mCryptoService.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8, 0x4b, 0x73, 0xcc };
                    break;

                default:
                    mCryptoService.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9 };
                    break;
            }
        }

        public virtual string Decrypt(string cryptoText)
        {
            var cryptoByte = Convert.FromBase64String(cryptoText);
            var keyByte = GetLegalKey();

            mCryptoService.Key = keyByte;
            SetLegalIV();

            var cryptoTransform = mCryptoService.CreateDecryptor();

            try
            {
                var ms = new MemoryStream(cryptoByte, 0, cryptoByte.Length);

                var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Read);

                var sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

        public virtual string Encrypt(string plainText)
        {
            var plainByte = ASCIIEncoding.ASCII.GetBytes(plainText);
            var keyByte = GetLegalKey();

            mCryptoService.Key = keyByte;
            SetLegalIV();

            var cryptoTransform = mCryptoService.CreateEncryptor();

            var ms = new MemoryStream();

            var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);

            cs.Write(plainByte, 0, plainByte.Length);
            cs.FlushFinalBlock();

            var cryptoByte = ms.ToArray();

            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
        }

        public virtual byte[] GetLegalKey()
        {
            if (mCryptoService.LegalKeySizes.Length > 0)
            {
                var keySize = mKey.Length * 8;
                var minSize = mCryptoService.LegalKeySizes[0].MinSize;
                var maxSize = mCryptoService.LegalKeySizes[0].MaxSize;
                var skipSize = mCryptoService.LegalKeySizes[0].SkipSize;

                if (keySize > maxSize)
                {
                    mKey = mKey.Substring(0, maxSize / 8);
                }
                else
                {
                    if (keySize < maxSize)
                    {
                        var validSize = (keySize <= minSize) ? minSize : (keySize - keySize % skipSize) + skipSize;
                        if (keySize < validSize)
                        {
                            mKey = mKey.PadRight(validSize / 8, '*');
                        }
                    }
                }
            }

            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(mKey, ASCIIEncoding.ASCII.GetBytes(mSalt)))
            {
                return passwordDeriveBytes.GetBytes(mKey.Length);
            }
        }
    }
}