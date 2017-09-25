using System.Collections.Generic;
using TExperiment.Excel.Builders;

namespace TExperiment.Excel.Renders
{
    public interface IRender
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }

        IList<IRender> Childs { get; set; }

        void Render(RenderContext context);
    }

}
