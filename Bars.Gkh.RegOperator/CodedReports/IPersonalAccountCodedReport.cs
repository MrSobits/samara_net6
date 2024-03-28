namespace Bars.Gkh.RegOperator.CodedReports
{
    using System.IO;
    using Bars.B4.Modules.Analytics.Reports;

    /// <summary>
    /// Расширение ICodedReport новыми полями
    /// </summary>
    public interface IPersonalAccountCodedReport: ICodedReport
    {
        /// <summary>
        /// Id лс
        /// </summary>
        long AccountId { get; set; }

        /// <summary>
        /// Название выходного файла
        /// </summary>
        string OutputFileName { get; set; }

        /// <summary>
        /// Stream сгенерированной печатной формы
        /// </summary>
        Stream ReportFileStream { get; set; }

        /// <summary>
        /// Генерация документа для выгрузки
        /// </summary>
        void Generate();

        /// <summary>
        /// Генерация документа для выгрузки
        /// </summary>
        void GenerateFromService();
    }
}
