using TExperiment.Excel.Models;

namespace TExperiment.Excel.Descriptor
{
    /// <summary>
    /// 参数描述
    /// </summary>
    public class ParameterDescriptor
    {
        /// <summary>
        /// 描述值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 所在单元格
        /// </summary>
        public CellLocation Cell { get; set; }
    }
}
