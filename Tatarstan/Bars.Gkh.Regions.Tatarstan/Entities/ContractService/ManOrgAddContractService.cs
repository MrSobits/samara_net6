namespace Bars.Gkh.Regions.Tatarstan.Entities.ContractService
{
    using System;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Работы и услуги управляющей организации
    /// </summary>
    public class ManOrgAddContractService : ManOrgContractService
    {
        /// <summary>
        /// Дополнительная услуга
        /// <para>Не использовать в подзапросах</para>
        /// </summary>
        public virtual AdditionalContractService AdditionalContractService => this.Service as AdditionalContractService;
    }
}