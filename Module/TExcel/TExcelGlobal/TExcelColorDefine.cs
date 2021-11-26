using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExcel.TExcelGlobal
{
    public class TExcelColorDefine
    {
        public static Color Header = Color.FromArgb(0, 32, 55, 100);
        public static Color Header_TOTAL = Color.FromArgb(0, 137, 73, 120);
        public static Color DaysInMonth = Color.FromArgb(0, 91, 155, 213);
        public static Color AM_PM = Color.FromArgb(0, 255, 242, 204);
        public static Color Extra = Color.FromArgb(0, 226, 239, 218);
        public static Color SAT = Color.FromArgb(0, 255, 51, 51);
        public static Color SUN = Color.FromArgb(0, 255, 51, 51);
        public static Color Good = Color.FromArgb(100, 211, 211, 211);
        public static Color Normal = Color.White;
        public static Color Highlight = Color.FromArgb(0, 237, 125, 49);
        public static Color IN_OUT = Color.LightBlue;

        public static Color Header_Note = Color.FromArgb(0, 221, 235, 247);

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
    }
}
