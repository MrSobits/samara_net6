namespace Bars.Gkh.Gis.Entities.ManOrg.Contract
{
    using System;

    using Gkh.Entities;

    /// <summary>
    /// Работы и услуги договора передачи управления УК
    /// </summary>
    public class TransferContractWorkService : BaseGkhEntity
    {
        /// <summary>
        /// Работы и услуги управляющей организации
        /// </summary>
        public virtual ManOrgBilWorkService WorkService { get; set; }

        /// <summary>
        /// Договор передачи управления УК
        /// </summary>
        public virtual ManOrgContractTransfer Contract { get; set; }

        /// <summary>
        /// Размер платы (цена) за услуги, работы по управлению домом
        /// </summary>
        public virtual decimal PaymentAmount { get; set; }
    }
}
