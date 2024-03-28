namespace Bars.Gkh.ConfigSections.RegOperator.Administration.BillingInfo
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Состояние дома
    /// </summary>
    [DisplayName(@"Состояние дома")]
    public class BillingInfoRealityObjectStateConfig : IGkhConfigSection
    {
        /// <summary>
        /// Исправный
        /// </summary>
        [GkhConfigProperty(DisplayName = "Исправный")]
        [DefaultValue(false)]
        public virtual bool IsServiceable { get; set; }

        /// <summary>
        /// Аварийный
        /// </summary>
        [GkhConfigProperty(DisplayName = "Аварийный")]
        [DefaultValue(false)]
        public virtual bool IsEmergency { get; set; }

        /// <summary>
        /// Ветхий
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ветхий")]
        [DefaultValue(false)]
        public virtual bool IsDilapidated { get; set; }

        /// <summary>
        /// Снесен
        /// </summary>
        [GkhConfigProperty(DisplayName = "Снесен")]
        [DefaultValue(false)]
        public virtual bool IsRazed { get; set; }

        /// <summary>
        /// Не задано
        /// </summary>
        [GkhConfigProperty(DisplayName = "Не задано")]
        [DefaultValue(true)]
        public virtual bool IsNotSet { get; set; }
    }
}