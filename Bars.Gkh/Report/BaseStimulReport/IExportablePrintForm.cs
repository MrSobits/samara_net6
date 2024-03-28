namespace Bars.Gkh.StimulReport
{
    using System.Collections.Generic;

    /// <summary>Интерфейс экспортируемого отчета</summary>
    public interface IExportablePrintForm
    {
        /// <summary>Получить список доступных форматов для печати</summary>
        /// <returns></returns>
        IList<PrintFormExportFormat> GetExportFormats();

        /// <summary>Установить формат печати</summary>
        /// <param name="format">Формат</param>
        void SetExportFormat(PrintFormExportFormat format); 
    }
}