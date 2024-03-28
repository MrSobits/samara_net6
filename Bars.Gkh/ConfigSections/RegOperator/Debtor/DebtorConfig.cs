namespace Bars.Gkh.ConfigSections.RegOperator.Debtor
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройки Реестра должников
    /// </summary>
    public class DebtorConfig : IGkhConfigSection
    {
        /// <summary>
        /// Способ формирования фонда в Реестре должников
        /// </summary>
        [GkhConfigProperty(DisplayName = "Способ формирования фонда в Реестре должников")]
        [Permissionable]
        public virtual FundFormationVariant FundFormationVariant { get; set; }


        /// <summary>
        /// Параметры настройки реестра неплательщиков
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры настройки Реестра должников")]
        [Permissionable]
        public virtual DebtorRegistryConfig DebtorRegistryConfig { get; set; }
    }
}