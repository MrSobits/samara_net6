namespace Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export
{
    using System.IO;
    using B4;
    using B4.Utils;

    /// <summary>
    /// Формат выдачи сериализованных данных экспорта
    /// </summary>
    public class ExportOutput
    {
        /// <summary>
        /// Данные
        /// </summary>
        public Stream Data { get; set; }

        /// <summary>
        /// Название выходного файла
        /// </summary>
        public string OutputName { get; set; }
    }

    public class ExportOutputResult : IDataResult<ExportOutput>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ExportOutput Data { get; set; }

        object IDataResult.Data
        {
            get { return Data; }
            set { Data = value.As<ExportOutput>(); }
        }

        public ExportOutputResult(string message = null)
        {
            Message = message;
        }
    }
}