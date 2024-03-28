namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Права доступа на узлы дерева настроек <see cref="RolePermission"/> для локального администратора
    /// </summary>
    public class LocalAdminRolePermission : PersistentObject
    {
        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Код права доступа
        /// </summary>
        public virtual string PermissionId { get; set; }
    }
}