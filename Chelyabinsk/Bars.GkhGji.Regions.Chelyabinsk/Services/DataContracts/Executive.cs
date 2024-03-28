namespace Bars.GkhGji.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Кем рассмотрено обращение (руководитель)
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "Executive")]
    public class Executive
    {
        /// <summary>
        /// ФИО руководителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Fio")]
        [Validation(Description = "ФИО руководителя",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 300)]
        public string Fio { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhoneNumber")]
        [Validation(Description = "Номер телефона руководителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 50)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        [Validation(Description = "E-mail руководителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 100)]
        public string Email { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Post")]
        [Validation(Description = "Должность руководителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 200)]
        public string Post { get; set; }
    }
}