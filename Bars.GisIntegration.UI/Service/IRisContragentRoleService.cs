namespace Bars.GisIntegration.UI.Service
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса RisContragentRole
    /// </summary>
    public interface IRisContragentRoleService
    {
        /// <summary>
        /// Добавить контрагенту роли ГИС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult AddContragentRoles(BaseParams baseParams);
    }
}
