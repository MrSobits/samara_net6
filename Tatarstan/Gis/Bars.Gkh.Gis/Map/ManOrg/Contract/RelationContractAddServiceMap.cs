namespace Bars.Gkh.Gis.Map.ManOrg.Contract
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg.Contract;

    /// <summary>
    /// Маппинг для RelationContractAddService
    /// </summary>
    public class RelationContractAddServiceMap : BaseEntityMap<RelationContractAddService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public RelationContractAddServiceMap()
            : base("Дополнительная услуга договора передачи управления ТСЖ/ЖСК", "RELATION_CONTRACT_ADD_SERVICE")
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
