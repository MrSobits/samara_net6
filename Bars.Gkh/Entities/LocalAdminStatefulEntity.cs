namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Права доступа на объекты <see cref="StatefulEntityInfo"/> для локального администратора
    /// </summary>
    public class LocalAdminStatefulEntity : PersistentObject
    {
        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Идентификатор статусной сущности <see cref="StatefulEntityInfo.TypeId"/>
        /// </summary>
        public virtual string TypeId { get; set; }
    }
}