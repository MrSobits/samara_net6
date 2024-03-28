namespace Bars.Gkh.ConfigSections.RegOperator.Administration
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.RegOperator.Enums.Administration;

    /// <summary>
    ///     Настройки лога операций для модуля Регоператор
    /// </summary>
    [GkhConfigSection("RegOperatorLogsConfig", UIParent = "Administration.Logs.Operation")]
    [DisplayName(@"Региональный фонд")]
    public class RegOperatorLogsConfig : IGkhConfigSection
    {
        /// <summary>
        ///     Формирование квитанций
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Лог формирования квитанций")]
        [DefaultValue(PaymentDocumentsLogLevel.None)]
        public virtual PaymentDocumentsLogLevel PaymentDocumentsLogLevel { get; set; }
    }
}