namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис экспорта обращений граждан
    /// </summary>
    public interface IExportSuggestionService
    {
        /// <summary>
        /// Экспорт обращений граждан в реестр обращений ГЖИ
        /// </summary>
        /// <param name="baseParams">
        /// The base Params.
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        IDataResult ExportCitizenSuggestionToGji(BaseParams baseParams);

        /// <summary>
        /// Проверяет наличие созданного обращения
        /// </summary>
        /// <param name="baseParams">
        /// The base Params.
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        IDataResult CheckSuggestionExists(BaseParams baseParams);
    }
}