/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения по квартирам жилого дома"
///     /// </summary>
///     public class RealityObjectApartInfoMap : BaseGkhEntityMap<RealityObjectApartInfo>
///     {
///         public RealityObjectApartInfoMap() : base("GKH_OBJ_APARTMENT_INFO")
///         {
///             Map(x => x.Privatized, "PRIVATIZED").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.AreaLiving, "AREA_LIVING");
///             Map(x => x.AreaTotal, "AREA_TOTAL");
///             Map(x => x.CountPeople, "COUNT_PEOPLE");
///             Map(x => x.NumApartment, "NUM_APARTMENT").Length(300);
///             Map(x => x.FioOwner, "FIO_OWNER").Length(500);
///             Map(x => x.Phone, "PHONE").Length(300);
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Сведения по квартирам жилого дома"</summary>
    public class RealityObjectApartInfoMap : BaseImportableEntityMap<RealityObjectApartInfo>
    {
        
        public RealityObjectApartInfoMap() : 
                base("Сведения по квартирам жилого дома", "GKH_OBJ_APARTMENT_INFO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Privatized, "Privatized").Column("PRIVATIZED").NotNull();
            Property(x => x.AreaLiving, "Жилая площадь").Column("AREA_LIVING");
            Property(x => x.AreaTotal, "Общая площадь").Column("AREA_TOTAL");
            Property(x => x.CountPeople, "Количество жителей").Column("COUNT_PEOPLE");
            Property(x => x.NumApartment, "№ квартиры").Column("NUM_APARTMENT").Length(300);
            Property(x => x.FioOwner, "FioOwner").Column("FIO_OWNER").Length(500);
            Property(x => x.Phone, "Phone").Column("PHONE").Length(300);
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
