namespace Bars.Gkh.Entities
{
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Отношение роли локального администратора к пользовательской
    /// </summary>
    public class LocalAdminRoleRelations : BaseGkhEntity
    {
        /// <summary>
        /// Родительская роль
        /// </summary>
        public virtual Role ParentRole { get; set; }

        /// <summary>
        /// Зависимая пользовательская роль
        /// </summary>
        public virtual Role ChildRole { get; set; }
    }
}