using System.Collections.Generic;
using TExperiment.Excel.Builders;

namespace TExperiment.Excel.Renders
{
    public abstract class RenderBase:IRender
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public IList<IRender> Childs { get; set; } = new List<IRender>();

        public virtual void Render(RenderContext context)
        {
            foreach (var childRender in Childs)
            {
                childRender.Render(context);
            }
        }
    }
}
