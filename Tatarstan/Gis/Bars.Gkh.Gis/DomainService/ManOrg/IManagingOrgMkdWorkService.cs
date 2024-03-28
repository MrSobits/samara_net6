namespace Bars.Gkh.Gis.DomainService.ManOrg
{
    using B4;

    /// <summary>
    /// Интерфейс для ManagingOrgMkdWorkService
    /// </summary>
    public interface IManagingOrgMkdWorkService
    {
        /// <summary>
        /// Добавить работы по МКД
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        IDataResult AddMkdWorks(BaseParams baseParams);
    }
}
