namespace Bars.Gkh.Gis.Entities.ManOrg.Contract
{
    using System;

    using Gkh.Entities;

    /// <summary>
    /// Дополнительная услуга договора управления с собственником
    /// </summary>
    public class ContractOwnersAddService : BaseGkhEntity
    {
        /// <summary>
        /// Дополнителная услуга управляющей организации
        /// </summary>
        public virtual ManOrgBilAdditionService AdditionService { get; set; }

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
