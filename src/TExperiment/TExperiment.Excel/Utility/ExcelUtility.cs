using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace TExperiment.Excel.Utility
{
    public static class ExcelUtility
    {
        public static IWorkbook Load(string fileName)
        {
            var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            return Load(stream);
        }

        public static IWorkbook Load(Stream stream)
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
