namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сущность прав доступа для создания домов с определенным типом для конкретной роли
    /// </summary>
    public class RoleTypeHousePermission : BaseEntity
    {
        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Доступный для создания тип дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }
    }
}