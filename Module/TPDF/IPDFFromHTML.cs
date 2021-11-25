using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TPDF
{
    public abstract class IPDFFromHTML
    {
        protected abstract void LoadItextLicense(string fileLicense);
        public abstract bool CreateReport(string fileName, string fileHTML);
        public abstract void Save(ref Exception exception);
    }
}
