namespace Bars.Gkh.Modules.ClaimWork
{
    using System.IO;

    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Расширение ICodedReport новыми полями
    /// </summary>
    public interface IClaimWorkCodedReport : ICodedReport
    {
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Идентификатор документа для печати
        /// </summary>
        string DocumentId { get; set; }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        string CodeForm { get; }

        /// <summary>
        /// Название выходного файла
        /// </summary>
        string OutputFileName { get; set; }

        /// <summary>
        /// Тип документа ПиР
        /// </summary>
        ClaimWorkDocumentType DocumentType { get; }

        /// <summary>
        /// Информация
        /// </summary>
        ClaimWorkReportInfo ReportInfo { get; set; }

        /// <summary>
        /// Stream сгенерированной печатной формы
        /// </summary>
        Stream ReportFileStream { get; set; }

        /// <summary>
        /// Генерация документа для выгрузки
        /// </summary>
        void Generate();
    }
}