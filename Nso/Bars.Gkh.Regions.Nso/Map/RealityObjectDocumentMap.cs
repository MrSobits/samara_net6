/// <mapping-converter-backup>
/// namespace Bars.Gkh.Regions.Nso.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Regions.Nso.Entities;
///     using Bars.Gkh.Regions.Nso.Enums;
/// 
///     public class RealityObjectDocumentMap : BaseEntityMap<RealityObjectDocument>
///     {
///         public RealityObjectDocumentMap()
///             : base("GKH_NSO_ROBJ_DOCUMENT")
///         {
///             Map(x => x.DocumentType, "DOCUMENT_TYPE").Not.Nullable().CustomType<RealityObjectDocumentType>();
///             Map(x => x.Name, "NAME").Length(300);
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Nso.Entities.RealityObjectDocument"</summary>
    public class RealityObjectDocumentMap : BaseEntityMap<RealityObjectDocument>
    {
        
        public RealityObjectDocumentMap() : 
                base("Bars.Gkh.Regions.Nso.Entities.RealityObjectDocument", "GKH_NSO_ROBJ_DOCUMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentType, "DocumentType").Column("DOCUMENT_TYPE").NotNull();
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").NotNull().Fetch();
        }
    }
}
