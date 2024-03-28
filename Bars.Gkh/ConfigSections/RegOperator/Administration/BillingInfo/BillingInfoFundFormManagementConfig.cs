namespace Bars.Gkh.ConfigSections.RegOperator.Administration.BillingInfo
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Способ формирования фонда
    /// </summary>
    [DisplayName(@"Способ формирования фонда")]
    public class BillingInfoFundFormManagementConfig : IGkhConfigSection
    {
        /// <summary>
        /// Счет регионального оператора
        /// </summary>
        [GkhConfigProperty(DisplayName = "Счет регионального оператора")]
        [DefaultValue(false)]
        public virtual bool IsRegOpAccount { get; set; }

        /// <summary>
        /// Специальный счет регионального оператора
        /// </summary>
        [GkhConfigProperty(DisplayName = "Специальный счет регионального оператора")]
        [DefaultValue(false)]
        public virtual bool IsSpecialRegOpAccount { get; set; }

        /// <summary>
        /// Счет регионального оператора
        /// </summary>
        [GkhConfigProperty(DisplayName = "Специальный счет")]
        [DefaultValue(false)]
        public virtual bool IsSpecialAccount { get; set; }

        /// <summary>
        /// Не задано
        /// </summary>
        [GkhConfigProperty(DisplayName = "Не задано")]
        [DefaultValue(true)]
        public virtual bool IsNotSet { get; set; }
    }
}