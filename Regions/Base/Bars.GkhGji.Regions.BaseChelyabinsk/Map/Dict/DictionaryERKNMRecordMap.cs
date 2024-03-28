
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class DictionaryERKNMRequestMap : BaseEntityMap<DictionaryERKNMRecord>
    {
        
        public DictionaryERKNMRequestMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.DictionaryERKNMRecord", "GJI_DICTIONARY_ERKNM_RECORD")
        {

        }
        
        protected override void Map()
        {
            this.Property(x => x.RecId, "RecId").Column("REC_ID");
            this.Property(x => x.Name, "Name").Column("NAME");
            this.Property(x => x.Name1, "Name1").Column("NAME1");
            this.Property(x => x.Name2, "Name2").Column("NAME2");
            this.Property(x => x.EntityName, "EntityName").Column("ENTITY_NAME");
            this.Property(x => x.EntityId, "EntityId").Column("ENTITY_ID");
           
        }
    }
}
