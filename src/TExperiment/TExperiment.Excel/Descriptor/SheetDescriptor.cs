using System.Collections.Generic;

namespace TExperiment.Excel.Descriptor
{
    /// <summary>
    /// sheet描述
    /// </summary>
    public class SheetDescriptor
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数描述集合
        /// </summary>
        public IList<ParameterDescriptor> ParameterDescriptors { get; set; } = new List<ParameterDescriptor>();
    }
}
