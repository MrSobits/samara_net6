namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    public class ClaimworkAllProxy
    {
        public string ClwId { get; set; } //Claimwork ("CLW_CLAIMWORK")
        public string LawId { get; set; } //Lawsuit ("CLW_LAWSUIT")
        public string RloiId { get; set; } //Lawsuit Owner ("REGOP_LAWSUIT_OWNER_INFO")
        public bool Solidary { get; set; } //Lawsuit Owner ("REGOP_LAWSUIT_OWNER_INFO")
        public string FIO { get; set; } //Lawsuit Owner ("REGOP_LAWSUIT_OWNER_INFO")
    }
}