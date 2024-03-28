namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActRemoval
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ActRemovalProvidedDoc"</summary>
    public class ActRemovalProvidedDocMap : BaseEntityMap<ActRemovalProvidedDoc>
    {
		public ActRemovalProvidedDocMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ActRemovalProvidedDoc", "GJI_NSO_ACTREMOVAL_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateProvided, "DateProvided").Column("DATE_PROVIDED");
            this.Reference(x => x.ProvidedDoc, "ProvidedDoc").Column("PROVDOC_ID").NotNull().Fetch();
            this.Reference(x => x.ActRemoval, "ActRemoval").Column("ACT_ID").NotNull().Fetch();
        }
    }
}
