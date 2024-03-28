namespace Bars.GisIntegration.Smev.DomainService
{
    using Bars.B4;

    public interface IBaseIntegrationService
    {
        /// <summary>
        /// Получить данные для представления протокола
        /// </summary>
        IDataResult GetTriggerProtocolView(BaseParams baseParams);

        /// <summary>
        /// Получить пакета
        /// </summary>
        IDataResult GetXmlData(BaseParams baseParams);

        /// <summary>
        /// Получить файл лога
        /// </summary>
        IDataResult GetLogFile(BaseParams baseParams);
    }
}