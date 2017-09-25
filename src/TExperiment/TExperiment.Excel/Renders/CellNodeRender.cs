using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TExperiment.Excel.Builders;
using TExperiment.Excel.Models;

namespace TExperiment.Excel.Renders
{
    public class CellNodeRender:CellRender
    {
        public IList<FilterCondition> Filters { get; set; } = new List<FilterCondition>();

        public override void Render(RenderContext context)
        {
            var data = context.GetValue(Name);
            var list = data as IEnumerable;
            if (list != null)
            {
                var enumType = typeof (Enumerable);
                var method = enumType.GetMethod("Where", new Type[] { list.GetType(),});
            }
            else
            {
                base.Render(context.CreateChildContext(data));
            }
        }
    }
}
