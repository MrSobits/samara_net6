namespace Bars.GisIntegration.Smev.DomainService
{
    using Bars.B4;

    public interface IErknmIntegrationService : IBaseIntegrationService
    {
        /// <summary>
        /// Получить список документов для реестра интеграции ЕРКНМ
        /// </summary>
        IDataResult DocumentList(BaseParams baseParams);
        
        /// <summary>
        /// Отправить в ЕРКНМ
        /// </summary>
        IDataResult SendToErknm(BaseParams baseParams);

        /// <summary>
        /// Проверка перед отправкой в ЕРКНМ
        /// </summary>
        IDataResult BeforeSendCheck(BaseParams baseParams);

        /// <summary>
        /// Выгрузить в Excel
        /// </summary>
        ReportStreamResult ExcelFileExport(BaseParams baseParams);
    }
}