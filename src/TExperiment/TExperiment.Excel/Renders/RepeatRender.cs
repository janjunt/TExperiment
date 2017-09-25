using System.Collections;
using TExperiment.Excel.Builders;
using TExperiment.Excel.Models;

namespace TExperiment.Excel.Renders
{
    public class RepeatRender: RenderBase
    {
        /// <summary>
        /// 重复范围
        /// </summary>
        public CellRange Range { get; set; }

        /// <summary>
        /// 重复类型
        /// </summary>
        public RepeatType Repeat { get; set; }

        public override void Render(RenderContext context)
        {
            var list = context.GetValue(Name) as IEnumerable;
            foreach (var item in list)
            {
                base.Render(context.CreateChildContext(item));
            }
        }
    }
}
