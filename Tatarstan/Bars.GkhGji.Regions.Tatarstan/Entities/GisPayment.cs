namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    using Newtonsoft.Json;

    public class GisPayment : BaseEntity
    {
        public virtual DateTime DateRecieve { get; set; }

        public virtual string Uip { get; set; }

        public virtual TatarstanResolutionPayFine PayFine { get; set; }

        public virtual GisPaymentJson JsonObject { get; set; }
    }

    public class GisPaymentJson
    {
        [JsonProperty("supplier_bill_id")]
        public string Uin { get; set; }

        [JsonProperty("payer_id")]
        public string Uip { get; set; }

        [JsonProperty("payment_date")]
        public string PaymentDate { get; set; }

        [JsonProperty("total_amount")]
        public string Amount { get; set; }

        [JsonProperty("change_status")]
        public string ChangeStatus { get; set; }

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }
    }
}