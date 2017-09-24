using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using TExperiment.Excel.Descriptor;
using TExperiment.Excel.Models;
using TExperiment.Excel.Renders;

namespace TExperiment.Excel.Loaders
{
    /// <summary>
    /// 模板渲染加载器
    /// </summary>
    public class TemplateRenderLoader:IRenderLoader
    {
        private const string ParameterPattern = @"\{\{([^\}]+)\}\}";

        public TemplateRender Load(string fileName)
        {
            var workbookDescriptor = CreateWorkbookDescriptor(fileName);

            return new TemplateRender(fileName, workbookDescriptor);
        }

        /// <summary>
        /// 创建工作表描述
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>工作表描述</returns>
        private WorkbookDescriptor CreateWorkbookDescriptor(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var workbook = Load(stream);
                var workbookDescriptor = new WorkbookDescriptor();
                for (var sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                {
                    var sheet = workbook.GetSheetAt(sheetIndex);

                    workbookDescriptor.Sheets.Add(CreateSheetDescriptor(sheet));
                }

                return workbookDescriptor;
            }

        }

        private SheetDescriptor CreateSheetDescriptor(ISheet sheet)
        {
            var sheetDescriptor = new SheetDescriptor
            {
                Name = sheet.SheetName
            };

            for(var rowIndex = sheet.FirstRowNum; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                {
                    continue;
                }

                for(var cellIndex = row.FirstCellNum; cellIndex < row.LastCellNum; cellIndex++)
                {
                    var cell = row.GetCell(cellIndex);
                    if (cell == null)
                    {
                        continue;
                    }

                    var cellValue = cell.ToString();
                    var matches = Regex.Matches(cellValue, ParameterPattern);
                    foreach(Match match in matches)
                    {
                        if (match.Success)
                        {
                            sheetDescriptor.ParameterDescriptors.Add(new ParameterDescriptor
                            {
                                Value = match.Groups[1].Value,
                                Cell = new CellLocation
                                {
                                    ColumnIndex = cellIndex,
                                    RowIndex = rowIndex
                                }
                            });
                        }
                    }
                }
            }

            return sheetDescriptor;
        }


        private IWorkbook Load(Stream stream)
        {
            IWorkbook workbook = null;
            try
            {
                workbook = new XSSFWorkbook(stream);
            }
            catch
            {
                workbook = new HSSFWorkbook(stream);
            }

            return workbook;
        }
    }
}
