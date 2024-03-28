namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "WorkAct")]
    public class WorkAct
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Number")]
        public string Number { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public DateTime Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public decimal Sum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// файл
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "App")]
        public File File { get; set; }
    }
}