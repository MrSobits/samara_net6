namespace Bars.Gkh.Gis.Entities.ManOrg.Contract
{
    using System;

    using Gkh.Entities;

    /// <summary>
    /// Коммунальная услуга договора управления ТСЖ/ЖСК
    /// </summary>
    public class JskTsjContractCommService : BaseGkhEntity
    {
        /// <summary>
        /// Коммунальная услуга управляющей организации
        /// </summary>
        public virtual ManOrgBilCommunalService CommunalService { get; set; }

        /// <summary>
        /// Договор управления ТСЖ/ЖСК
        /// </summary>
        public virtual ManOrgJskTsjContract Contract { get; set; }

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
