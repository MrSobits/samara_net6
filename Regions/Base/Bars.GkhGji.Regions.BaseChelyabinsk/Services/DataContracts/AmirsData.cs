namespace Bars.GkhGji.Services.DataContracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl;

    /// <summary>
    /// AmirsData
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "AmirsData")]
    public class AmirsData
    {
        /// <summary>
        /// Дата протокола
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "prot_date")]
        [Validation(Description = "Дата протокола",
            TypeData = TypeData.Date,
            Required = true)]
        public DateTime prot_date { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "prot_num")]
        [Validation(Description = "Номер протокола",
            TypeData = TypeData.Text,
            Required = true)]
        public string prot_num { get; set; }

        /// <summary>
        /// uin
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "uin")]
        [Validation(Description = "uin",
            TypeData = TypeData.Text,
            Required = false)]
        public string uin { get; set; }

        /// <summary>
        /// Тип постановления
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "resolution_type")]
        [Validation(Description = "Тип постановления",
            TypeData = TypeData.Text,
            Required = true)]
        public string resolution_type { get; set; }

        /// <summary>
        /// Дата вступления в законную силу
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "in_law_date")]
        [Validation(Description = "Дата вступления в законную силу",
            TypeData = TypeData.Text,
            Required = false)]
        public string in_law_date { get; set; }

        /// <summary>
        /// Дата постановления
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "resolution_date")]
        [Validation(Description = "Дата постановления",
            TypeData = TypeData.Date,
            Required = true)]
        public DateTime resolution_date { get; set; }

        /// <summary>
        /// Номер постановления
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "resolution_num")]
        [Validation(Description = "Номер постановления",
            TypeData = TypeData.Text,
            Required = true)]
        public string resolution_num { get; set; }

        /// <summary>
        /// Санкция
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "sanction")]
        [Validation(Description = "Санкция",
            TypeData = TypeData.Text,
            Required = false)]
        public string sanction { get; set; }

        /// <summary>
        /// Штраф
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "penalty")]
        [Validation(Description = "Штраф",
            TypeData = TypeData.Text,
            Required = false)]
        public string penalty { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "executer")]
        [Validation(Description = "Исполнитель",
            TypeData = TypeData.Text,
            Required = false)]
        public string executer { get; set; }

        /// <summary>
        /// ИНН исполнителя
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "executer_inn")]
        [Validation(Description = "ИНН исполнителя",
            TypeData = TypeData.Text,
            Required = false)]
        public string executer_inn { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "fio")]
        [Validation(Description = "ФИО",
            TypeData = TypeData.Text,
            Required = false)]
        public string fio { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "doc_type")]
        [Validation(Description = "Тип документа",
            TypeData = TypeData.Text,
            Required = false)]
        public string doc_type { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "series")]
        [Validation(Description = "Серия",
            TypeData = TypeData.Text,
            Required = false)]
        public string series { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "number")]
        [Validation(Description = "Номер",
            TypeData = TypeData.Text,
            Required = false)]
        public string number { get; set; }

        /// <summary>
        /// Судебный участок
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "judical_office")]
        [Validation(Description = "Судебный участок",
            TypeData = TypeData.Text,
            Required = false)]
        public string judical_office { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "place_name")]
        [Validation(Description = "Адрес",
            TypeData = TypeData.Text,
            Required = false)]
        public string place_name { get; set; }
    }
}