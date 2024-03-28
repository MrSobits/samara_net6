namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncDictionaries
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "DictionaryRecord")]
    public class DictionaryRecord
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Attr1")]
        public string Attr1 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Attr2")]
        public string Attr2 { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ObjectEditDate")]
        public DateTime ObjectEditDate { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "DictionaryGJI")]
    public class DictionaryGJI
    {
        [DataMember]
        [XmlArray(ElementName = "DictionaryRecords")]
        public DictionaryRecord[] DictionaryRecords { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DictionaryCode")]
        public string DictionaryCode { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DictionaryName")]
        public string DictionaryName { get; set; }
    }
}