namespace Bars.GkhCr.DomainService
{
    using B4;

    /// <summary>
    /// Интерфейс сервиса протоколов Объекта КР
    /// </summary>
    public interface IProtocolService
    {
        /// <summary>
        /// Вернуть актуальные даты
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetDates(BaseParams baseParams);

        /// <summary>
        /// Вернуть актуальные типы документов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetTypeDocumentCr(BaseParams baseParams);

        /// <summary>
        /// Добавить типы работ
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
		IDataResult AddTypeWorks(BaseParams baseParams);
    }
}
