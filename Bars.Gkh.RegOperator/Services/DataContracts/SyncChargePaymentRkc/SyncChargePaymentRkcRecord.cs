namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [Serializable]
    [XmlRoot(ElementName = "FILE")]
    public class SyncChargePaymentRkcRecord
    {
        [DataMember]
        [XmlArray("PAYMENTS")]
        [XmlArrayItem("PAYMENT", typeof(SyncChargePayment))]
        public SyncChargePayment[] Payments { get; set; }

        [DataMember]
        [XmlAttribute("REG_DATE")]
        public string RegDate { get; set; }

        [DataMember]
        [XmlAttribute("REG_NUM")]
        public string RegNumber { get; set; }

        [DataMember]
        [XmlAttribute("FORMAT_VERSION")]
        public string FormatVersion { get; set; }

        [DataMember]
        [XmlAttribute("RKC_INN")]
        public string RkcInn { get; set; }

        [DataMember]
        [XmlAttribute("REG_CHARGE_SUM")]
        public string RegChargeSum { get; set; }

        [DataMember]
        [XmlAttribute("REG_PAID_SUM")]
        public string RegPaidSum { get; set; }
    }

    [DataContract]
    [Serializable]
    [XmlType(TypeName = "PAYMENT")]
    public class SyncChargePayment
    {
        public SyncChargePayment()
        {
            BalanceIn = string.Empty;
            BalanceOut = string.Empty;
            BalanceChange = string.Empty;
            ChargedSum = string.Empty;
            ChargedPenalty = string.Empty;
            ForPay = string.Empty;
            RecalcSum = string.Empty;
            PaidPenalty = string.Empty;
            PaidSum = string.Empty;
            PaidDate = string.Empty;
        }

        [DataMember]
        [XmlAttribute("PERSONAL_ACC")]
        public string AccNumber { get; set; }

        [DataMember]
        [XmlAttribute("PERSACC_NAME")]
        public string AccountOwnerName { get; set; }

        [DataMember]
        [XmlAttribute("MONTH")]
        public string Month { get; set; }

        [DataMember]
        [XmlAttribute("YEAR")]
        public string Year { get; set; }

        [DataMember]
        [XmlAttribute("BALANCEIN")]
        public string BalanceIn { get; set; }

        [DataMember]
        [XmlAttribute("BALANCECHANGE")]
        public string BalanceChange { get; set; }

        [DataMember]
        [XmlAttribute("BALANCEOUT")]
        public string BalanceOut { get; set; }

        [DataMember]
        [XmlAttribute("CHARGED_SUM")]
        public string ChargedSum { get; set; }

        [DataMember]
        [XmlAttribute("CHARGED_PENALTY")]
        public string ChargedPenalty { get; set; }

        [DataMember]
        [XmlAttribute("FORPAY")]
        public string ForPay { get; set; }

        [DataMember]
        [XmlAttribute("PAID_SUM")]
        public string PaidSum { get; set; }

        [DataMember]
        [XmlAttribute("PAID_PENALTY")]
        public string PaidPenalty { get; set; }

        [DataMember]
        [XmlAttribute("PAID_DATE")]
        public string PaidDate { get; set; }

        [DataMember]
        [XmlAttribute("RECALCSUM")]
        public string RecalcSum { get; set; }        
    }

    public class SyncRkcAccountResult
    {
        [DataMember]
        [XmlAttribute("KOD")]
        public string Code { get; set; }

        [DataMember]
        [XmlAttribute("DESCRIPTION")]
        public string Description { get; set; }

        [DataMember]
        [XmlAttribute("ACCNUMBER")]
        public string AccountNumber { get; set; }
    }

    [DataContract]
    [Serializable]
    [XmlRoot(ElementName = "RESULT")]
    public class SyncRkcResult
    {
        [DataMember]
        [XmlAttribute("KOD")]
        public string Code { get; set; }

        [DataMember]
        [XmlAttribute("LOG_FILE_ID")]
        public long LogFileId { get; set; }

        [DataMember]
        [XmlAttribute("DESCRIPTION")]
        public string Description { get; set; }

        [DataMember]
        [XmlElement("ACCRESULT")]
        public List<SyncRkcAccountResult> AccountResult { get; set; }

    }
}