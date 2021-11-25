using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HNBackend.Module.TDowloadFile
{
    public class TWebDownloadFile
    {
        private string _fullPathFile = string.Empty;
        private string _fileName = string.Empty;

        public string FullPathFile { get => _fullPathFile; set => _fullPathFile = value; }
        public string FileName { get => _fileName; }


        public TWebDownloadFile(string fullPathFile)
        {
            _fullPathFile = fullPathFile;
        }

        public void Download(HttpResponse context)
        {
            try
            {
                if (string.IsNullOrEmpty(_fullPathFile))
                    throw new Exception("Full Path File IsNullOrEmpty.");

                string extension = Path.GetExtension(_fullPathFile);
                _fileName = Path.GetFileName(_fullPathFile);

                FileInfo fs = new FileInfo(_fullPathFile);
                context.ClearHeaders();
                context.ClearContent();
                context.AddHeader("Content-Disposition", "attachment;filename=\"" + _fileName + "\"");
                if (extension == ".zip")
                    context.ContentType = "Application/X-zip-compressed";
                else
                    context.ContentType = "Application/octet-stream";

                context.AppendHeader("Content-Length", fs.Length.ToString());
                context.TransmitFile(_fullPathFile);
                context.Flush();
                context.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
