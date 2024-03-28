namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Раздел Региональный фонд - Настройки рассылки квитанций
    /// </summary>
    [Permissionable]
    public class PaymentDocumentEmailConfig : IGkhConfigSection
    {
        /// <summary>
        /// Отправлять на электронную почту
        /// </summary>
        [GkhConfigProperty(DisplayName = "Отправлять на электронную почту")]
        [DefaultValue(PaymentDocumentEmailOwnerType.All)]
        public virtual PaymentDocumentEmailOwnerType PaymentDocumentEmailOwnerType { get; set; }

        /// <summary>
        /// Для рассылки квитанций
        /// </summary>
        [GkhConfigProperty(DisplayName = "Для рассылки квитанций")]
        public virtual PaymentDocumentEmailOptions PaymentDocumentEmailOptions { get; set; }
    }
}
