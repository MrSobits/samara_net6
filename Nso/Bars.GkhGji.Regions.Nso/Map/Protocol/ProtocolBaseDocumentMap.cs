/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Nso.Entities.Protocol;
/// 
///     public class ProtocolBaseDocumentMap : BaseEntityMap<ProtocolBaseDocument>
///     {
///         public ProtocolBaseDocumentMap()
///             : base("GJI_PROT_BASE_DOC")
///         {
/// 
///             Map(x => x.DateDoc, "DATE_DOC");
///             Map(x => x.NumDoc, "NUM_DOC", false, 300);
/// 
///             References(x => x.KindBaseDocument, "KIND_BASE_DOC_ID", ReferenceMapConfig.NotNull);
///             References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNull);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities.Protocol;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.Protocol.ProtocolBaseDocument"</summary>
    public class ProtocolBaseDocumentMap : BaseEntityMap<ProtocolBaseDocument>
    {
        
        public ProtocolBaseDocumentMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.Protocol.ProtocolBaseDocument", "GJI_PROT_BASE_DOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID").NotNull();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull();
            Reference(x => x.KindBaseDocument, "KindBaseDocument").Column("KIND_BASE_DOC_ID").NotNull();
            Property(x => x.DateDoc, "DateDoc").Column("DATE_DOC");
            Property(x => x.NumDoc, "NumDoc").Column("NUM_DOC").Length(300);
        }
    }
}
