namespace Bars.Gkh.Gis.Entities.ManOrg
{
    using Enum.ManOrg;
    using Gkh.Entities;
    using Kp50;

    /// <summary>
    /// Работы и услуги управляющей организации
    /// </summary>
    public class ManOrgBilWorkService : BaseGkhEntity
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
        /// Назначение работ
        /// </summary>
        public virtual ServiceWorkPurpose Purpose { get; set; }

        /// <summary>
        /// Тип работ
        /// </summary>
        public virtual ServiceWorkType Type { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}