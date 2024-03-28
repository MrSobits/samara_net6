namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    public class EntityOldValueHistoryMap : BaseEntityMap<EntityOldValueHistory>
    {
        public EntityOldValueHistoryMap() :
                base("Bars.GkhGji.Entities.EntityOldValueHistory", "GKH_ENTITY_OLDVALUE_HISTORY")
        {
        }

        protected override void Map()
        {            
            this.Property(x => x.EntityId, "Description").Column("ENTITY_ID");
            this.Property(x => x.TypeEntityLogging, "TypeEntityLogging").Column("ENTITY_TYPE");
            this.Property(x => x.OldValue, "OldValue").Column("OLDVALUE");
        }
    }
    public class EntityOldValueHistoryNHibernateMapping : ClassMapping<EntityOldValueHistory>
    {
        public EntityOldValueHistoryNHibernateMapping()
        {
            this.Property(
                x => x.OldValue,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });
           
        }
    }
}
