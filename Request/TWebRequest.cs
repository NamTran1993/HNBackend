using HNBackend.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Request
{
    public class TWebRequest : IAPIWebRequest
    {
        public TWebRequest(string url, TMETHOD method, string dataPOST = "", string content_type = "application/json", List<string> lstHeader = null)
        {
            this._url = url;
            this._method = method;
            this._data_post = dataPOST;
            this._content_type = content_type;
            this._lstHeader = lstHeader;
        }

        public override void CreateWebRequest()
        {
            try
            {
                this._webRequest = WebRequest.Create(this._url);
                this._webRequest.UseDefaultCredentials = true;

                this._webRequest.PreAuthenticate = true;

                this._webRequest.Credentials = CredentialCache.DefaultNetworkCredentials;


                if (this._lstHeader != null && this._lstHeader.Count > 0)
                {
                    foreach(var header in this._lstHeader)
                    {
                        this._webRequest.Headers.Add(header);
                    }
                }

                if (this._method == TMETHOD.GET)
                    this._webRequest.Method = "GET";
                else if (this._method == TMETHOD.POST)
                    this._webRequest.Method = "POST";
                string empty = string.Empty;
                if (!string.IsNullOrEmpty(this._data_post))
                {
                    string dataPost = this._data_post;
                    byte[] bytes = Encoding.UTF8.GetBytes(this._data_post);
                    this._webRequest.ContentType = this._content_type;
                    this._webRequest.ContentLength = (long)bytes.Length;
                    this._stream = this._webRequest.GetRequestStream();
                    this._stream.Write(bytes, 0, bytes.Length);
                    this._stream.Close();

                   
                }
                else
                    this._webRequest.ContentLength = 0L;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool DownloadFile(string url, string file_save_as)
        {
            try
            {
                new WebClient().DownloadFile(url, file_save_as);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override byte[] GetImageFromUrl(string url)
        {
            byte[] numArray = (byte[])null;
            try
            {
                if (string.IsNullOrEmpty(url))
                    return (byte[])null;
                numArray = new WebClient().DownloadData(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return numArray;
        }

        public override string GetResponse()
        {
            try
            {
                string empty = string.Empty;
                this._webResponse = this._webRequest.GetResponse();
                this._status = ((HttpWebResponse)this._webResponse).StatusDescription;
                this._stream = this._webResponse.GetResponseStream();
                this._stream_reader = new StreamReader(this._stream);
                string end = this._stream_reader.ReadToEnd();
                this._stream_reader.Close();
                this._stream.Close();
                this._webResponse.Close();
                return end;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
