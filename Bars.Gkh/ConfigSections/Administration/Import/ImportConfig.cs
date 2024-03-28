namespace Bars.Gkh.ConfigSections.Administration.Import
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.RegOperator.Administration.BillingInfo;

    /// <summary>
    /// Импорт
    /// </summary>
    [Permissionable]
    [DisplayName(@"Импорт")]
    public class ImportConfig : IGkhConfigSection
    {
        /// <summary>
        /// Импорт сведений от биллинга
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Импорт сведений от биллинга")]
        [Navigation]
        public virtual BillingInfoConfig BillingInfo { get; set; }
    }
}