namespace Bars.Gkh.DomainService.Permission
{
    using Bars.B4;

    /// <summary>
    /// Сервис настройки прав доступа на форме
    /// </summary>
    public interface IFormPermssionService
    {
        /// <summary>
        /// Получить права доступа для формы
        /// </summary>
        IDataResult GetFormPermissions(BaseParams baseParams);

        /// <summary>
        /// Получить список типов на форме
        /// </summary>
        IDataResult GetEntityTypes(BaseParams baseParams);
    }
}