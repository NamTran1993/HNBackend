using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExcel.TExcelGlobal
{
    public static class TExcelEnumerable
    {
        public const string Arial_9f = "Arial_9f";
        public const string Arial_10f = "Arial_10f";
        public const string Arial_10f_Green = "Arial_10f_Green";
        public const string Arial_12f = "Arial_12f";
        public const string Arial_12f_Left = "Arial_12f_Left";
        public const string Arial_11f_Bold = "Arial_11f_Bold";
        public const string Arial_10f_Bold = "Arial_10f_Bold";
        public const string Arial_13f_Bold_Center = "Arial_13f_Bold_Center";
        public const string Arial_18f_Bold_Center = "Arial_18f_Bold_Center";
        public const string Norwester_18f_Bold_Center = "Norwester_18f_Bold_Center";
        public const string Arial_10f_Bold_Left = "Arial_10f_Bold_Left";
        public const string Arial_10f_Normal_Left = "Arial_10f_Normal_Left";
        public const string Arial_18f_Bold_Left = "Arial_18f_Bold_Left";

        public static ExcelVerticalAlignment ToVerticalAlignment(this ExcelVAlign input)
        {
            switch (input)
            {
                case ExcelVAlign.Top:
                    return ExcelVerticalAlignment.Top;
                case ExcelVAlign.Bottom:
                    return ExcelVerticalAlignment.Bottom;
                default:
                    return ExcelVerticalAlignment.Center;
            }
        }

        public static ExcelHorizontalAlignment ToHorizontalAlignment(this ExcelHAlign input)
        {
            switch (input)
            {
                case ExcelHAlign.Left:
                    return ExcelHorizontalAlignment.Left;
                case ExcelHAlign.Right:
                    return ExcelHorizontalAlignment.Right;
                default:
                    return ExcelHorizontalAlignment.Center;
            }
        }

        public static void CreateStyle(this ExcelWorksheet workSheet, TExcelStyle eStyle)
        {
            try
            {
                if (eStyle != null)
                {
                    ExcelNamedStyleXml style = workSheet.Workbook.Styles.CreateNamedStyle(eStyle.StyleName);
                    if (eStyle.FontStyle == TExcelFontStyle.Bold)
                        style.Style.Font.Bold = true;
                    else if (eStyle.FontStyle == TExcelFontStyle.Italic)
                        style.Style.Font.Italic = true;
                    else if (eStyle.FontStyle == TExcelFontStyle.BoldItalic)
                    {
                        style.Style.Font.Bold = true;
                        style.Style.Font.Italic = true;
                    }
                    style.Style.Font.Size = eStyle.FontSize;
                    style.Style.Font.Name = eStyle.FontName;
                    style.Style.VerticalAlignment = ToVerticalAlignment(eStyle.VerticalAlignment);
                    style.Style.HorizontalAlignment = ToHorizontalAlignment(eStyle.HorizontalAlignment);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateDefaultStyles(this ExcelWorksheet workSheet)
        {
            List<TExcelStyle> styles = TExcelStyle.GetDefaultStyles();
            styles.ForEach(ite => workSheet.CreateStyle(ite));
        }

        public static void ReleaseDefaultStyles()
        {
            TExcelStyle.ReleaseDefaultStyles();
        }

        public static Color ToColor(this TExcelColor color)
        {
            switch (color)
            {
                case TExcelColor.Blue:
                    return Color.Blue;
                case TExcelColor.DarkGray:
                    return Color.DarkGray;
                case TExcelColor.Gray:
                    return Color.Gray;
                case TExcelColor.Green:
                    return Color.Green;
                case TExcelColor.Orange:
                    return Color.Orange;
                case TExcelColor.Red:
                    return Color.Red;
                case TExcelColor.White:
                    return Color.White;
                case TExcelColor.Yellow:
                    return Color.Yellow;
                default: return Color.Black;
            }
        }

        public static Color ToColor(int red, int green, int blue)
        {
            return Color.FromArgb(red, green, blue);
        }
    }
}
