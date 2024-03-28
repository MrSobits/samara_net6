namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "CurrentDirector")]
    public class CurrentDirector
    {
        /// <summary>
        /// ФИОРуководителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FioDirector")]
        public string FioDirector { get; set; }

        /// <summary>
        /// ДатаНачалаРаботы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }
    }
}