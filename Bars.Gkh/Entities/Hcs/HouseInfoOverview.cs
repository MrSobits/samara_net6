namespace Bars.Gkh.Entities.Hcs
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Общие сведения по дому
    /// </summary>
    public class HouseInfoOverview: BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Общее количество лицевых счетов физических лиц
        /// </summary>
        public virtual int IndividualAccountsCount { get; set; }

        /// <summary>
        /// Количество лицевых счетов физических лиц-собственников
        /// </summary>
        public virtual int IndividualTenantAccountsCount { get; set; }

        /// <summary>
        /// Количество лицевых счетов физических лиц-нанимателей
        /// </summary>
        public virtual int IndividualOwnerAccountsCount { get; set; }

        /// <summary>
        /// Общее количество лицевых счетов юридических лиц
        /// </summary>
        public virtual int LegalAccountsCount { get; set; }

        /// <summary>
        /// Количество лицевых счетов юридических лиц-собственников
        /// </summary>
        public virtual int LegalTenantAccountsCount { get; set; }

        /// <summary>
        /// Количество лицевых счетов юридических лиц-нанимателей
        /// </summary>
        public virtual int LegalOwnerAccountsCount { get; set; }
    }
}
