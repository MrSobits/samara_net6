namespace Bars.Gkh.Gis.Map.ManOrg.Contract
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg.Contract;

    /// <summary>
    /// Маппинг для ContractOwnersCommService
    /// </summary>
    public class ContractOwnersCommServiceMap : BaseEntityMap<ContractOwnersCommService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ContractOwnersCommServiceMap()
            : base("Коммунальная услуга договора управления с собственником", "CONTRACT_OWNERS_COMM_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Contract, "Договор управления").Column("CONTRACT_ID").NotNull().Fetch();
            this.Reference(x => x.CommunalService, "Коммунальная услуга").Column("COMMUNAL_SERVICE_ID").NotNull().Fetch();
            this.Property(x => x.StartDate, "Дата начала").Column("START_DATE");
            this.Property(x => x.EndDate, "Дата окончания").Column("END_DATE");
        }
    }
}
