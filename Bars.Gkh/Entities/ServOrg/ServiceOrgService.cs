namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Услуга обслуживающей организации
    /// </summary>
    public class ServiceOrgService : BaseGkhEntity
    {
        /// <summary>
        /// Обслуживающая организация
        /// </summary>
        public virtual ServiceOrganization ServiceOrganization { get; set; }

        /// <summary>
        /// Тип обслуживания
        /// </summary>
        public virtual TypeService TypeService { get; set; }
    }
}
