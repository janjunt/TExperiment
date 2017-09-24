using TExperiment.Excel.Models;

namespace TExperiment.Excel.Renders
{
    public class RepeatRender:Render
    {
        /// <summary>
        /// 起始单元格
        /// </summary>
        public CellLocation StartCell { get; set; }
        /// <summary>
        /// 终止单元格
        /// </summary>
        public CellLocation EndCell { get; set; }
        /// <summary>
        /// 重复类型
        /// </summary>
        public RepeatType Repeat { get; set; }

        public RepeatRender()
        {
        }
    }
}
