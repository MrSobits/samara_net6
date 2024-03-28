/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     class ProvidedDocDateMap : BaseEntityMap<DisposalProvidedDocDate>
///     {
///         public ProvidedDocDateMap()
///             : base("GJI_TOMSK_PROVDOC_DATE")
///         {
///             Map(x => x.DateTimeDocument, "DATE_TIME_DOCUMENT");
///             
///             References(x => x.Disposal, "DISPOSAL_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.DisposalProvidedDocDate"</summary>
    public class DisposalProvidedDocNumMap : BaseEntityMap<DisposalProvidedDocNum>
    {
        
        public DisposalProvidedDocNumMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.DisposalProvidedDocNum", "GJI_TOMSK_PROVDOC_NUM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ProvideDocumentsNum, "DateTimeDocument").Column("NUM_TIME_DOCUMENT");
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
        }
    }
}
