using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TSendGrid
{
    public class TSendGridData
    {
        public string DisplayName { get; set; }
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
    }
}
