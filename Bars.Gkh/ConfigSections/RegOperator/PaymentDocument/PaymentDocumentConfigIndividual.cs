namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.RegOperator.Enums;
    using Config.Attributes.UI;

    /// <summary>
    /// Настройка печати квитанций (физ.лица)
    /// </summary>
    [Navigation]
    public class PaymentDocumentConfigIndividual : IGkhConfigSection
    {
        /// <summary>
        /// Вид документов на оплату
        /// </summary>
        [GkhConfigProperty(DisplayName = "Вид документов на оплату")]
        [DefaultValue(PaymentDocumentFormat.Standard)]
        public virtual PaymentDocumentFormat PaymentDocFormat { get; set; }

        /// <summary>
        /// Ответственный по доставке квитанций
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ответственный по доставке квитанций")]
        public virtual string RepresentativeOnPrintReceipts { get; set; }

        /// <summary>
        /// Количество лицевых счетов в квитанции для физических лиц
        /// </summary>
        [GkhConfigProperty(DisplayName = "Количество лицевых счетов в квитанции для физических лиц")]
        public virtual int PhysicalAccountsPerDocument { get; set; }

        /// <summary>
        /// Параметры сортировки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры сортировки")]
        public virtual SortOptions SortOptions { get; set; }

        /// <summary>
        /// Параметры компоновки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры компоновки")]
        public virtual CompositionOptions CompositionOptions { get; set; }

        /// <summary>
        /// Параметры группировки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры группировки")]
        public virtual GroupingOptions GroupingOptions { get; set; }
    }
}