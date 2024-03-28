namespace Bars.Gkh.Modules.Gkh1468.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Услуга по договору поставщика ресурсов с жилым домом
    /// </summary>
    public class PublicServiceOrgContractService : BaseImportableEntity
    {
        /// <summary>
        /// Договор поставщика ресурсов с домом
        /// </summary>
        public virtual PublicServiceOrgContract ResOrgContract { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }

        /// <summary>
        /// Дата начала предоставления услуги
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания предоставления услуги
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Тип системы теплоснабжения
        /// </summary>
        public virtual HeatingSystemType? HeatingSystemType { get; set; }

        /// <summary>
        /// Схема присоединения
        /// </summary>
        public virtual SchemeConnectionType? SchemeConnectionType { get; set; }

        /// <summary>
        /// Коммунальный ресурс
        /// </summary>
        public virtual CommunalResource CommunalResource { get; set; }

        /// <summary>
        /// Плановый объём
        /// </summary>
        public virtual decimal? PlanVolume { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Режим подачи
        /// </summary>
        public virtual string ServicePeriod { get; set; }
    }
}
