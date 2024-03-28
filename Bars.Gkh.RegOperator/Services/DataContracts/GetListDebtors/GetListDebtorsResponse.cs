namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetListDebtorsResponse")]
    public class GetListDebtorsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Debtors")]
        public DebtorProxy[] Debtors { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}