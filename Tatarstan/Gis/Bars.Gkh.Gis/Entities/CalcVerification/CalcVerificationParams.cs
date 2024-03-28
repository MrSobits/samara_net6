namespace Bars.Gkh.Gis.Entities.CalcVerification
{
    using System;

    using Bars.Gkh.Gis.KP_legacy;

    public class CalcVerificationParams
    {
        public CalcTypes.ParamCalc ParamCalc { get; set; }
        public string Pref { get; set; }
        public string CentralPref { get; set; }
        public long? BillingHouseCode { get; set; }
        public int? HouseId { get; set; }
        public long? PersonalAccountId { get; set; }
        public bool IsHouseList { get; set; }
        public DateTime? DateCalc { get; set; }
        public long SchemaId { get; set; }
    }

    public class DeltaParams
    {
        public string ConnectionString { get; set; }
        public int? HouseId { get; set; }
        public long? BillingHouseCode { get; set; }
        public long? PersonalAccountId { get; set; }
        public int? DateMonth { get; set; }
        public int? DateYear { get; set; }
        public string CentralPref { get; set; }
        public string Pref { get; set; }
        public bool ShowNulls { get; set; }
        public bool ActGroupByService { get; set; }
        public bool ActGroupBySupplier { get; set; }
        public bool ActGroupByFormula { get; set; }
    }

}
