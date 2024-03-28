namespace Bars.Gkh.Regions.Tatarstan.Entities.ContractService
{
    using System;

    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Работы и услуги управляющей организации
    /// </summary>
    public class ManOrgAgrContractService : ManOrgContractService
    {
        /// <summary>
        /// Жилищная услуга
        /// <para>Не использовать в подзапросах</para>
        /// </summary>
        public virtual AgreementContractService AgreementContractService => this.Service as AgreementContractService;

        /// <summary>
        /// Размер платы (цена) за услуги, работы по управлению домом
        /// </summary>
        public virtual decimal? PaymentAmount { get; set; }
    }
}