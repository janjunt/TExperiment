using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using TExperiment.Excel.Descriptor;

namespace TExperiment.Excel.Renders
{
    /// <summary>
    /// 模板渲染器
    /// </summary>
    public class TemplateRender:Render
    {
        private readonly string _templateFileName;
        private readonly WorkbookDescriptor _workbookDescriptor;

        public TemplateRender(string fileName, WorkbookDescriptor workbookDescriptor)
        {
            _templateFileName = fileName;
            _workbookDescriptor = workbookDescriptor;
            foreach(var sheetDescriptor in workbookDescriptor.Sheets)
            {
                Childs.Add(new SheetRender(sheetDescriptor));
            }
        }
    }
}
