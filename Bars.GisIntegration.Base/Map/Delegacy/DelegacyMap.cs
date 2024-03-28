namespace Bars.GisIntegration.Base.Map.Delegacy
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.Delegacy;

    /// <summary>
    /// Маппинг для делегирования
    /// </summary>
    public class DelegacyMap : BaseEntityMap<Delegacy> 
    {
        public DelegacyMap()
            : base("Bars.GisIntegration.Base.Entities.Delegacy.Delegacy", "DELEGACY")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.OperatorIS, "OperatorIS").Column("OPERATOR_IS_ID").NotNull().Fetch();
            this.Reference(x => x.InformationProvider, "InformationProvider").Column("INFORMATION_PROVIDER_ID").NotNull().Fetch();
            this.Property(x => x.EndDate, "EndDate").Column("END_DATE");
            this.Property(x => x.StartDate, "StartDate").Column("START_DATE");
        }
    }
}
