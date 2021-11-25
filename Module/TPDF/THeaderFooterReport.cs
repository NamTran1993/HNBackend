using iText.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TPDF
{
    public class THeaderFooterReport
    {
        public string PathIcon { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public double TimeZone { get; set; } = 0;
        public Style HeaderStyle { get; set; } = null;
        public Style FooterStyle { get; set; } = null;
        public Style ContentFooterStyle { get; set; } = null;
        public string ContentFooter { get; set; } = string.Empty;
        public string PathImageBackground { get; set; } = string.Empty;
    }
}
