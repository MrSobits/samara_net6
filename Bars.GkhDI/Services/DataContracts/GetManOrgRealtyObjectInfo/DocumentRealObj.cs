namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "DocumentRealObj")]
    public class DocumentRealObj
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// ИдентификаторФайла
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdFile")]
        public long FileId { get; set; }

        /// <summary>
        /// ИсходноеИмяФайла
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NameFile")]
        public string FileName { get; set; }
    }
}