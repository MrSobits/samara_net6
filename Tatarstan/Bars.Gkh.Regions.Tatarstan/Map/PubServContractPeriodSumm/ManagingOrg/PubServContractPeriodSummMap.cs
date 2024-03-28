namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class PubServContractPeriodSummMap : BaseEntityMap<PubServContractPeriodSumm>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PubServContractPeriodSummMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.PubServContractPeriodSumm", "GKH_CONTR_PERIOD_SUMM")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Муниципальный район").Column("MU_ID").NotNull().Fetch();
            this.Reference(x => x.ContractPeriod, "Отчетный период договора").Column("PER_ID").NotNull().Fetch();
            this.Reference(x => x.ContractPeriodSummRso, "Информация по Периоду для РСО").Column("PER_SUMM_RSO_ID").NotNull().Fetch();
            this.Reference(x => x.ContractPeriodSummUo, "Информация по Периоду для УО").Column("PER_SUMM_UO_ID").NotNull().Fetch();
            this.Reference(x => x.PublicService, "Услуга по Договору РСО с Домами").Column("PUB_SERV_ID").NotNull().Fetch();
        }
    }
}