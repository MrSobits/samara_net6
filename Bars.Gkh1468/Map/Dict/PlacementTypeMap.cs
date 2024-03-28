namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;

    public class PlacementTypeMap : BaseEntityMap<PlacementType>
    {
        
        public PlacementTypeMap() : 
                base("Bars.Gkh1468.Entities.PlacementType", "GKH_PLACEMENT_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(500);
            Property(x => x.ShortName, "ShortName").Column("SHORT_NAME").Length(10);
        }
    }
}
