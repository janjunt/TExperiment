using System.IO;

namespace TExperiment.Excel.Exports
{
    /// <summary>
    /// Excel导出
    /// </summary>
    public abstract class ExcelExport
    {
        /// <summary>
        /// 将数据转换成Excel文件格式数据，并导出到数据流中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="stream">数据流</param>
        /// <param name="data">数据</param>
        public abstract void ExportToStream<T>(Stream stream, T data) where T : class;

        /// <summary>
        /// 根据模板文件，将数据转换成Excel文件格式数据，并导出到缓冲区。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="templateFile">模板文件</param>
        /// <param name="data">数据</param>
        /// <returns>缓冲区</returns>
        public static byte[] ExportToBuffer<T>(string templateFile, T data) where T : class
        {
            using (var stream = new MemoryStream())
            {
                var export = new TemplateExport(templateFile);
                export.ExportToStream(stream, data);

                return stream.ToArray();
            }
        }
    }
}
