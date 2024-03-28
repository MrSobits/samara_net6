
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

 
    public class DictionaryERKNMMap : BaseEntityMap<DictionaryERKNM>
    {
        
        public DictionaryERKNMMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.DictionaryERKNM", "GJI_DICTIONARY_ERKNM")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DictionaryERKNMGuid, "DirectoryEPKNMGuid").Column("DICTIONARY_ERKNM_GUID");
            this.Property(x => x.Name, "Name").Column("NAME");
            this.Property(x => x.Type, "Type").Column("TYPE");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Property(x => x.Order, "Order").Column("ORDER");
            this.Property(x => x.Required, "Required").Column("REQUIRED");
            this.Property(x => x.DateLastUpdate, "DateLastUpdate").Column("DATE_LAST_UPDATE");
            this.Property(x => x.EntityName, "EntityName").Column("ENTITY_NAME");
            this.Property(x => x.EntityId, "EntityId").Column("ENTITY_ID");
        }
    }
}
