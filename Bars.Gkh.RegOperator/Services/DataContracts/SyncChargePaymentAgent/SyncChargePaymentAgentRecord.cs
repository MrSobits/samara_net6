namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [Serializable]
    [XmlRoot(Namespace = "SyncChargePaymentAgent", ElementName = "FILE")]
    public class SyncChargePaymentAgentRecord
    {
        [DataMember]
        [XmlArray("PAYMENTS")]
        [XmlArrayItem("PAYMENT", typeof(SyncChargePayAgentPayment))]
        public SyncChargePayAgentPayment[] Payments { get; set; }

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
        [XmlAttribute("AGENT_INN")]
        public string AgentInn { get; set; }

        [DataMember]
        [XmlAttribute("REG_PAID_SUM")]
        public string RegPaidSum { get; set; }
    }

    [DataContract]
    [Serializable]
    [XmlType(Namespace = "SyncChargePaymentAgent", TypeName = "PAYMENT")]
    public class SyncChargePayAgentPayment
    {
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
        [XmlAttribute("PAID_SUM")]
        public string PaidSum { get; set; }

        [DataMember]
        [XmlAttribute("PAID_PENALTY")]
        public string PaidPenalty { get; set; }

        [DataMember]
        [XmlAttribute("PAID_DATE")]
        public string PaidDate { get; set; }       
    }

    [DataContract]
    [Serializable]
    [XmlRoot(Namespace = "SyncChargePaymentAgent", ElementName = "RESULT")]
    public class SyncPayAgentResult
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
    }
}