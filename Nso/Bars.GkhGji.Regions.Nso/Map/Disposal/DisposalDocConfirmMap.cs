/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map.Disposal
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Nso.Entities.Disposal;
/// 
///     public class DisposalDocConfirmMap : BaseEntityMap<DisposalDocConfirm>
///     {
///         public DisposalDocConfirmMap()
///             : base("GJI_NSO_DISP_DOCCONFIRM")
///         {
///             Map(x => x.DocumentName, "DOC_NAME");
/// 
///             References(x => x.Disposal, "DISPOSAL_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.Disposal.DisposalDocConfirm"</summary>
    public class DisposalDocConfirmMap : BaseEntityMap<DisposalDocConfirm>
    {
        
        public DisposalDocConfirmMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.Disposal.DisposalDocConfirm", "GJI_NSO_DISP_DOCCONFIRM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentName, "DocumentName").Column("DOC_NAME");
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
        }
    }
}
