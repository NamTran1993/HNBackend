using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Font;
using iText.License;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TPDF
{
    public class TPDFFromHTML : IPDFFromHTML
    {
        // ----
        private string _fileLicenseItext = string.Empty;
        private string _filePDF = string.Empty;
        private string _fileHTML = string.Empty;
        private bool _landscape = false;
        private string _pathFont = string.Empty;
        private string _pathImageBackground = string.Empty;

        // ----
        private TPDFProperties _properties = null;
        private TPDFFooter _footer = null;
        private TPDFMarginDocument _marginDocument = null;

        // ----
        private MemoryStream _contents = null;
        private PdfDocument _pdfDocument = null;
        private WriterProperties _writerProperties = null;
        private PdfWriter _writer = null;
        private Document _document = null;
        private PageSize _pageSize = null;
        private Rectangle _rect = null;

        // ----
        private TPDFFont _fontFooter = null;
        private TPDFFont _fontContentFooter = null;

        // ----
        private double _timeZone = 0;

        public TPDFFromHTML(string fileLicenseItext, string pathFont, bool landscape, string pathImageBackground, TPDFProperties properties, TPDFFooter footer)
        {
            _fileLicenseItext = fileLicenseItext;
            _properties = properties;
            _landscape = landscape;
            _pathFont = pathFont;
            _footer = footer;
            _timeZone = footer.TimeZone;
            _fontFooter = new TPDFFont(7, Color.BLACK, true, _pathFont, "norwester", false);
            _fontContentFooter = new TPDFFont(9, Color.BLACK, true, _pathFont, "norwester", false);
            _pathImageBackground = pathImageBackground;
            _marginDocument = properties.PDFMarginDocument;

            LoadItextLicense(_fileLicenseItext);
        }


        public override bool CreateReport(string fileName, string fileHTML)
        {
            try
            {
                _fileHTML = fileHTML;

                ValidateInput();
                return InternalCSS(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Save(ref Exception exception)
        {
            try
            {
                if (_contents != null)
                {
                    _document.Close();

                    using (var stream = new FileStream(path: _filePDF, mode: FileMode.Create, access: FileAccess.Write))
                    {
                        stream.Write(_contents.ToArray(), 0, _contents.ToArray().Length);
                    }

                    _contents.Close();
                    _contents.Dispose();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        protected override void LoadItextLicense(string fileLicense)
        {
            try
            {
                if (File.Exists(fileLicense))
                    LicenseKey.LoadLicenseFile(fileLicense);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidateInput()
        {
            try
            {
                if (!File.Exists(_fileHTML))
                    throw new Exception("File HTML not exists.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ---- CSS Trong HTML -----
        private bool InternalCSS(string fileName)
        {
            try
            {
                // ----
                _filePDF = fileName;

                // ----
                InitPDFDocument();

                // ----
                var pdfDocument = _document != null ? _document.GetPdfDocument() : null;
                if (pdfDocument != null)
                {
                    ConverterProperties converter = new ConverterProperties();
                    FontProvider fontProvider = new DefaultFontProvider(true, false, false);
                    fontProvider.AddFont(_pathFont);
                    converter.SetFontProvider(fontProvider);

                    using (var stream = new FileStream(path: _fileHTML, mode: FileMode.Open, access: FileAccess.Read))
                    {
                        HtmlConverter.ConvertToPdf(stream, pdfDocument, converter);
                    }
                }
                else
                {
                    using (var stream = new FileStream(path: _fileHTML, mode: FileMode.Open, access: FileAccess.Read))
                    {
                        HtmlConverter.ConvertToPdf(new FileInfo(_fileHTML), new FileInfo(_filePDF));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void InitPDFDocument()
        {
            try
            {
                _contents = new MemoryStream();

                // ---
                _writerProperties = new WriterProperties();
                _writerProperties.SetCompressionLevel(CompressionConstants.BEST_COMPRESSION);
                _writerProperties.SetFullCompressionMode(true);
                _writerProperties.SetPdfVersion(PdfVersion.PDF_1_5);

                // ----
                _writer = new PdfWriter(_contents, _writerProperties);

                // ----
                _pdfDocument = new PdfDocument(_writer);

                // ----
                if (_properties != null)
                {
                    if (!string.IsNullOrEmpty(_properties.Author))
                        _pdfDocument.GetDocumentInfo().SetAuthor(_properties.Author);

                    if (!string.IsNullOrEmpty(_properties.Creator))
                        _pdfDocument.GetDocumentInfo().SetCreator(_properties.Creator);

                    if (!string.IsNullOrEmpty(_properties.Keywords))
                        _pdfDocument.GetDocumentInfo().SetKeywords(_properties.Keywords);

                    if (!string.IsNullOrEmpty(_properties.Subject))
                        _pdfDocument.GetDocumentInfo().SetSubject(_properties.Subject);

                    if (!string.IsNullOrEmpty(_properties.Title))
                        _pdfDocument.GetDocumentInfo().SetTitle(_properties.Title);
                }

                // ----
                _rect = _landscape ? new Rectangle(842, 595) : PageSize.A4;
                _pageSize = new PageSize(_rect);

                // ----
                _document = new Document(_pdfDocument, _pageSize);

                // ----
                if (_marginDocument != null)
                {
                    if (_marginDocument.Top > 0)
                        _document.SetTopMargin(_marginDocument.Top);

                    if (_marginDocument.Right > 0)
                        _document.SetRightMargin(_marginDocument.Right);

                    if (_marginDocument.Bottom > 0)
                        _document.SetBottomMargin(_marginDocument.Bottom);

                    if (_marginDocument.Left > 0)
                        _document.SetLeftMargin(_marginDocument.Left);
                }

                // ----
                THeaderFooterReport headerFooterReport = new THeaderFooterReport()
                {
                    FooterStyle = _fontFooter.Style,
                    ContentFooter = _footer != null && !string.IsNullOrEmpty(_footer.ContentSologan) ? _footer.ContentSologan : string.Empty,
                    ContentFooterStyle = _fontContentFooter.Style,
                    TimeZone = _timeZone,
                    PathImageBackground = _pathImageBackground
                };

                _document.GetPdfDocument().AddEventHandler(PdfDocumentEvent.END_PAGE, new TEndPageEventHandler(headerFooterReport, _landscape));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
