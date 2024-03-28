namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class ContractPeriodMap : BaseEntityMap<ContractPeriod>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ContractPeriodMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.ContractPeriod", "GKH_CONTRACT_PERIOD")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.StartDate, "Начало периода").Column("START_DATE");
            this.Property(x => x.EndDate, "Конец периода").Column("END_DATE");
            this.Property(x => x.Name, "Название").Column("NAME").NotNull();
            this.Property(x => x.UoNumber, "Количество УО").Column("UO_NUMBER");
            this.Property(x => x.RsoNumber, "Количество РСО").Column("RSO_NUMBER");
            this.Property(x => x.RoNumber, "Количество жилых домов").Column("RO_NUMBER");
        }
    }
}