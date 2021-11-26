using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExcel.TExcelGlobal
{
    public class TExcelBorder
    {
        public TExcelBorderStyle Style { get; set; }
        public TExcelColor Color { get; set; }
        public TExcelBorder()
        {
            Style = TExcelBorderStyle.None;
            Color = TExcelColor.Black;
        }
        public TExcelBorder(TExcelBorderStyle style, TExcelColor color)
        {
            Style = style;
            Color = color;
        }

        static TExcelBorder _none = new TExcelBorder();
        public static TExcelBorder None { get { return TExcelBorder._none; } }

        static TExcelBorder _thinBlack = new TExcelBorder(TExcelBorderStyle.Thin, TExcelColor.Black);
        public static TExcelBorder ThinBlack { get { return _thinBlack; } }

        static TExcelBorder _thinRed = new TExcelBorder(TExcelBorderStyle.Thin, TExcelColor.Red);
        public static TExcelBorder ThinRed { get { return _thinRed; } }

        static TExcelBorder _thinGreen = new TExcelBorder(TExcelBorderStyle.Thin, TExcelColor.Green);
        public static TExcelBorder ThinGreen { get { return _thinGreen; } }

        static TExcelBorder _thinOrange = new TExcelBorder(TExcelBorderStyle.Thin, TExcelColor.Orange);
        public static TExcelBorder ThinOrange { get { return _thinOrange; } }
    }
}
