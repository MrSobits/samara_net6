namespace Bars.GkhGji.Services.DataContracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Источник поступления в ЕАИС «Обращения граждан»
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "SourceOfReceipt")]
    public class SourceOfReceipt
    {
        /// <summary>
        /// Источник поступления. Код из справочника «Источники поступления»
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SourceOfReceiptName")]
        [Validation(Description = "Источник поступления. Код из справочника «Источники поступления»",
            TypeData = TypeData.Number,
            Required = true)]
        public long SourceOfReceiptName { get; set; }

        /// <summary>
        /// Исходящий номер поступившего из другого ведомства обращения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SourceOfReceiptNumber")]
        [Validation(Description = "Исходящий номер поступившего из другого ведомства обращения",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 50)]
        public string SourceOfReceiptNumber { get; set; }

        /// <summary>
        /// Исходящая дата поступившего из другого ведомства обращения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "SourceOfReceiptDate")]
        [Validation(Description = "Исходящая дата поступившего из другого ведомства обращения",
            TypeData = TypeData.Date,
            Required = false)]
        public DateTime? SourceOfReceiptDate { get; set; }
    }
}
