namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.Collections.Generic;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Config.Attributes.UI;
    using PaymentDocument;

    /// <summary>
    /// Настройка номера квитанций для юр. лица
    /// </summary>
    //[GkhConfigSection("PaymentDocument", DisplayName = "Настройки печати квитанций?", UIParent = typeof(PaymentDocumentConfigLegal))]
    [Navigation]
    public class PaymentDocumentConfigLegalNumber : IGkhConfigSection
    {
        /// <summary>
        /// Организационно-правовые формы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка номера квитанций для юр. лица")]
        [GkhConfigPropertyEditor("B4.ux.config.LegalNumberEditor", "legalnumbereditor")]
        public virtual List<NumberBuilderConfig> NumberBuilderConfigs { get; set; }
    }
}