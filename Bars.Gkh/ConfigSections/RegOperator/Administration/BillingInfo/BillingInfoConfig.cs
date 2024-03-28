namespace Bars.Gkh.ConfigSections.RegOperator.Administration.BillingInfo
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Импорт сведений от биллинга
    /// </summary>
    [Permissionable]
    [DisplayName(@"Импорт сведений от биллинга")]
    public class BillingInfoConfig : IGkhConfigSection
    {
        /// <summary>
        /// Примечание
        /// </summary>
        [GkhConfigProperty(DisplayName = "Примечание")]
        [GkhConfigPropertyEditor("B4.ux.config.BillingInfo", "billinginfo")]
        public virtual int BillingInfo { get; set; }

        /// <summary>
        /// Способ формирования фонда
        /// </summary>
        [GkhConfigProperty(DisplayName = "Способ формирования фонда")]
        public virtual BillingInfoFundFormManagementConfig FundFormManagement { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        [GkhConfigProperty(DisplayName = "Тип дома")]
        public virtual BillingInfoRealityObjectTypeConfig RealityObjectType { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        [GkhConfigProperty(DisplayName = "Состояние дома")]
        public virtual BillingInfoRealityObjectStateConfig RealityObjectState { get; set; }
    }
}