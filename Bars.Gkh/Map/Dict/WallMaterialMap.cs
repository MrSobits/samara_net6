namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Материал стен"</summary>
    public class WallMaterialMap : BaseEntityMap<WallMaterial>
    {
        public WallMaterialMap() : 
        base("Материал стен", "GKH_DICT_WALL_MATERIAL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.ImportEntityId, "Идентификатор сущности внешней системы").Column("IMPORT_ENTITY_ID");
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}
