namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для работы с пакетами
    /// </summary>
    public interface IPackageService
    {
        /// <summary>
        /// Получить xml данные пакета форматированные для просмотра
        /// либо неформатированные для подписи
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие 
        /// идентификатор пакета, 
        /// тип пакета,
        /// признак: для предпросмотра
        /// признак: подписанные/неподписанные данные</param>
        /// <returns>xml данные пакета</returns>
        IDataResult GetPackageXmlData(BaseParams baseParams);

        /// <summary>
        /// Сохранить подписанную xml
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// идентификатор пакета, 
        /// тип пакета,
        /// подписанные данные
        /// </param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult SaveSignedData(BaseParams baseParams);
    }
}
