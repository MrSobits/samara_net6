namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис для "Предоставленные документы акта без взаимодействия"
    /// </summary>
    public interface IActIsolatedProvidedDocService
    {
        /// <summary>
        /// Добавить Предоставленные документы акта без взаимодействия
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult AddProvidedDocs(BaseParams baseParams);
    }
}