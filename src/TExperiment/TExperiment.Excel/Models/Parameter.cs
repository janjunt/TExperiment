using System.Collections.Generic;

namespace TExperiment.Excel.Models
{
    /// <summary>
    /// 参数
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所在单元格
        /// </summary>
        public CellLocation Cell { get; set; }
        /// <summary>
        /// 过滤器
        /// </summary>
        public IList<FilterParameter> Filters { get; set; }
    }
}
