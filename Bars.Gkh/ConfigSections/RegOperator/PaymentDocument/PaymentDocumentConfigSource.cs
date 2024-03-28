namespace Bars.Gkh.ConfigSections.RegOperator.PaymentDocument
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройка источников данных
    /// </summary>
    [Navigation]
    public class PaymentDocumentConfigSource : IGkhConfigSection
    {
        /// <summary>
        /// Дерево настроек источников данных
        /// </summary>
        [GkhConfigProperty(DisplayName = "Дерево настроек источников данных")]
        [GkhConfigPropertyEditor("B4.ux.config.PaymentSourceTreeEditor", "paymentsourcetreeeditor")]
        public virtual int PaymentDocumentSourceTree { get; set; }
    }
}