namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Сервис фильтрации прав доступа
    /// </summary>
    public interface IFilterPermissionService
    {
        /// <summary>
        /// Получить доступные для настройки идентификаторы прав доступа
        /// </summary>
        /// <param name="roleId">Идентификатор роли <see cref="Role.Id"/></param>
        /// <returns>Набор идентификаторов <see cref="RolePermission.PermissionId"/></returns>
        ISet<string> GetAllowRolePermissionNodes(long roleId);

        /// <summary>
        /// Получить доступные для настройки идентификаторы прав доступа по статусам
        /// </summary>
        /// <param name="roleId">Идентификатор роли <see cref="Role.Id"/></param>
        /// <param name="stateId">Идентификатор статуса <see cref="State.Id"/></param>
        /// <returns>Набор идентификаторов <see cref="StateRolePermission.PermissionId"/></returns>
        ISet<string> GetAllowStatePermissionNodes(long roleId, long stateId);

        /// <summary>
        /// Получить доступные для настройки прав доступа идентификаторы статусных объектов
        /// </summary>
        /// <param name="roleId">Идентификатор роли <see cref="Role.Id"/></param>
        /// <returns>Набор идентификаторов <see cref="StatefulEntityInfo.TypeId"/></returns>
        ISet<string> GetAllowStatefulEntities(long roleId);
    }
}