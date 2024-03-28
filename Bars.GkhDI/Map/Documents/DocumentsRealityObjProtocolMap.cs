/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документы объекта недвижимости протоколы"
///     /// </summary>
///     public class DocumentsRealityObjProtocolMap : BaseGkhEntityMap<DocumentsRealityObjProtocol>
///     {
///         public DocumentsRealityObjProtocolMap(): base("DI_DISINFO_DOC_PROT")
///         {
///             Map(x => x.Year, "YEAR").Not.Nullable();
/// 
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DocumentsRealityObjProtocol"</summary>
    public class DocumentsRealityObjProtocolMap : BaseImportableEntityMap<DocumentsRealityObjProtocol>
    {
        
        public DocumentsRealityObjProtocolMap() : 
                base("Bars.GkhDi.Entities.DocumentsRealityObjProtocol", "DI_DISINFO_DOC_PROT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.DocDate, "DocDate").Column("DOC_DATE");
            Property(x => x.DocNum, "DocNum").Column("DOC_NUM");
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJ_ID").NotNull().Fetch();
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
