namespace Bars.Gkh.DomainService.Dict
{
    using Bars.B4;
    using Entities.Dicts;

    /// <summary>
    /// Интерфейс сервиса для работы с сущностью <see cref="BuilderDocumentType"/> - тип документа подрядных организаций
    /// </summary>
    public interface IBuilderDocumentTypeService
    {
        /// <summary>
        /// Вернуть все типы документов без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}