namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Инспектируемая часть в акте без взаимодействия"</summary>
    public class ActIsolatedInspectedPartMap : BaseEntityMap<ActIsolatedInspectedPart>
    {      
        public ActIsolatedInspectedPartMap() : 
                base("Инспектируемая часть в акте без взаимодействия", "GJI_ACTISOLATED_INSPECTPART")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Character, "Характер и местоположение").Column("CHARACTER").Length(300);
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Reference(x => x.ActIsolated, "Акт без взаимодействия").Column("ACTISOLATED_ID").NotNull().Fetch();
            this.Reference(x => x.InspectedPart, "Инспектируемая часть").Column("INSPECTIONPART_ID").NotNull().Fetch();
        }
    }
}