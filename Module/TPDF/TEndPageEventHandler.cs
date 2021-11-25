using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TPDF
{
    public class TEndPageEventHandler : IEventHandler
    {
        private THeaderFooterReport _rpHeaderFooter = null;

        private Style _styleHeader = null;
        private Style _styleFooter = null;
        private Style _styleContentFooter = null;

        private PdfDocument _pdfDoc = null;
        private PdfCanvas _pdfCanvas = null;
        private PdfDocumentEvent _docEvent = null;
        private PdfPage _page = null;
        private Canvas _canvas = null;
        private Rectangle _area = null;
        private Paragraph _pTime = null;
        private Paragraph _pDate = null;
        private Paragraph _pPage = null;
        private Paragraph _pFooterContent = null;

        private int _iPageNumber = 0;
        private bool _isLanscape = false;
        private string _pathImageBackground = string.Empty;

        public TEndPageEventHandler(THeaderFooterReport rpHeaderFooter, bool isLanscape)
        {
            _rpHeaderFooter = rpHeaderFooter;
            _styleHeader = rpHeaderFooter.HeaderStyle;
            _styleFooter = rpHeaderFooter.FooterStyle;
            _styleContentFooter = rpHeaderFooter.ContentFooterStyle;
            _isLanscape = isLanscape;
            _pathImageBackground = rpHeaderFooter.PathImageBackground;
        }

        public void HandleEvent(Event @event)
        {
            try
            {
                // -- 
                string date = string.Empty;
                string time = string.Empty;

                // -- 
                _docEvent = (PdfDocumentEvent)@event;
                _pdfDoc = _docEvent.GetDocument();
                _page = _docEvent.GetPage();
                _pdfCanvas = new PdfCanvas(_page.NewContentStreamBefore(), _page.GetResources(), _pdfDoc);
                _area = _pdfDoc.GetDefaultPageSize();

                float fWidthFooter = _isLanscape ? 790f : 550f;
                float fWidthHeader = _isLanscape ? 790f : 550f;

                _iPageNumber = _pdfDoc.GetPageNumber(_page);

                // --
                string currDateTime = string.Empty;

                DateTime dtNow = DateTime.UtcNow;

                if (dtNow.IsDaylightSavingTime())
                    _rpHeaderFooter.TimeZone = _rpHeaderFooter.TimeZone + 1;

                currDateTime = dtNow.AddHours(_rpHeaderFooter.TimeZone).ToString("MM/dd/yyyy-hh:mm:ss");

                string[] currDateTimes = currDateTime.Split('-');
                date = currDateTimes[0].ToString().ToUpper();
                time = currDateTimes[1].ToString().ToUpper();


                // --
                _pTime = new Paragraph();
                _pTime.Add(time);
                _pTime.AddStyle(_styleFooter);

                _pDate = new Paragraph();
                _pDate.Add(date);
                _pDate.AddStyle(_styleFooter);

                string pageNumber = string.Format("PAGE {0}", _iPageNumber);
                _pPage = new Paragraph();
                _pPage.Add(pageNumber.ToUpper());
                _pPage.AddStyle(_styleFooter);


                // --
                if (!string.IsNullOrEmpty(_pathImageBackground))
                {
                    Image background = new Image(ImageDataFactory.Create(_pathImageBackground));
                    background.ScaleToFit(_area.GetWidth(), _area.GetHeight());
                    background.SetRelativePosition(0, 0, 0, 0);

                    _canvas = new Canvas(_pdfCanvas, _pdfDoc, _area);
                    _canvas.Add(background);
                    _canvas.Flush();
                }

                // --
                if (!string.IsNullOrEmpty(_rpHeaderFooter.ContentFooter))
                {
                    _pFooterContent = new Paragraph();
                    _pFooterContent.Add(_rpHeaderFooter.ContentFooter);
                    _pFooterContent.AddStyle(_styleContentFooter);
                }

                // --
                if (_pDate != null)
                {
                    float fDelta = 30;
                    float x = _area.GetRight() - fDelta;
                    float y = 2;
                    _canvas = new Canvas(_pdfCanvas, _pdfDoc, _area);
                    _canvas.ShowTextAligned(_pDate, x, y, TextAlignment.CENTER);
                    _canvas.Flush();
                }

                if (_pTime != null)
                {
                    float fDelta = 30;
                    float x = _area.GetRight() - fDelta;
                    float y = 10;
                    _canvas = new Canvas(_pdfCanvas, _pdfDoc, _area);
                    _canvas.ShowTextAligned(_pTime, x, y, TextAlignment.CENTER);
                    _canvas.Flush();
                }
       
                if (_pFooterContent != null)
                {
                    float fDelta = 10;
                    float x = _area.GetLeft() + fDelta;
                    float y = 5;
                    _canvas = new Canvas(_pdfCanvas, _pdfDoc, _area);
                    _canvas.ShowTextAligned(_pFooterContent, x, y, TextAlignment.LEFT);
                    _canvas.Flush();

                    if (_pPage != null)
                    {
                        _canvas = new Canvas(_pdfCanvas, _pdfDoc, _area);
                        _canvas.ShowTextAligned(_pPage, (_area.GetWidth() / 2), 7, TextAlignment.CENTER);
                        _canvas.Flush();
                    }
                }
                else
                {
                    if (_pPage != null)
                    {
                        float fDelta = 10;
                        float x = _area.GetLeft() + fDelta;
                        float y = 5;
                        _canvas = new Canvas(_pdfCanvas, _pdfDoc, _area);
                        _canvas.ShowTextAligned(_pPage, x, y, TextAlignment.LEFT);
                        _canvas.Flush();
                    }
                }

                try
                {
                    _page.NewContentStreamBefore().Release();
                    _page.GetResources().Flush();

                    if (_canvas != null)
                        _canvas.Flush();

                    if (_pdfCanvas != null)
                        _pdfCanvas.Release();
                }
                catch (Exception)
                { }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
