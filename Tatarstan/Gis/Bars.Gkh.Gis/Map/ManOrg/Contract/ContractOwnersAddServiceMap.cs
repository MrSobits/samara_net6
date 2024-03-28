namespace Bars.Gkh.Gis.Map.ManOrg.Contract
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg.Contract;

    /// <summary>
    /// Маппинг для ContractOwnersAddService
    /// </summary>
    public class ContractOwnersAddServiceMap : BaseEntityMap<ContractOwnersAddService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ContractOwnersAddServiceMap()
            : base("Дополнительная услуга договора управления с собственником", "CONTRACT_OWNERS_ADD_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Contract, "Договор управления").Column("CONTRACT_ID").NotNull().Fetch();
            this.Reference(x => x.AdditionService, "Дополнителная услуга").Column("ADDITION_SERVICE_ID").NotNull().Fetch();
            this.Property(x => x.StartDate, "Дата начала").Column("START_DATE");
            this.Property(x => x.EndDate, "Дата окончания").Column("END_DATE");
        }
    }
}
