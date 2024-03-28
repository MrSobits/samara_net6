namespace Bars.Gkh.Gis.Entities.ManOrg.Contract
{
    using System;

    using Gkh.Entities;

    /// <summary>
    /// Коммунальная услуга договора управления с собственником
    /// </summary>
    public class ContractOwnersCommService : BaseGkhEntity
    {
        /// <summary>
        /// Коммунальная услуга управляющей организации
        /// </summary>
        public virtual ManOrgBilCommunalService CommunalService { get; set; }

        /// <summary>
        /// Договор управления с собственником
        /// </summary>
        public virtual ManOrgContractOwners Contract { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime EndDate { get; set; }
    }
}
