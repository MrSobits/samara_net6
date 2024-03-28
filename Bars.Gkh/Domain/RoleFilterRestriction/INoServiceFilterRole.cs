namespace Bars.Gkh.Domain.RoleFilterRestriction
{
    /// <summary>
    /// Сервис возврщающий роли, дома которых не фильтруются по организации
    /// </summary>
    public interface INoServiceFilterRole
    {
        /// <summary>
        /// Имя роли, дома которой не фильтруются по организации
        /// </summary>
        string RoleName { get; } 
    }
}