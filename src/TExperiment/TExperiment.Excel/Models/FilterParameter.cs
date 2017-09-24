namespace TExperiment.Excel.Models
{
    /// <summary>
    /// 过滤参数
    /// </summary>
    public class FilterParameter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 依赖参数
        /// </summary>
        public Parameter Dependency { get; set; }
    }
}
