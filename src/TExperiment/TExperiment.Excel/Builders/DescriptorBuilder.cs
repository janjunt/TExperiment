using System.Collections.Generic;
using NPOI.SS.UserModel;
using System.Text.RegularExpressions;
using TExperiment.Excel.Descriptor;
using TExperiment.Excel.Utility;
using TExperiment.Excel.Models;

namespace TExperiment.Excel.Builders
{
    public class DescriptorBuilder
    {
        private const string ParameterPattern = @"\{\{([^\}]+)\}\}";

        public IList<SheetDescriptor> Build(string filePath)
        {
            var result = new List<SheetDescriptor>();
            var workbook = NpoiUtility.LoadWorkbook(filePath);
            for (var sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
            {
                var sheet = workbook.GetSheetAt(sheetIndex);

                result.Add(CreateSheetDescriptor(sheet));
            }

            return result;
        }

        private SheetDescriptor CreateSheetDescriptor(ISheet sheet)
        {
            var sheetDescriptor = new SheetDescriptor
            {
                Name = sheet.SheetName
            };

            for (var rowIndex = sheet.FirstRowNum; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                {
                    continue;
                }

                for (var cellIndex = row.FirstCellNum; cellIndex < row.LastCellNum; cellIndex++)
                {
                    var cell = row.GetCell(cellIndex);
                    if (cell == null)
                    {
                        continue;
                    }

                    var cellValue = cell.ToString();
                    var matches = Regex.Matches(cellValue, ParameterPattern);
                    foreach (Match match in matches)
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
    }
}
