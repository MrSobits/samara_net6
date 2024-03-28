/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     public class ViewDisposalNullInspectionMap : PersistentObjectMap<ViewDisposalNullInspection>
///     {
///         public ViewDisposalNullInspectionMap() : base("view_gji_disposal_null_insp")
///         {
///             References(x => x.Disposal, "DOCUMENT_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewDisposalNullInspection"</summary>
    public class ViewDisposalNullInspectionMap : PersistentObjectMap<ViewDisposalNullInspection>
    {
        
        public ViewDisposalNullInspectionMap() : 
                base("Bars.GkhGji.Entities.ViewDisposalNullInspection", "VIEW_GJI_DISPOSAL_NULL_INSP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Распоряжение").Column("DOCUMENT_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
