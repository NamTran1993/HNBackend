using HNBackend.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TEncode
{
    public class TEncode
    {
        public static string _IV_KEY = TGlobal.BytesToHexString(Encoding.UTF8.GetBytes("!TranHuyNam12^^!"));

        #region PUBLIC
        public static string Encode(string passwordEndcode, string inputString, TENCODE typeEncode)
        {
            string res = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(passwordEndcode))
                    throw new Exception("Please input Password Endcode!");

                if (string.IsNullOrEmpty(inputString))
                    throw new Exception("Input String IsNullOrEmpty!");

                switch (typeEncode)
                {
                    case TENCODE.DES:
                        {
                            // --- PASS Max 8 byte
                            res = TEncodeDES(passwordEndcode, inputString);
                            break;
                        }

                    case TENCODE.TDES:
                        {
                            res = TEncodeTDES(passwordEndcode, inputString);
                            break;
                        }

                    case TENCODE.AES_256:
                        {
                            res = TEncodeAES256(passwordEndcode, inputString);
                            break;
                        }

                    default:
                        break;
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Decode(string passwordEndcode, string inputString, TENCODE typeEncode)
        {
            string res = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(passwordEndcode))
                    throw new Exception("Please input Password Endcode!");

                if (string.IsNullOrEmpty(inputString))
                    throw new Exception("Input String IsNullOrEmpty!");

                switch (typeEncode)
                {
                    case TENCODE.DES:
                        {
                            // --- PASS Max 8 byte
                            res = TDecodeDES(passwordEndcode, inputString);
                            break;
                        }

                    case TENCODE.TDES:
                        {
                            res = TDecodeTDES(passwordEndcode, inputString);
                            break;
                        }

                    case TENCODE.AES_256:
                        {
                            res = TDecodeAES256S(passwordEndcode, inputString);
                            break;
                        }

                    default:
                        break;
                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DES
        private static string TEncodeDES(string passwordEndcode, string inputString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(passwordEndcode);
                byte[] ivKey = !string.IsNullOrEmpty(_IV_KEY) ? Encoding.UTF8.GetBytes(_IV_KEY) : rgbKey;

                if (rgbKey.Length != 8)
                    throw new Exception("Please input Password 8 bytes!");

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(rgbKey, ivKey), CryptoStreamMode.Write);

                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(inputString);

                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();

                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string TDecodeDES(string passwordEndcode, string inputString)
        {
            string res = string.Empty;
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(passwordEndcode);
                byte[] ivKey = !string.IsNullOrEmpty(_IV_KEY) ? Encoding.UTF8.GetBytes(_IV_KEY) : rgbKey;

                if (rgbKey.Length != 8)
                    throw new Exception("Please input Password 8 bytes!");

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(inputString));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(rgbKey, ivKey), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region TDES
        private static string TEncodeTDES(string passwordEndcode, string inputString)
        {
            string res = string.Empty;
            try
            {
                TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

                byte[] byteHash = null;
                byte[] byteBuff = null;
                byte[] byteKey = Encoding.Unicode.GetBytes(passwordEndcode);
             
                byteHash = hashMD5Provider.ComputeHash(byteKey);
                desCryptoProvider.Key = byteHash;
                desCryptoProvider.Mode = CipherMode.ECB;
                byteBuff = Encoding.Unicode.GetBytes(inputString);

                res = Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string TDecodeTDES(string passwordEndcode, string inputString)
        {
            string res = string.Empty;
            try
            {
                TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

                byte[] byteHash = null;
                byte[] byteBuff = null;
                byte[] byteKey = Encoding.Unicode.GetBytes(passwordEndcode);

                byteHash = hashMD5Provider.ComputeHash(byteKey);
                desCryptoProvider.Key = byteHash;
                desCryptoProvider.Mode = CipherMode.ECB;
                byteBuff = Convert.FromBase64String(inputString);

                res = Encoding.Unicode.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AES 256
        private static string TEncodeAES256(string passwordEndcode, string inputString)
        {
            string res = string.Empty;
            try
            {    
                byte[] encrypted = null;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = TGlobal.ConvertStringToByteArray(passwordEndcode);
                    aesAlg.IV = !string.IsNullOrEmpty(_IV_KEY) ? TGlobal.ConvertStringToByteArray(_IV_KEY) : TGlobal.ConvertStringToByteArray(passwordEndcode);

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(inputString);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                res = TGlobal.ConvertByteArrayToString(encrypted);
                return res.ToUpper() ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static string TDecodeAES256S(string passwordEndcode, string inputString)
        {
            string res = string.Empty;
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = TGlobal.ConvertStringToByteArray(passwordEndcode);
                    aesAlg.IV = !string.IsNullOrEmpty(_IV_KEY) ? TGlobal.ConvertStringToByteArray(_IV_KEY) : TGlobal.ConvertStringToByteArray(passwordEndcode); ;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(TGlobal.ConvertStringToByteArray(inputString)))
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
        #endregion
    }
}
