/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения по помещениям жилого дома"
///     /// </summary>
///     public class RealityObjectHouseInfoMap : BaseGkhEntityMap<RealityObjectHouseInfo>
///     {
///         public RealityObjectHouseInfoMap() : base("GKH_OBJ_HOUSE_INFO")
///         {
///             Map(x => x.KindRight, "KIND_RIGHT").Not.Nullable().CustomType<KindRightToObject>();
///             Map(x => x.DateRegistration, "DATE_REG");
///             Map(x => x.DateRegistrationOwner, "DATE_REG_OWNER");
///             Map(x => x.NumObject, "NUM").Length(300);
///             Map(x => x.NumRegistrationOwner, "NUM_REG_RIGHT").Length(300);
///             Map(x => x.Name, "NAME").Length(100);
///             Map(x => x.Owner, "OWNER").Length(300);
///             Map(x => x.TotalArea, "TOTAL_AREA");
/// 
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID").Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Сведения о помещениях жилого дома"</summary>
    public class RealityObjectHouseInfoMap : BaseImportableEntityMap<RealityObjectHouseInfo>
    {
        
        public RealityObjectHouseInfoMap() : 
                base("Сведения о помещениях жилого дома", "GKH_OBJ_HOUSE_INFO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.KindRight, "Вид права").Column("KIND_RIGHT").NotNull();
            Property(x => x.DateRegistration, "Дата регистрации права").Column("DATE_REG");
            Property(x => x.DateRegistrationOwner, "Дата регистрации (рождения) правообладателя").Column("DATE_REG_OWNER");
            Property(x => x.NumObject, "№ объекта").Column("NUM").Length(300);
            Property(x => x.NumRegistrationOwner, "№ зарегистрированного права / ограничения").Column("NUM_REG_RIGHT").Length(300);
            Property(x => x.Name, "Наименование").Column("NAME").Length(100);
            Property(x => x.Owner, "Правообладатель").Column("OWNER").Length(300);
            Property(x => x.TotalArea, "Площадь").Column("TOTAL_AREA");
            Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID").Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
