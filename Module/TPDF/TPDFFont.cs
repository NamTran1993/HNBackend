using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TPDF
{
    public class TPDFFont
    {
        private PdfFont _font = null;
        public PdfFont Font { get => _font; set => _font = value; }


        private Style _style = null;
        public Style Style { get => _style; set => _style = value; }


        private TFontReport _mfontReport = null;

        public TPDFFont(float fontSize, Color color, bool bold, string pathFont, string fontName, bool useSignleton)
        {
            try
            {
                _mfontReport = TFontReport.Instance(pathFont, fontName, useSignleton);
                _font = _mfontReport.Font;

                _style = new Style();
                if (bold)
                    _style.SetFont(_font);

                _style.SetFontSize(fontSize);
                _style.SetFontColor(color);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
