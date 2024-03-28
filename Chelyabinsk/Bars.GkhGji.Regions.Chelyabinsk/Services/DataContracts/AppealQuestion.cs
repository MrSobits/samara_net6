namespace Bars.GkhGji.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Список вопросов, поставленных в обращении (из Тематического классификатора)
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "AppealQuestion")]
    public class AppealQuestion
    {
        /// <summary>
        /// Код вопроса в виде 0000.0000.0000.0000 !!! Изменена максимальная длина кода на 24 символа в соответствии с требованиями минсвязи
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        [Validation(Description = "Код вопроса в виде 0000.0000.0000.0000",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 24)]
        public string Code { get; set; }

        /// <summary>
        /// Наименование раздела
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SectionName")]
        [Validation(Description = "Наименование раздела",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 500)]
        public string SectionName { get; set; }

        /// <summary>
        /// Наименование тематики
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "StatementSubjectName")]
        [Validation(Description = "Наименование тематики",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 1000)]
        public string StatementSubjectName { get; set; }

        /// <summary>
        /// Наименование темы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SubsubjectName")]
        [Validation(Description = "Наименование темы",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 1000)]
        public string SubsubjectName { get; set; }

        /// <summary>
        /// Наименование вопроса
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "QuestionName")]
        [Validation(Description = "Наименование вопроса",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 1000)]
        public string QuestionName { get; set; }
    }
}