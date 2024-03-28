namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using Entities;
    using Enums;

    /// <summary>
    /// Данный интерфейс возвращает данные для отчетов
    /// </summary>
    public interface IClaimWorkService
    {
        /// <summary>
        /// Тип документа ПиР
        /// </summary>
        ClaimWorkTypeBase TypeBase { get; }

        /// <summary>
        /// Метод получения информации для отчета
        /// </summary>
        /// <param name="id"><see cref="BaseClaimWork"/></param>
        ClaimWorkReportInfo ReportInfoByClaimwork(long id);

        /// <summary>
        /// Метод получения информации для отчета
        /// </summary>
        ClaimWorkReportInfo ReportInfoByClaimworkDetail(long id);
    }
}