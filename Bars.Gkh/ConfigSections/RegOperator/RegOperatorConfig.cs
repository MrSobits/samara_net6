namespace Bars.Gkh.ConfigSections.RegOperator
{
    using Bars.Gkh.ConfigSections.RegOperator.Debtor;
    using Bars.Gkh.ConfigSections.RegOperator.PaymentPenalties;

    using Config;
    using Config.Attributes;
    using Config.Attributes.UI;

    /// <summary>
    /// Настройки модуля "Региональный фонд"
    /// </summary>
    [GkhConfigSection("RegOperator", DisplayName = "Региональный фонд")]
    [Permissionable]
    public class RegOperatorConfig : IGkhConfigSection
    {
        /// <summary>
        /// Общие
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общие")]
        [Navigation]
        public virtual GeneralRegOpConfig GeneralConfig { get; set; }

        /// <summary>
        /// Настройки печати квитанций
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки печати квитанций")]
        [Navigation]
        public virtual PaymentDocumentConfigContainer PaymentDocumentConfigContainer { get; set; }

        /// <summary>
        /// Параметры начисления пени
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры начисления пени")]
        [Navigation]
        public virtual PaymentPenaltiesNodeConfig PaymentPenaltiesNodeConfig { get; set; }

        /// <summary>
        /// Настройки Реестра должников
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки Реестра должников")]
        [Navigation]
        public virtual DebtorConfig DebtorConfig { get; set; }

        /// <summary>
        /// Настройки рассылки квитанций
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки рассылки квитанций")]
        [Navigation]
        public virtual PaymentDocumentEmailConfig PaymentDocumentEmailConfig { get; set; }
    }
}
