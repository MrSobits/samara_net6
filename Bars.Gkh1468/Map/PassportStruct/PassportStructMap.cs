/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.Gkh1468.Entities;
/// 
///     public class PassportStructMap : BaseGkhEntityByCodeMap<PassportStruct>
///     {
///         public PassportStructMap()
///             : base("GKH_PSTRUCT_PSTRUCT")
///         {
///             Map(x => x.Name, "NAME");
///             Map(x => x.ValidFromMonth, "VALID_FROM_MONTH");
///             Map(x => x.ValidFromYear, "VALID_FROM_YEAR");
///             Map(x => x.PassportType, "PASSPORT_TYPE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh1468.Entities.PassportStruct"</summary>
    public class PassportStructMap : BaseEntityMap<PassportStruct>
    {
        
        public PassportStructMap() : 
                base("Bars.Gkh1468.Entities.PassportStruct", "GKH_PSTRUCT_PSTRUCT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Property(x => x.Name, "Наименование").Column("NAME").Length(250);
            Property(x => x.ValidFromMonth, "Месяц начала действия").Column("VALID_FROM_MONTH");
            Property(x => x.ValidFromYear, "Год начала действия").Column("VALID_FROM_YEAR");
            Property(x => x.PassportType, "Тип паспорта").Column("PASSPORT_TYPE");
        }
    }
}
