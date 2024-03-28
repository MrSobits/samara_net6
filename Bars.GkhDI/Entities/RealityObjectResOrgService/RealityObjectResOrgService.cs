namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Услуга по договору поставщика коммунальных услуг с жилым домом
    /// </summary>
    public class RealityObjectResOrgService : BaseImportableEntity
    {
        /// <summary>
        /// Договор с жилым домом
        /// </summary>
        public virtual RealityObjectResOrg RoResOrg { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual TemplateService Service { get; set; }

        /// <summary>
        /// Дата начала предоставления услуги
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания предоставления услуги
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
    }
}
