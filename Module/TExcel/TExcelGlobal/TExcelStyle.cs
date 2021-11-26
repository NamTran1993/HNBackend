using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExcel.TExcelGlobal
{
    public class TExcelStyle
    {
        private static string _font_Arial = "Calibri";
        private static string _font_Norwester = "Norwester";

        public string StyleName { get; set; }
        public float FontSize { get; set; }
        public string FontName { get; set; }
        public TExcelColor FontColor { get; set; }
        public TExcelFontStyle FontStyle { get; set; }
        public ExcelVAlign VerticalAlignment { get; set; }
        public ExcelHAlign HorizontalAlignment { get; set; }

        // Define Font
        public static TExcelStyle Arial_9f = null;
        public static TExcelStyle Arial_10f = null;
        public static TExcelStyle Arial_12f_Left = null;
        public static TExcelStyle Arial_12f = null;
        public static TExcelStyle Arial_11f_Bold = null;
        public static TExcelStyle Arial_10f_Bold = null;
        public static TExcelStyle Arial_13f_Bold_Center = null;
        public static TExcelStyle Arial_18f_Bold_Center = null;
        public static TExcelStyle Norwester_18f_Bold_Center = null;
        public static TExcelStyle Arial_10f_Bold_Left = null;
        public static TExcelStyle Arial_10f_Normal_Left = null;
        public static TExcelStyle Arial_18f_Bold_Left = null;

        private static System.Drawing.Color _Color_Even = System.Drawing.Color.FromArgb(0, 228, 229, 228);
        public static List<System.Drawing.Color> _ArrayColorODD_EVEN = new List<System.Drawing.Color>()
        {
            System.Drawing.Color.White,
            _Color_Even
        };

        public static List<OfficeOpenXml.Style.ExcelBorderStyle> _ArrayStyleBorder = new List<OfficeOpenXml.Style.ExcelBorderStyle>()
        {
           OfficeOpenXml.Style.ExcelBorderStyle.Thin
        };

        public TExcelStyle()
        {
        }

        public TExcelStyle(string styleName, TExcelColor fontColor, TExcelFontStyle fontStyle, float fontSize, string fontName,
            ExcelVAlign verticalAlignment, ExcelHAlign horizontalAlignment)
        {
            StyleName = styleName;
            FontSize = fontSize;
            FontName = fontName;
            FontColor = fontColor;
            FontStyle = fontStyle;
            VerticalAlignment = verticalAlignment;
            HorizontalAlignment = horizontalAlignment;
        }

        public static List<TExcelStyle> GetDefaultStyles()
        {
            try
            {
                Arial_9f = new TExcelStyle("Arial_9f", TExcelColor.Black, TExcelFontStyle.Bold, 9f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Center);
                Arial_10f = new TExcelStyle("Arial_10f", TExcelColor.Black, TExcelFontStyle.Normal, 10f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Center);
                Arial_12f = new TExcelStyle("Arial_12f", TExcelColor.Black, TExcelFontStyle.Bold, 15f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Center);
                Arial_11f_Bold = new TExcelStyle("Arial_11f_Bold", TExcelColor.Black, TExcelFontStyle.Bold, 11f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Center);
                Arial_10f_Bold = new TExcelStyle("Arial_10f_Bold", TExcelColor.Black, TExcelFontStyle.Bold, 10f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Center);
                Arial_13f_Bold_Center = new TExcelStyle("Arial_13f_Bold_Center", TExcelColor.Black, TExcelFontStyle.Bold, 13f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Center);
                Arial_18f_Bold_Center = new TExcelStyle("Arial_18f_Bold_Center", TExcelColor.Black, TExcelFontStyle.Bold, 18f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Center);
                Norwester_18f_Bold_Center = new TExcelStyle("Norwester_18f_Bold_Center", TExcelColor.Black, TExcelFontStyle.Bold, 13f, _font_Norwester, ExcelVAlign.Center, ExcelHAlign.Center);
                Arial_10f_Bold_Left = new TExcelStyle("Arial_10f_Bold_Left", TExcelColor.Black, TExcelFontStyle.Bold, 10f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Left);
                Arial_10f_Normal_Left = new TExcelStyle("Arial_10f_Normal_Left", TExcelColor.Black, TExcelFontStyle.Normal, 10f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Left);
                Arial_18f_Bold_Left = new TExcelStyle("Arial_18f_Bold_Left", TExcelColor.Black, TExcelFontStyle.Bold, 18f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Left);
                Arial_12f_Left = new TExcelStyle("Arial_12f_Left", TExcelColor.Black, TExcelFontStyle.Bold, 15f, _font_Arial, ExcelVAlign.Center, ExcelHAlign.Left);

                List<TExcelStyle> result = typeof(TExcelStyle).GetFields(BindingFlags.Public | BindingFlags.Static)
                            .Select(ite => ite.GetValue(null) as TExcelStyle)
                            .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ReleaseDefaultStyles()
        {
            try
            {
                Arial_9f = null;
                Arial_10f = null;
                Arial_12f = null;
                Arial_11f_Bold = null;
                Arial_10f_Bold = null;
                Arial_13f_Bold_Center = null;
                Norwester_18f_Bold_Center = null;
                Arial_10f_Bold_Left = null;
                Arial_10f_Normal_Left = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
