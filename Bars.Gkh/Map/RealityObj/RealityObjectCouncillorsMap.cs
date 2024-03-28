/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Члены совета Многоквартирного дома (МКД)"
///     /// </summary>
///     public class RealityObjectCouncillorsMap : BaseGkhEntityMap<RealityObjectCouncillors>
///     {
///         public RealityObjectCouncillorsMap()
///             : base("GKH_OBJ_COUNCILLORS")
///         {
///             Map(x => x.Fio, "FIO").Length(300);
///             Map(x => x.Post, "POST");
///             Map(x => x.Phone, "PHONE").Length(50);
///             Map(x => x.Email, "EMAIL").Length(100);
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
    
    
    /// <summary>Маппинг для "Члены совета Многоквартирного дома (МКД)"</summary>
    public class RealityObjectCouncillorsMap : BaseImportableEntityMap<RealityObjectCouncillors>
    {
        
        public RealityObjectCouncillorsMap() : 
                base("Члены совета Многоквартирного дома (МКД)", "GKH_OBJ_COUNCILLORS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300);
            Property(x => x.Post, "Должность").Column("POST");
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(50);
            Property(x => x.Email, "Email").Column("EMAIL").Length(100);
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
