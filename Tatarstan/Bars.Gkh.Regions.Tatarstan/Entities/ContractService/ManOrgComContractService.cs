namespace Bars.Gkh.Regions.Tatarstan.Entities.ContractService
{
    using System;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Работы и услуги управляющей организации
    /// </summary>
    public class ManOrgComContractService : ManOrgContractService
    {
        /// <summary>
        /// Коммунальная услуга
        /// <para>Не использовать в подзапросах</para>
        /// </summary>
        public virtual CommunalContractService CommunalContractService => this.Service as CommunalContractService;
    }
}