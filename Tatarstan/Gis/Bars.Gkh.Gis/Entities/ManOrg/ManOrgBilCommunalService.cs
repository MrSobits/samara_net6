namespace Bars.Gkh.Gis.Entities.ManOrg
{
    using Enum.ManOrg;
    using Gkh.Entities;
    using Kp50;

    /// <summary>
    /// Коммунальная услуга управляющей организации
    /// </summary>
    public class ManOrgBilCommunalService : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Услуга из биллинга
        /// </summary>
        public virtual BilServiceDictionary BilService { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual CommunalServiceName Name { get; set; }

        /// <summary>
        /// Коммунальный ресурс
        /// </summary>
        public virtual CommunalServiceResource Resource { get; set; }
    }
}
