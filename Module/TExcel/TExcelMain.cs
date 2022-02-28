using HNBackend.Module.TExcel.TExcelGlobal;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExcel
{
    public class TExcelMain : IExcel
    {
        private ExcelPackage _package;
        private ExcelPicture _xPicture;
        private ExcelDrawings _xDrawing;
        private int _height = 14;
        private int _xPicId = 0;

        public ExcelWorksheet _worksheet { get; set; }
        public ExcelRange _range { get; set; }
        public ExcelPackage Package { get => _package; set => _package = value; }


        public TExcelMain()
        {
            _xPicId = 0;
            _package = new ExcelPackage();
        }

        public override void InitDefaultStyles()
        {
            try
            {
                TExcelEnumerable.CreateDefaultStyles(_worksheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void ReleaseDefaultStyles()
        {
            try
            {
                TExcelEnumerable.ReleaseDefaultStyles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void NewSheet(string sheetName, string copySheetName = "")
        {
            try
            {
                if (string.IsNullOrEmpty(copySheetName))
                    _worksheet = _package.Workbook.Worksheets.Add(sheetName);
                else
                {
                    ExcelWorksheet copySheet = _package.Workbook.Worksheets[copySheetName];
                    _worksheet = _package.Workbook.Worksheets.Add(sheetName, copySheet);
                    copySheet.Dispose();
                }
                try
                {
                    InitDefaultStyles();
                }
                catch (Exception ex)
                { }
                _xDrawing = _worksheet.Drawings;
                _worksheet.DefaultColWidth = 50;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddValue(int row, int col, object value, bool isNumber = false)
        {
            try
            {
                _range = _worksheet.Cells[row, col];
                if (isNumber)
                {
                    _range.Style.Numberformat.Format = "0";
                    _range.Value = int.Parse(value.ToString());
                }
                else
                    _range.Value = value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddValue(string range, object value)
        {
            try
            {
                _range = _worksheet.Cells[range];
                _range.Merge = true;
                _range.Value = value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddValue(int fromRow, int fromColumn, int toRow, int toColumn, object value, string position = "", bool isNumber = false)
        {
            try
            {
                _worksheet.DefaultColWidth = 25;
                _range = _worksheet.Cells[fromRow, fromColumn, toRow, toColumn];

                if (fromRow != toRow || fromColumn != toColumn)
                    _range.Merge = true;
                else
                    _range.Merge = false;

                if (isNumber)
                {
                    _range.Style.Numberformat.Format = "0";
                    _range.Value = int.Parse(value.ToString());
                }
                else
                    _range.Value = value;

                _worksheet.Cells.Style.WrapText = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddValue(int fromRow, int fromColumn, int toRow, int toColumn, object value, TExcelBorder border, TExcelColor backgroundColor, Color font, string styleName = "",
             TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, bool bFreezes = false, string position = "", bool isNumber = false)
        {
            try
            {
                AddValue(fromRow, fromColumn, toRow, toColumn, value, position, isNumber);
                if (!string.IsNullOrEmpty(styleName)) _range.StyleName = styleName;

                if (border.Style != TExcelBorderStyle.None)
                {
                    if (borderPosition == TExcelBorderPosition.Box)
                        _range.Style.Border.BorderAround((OfficeOpenXml.Style.ExcelBorderStyle)border.Style, border.Color.ToColor());
                    else if (borderPosition == TExcelBorderPosition.Only_Right)
                    {
                        _range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        _range.Style.Border.Right.Color.SetColor(border.Color.ToColor());
                    }
                }

                if (backgroundColor != TExcelColor.None)
                {
                    _range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    if (backgroundColor == TExcelColor.Header)
                        _range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 68, 114, 196));
                    else if (backgroundColor == TExcelColor.Normal)
                        _range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    else if (backgroundColor == TExcelColor.Highlight)
                        _range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 190, 213, 239));
                    else if (backgroundColor == TExcelColor.IN_OUT)
                        _range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    else if (backgroundColor == TExcelColor.ColorHeader)
                        _range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 28, 54, 79));
                    else if (backgroundColor == TExcelColor.Gray91)
                        _range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 217, 230, 246));
                    else if (backgroundColor == TExcelColor.ColorHeaderTable)
                        _range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 68, 114, 196));
                    else
                        _range.Style.Fill.BackgroundColor.SetColor(backgroundColor.ToColor());
                }

                if (font != null)
                    _range.Style.Font.Color.SetColor(font);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddImage(Image image, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder borderImage, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float fMargin_Left = 50)
        {
            try
            {
                _worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _range = _worksheet.Cells[fromRow, fromColumn, toRow, toColumn];
                _range.Merge = true;

                _xPicId = new Random().Next(100000);

                _xPicture = _xDrawing.AddPicture(_xPicId.ToString(), image);
                _xPicId++;

                // Add border cua Image
                if (borderImage != null && borderImage != TExcelBorder.None)
                {
                    _xPicture.Border.Width = 1;
                    _xPicture.Border.Fill.Color = borderImage.Color.ToColor();
                }
                if (imageSize != null)
                    _xPicture.SetSize(imageSize.Width, imageSize.Height);

                if (backgroundColor != null)
                {
                    _worksheet.SelectedRange[fromRow, fromColumn, toRow, toColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    _worksheet.SelectedRange[fromRow, fromColumn, toRow, toColumn].Style.Fill.BackgroundColor.SetColor(backgroundColor);
                }

                // Add Border Cell chua Images
                if (borderCell != null && borderCell != TExcelBorder.None)
                {
                    if (position == TExcelBorderPosition.Box)
                    {
                        _range.Style.Border.BorderAround((OfficeOpenXml.Style.ExcelBorderStyle)borderCell.Style, borderCell.Color.ToColor());
                    }
                }

                _xPicture.SetPosition(_range.Start.Row - 1, _height, _range.Start.Column - 1, _height + (int)fMargin_Left);
                _xPicture.Locked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override void AddImage(List<Image> image, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder border, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float marginLeft = 50)
        {
            try
            {
                _worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _range = _worksheet.Cells[fromRow, fromColumn, toRow, toColumn];
                _range.Merge = true;

                int r = _range.Start.Row - 1;
                for (int i = 0; i < image.Count; i++)
                {
                    _xPicture = _xDrawing.AddPicture(_xPicId.ToString(), image[i]);
                    _xPicId++;

                    if (imageSize != null)
                        _xPicture.SetSize(imageSize.Width, imageSize.Height);

                    _xPicture.SetPosition(r, _height, _range.Start.Column - 1, _height + (int)marginLeft);
                    _xPicture.Locked = false;
                    r++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddImage(byte[] imgb, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder borderImage, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float fMargin_Left = 50)
        {
            try
            {
                if (imgb != null)
                {
                    Image image = null;
                    using (var memoryStream = new MemoryStream(imgb))
                    {
                        image = Image.FromStream(memoryStream);
                        AddImage(image, fromRow, fromColumn, toRow, toColumn, borderImage, imageSize, backgroundColor, borderCell, position, fMargin_Left);
                        image.Dispose();
                        memoryStream.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddImage(List<byte[]> arrayImage, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder border, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float marginLeft = 50)
        {
            try
            {
                if (arrayImage != null && arrayImage.Count > 0)
                {
                    _worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    _range = _worksheet.Cells[fromRow, fromColumn, toRow, toColumn];
                    _range.Merge = true;
                    _range.Worksheet.Hidden = eWorkSheetHidden.Hidden;
                    _worksheet.Protection.IsProtected = false;
                    _worksheet.Protection.AllowSelectLockedCells = false;

                    int r = _range.Start.Row - 1;
                    int c = _range.Start.Column - 1;

                    Image image = null;

                    GC.Collect();
                    for (int i = 0; i < arrayImage.Count; i++)
                    {
                        using (var memoryStream = new MemoryStream(arrayImage[i]))
                        {
                            image = Image.FromStream(memoryStream);
                            _xPicture = _xDrawing.AddPicture(_xPicId.ToString(), image);
                            _xPicId++;

                            if (imageSize != null)
                                _xPicture.SetSize(imageSize.Width, imageSize.Height);

                            _xPicture.SetPosition(r, _height, c, _height + (int)marginLeft);
                            _xPicture.Locked = false;

                            if (memoryStream != null)
                            {
                                memoryStream.Close();
                                memoryStream.Dispose();
                            }

                            if (image != null)
                                image.Dispose();

                            if (_xPicture != null)
                                _xPicture.Dispose();
                        }

                        r++;
                    }

                    _range.Worksheet.Hidden = eWorkSheetHidden.Visible;
                    this._package.DoAdjustDrawings = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Save(string outputPath)
        {
            try
            {
                using (FileStream fileStream = File.Create(outputPath))
                {
                    _package.SaveAs(fileStream);
                    fileStream.Close();
                    fileStream.Dispose();
                }

                if (_package != null)
                    _package.Dispose();

                if (_xPicture != null)
                    _xPicture.Dispose();

                if (_worksheet != null)
                    _worksheet.Dispose();

                if (_range != null)
                    _range.Dispose();

                ReleaseDefaultStyles();


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Save(string outputPath, bool isReleaseImage)
        {
            try
            {
                using (FileStream fileStream = File.Create(outputPath))
                {
                    _package.SaveAs(fileStream);
                    fileStream.Close();
                    fileStream.Dispose();
                }

                if (_package != null)
                    _package.Dispose();

                if (_worksheet != null)
                    _worksheet.Dispose();

                if (_range != null)
                    _range.Dispose();

                ReleaseDefaultStyles();


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override byte[] ToBinary()
        {
            try
            {
                byte[] result = _package.GetAsByteArray();
                _package.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void SetWidths(params double[] widths)
        {
            try
            {
                for (int i = 1; i <= widths.Length; i++)
                {
                    _worksheet.Column(i).Width = widths[i - 1];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void SetRowHeights(Dictionary<int, double> heights)
        {
            try
            {
                foreach (KeyValuePair<int, double> height in heights)
                {
                    _worksheet.Row(height.Key).Height = height.Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void SetHeightRanges(int startRow, int startColumn, int endRow, int endColumn, int height)
        {
            try
            {
                if (startRow <= endRow)
                {
                    int numberOfColors = TExcelStyle._ArrayColorODD_EVEN.Count;
                    for (int row = startRow; row <= endRow; row++)
                    {
                        _worksheet.Row(row).Height = height;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override void SetWidthsColumns(int col, float width)
        {
            try
            {
                _worksheet.Column(col).Width = width;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override void AddCollectionValueString(List<string> Collection, int fromRow, int fromCol, int toRow, int toCol, string style_name, bool is_merge, TExcelColor backgroundColor, TExcelBorder border, TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, string formatNumber = "")
        {

            try
            {
                ExcelRange range = _worksheet.Cells[fromRow, fromCol, toRow, toCol];
                range.LoadFromCollection(Collection, false, OfficeOpenXml.Table.TableStyles.None).StyleName = style_name;
                range.Merge = is_merge;

                if (backgroundColor != TExcelColor.None)
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    if (backgroundColor == TExcelColor.Normal)
                        range.Style.Fill.BackgroundColor.SetColor(Color.White);
                    else if (backgroundColor == TExcelColor.Highlight)
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 190, 213, 239));
                    else if (backgroundColor == TExcelColor.Gray91)
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 217, 230, 246));
                    else if (backgroundColor == TExcelColor.Even)
                        range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 228, 229, 228));
                    else
                        range.Style.Fill.BackgroundColor.SetColor(backgroundColor.ToColor());
                }

                if (border.Style != TExcelBorderStyle.None)
                {
                    if (borderPosition == TExcelBorderPosition.Box)
                        range.Style.Border.BorderAround((OfficeOpenXml.Style.ExcelBorderStyle)border.Style, border.Color.ToColor());
                    else if (borderPosition == TExcelBorderPosition.Only_Right)
                    {
                        range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        range.Style.Border.Right.Color.SetColor(border.Color.ToColor());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddCollectionValueString(List<string> Collection, int fromRow, int fromCol, int toRow, int toCol, string style_name, bool is_merge, TExcelBorder border, TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, string formatNumber = "")
        {
            try
            {
                ExcelRange range = _worksheet.Cells[fromRow, toCol];
                range.LoadFromCollection(Collection, false, OfficeOpenXml.Table.TableStyles.None).StyleName = style_name;
                range.Merge = is_merge;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void AddCollectionValueInt(List<int> Collection, int fromRow, int fromCol, int toRow, int toCol, string style_name, bool is_merge, TExcelBorder border, TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, string formatNumber = "")
        {
            try
            {
                ExcelRange range = _worksheet.Cells[fromRow, toCol];
                range.LoadFromCollection(Collection, false, OfficeOpenXml.Table.TableStyles.None).StyleName = style_name;
                range.Merge = is_merge;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override void SetBackgroundColorRange(int startRow, int startColumn, int endRow, int endColumn)
        {
            try
            {
                if (startRow <= endRow)
                {
                    int numberOfColors = TExcelStyle._ArrayColorODD_EVEN.Count;
                    for (int row = startRow; row <= endRow; row++)
                    {
                        using (OfficeOpenXml.ExcelRange range = _worksheet.Cells[row, startColumn, row, endColumn])
                        {
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(TExcelStyle._ArrayColorODD_EVEN[(row - startRow) % numberOfColors]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void SetBorderStyle(int startRow, int startColumn, int endRow, int endColumn)
        {
            try
            {
                if (startRow <= endRow)
                {
                    int numberOfColors = TExcelStyle._ArrayStyleBorder.Count;
                    for (int row = startRow; row <= endRow; row++)
                    {
                        using (OfficeOpenXml.ExcelRange range = _worksheet.Cells[row, startColumn, row, endColumn])
                        {
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Border.BorderAround(TExcelStyle._ArrayStyleBorder[(row - startRow) % numberOfColors], Color.Black);
                        }
                    }

                    for (int col = startColumn; col <= endColumn; col++)
                    {
                        using (OfficeOpenXml.ExcelRange range = _worksheet.Cells[startRow, col, endRow, col])
                        {
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Border.BorderAround(TExcelStyle._ArrayStyleBorder[(col - startColumn) % numberOfColors], Color.Black);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
