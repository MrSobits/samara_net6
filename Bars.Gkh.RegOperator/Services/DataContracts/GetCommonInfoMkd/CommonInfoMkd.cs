namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "CommonInfoMkd")]
    public class CommonInfoMkd
    {
        [DataMember]
        [XmlArray(ElementName = "Municipals")]
        public MunInion[] MunInion { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "MunInion")]
    public class MunInion
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement(ElementName = "MKD")]
        public Mkd Mkd { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Inhabitants")]
        public Count Inhabitants { get; set; }

        [DataMember]
        [XmlElement(ElementName = "EmergencyMKD")]
        public Mkd EmergencyMkd { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ProgramIncludedMKD")]
        public Mkd ProgramIncludedMkd { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "ProgramIncludedInhabitants")]
        public Count ProgramIncludedInhabitants { get; set; }

        [DataMember]
        [XmlElement(ElementName = "CreditPayments")]
        public SumType CreditPayments { get; set; }

        [DataMember]
        [XmlElement(ElementName = "PaidPayments")]
        public SumType PaidPayments { get; set; }

        [DataMember]
        [XmlElement(ElementName = "PlannedMKDKR")]
        public MkdKr PlannedMkdKKr { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FactMKDKR")]
        public MkdKr FactMkdKr { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "MkdKr")]
    public class MkdKr
    {
        [DataMember]
        [XmlAttribute(AttributeName = "TotalCount")]
        public int TotalCount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TotalArea")]
        public decimal TotalArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TotalCost")]
        public decimal TotalCost { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "SumType")]
    public class SumType
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public decimal Sum { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "Count")]
    public class Count
    {
        [DataMember]
        [XmlAttribute(AttributeName = "TotalCount")]
        public decimal TotalCount { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "Mkd")]
    public class Mkd
    {
        [DataMember]
        [XmlAttribute(AttributeName = "TotalCount")]
        public int TotalCount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TotalArea")]
        public decimal TotalArea { get; set; }
    }
}

