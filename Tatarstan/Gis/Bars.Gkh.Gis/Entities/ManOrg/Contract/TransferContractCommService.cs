namespace Bars.Gkh.Gis.Entities.ManOrg.Contract
{
    using System;

    using Gkh.Entities;

    /// <summary>
    /// Коммунальная услуга договора передачи управления УК
    /// </summary>
    public class TransferContractCommService : BaseGkhEntity
    {
        /// <summary>
        /// Коммунальная услуга управляющей организации
        /// </summary>
        public virtual ManOrgBilCommunalService CommunalService { get; set; }

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
