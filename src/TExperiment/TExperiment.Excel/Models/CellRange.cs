namespace TExperiment.Excel.Models
{
    /// <summary>
    /// 单元格范围
    /// </summary>
    public class CellRange
    {
        /// <summary>
        /// 起始单元格
        /// </summary>
        public CellLocation StartCell { get; set; }
        /// <summary>
        /// 终止单元格
        /// </summary>
        public CellLocation EndCell { get; set; }
    }

    public static class CellRangeExtensions
    {
        public static bool HasCross(this CellRange source, CellRange other)
        {
            return !(source.StartCell.ColumnIndex > other.EndCell.ColumnIndex ||
                     source.EndCell.ColumnIndex < other.StartCell.ColumnIndex ||
                     source.StartCell.RowIndex > other.EndCell.RowIndex ||
                     source.EndCell.RowIndex < other.StartCell.RowIndex);
        }

        public static bool Include(this CellRange range, CellLocation cell)
        {
            return cell.ColumnIndex >= range.StartCell.ColumnIndex &&
                   cell.ColumnIndex <= range.EndCell.ColumnIndex &&
                   cell.RowIndex >= range.StartCell.RowIndex &&
                   cell.RowIndex <= range.EndCell.RowIndex;
        }
    }
}
