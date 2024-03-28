namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisShareEncbrNonResPrem"
    /// </summary>
    public class RisShareEncbrNonResPremMap : BaseRisEntityMap<RisShareEncbrNonResPrem>
    {
        public RisShareEncbrNonResPremMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisShareEncbrNonResPrem", "RIS_SHAREECNBRNONRESPREM")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.IntPart, "IntPart").Column("INTPART").Length(50);
            this.Property(x => x.FracPart, "FracPart").Column("FRACPART").Length(50);
            this.Reference(x => x.Ecnbr, "Ecnbr").Column("ECNBR_ID").Fetch();
            this.Reference(x => x.Share, "Share").Column("SHARE_ID").Fetch();
            this.Reference(x => x.NonResidentialPremises, "NonResidentialPremises").Column("NONRESIDENTIALPREMISES_ID").Fetch();
        }
    }
}
