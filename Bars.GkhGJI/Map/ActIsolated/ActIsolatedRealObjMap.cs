namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Дом акта без взаимодействия"</summary>
    public class ActIsolatedRealObjMap : BaseEntityMap<ActIsolatedRealObj>
    {
        public ActIsolatedRealObjMap() : 
                base("Дом акта без взаимодействия", "GJI_ACTISOLATED_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.HaveViolation, "Признак выявлено или невыявлено нарушение").Column("HAVE_VIOLATION").NotNull();
            this.Reference(x => x.ActIsolated, "Акт без взаимодействия").Column("ACTISOLATED_ID").NotNull();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull();
        }
    }
}