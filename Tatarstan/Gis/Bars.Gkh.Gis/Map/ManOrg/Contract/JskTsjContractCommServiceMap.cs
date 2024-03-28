namespace Bars.Gkh.Gis.Map.ManOrg.Contract
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg.Contract;

    /// <summary>
    /// Маппинг для JskTsjContractCommService
    /// </summary>
    public class JskTsjContractCommServiceMap : BaseEntityMap<JskTsjContractCommService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public JskTsjContractCommServiceMap()
            : base("Коммунальная услуга договора управления ТСЖ/ЖСК", "JSKTSJ_CONTRACT_COMM_SERVICE")
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
