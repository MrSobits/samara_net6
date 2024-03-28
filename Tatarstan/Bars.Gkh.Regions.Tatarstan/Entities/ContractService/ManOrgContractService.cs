namespace Bars.Gkh.Regions.Tatarstan.Entities.ContractService
{
    using System;

    using Bars.Gkh.Entities.Dicts.ContractService;
    using Bars.Gkh.Enums;

    using Gkh.Entities;

    /// <summary>
    /// Работа / услуга управляющей организации
    /// </summary>
    public class ManOrgContractService : BaseGkhEntity
    {
        /// <summary>
        /// Тип работы
        /// </summary>
        public virtual ManagementContractServiceType ServiceType { get; set; }

        /// <summary>
        /// Услуга по договору управления
        /// </summary>
        public virtual ManagementContractService Service { get; set; }

        /// <summary>
        /// Договор управления
        /// </summary>
        public virtual ManOrgBaseContract Contract { get; set; }

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