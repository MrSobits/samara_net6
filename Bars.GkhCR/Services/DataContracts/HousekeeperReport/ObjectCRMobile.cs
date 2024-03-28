namespace Bars.GkhCr.Services
{
    using Bars.GkhCr.Services.DataContracts;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Ответ на добавление отчета
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "ObjectCRMobileResponce")]
    public class ObjectCRMobileResponce
    {
        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectsCRMobile")]
        public ObjectCRMobile[] ObjectsCRMobile { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    /// <summary>
    /// Информация об объектах КР
    /// </summary>
    public class ObjectCRMobile
    {
        /// <summary>
        /// Id программы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectId")]
        public long ObjectId { get; set; }

        /// <summary>
        /// Наименование программы КПР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProgramName")]
        public string ProgramName { get; set; }

        /// <summary>
        /// Период проведения ремонта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Period")]
        public string Period { get; set; }     

        /// <summary>
        /// Файлы  отчета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Works")]
        public CRObjectWork[] Works { get; set; }
    }
 

    [DataContract]
    [XmlRoot(ElementName = "CRObjectWork")]
    public class CRObjectWork
    {
        [DataMember]
        [XmlElement(ElementName = "WorkId")]
        public long WorkId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "WorkName")]
        public string WorkName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ContractNumber")]
        public string ContractNumber { get; set; }       

        [DataMember]
        [XmlElement(ElementName = "ContractState")]
        public string ContractState { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DateFrom")]
        public DateTime DateFrom { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DateTo")]
        public DateTime DateTo { get; set; }
    }
}