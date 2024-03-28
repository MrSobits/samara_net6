namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    public class EntityChangeLogRecordMap : BaseEntityMap<EntityChangeLogRecord>
    {
        public EntityChangeLogRecordMap() :
                base("Bars.GkhGji.Entities.EntityOldValueHistory", "GKH_ENTITY_CHANGE_LOG")
        {
        }

        protected override void Map()
        {            
            this.Property(x => x.EntityId, "Description").Column("ENTITY_ID");
            this.Property(x => x.TypeEntityLogging, "TypeEntityLogging").Column("ENTITY_TYPE");
            this.Property(x => x.OperationType, "OperationType").Column("OPERATION_TYPE");
            this.Property(x => x.OldValue, "OldValue").Column("OLDVALUE");
            this.Property(x => x.NewValue, "NewValue").Column("NEWVALUE");
            this.Property(x => x.PropertyType, "PropertyType").Column("PROPERTY_TYPE");
            this.Property(x => x.PropertyName, "PropertyName").Column("PROPERTY_NAME");
            this.Property(x => x.OperatorLogin, "OperatorLogin").Column("OPERATOR_LOGIN");
            this.Property(x => x.OperatorName, "OperatorName").Column("OPERATOR_NAME");
            this.Property(x => x.OperatorId, "OperatorId").Column("OPERATOR_ID");
            this.Property(x => x.DocumentValue, "DocumentValue").Column("ENTITY_VALUE");
            this.Property(x => x.ParrentEntity, "ParrentEntity").Column("PARRENT_ENTITY_ID");
        }
    }   
}
