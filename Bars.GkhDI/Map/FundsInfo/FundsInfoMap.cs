/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения о фондах"
///     /// </summary>
///     public class FundsInfoMap : BaseGkhEntityMap<FundsInfo>
///     {
///         public FundsInfoMap(): base("DI_DISINFO_FUNDS")
///         {
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Size, "SIZE_INFO");
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FundsInfo"</summary>
    public class FundsInfoMap : BaseImportableEntityMap<FundsInfo>
    {
        
        public FundsInfoMap() : 
                base("Bars.GkhDi.Entities.FundsInfo", "DI_DISINFO_FUNDS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.Size, "Size").Column("SIZE_INFO");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
        }
    }
}
