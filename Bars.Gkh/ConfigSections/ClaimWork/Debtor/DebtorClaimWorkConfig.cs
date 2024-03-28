namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.ClaimWork;

    [GkhConfigSection("DebtorClaimWork", DisplayName = "Настройки реестра неплательщиков", UIParent = typeof(ClaimWorkConfig))]
    [Navigation]
    public class DebtorClaimWorkConfig : IGkhConfigSection
    {
        /// <summary>
        /// Общие
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общие")]
        [Navigation]
        public virtual GeneralConfig General { get; set; }

        /// <summary>
        /// Физ. лицо
        /// </summary>
        [GkhConfigProperty(DisplayName = "Физ. лицо")]
        [Navigation]
        public virtual DebtorTypeConfig Individual { get; set; }

        /// <summary>
        /// Юр. лицо
        /// </summary>
        [GkhConfigProperty(DisplayName = "Юр. лицо")]
        [Navigation]
        public virtual DebtorTypeConfig Legal { get; set; }
    }
}