using System.Collections.Generic;

namespace TExperiment.Excel.Descriptor
{
    /// <summary>
    /// 工作薄描述
    /// </summary>
    public class WorkbookDescriptor
    {
        /// <summary>
        /// sheet描述集合
        /// </summary>
        public IList<SheetDescriptor> Sheets { get; set; } = new List<SheetDescriptor>();
    }
}
