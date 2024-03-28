namespace Bars.GkhGji.Regions.Tatarstan.Entities.Orgs
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Контрольно-надзорный орган (КНО).
    /// </summary>
    public class ControlOrganization : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Контрагент.
        /// </summary>
        public virtual Contragent Contragent { get; set; }

	    /// <summary>
	    /// Идентификатор в ТОР
	    /// </summary>
	    public virtual Guid? TorId { get; set; }
	}
}
