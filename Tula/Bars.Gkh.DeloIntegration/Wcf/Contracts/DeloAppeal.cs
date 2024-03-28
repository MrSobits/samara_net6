namespace Bars.Gkh.DeloIntegration.Wcf.Contracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "DeloAppeal")]
    public class DeloAppeal
    {
        /// <summary> Номер обращения в системе </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExtId")]
        public string ExtId { get; set; }

        /// <summary> От </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public DateTime StartDate { get; set; }

        /// <summary> План </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TargetDate")]
        public DateTime TargetDate { get; set; }

        /// <summary> Гражданин </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Citizen")]
        public string Citizen { get; set; }

        /// <summary> Уникальный номер гражданина </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "isn_citizen")]
        public int CitizenId { get; set; }

        /// <summary> Адрес </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        /// <summary> Содержание </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Description")]
        public string Description { get; set; }

        /// <summary> Источник поступления </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Source")]
        public string Source { get; set; }

        /// <summary> Форма поступления </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Form")]
        public string Form { get; set; }

        /// <summary> Номер </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Num")]
        public string Num { get; set; }

        /// <summary> Дата поступления </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ReciveDate")]
        public DateTime ReciveDate { get; set; }

        /// <summary> Поручитель </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Guarantor")]
        public string Guarantor { get; set; }

        /// <summary> Должность </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Post")]
        public string Post { get; set; }

        /// <summary> Испольнитель </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Performer")]
        public string Performer { get; set; }

        /// <summary> Должность исполнителя </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PerformersPost")]
        public string PerformersPost { get; set; }

        /// <summary> Адресат </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Addressee")]
        public string Addressee { get; set; }

        /// <summary> Номер исходящего документа </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocNumber")]
        public string DocNumber { get; set; }

        /// <summary> Дата исходящего документа </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocDate")]
        public DateTime DocDate { get; set; }

        /// <summary> File </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "File")]
        public byte[] File { get; set; }

        /// <summary> Имя файла (вместе с расширением) </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }

        /// <summary> Cтатус обращения </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "State")]
        public string State { get; set; }
    }
}
