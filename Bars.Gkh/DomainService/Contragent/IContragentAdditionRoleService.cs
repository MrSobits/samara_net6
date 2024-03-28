namespace Bars.Gkh.DomainService
{
    using B4;

    /// <summary>
    /// Интерфейс Сервиса для доп роли контагента
    /// </summary>
    public interface IContragentAdditionRoleService
    {
        /// <summary>
        /// Добавить дополнительные роли контагенту
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        IDataResult AddAdditionRole(BaseParams baseParams);
    }
}