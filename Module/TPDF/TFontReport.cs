using iText.IO.Font;
using iText.Kernel.Font;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TPDF
{
    public class TFontReport
    {
        private static TFontReport _instance = null;

        private string _pathFontReport = string.Empty;
        private string _fontName = string.Empty;

        private PdfFont _font = null;

        protected TFontReport(string pathFontReport, string fontName)
        {
            _pathFontReport = pathFontReport;
            _fontName = fontName;

            InitFontReport();
        }

        public PdfFont Font { get => _font; set => _font = value; }

        public static TFontReport Instance(string pathFontReport, string fontName, bool useSingleton)
        {
            try
            {
                // Khong su dung Singleton
                if (!useSingleton)
                    _instance = null;

                if (_instance == null)
                    _instance = new TFontReport(pathFontReport, fontName);

                return _instance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitFontReport()
        {
            try
            {
                FontProgramFactory.RegisterFont(_pathFontReport, _fontName);
                Font = PdfFontFactory.CreateFont(FontProgramFactory.CreateRegisteredFont(_fontName), PdfEncodings.IDENTITY_H, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
