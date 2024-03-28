namespace Bars.GkhGji.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Регистратор обращения (оператор)
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "RegistrarOfAppeal")]
    public class RegistrarOfAppeal
    {
        /// <summary>
        /// ФИО регистратора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Fio")]
        [Validation(Description = "ФИО регистратора",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 300)]
        public string Fio { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhoneNumber")]
        [Validation(Description = "Номер телефона регистратора",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 50)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        [Validation(Description = "Email регистратора",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 100)]
        public string Email { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Post")]
        [Validation(Description = "Должность регистратора",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 200)]
        public string Post { get; set; }

        /// <summary>
        /// Наименование подразделения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SubdivisionName")]
        [Validation(Description = "Наименование подразделения регистратора",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 2000)]
        public string SubdivisionName { get; set; }
    }
}
