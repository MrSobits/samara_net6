namespace Bars.GkhGji.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Вложение
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "AppealAttachment")]
    public class AppealAttachment
    {
        /// <summary>
        /// Уникальное наименование файла (путь к файлу)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "UniqueName")]
        [Validation(Description = "Уникальное наименование файла (путь к файлу)",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 300)]
        public string UniqueName { get; set; }

        /// <summary>
        /// Наименование вложения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        [Validation(Description = "Наименование вложения",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 100)]
        public string Name { get; set; }

        /// <summary>
        /// Описание вложения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Description")]
        [Validation(Description = "Описание вложения",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 500)]
        public string Description { get; set; }
    }
}
