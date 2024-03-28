namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActRemoval
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>Маппинг для "Инспектируемая часть в акте проверки предписания"</summary>
    public class ActRemovalInspectedPartMap : BaseEntityMap<ActRemovalInspectedPart>
    {
		public ActRemovalInspectedPartMap() : 
                base("Инспектируемая часть в акте проверки предписания", "GJI_NSO_ACTREMOVAL_INSPECTPART")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Character, "Характер и местоположение").Column("CHARACTER").Length(300);
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
            this.Reference(x => x.InspectedPart, "Инспектируемая часть").Column("INSPECTIONPART_ID").NotNull().Fetch();
        }
    }
}
