using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExcel.TExcelGlobal
{
    #region Define Enum
    public enum TExcelBorderPosition
    {
        None,
        Top,
        Left,
        Right,
        Bottom,
        Box,

        Only_Right
    }

    public enum TExcelBorderStyle : int
    {
        None = 0,
        Hair = 1,
        Dotted = 2,
        DashDot = 3,
        Thin = 4,
        DashDotDot = 5,
        Dashed = 6,
        MediumDashDotDot = 7,
        MediumDashed = 8,
        MediumDashDot = 9,
        Thick = 10,
        Medium = 11,
        Double = 12
    }

    public enum TExcelColor
    {
        Black,
        White,
        Blue,
        Green,
        Red,
        Yellow,
        Gray,
        DarkGray,
        Orange,
        Header,
        DaysInMonth,
        IN_OUT,
        Good,
        Normal,
        Highlight,
        ColorHeader,
        LightGreen,
        BlueGray,
        Gray91,
        ColorHeaderTable,
        Odd,
        Even,
        None,
    }

    public enum TExcelFontStyle : int
    {
        Normal = 0,
        Bold = 1,
        Italic = 2,
        BoldItalic = 3
    }

    public enum ExcelVAlign
    {
        Top,
        Center,
        Bottom
    }

    public enum ExcelHAlign
    {
        Left,
        Center,
        Right
    }
    #endregion
}
