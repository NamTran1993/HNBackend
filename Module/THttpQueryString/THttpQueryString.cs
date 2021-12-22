using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HNBackend.Module.THttpQueryString
{
    public class THttpQueryString
    {
        public static string TQueryString(HttpContext context, string keyParams)
        {
            string res = string.Empty;
            try
            {
                if (context != null && !string.IsNullOrEmpty(keyParams))
                    res = context.Request.QueryString[keyParams];
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetJsonRequest(HttpContext context)
        {
            string jsonString = string.Empty;
            try
            {
                StreamReader pipe = new StreamReader(context.Request.InputStream);
                jsonString = pipe.ReadToEnd();
                if (pipe != null)
                {
                    pipe.Close();
                    pipe.Dispose();
                }
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
