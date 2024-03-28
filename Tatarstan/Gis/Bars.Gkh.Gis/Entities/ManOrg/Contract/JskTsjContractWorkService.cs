namespace Bars.Gkh.Gis.Entities.ManOrg.Contract
{
    using Gkh.Entities;

    /// <summary>
    /// Работы и услуги договора управления ТСЖ/ЖСК
    /// </summary>
    public class JskTsjContractWorkService : BaseGkhEntity
    {
        /// <summary>
        /// Работы и услуги управляющей организации
        /// </summary>
        public virtual ManOrgBilWorkService WorkService { get; set; }

        /// <summary>
        /// Договор управления ТСЖ/ЖСК
        /// </summary>
        public virtual ManOrgJskTsjContract Contract { get; set; }

        /// <summary>
        /// Размер платы (цена) за услуги, работы по управлению домом
        /// </summary>
        public virtual decimal PaymentAmount { get; set; }
    }
}
