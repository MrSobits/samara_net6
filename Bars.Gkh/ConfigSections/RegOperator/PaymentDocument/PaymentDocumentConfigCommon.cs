namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Enums.PaymentDocumentOptions;
    using Config.Attributes.UI;

    /// <summary>
    /// Настройки печати квитанций
    /// </summary>
    [Navigation]
    public class PaymentDocumentConfigCommon : IGkhConfigSection
    {
        /// <summary>
        /// Формат документов на оплату
        /// </summary>
        [GkhConfigProperty(DisplayName = "Формат документов на оплату")]
        [DefaultValue(FileFormat.Pdf)]
        public virtual FileFormat FileFormat { get; set; }

        /// <summary>
        /// Размер документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Размер документа")]
        [DefaultValue(PaperFormat.A4)]
        public virtual PaperFormat PaperFormat { get; set; }

        /// <summary>
        /// Опция, разрешающая формировать квитанции по открытому периоду
        /// </summary>
        [GkhConfigProperty(DisplayName = "Печать квитанций по открытому периоду")]
        public virtual bool PrintingInOpenPeriod { get; set; }

        /// <summary>
        /// Опции качества документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки качества документа")]
        public virtual DocumentQualityOptions QualityOptions { get; set; }
    }
}