namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Запись для отправки начисления штрафа в гис гмп
    /// </summary>
    public class GisChargeToSend : BaseEntity
    {
        /// <summary>
        /// Постановление
        /// </summary>
        public virtual DocumentGji Document { get; set; }

        /// <summary>
        /// Дата и время отправки
        /// </summary>
        public virtual DateTime? DateSend { get; set; }

        /// <summary>
        /// Флаг, отправлено или нет
        /// </summary>
        public virtual bool IsSent { get; set; }

        /// <summary>
        /// Json-объект, который нужно отправить
        /// </summary>
        public virtual GisChargeJson JsonObject { get; set; }

        /// <summary>
        /// Лог отправки начисления
        /// </summary>
        public virtual string SendLog { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GisChargeJson
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string PatternCode { get; set; }

        public string BillDate { get; set; }

        public string ValidUntil { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string TotalAmount { get; set; }

        public string BillFor { get; set; }

        [JsonProperty(PropertyName = "details_payment", NullValueHandling = NullValueHandling.Include)]
        public string Details { get; set; }

        [JsonProperty(PropertyName = "change_status", NullValueHandling=NullValueHandling.Include)]
        public string ChargeStatus { get; set; }

        public string OperationName { get; set; }

        public string Oktmo { get; set; }

        [JsonProperty(PropertyName = "supplier_billd")]
        public string SupplierBillId { get; set; }

        [JsonIgnore]
        public string Kbk { get; set; }

        [JsonIgnore]
        public string Okato { get; set; }

        [JsonIgnore]
        public string SupplierOrgId { get; set; }

        public GisChargeBudgetIndex BudgetIndex { get; set; }

        [JsonProperty(PropertyName = "payer_info", NullValueHandling = NullValueHandling.Include)]
        public GisChargeJsonPayer Payer { get; set; }

        [JsonProperty(PropertyName = "AdditionalData")]
        public List<GisJsonAdditionalData> AdditionalData { get; set; }

        [JsonIgnore]
        public GisChargeJsonSupplier Supplier { get; set; }

        [JsonIgnore]
        public GisChargeJsonAccount Account { get; set; }

        [JsonIgnore]
        public GisChargeJsonBank Bank { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GisChargeBudgetIndex
    {
        public string Status { get; set; }

        [JsonIgnore]
        public string PaymentType { get; set; }

        [JsonIgnore]
        public string Purpose { get; set; }

        [JsonIgnore]
        public string TaxPeriod { get; set; }

        [JsonIgnore]
        public string TaxDocNumber { get; set; }

        [JsonIgnore]
        public string TaxDocDate { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GisChargeJsonPayer
    {
        [JsonProperty(NullValueHandling=NullValueHandling.Include)]
        public string PayerType { get; set; }

        public string PayerSurname { get; set; }

        public string PayerName { get; set; }

        public string PayerPatronymic { get; set; }

        public string PayerBirthday { get; set; }

        public string PayerAddress { get; set; }

        [JsonProperty(PropertyName = "payer_u_code")]
        public string PayerCode { get; set; }

        [JsonProperty(PropertyName = "payer_u_caption")]
        public string PayerCaption { get; set; }

        public string PayerDoctype { get; set; }

        [JsonProperty(PropertyName = "payer_docnumber", NullValueHandling = NullValueHandling.Include)]
        public string PayerDocNumber { get; set; }

        public string PayerDocnat { get; set; }

        [JsonProperty(PropertyName = "payer_kpp", NullValueHandling=NullValueHandling.Include)]
        public string PayerKpp { get; set; }
    }

    public class GisChargeJsonSupplier
    {
        [JsonProperty(PropertyName = "INN")]
        public string Inn { get; set; }

        [JsonProperty(PropertyName = "KPP")]
        public string Kpp { get; set; }
    }

    public class GisChargeJsonAccount
    {
        [JsonProperty(PropertyName = "Account")]
        public string Account { get; set; }
    }

    public class GisChargeJsonBank
    {
        [JsonProperty(PropertyName = "BIK")]
        public string Bik { get; set; }
    }

    public class GisJsonAdditionalData
    {
        [JsonProperty(PropertyName = "AdditionalName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "AdditionalValue")]
        public string Value { get; set; }
    }
}