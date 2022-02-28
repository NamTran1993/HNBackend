using HNBackend.Module.TExcel.TExcelGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExcel
{
    public abstract class IExcel
    {
        public abstract void InitDefaultStyles();
        public abstract void ReleaseDefaultStyles();
        public abstract void NewSheet(string sheetName, string copySheet = "");
        public abstract void AddValue(int row, int col, object value, bool isNumber = false);
        public abstract void AddValue(string range, object value);
        public abstract void AddValue(int fromRow, int fromColumn, int toRow, int toColumn, object value, string position = "", bool isNumber = false);
        public abstract void AddValue(int fromRow, int fromColumn, int toRow, int toColumn, object value, TExcelBorder border, TExcelColor backgroundColor, Color font, string styleName = "", TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, bool bFreezes = false, string position = "", bool isNumber = false);
        public abstract void AddImage(Image image, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder border, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float marginLeft = 50.0f);
        public abstract void AddImage(List<Image> image, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder border, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float marginLeft = 50.0f);
        public abstract void AddImage(byte[] imgb, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder border, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float marginLeft = 50);
        public abstract void AddImage(List<byte[]> imgb, int fromRow, int fromColumn, int toRow, int toColumn, TExcelBorder border, Size imageSize, Color backgroundColor, TExcelBorder borderCell, TExcelBorderPosition position = TExcelBorderPosition.Box, float marginLeft = 50);

        public abstract bool Save(string outputPath);
        public abstract bool Save(string outputPath, bool isReleaseImage);
        public abstract byte[] ToBinary();
        public abstract void SetWidths(params double[] widths);
        public abstract void SetRowHeights(Dictionary<int, double> height);
        public abstract void SetHeightRanges(int startRow, int startColumn, int endRow, int endColumn, int height);
        public abstract void SetWidthsColumns(int col, float width);
        public abstract void AddCollectionValueString(List<string> Collection, int fromRow, int fromCol, int toRow, int toCol, string style_name, bool is_merge, TExcelColor backgroundColor, TExcelBorder border, TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, string formatNumber = "");
        public abstract void AddCollectionValueString(List<string> Collection, int fromRow, int fromCol, int toRow, int toCol, string style_name, bool is_merge, TExcelBorder border, TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, string formatNumber = "");
        public abstract void AddCollectionValueInt(List<int> Collection, int fromRow, int fromCol, int toRow, int toCol, string style_name, bool is_merge, TExcelBorder border, TExcelBorderPosition borderPosition = TExcelBorderPosition.Box, string formatNumber = "");
        public abstract void SetBackgroundColorRange(int startRow, int startColumn, int endRow, int endColumn);
        public abstract void SetBorderStyle(int startRow, int startColumn, int endRow, int endColumn);
    }
}
