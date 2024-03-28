namespace Bars.GkhGji.Services.DataContracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Исполнитель
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "Executant")]
    public class Executant
    {
        /// <summary>
        /// Идентификатор подразделения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SubdivisionName")]
        [Validation(Description = "Идентификатор подразделения",
            TypeData = TypeData.Number,
            Required = true)]
        public long SubdivisionName { get; set; }

        /// <summary>
        /// ФИО исполнителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExecutantFio")]
        [Validation(Description = "ФИО исполнителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 300)]
        public string ExecutantFio { get; set; }

        /// <summary>
        /// Должность исполнителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExecutantPost")]
        [Validation(Description = "Должность исполнителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 300)]
        public string ExecutantPost { get; set; }

        /// <summary>
        /// Дата отправки исполнителю
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SendingDateToExecutant")]
        [Validation(Description = "Дата отправки исполнителю",
            TypeData = TypeData.Date,
            Required = true)]
        public DateTime SendingDateToExecutant { get; set; }

        /// <summary>
        /// ФИО назначающего лица
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FioOfAppointingPerson")]
        [Validation(Description = "ФИО назначающего лица",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 300)]
        public string FioOfAppointingPerson { get; set; }

        /// <summary>
        /// Номер телефона назначающего лица
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhoneNumberOfAppointingPerson")]
        [Validation(Description = "Номер телефона назначающего лица",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 50)]
        public string PhoneNumberOfAppointingPerson { get; set; }

        /// <summary>
        /// Должность назначающего лица
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PostOfAppointingPerson")]
        [Validation(Description = " Должность назначающего лица",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 200)]
        public string PostOfAppointingPerson { get; set; }
    }
}