namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.NsoActRemoval"</summary>
    public class NsoActRemovalMap : JoinedSubClassMap<NsoActRemoval>
    {

		public NsoActRemovalMap() :
			base("Bars.GkhGji.Regions.Nso.Entities.NsoActRemoval", "GJI_NSO_ACTREMOVAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AcquaintedWithDisposalCopy, "AcquaintedWithDisposalCopy").Column("ACQUAINT_WITH_DISP");
            Property(x => x.DocumentPlace, "DocumentPlace").Column("DOCUMENT_PLACE").Length(1000);
            Property(x => x.DocumentTime, "DocumentTime").Column("DOCUMENT_TIME");
        }
    }
}
