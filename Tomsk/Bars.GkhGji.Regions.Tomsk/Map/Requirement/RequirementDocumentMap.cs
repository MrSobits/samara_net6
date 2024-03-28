/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class RequirementDocumentMap : BaseEntityMap<RequirementDocument>
///     {
///         public RequirementDocumentMap()
///             : base("GJI_REQUIREMENT_DOC")
///         {
///             References(x => x.Document, "DOC_ID").Not.Nullable().LazyLoad();
///             References(x => x.Requirement, "REQ_ID").Not.Nullable().Fetch.Join();
///         } 
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.RequirementDocument"</summary>
    public class RequirementDocumentMap : BaseEntityMap<RequirementDocument>
    {
        
        public RequirementDocumentMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.RequirementDocument", "GJI_REQUIREMENT_DOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Document, "Document").Column("DOC_ID").NotNull();
            Reference(x => x.Requirement, "Requirement").Column("REQ_ID").NotNull().Fetch();
        }
    }
}
