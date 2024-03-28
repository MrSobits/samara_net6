namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>Маппинг для "Инспектируемая часть в акте проверки предписания"</summary>
    public class ActRemovalInspectedPartMap : BaseEntityMap<ActRemovalInspectedPart>
    {
		public ActRemovalInspectedPartMap() : 
                base("Инспектируемая часть в акте проверки предписания", "GJI_NSO_ACTREMOVAL_INSPECTPART")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Character, "Характер и местоположение").Column("CHARACTER").Length(300);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
            Reference(x => x.InspectedPart, "Инспектируемая часть").Column("INSPECTIONPART_ID").NotNull().Fetch();
        }
    }
}
