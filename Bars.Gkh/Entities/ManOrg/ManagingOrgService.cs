namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Услуга управляющей организации
    /// </summary>
    public class ManagingOrgService : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Вид работы/услуги
        /// </summary>
        public virtual Work Work { get; set; }
    }
}
