namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.SupResContractSubjectOtherQuality"
    /// </summary>
    public class SupResContractSubjectOtherQualityMap : BaseRisEntityMap<SupResContractSubjectOtherQuality>
    {
        public SupResContractSubjectOtherQualityMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.SupResContractSubjectOtherQuality", "SUP_RES_CONTRACT_SUBJECT_OTHER_QUALITY")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ContractSubject, "ContractSubject").Column("CONTRACT_SUBJECT_ID").Fetch();
            this.Property(x => x.IndicatorName, "IndicatorName").Column("INDICATOR_NAME");
            this.Property(x => x.Number, "Number").Column("NUMBER");
            this.Property(x => x.Okei, "Okei").Column("OKEI");
        }
    }
}
