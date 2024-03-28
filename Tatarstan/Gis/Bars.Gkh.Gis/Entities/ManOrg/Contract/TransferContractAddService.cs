namespace Bars.Gkh.Gis.Entities.ManOrg.Contract
{
    using System;

    using Gkh.Entities;

    /// <summary>
    /// Дополнительная услуга договора передачи управления УК
    /// </summary>
    public class TransferContractAddService : BaseGkhEntity
    {
        /// <summary>
        /// Дополнителная услуга управляющей организации
        /// </summary>
        public virtual ManOrgBilAdditionService AdditionService { get; set; }

        /// <summary>
        /// Договор передачи управления УК
        /// </summary>
        public virtual ManOrgContractTransfer Contract { get; set; }

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
