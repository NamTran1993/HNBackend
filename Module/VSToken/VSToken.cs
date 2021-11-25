using HNBackend.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.VSToken
{
    public enum VEncrypt
    {
        NONE,
        OLD,
        NEW
    }

    public class VSToken
    {
        private string _privateKey = string.Empty;
        private string _rgbkey = string.Empty;
        private string _rgbIV = string.Empty;

        public VSToken(string privateKey, string rgbkey, string rgbIV)
        {
            _privateKey = privateKey;
            _rgbkey = rgbkey;
            _rgbIV = rgbIV;
        }

        public string CreateToken(string fToken, VEncrypt vEncrypt)
        {
            string resToken = string.Empty;
            try
            {
                switch (vEncrypt)
                {
                    case VEncrypt.NEW:
                        {
                            var decryptToken = DecryptNew(fToken);
                            string strToken = decryptToken.Substring(0, decryptToken.LastIndexOf('{'));
                            strToken += string.Format("{0}{1}", "{}", DateTime.UtcNow.Ticks);
                            resToken = CreateTokenNew(strToken);
                            break;
                        }
                    case VEncrypt.OLD:
                        {
                            var decryptToken = DecryptOld(fToken);
                            string strToken = decryptToken.Substring(0, decryptToken.LastIndexOf('{'));
                            strToken += string.Format("{0}{1}", "{}", DateTime.UtcNow.Ticks);
                            resToken = CreateTokenOld(strToken);
                            break;
                        }
                }

                return resToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region PRIVATE METHOD
        private string CreateTokenOld(string fToken)
        {
            try
            {
                var token = new StringBuilder();
                var _key = TGlobal.ConvertStringToByteArray(_privateKey);
                using (var aesAlg = new AesManaged())
                {
                    aesAlg.KeySize = 256;
                    aesAlg.GenerateIV();
                    aesAlg.Key = _key;
                    var encryptor = aesAlg.CreateEncryptor();
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(fToken);
                            }
                            token.Append(TGlobal.ConvertByteArrayToString(msEncrypt.ToArray()));
                            token.Append(TGlobal.ConvertByteArrayToString(aesAlg.IV));
                        }
                    }
                }

                return token.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CreateTokenNew(string fToken)
        {
            string resToken = string.Empty;
            try
            {
                byte[] key = null;
                byte[] iv = null;

                if (fToken == null || fToken.Length <= 0)
                    throw new Exception("FToken Is Null");
                if (_rgbkey == null || _rgbkey.Length <= 0)
                    throw new Exception("rgbkey Is Null");
                if (_rgbIV == null || _rgbIV.Length <= 0)
                    throw new Exception("IV Is Null");

                key = TGlobal.ConvertStringToByteArray(_rgbkey);
                iv = TGlobal.ConvertStringToByteArray(_rgbIV);

                byte[] encrypted = null;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(fToken);
                            }

                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                resToken = TGlobal.ConvertByteArrayToString(encrypted);

                return resToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string DecryptNew(string fToken)
        {
            string res = string.Empty;

            try
            {
                byte[] key = null;
                byte[] iv = null;

                if (fToken == null || fToken.Length <= 0)
                    throw new Exception("FToken Is Null");
                if (_rgbkey == null || _rgbkey.Length <= 0)
                    throw new Exception("rgbkey Is Null");
                if (_rgbIV == null || _rgbIV.Length <= 0)
                    throw new Exception("IV Is Null");

                key = TGlobal.ConvertStringToByteArray(_rgbkey);
                iv = TGlobal.ConvertStringToByteArray(_rgbIV);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    var text = TGlobal.ConvertStringToByteArray(fToken);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(text))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                res = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string DecryptOld(string fToken)
        {
            string result = string.Empty;
            try
            {
                var key = TGlobal.ConvertStringToByteArray(_privateKey);
                var iv = fToken.Substring(fToken.Length - 32);
                var message = fToken.Substring(0, fToken.Length - 32);

                using (var aesAlg = new AesManaged())
                {
                    aesAlg.KeySize = 256;
                    aesAlg.IV = TGlobal.ConvertStringToByteArray(iv);
                    aesAlg.Key = key;

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(TGlobal.ConvertStringToByteArray(message)))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
