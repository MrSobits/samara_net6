namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Права доступа на узлы дерева настроек <see cref="StateRolePermission"/> для локального администратора
    /// </summary>
    public class LocalAdminStatePermission : PersistentObject
    {
        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Код права доступа
        /// </summary>
        public virtual string PermissionId { get; set; }
    }
}