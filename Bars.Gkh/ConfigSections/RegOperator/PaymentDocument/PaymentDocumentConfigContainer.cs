namespace Bars.Gkh.ConfigSections.RegOperator
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;

    using Config.Attributes.UI;

    /// <summary>
    /// Настройки печати квитанций
    /// </summary>
    public class PaymentDocumentConfigContainer : IGkhConfigSection
    {
        /// <summary>
        /// Общие настройки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общие")]
        [Navigation]
        public virtual PaymentDocumentConfigCommon PaymentDocumentConfigCommon { get; set; }

        /// <summary>
        /// Настройка печати квитанций (физ.лица)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка печати квитанций (физ.лица)")]
        [Navigation]
        public virtual PaymentDocumentConfigIndividual PaymentDocumentConfigIndividual { get; set; }

        /// <summary>
        /// Настройка печати квитанций (юр.лица)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка печати квитанций (юр.лица)")]
        [Navigation]
        public virtual PaymentDocumentConfigLegal PaymentDocumentConfigLegal { get; set; }

        /// <summary>
        /// Настройка источников данных
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка источников данных")]
        [Navigation]
        public virtual PaymentDocumentConfigSource PaymentDocumentConfigSource { get; set; }
    }
}