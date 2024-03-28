/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map.Disposal
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal;
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

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.DisposalDocConfirm"</summary>
    public class DisposalDocConfirmMap : BaseEntityMap<DisposalDocConfirm>
    {
        
        public DisposalDocConfirmMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.DisposalDocConfirm", "GJI_NSO_DISP_DOCCONFIRM")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DocumentName, "DocumentName").Column("DOC_NAME");
            this.Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
        }
    }
}
