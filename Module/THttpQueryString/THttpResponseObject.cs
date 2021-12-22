using HNBackend.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.THttpQueryString
{
    public class THttpResponseObject
    {
        public TCODE TCode { get; set; }
        public string TMessageCode { get; set; }
        public string TData { get; set; }
        public string TVersion { get; set; }
    }
}
