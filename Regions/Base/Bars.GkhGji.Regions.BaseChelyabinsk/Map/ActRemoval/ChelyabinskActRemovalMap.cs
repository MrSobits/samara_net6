namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActRemoval
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskActRemoval"</summary>
    public class ChelyabinskActRemovalMap : JoinedSubClassMap<ChelyabinskActRemoval>
    {

		public ChelyabinskActRemovalMap() :
			base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskActRemoval", "GJI_NSO_ACTREMOVAL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.AcquaintedWithDisposalCopy, "AcquaintedWithDisposalCopy").Column("ACQUAINT_WITH_DISP");
            this.Property(x => x.DocumentPlace, "DocumentPlace").Column("DOCUMENT_PLACE").Length(1000);
            this.Property(x => x.DocumentTime, "DocumentTime").Column("DOCUMENT_TIME");
        }
    }
}
