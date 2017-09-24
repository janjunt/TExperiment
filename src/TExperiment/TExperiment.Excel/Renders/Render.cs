using System.Collections.Generic;

namespace TExperiment.Excel.Renders
{
    public abstract class Render:IRender
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public virtual IList<Render> Childs { get; set; } = new List<Render>();
    }
}
