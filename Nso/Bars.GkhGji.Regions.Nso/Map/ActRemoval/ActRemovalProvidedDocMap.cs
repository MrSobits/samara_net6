namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.ActRemovalProvidedDoc"</summary>
    public class ActRemovalProvidedDocMap : BaseEntityMap<ActRemovalProvidedDoc>
    {
		public ActRemovalProvidedDocMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.ActRemovalProvidedDoc", "GJI_NSO_ACTREMOVAL_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateProvided, "DateProvided").Column("DATE_PROVIDED");
            Reference(x => x.ProvidedDoc, "ProvidedDoc").Column("PROVDOC_ID").NotNull().Fetch();
            Reference(x => x.ActRemoval, "ActRemoval").Column("ACT_ID").NotNull().Fetch();
        }
    }
}
