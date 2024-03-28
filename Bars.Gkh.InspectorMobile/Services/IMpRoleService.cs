using Bars.B4;

namespace Bars.Gkh.InspectorMobile.Services
{
    /// <summary>
    /// Сервис для работы с ролями мобильного приложения
    /// </summary>
    public interface IMpRoleService
    {
        /// <summary>
        /// Добавить роли
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат</returns>
        IDataResult AddRoles(BaseParams baseParams);

        /// <summary>
        /// Получить список ролей приложения
        /// </summary>
        /// /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список ролей</returns>
        IDataResult GetRoles(BaseParams baseParams);
    }
}