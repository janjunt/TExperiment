using System.Collections.Generic;
using System.IO;
using TExperiment.Excel.Builders;
using TExperiment.Excel.Renders;
using TExperiment.Excel.Utility;

namespace TExperiment.Excel.Exports
{
    /// <summary>
    /// Excel模板方式导出
    /// </summary>
    public class TemplateExport:ExcelExport
    {
        private readonly string _templateFile;
        private readonly IList<IRender> _renders;

        public TemplateExport(string templateFile)
        {
            _templateFile = templateFile;
            var sheetDescriptors = new DescriptorBuilder().Build(_templateFile);
            _renders = new RenderBuilder().Build(sheetDescriptors);
        }

        public override void ExportToStream<T>(Stream stream, T data)
        {
            var workBook = NpoiUtility.LoadWorkbook(_templateFile);
            foreach (var render in _renders)
            {
                var context = new RenderContext
                {
                    Sheet = workBook.GetSheet(render.Name),
                    Data = data,
                };

                render.Render(context);
            }
        }
    }
}
