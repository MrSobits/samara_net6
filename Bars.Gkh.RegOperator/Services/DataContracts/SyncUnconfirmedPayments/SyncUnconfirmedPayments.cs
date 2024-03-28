namespace Bars.Gkh.RegOperator.Services.DataContracts.SyncUnconfirmedPayments
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public class SyncUnconfirmedPaymentsCheckResult
    {
        [DataMember]
        [XmlAttribute("Code")]
        public int Code { get; set; }

        [DataMember]
        [XmlAttribute("Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlAttribute("Description")]
        public string Description { get; set; }
    }

    public class SyncUnconfirmedPaymentsPayResult
    {
        [DataMember]
        [XmlAttribute("Code")]
        public int Code { get; set; }

        [DataMember]
        [XmlAttribute("Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlAttribute("PlatID")]
        public string PlatID { get; set; }

        [DataMember]
        [XmlAttribute("Status")]
        public string Status { get; set; }

        [DataMember]
        [XmlAttribute("Date")]
        public string Date { get; set; }

        [DataMember]
        [XmlAttribute("Guid")]
        public string Guid { get; set; }
    }
}
