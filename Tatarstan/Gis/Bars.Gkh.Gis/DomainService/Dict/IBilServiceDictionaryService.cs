namespace Bars.Gkh.Gis.DomainService.Dict
{
    using B4;

    /// <summary>
    /// Сервис для работы со справочником услуг биллинга
    /// </summary>
    public interface IBilServiceDictionaryService
    {
        /// <summary>
        /// Получить список дополнительных услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список дополнительных услуг</returns>
        IDataResult ListAdditionalService(BaseParams baseParams);

        /// <summary>
        /// Получить список коммунальных услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список коммунальных услуг</returns>
        IDataResult ListCommunalService(BaseParams baseParams);

        /// <summary>
        /// Получить список работ и услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список работ и услуг</returns>
        IDataResult ListServiceWork(BaseParams baseParams);
    }
}
