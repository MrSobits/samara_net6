namespace Bars.GkhDi.Services.DataContracts.GetPeriods
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Period")]
    public class Period
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }
    }
}