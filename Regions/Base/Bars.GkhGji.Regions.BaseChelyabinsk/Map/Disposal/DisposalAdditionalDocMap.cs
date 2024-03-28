
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.DisposalDocConfirm"</summary>
    public class DisposalAdditionalDocMap : BaseEntityMap<DisposalAdditionalDoc>
    {
        
        public DisposalAdditionalDocMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.DisposalDocConfirm", "GJI_DISP_ADDITIONAL_DOC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DocumentName, "DocumentName").Column("DOC_NAME");
            this.Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
        }
    }
}
