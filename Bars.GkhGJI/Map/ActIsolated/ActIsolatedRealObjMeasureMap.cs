namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>
    /// Маппинг для "Меры в доме для акта без взаимодействия"
    /// </summary>
    public class ActIsolatedRealObjMeasureMap : BaseEntityMap<ActIsolatedRealObjMeasure>
    {
        public ActIsolatedRealObjMeasureMap() : 
                base("Меры в доме для акта без взаимодействия", "GJI_ACTISOLATED_ROBJECT_MEASURE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Measure, "Меры, принятые по пресечению нарушения обязательных требования").Column("MEASURE").Length(300).NotNull();
            this.Reference(x => x.ActIsolatedRealObj, "Дом акта без взаимодействия").Column("ACTISOLATED_RO_ID").NotNull().Fetch();
        }
    }
}