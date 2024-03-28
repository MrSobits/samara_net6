namespace Bars.Gkh.Gis.Entities.ManOrg
{
    using Gkh.Entities;
    using Kp50;

    /// <summary>
    /// Дополнительная услуга управляющей организации
    /// </summary>
    public class ManOrgBilAdditionService : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Услуга из биллинга
        /// </summary>
        public virtual BilServiceDictionary BilService { get; set; }
    }
}
