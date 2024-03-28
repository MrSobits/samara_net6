namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisShare"
    /// </summary>
    public class RisShareMap : BaseRisEntityMap<RisShare>
    {
        public RisShareMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisShare", "RIS_SHARE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.IsPrivatized, "IsPrivatized").Column("ISPRIVATIZED");
            this.Property(x => x.TermDate, "TermDate").Column("TERMDATE");
            this.Reference(x => x.RisShareContragent, "Contragent").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.Account, "Account").Column("ACCOUNT_ID").Fetch();
        }
    }
}
