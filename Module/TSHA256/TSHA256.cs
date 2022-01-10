using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TSHA256
{
    public class TSHA256
    {
        public static string THashSHA256(string inputValue)
        {
            string resSHA256 = string.Empty;
            try
            {
                var crypt = new System.Security.Cryptography.SHA256Managed();
                var hash = new StringBuilder();
                byte[] crypto = crypt.ComputeHash(Encoding.Unicode.GetBytes(inputValue));
                foreach (byte theByte in crypto)
                {
                    hash.Append(theByte.ToString("x2"));
                }
                resSHA256 = hash.ToString();
                return resSHA256;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
