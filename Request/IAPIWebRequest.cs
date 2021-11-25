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
    public abstract class IAPIWebRequest
    {
        protected string _LOG_FILE = "APIWebRequest.log";
        protected WebRequest _webRequest;
        protected WebResponse _webResponse;
        protected Stream _stream;
        protected StreamReader _stream_reader;
        protected TMETHOD _method;
        protected string _status = string.Empty;
        protected string _url = string.Empty;
        protected string _data_post = string.Empty;
        protected string _content_type = "application/json";
        protected List<string> _lstHeader = null;

        public abstract void CreateWebRequest();

        public abstract string GetResponse();

        public abstract bool DownloadFile(string url, string file_save_as);

        public abstract byte[] GetImageFromUrl(string url);
    }
}
