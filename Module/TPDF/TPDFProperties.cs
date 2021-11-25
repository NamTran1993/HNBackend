using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TPDF
{
    public class TPDFProperties
    {
        public string Author { get; set; } = string.Empty;
        public string Creator { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public TPDFMarginDocument PDFMarginDocument { get; set; } = new TPDFMarginDocument();
    }

    public class TPDFFooter
    {
        public string ContentSologan { get; set; }
        public double TimeZone { get; set; }
    }

    public class TPDFMarginDocument
    {
        public float Top { get; set; } = 65;
        public float Right { get; set; } = 16;
        public float Bottom { get; set; } = 20;
        public float Left { get; set; } = 25;
    }
}
