namespace Bars.Gkh.ConfigSections.RegOperator.Administration.BillingInfo
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Тип дома
    /// </summary>
    [DisplayName(@"Тип дома")]
    public class BillingInfoRealityObjectTypeConfig : IGkhConfigSection
    {
        /// <summary>
        /// Многоквартирный
        /// </summary>
        [GkhConfigProperty(DisplayName = "Многоквартирный")]
        [DefaultValue(false)]
        public virtual bool IsManyApartments { get; set; }

        /// <summary>
        /// Блокированной застройки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Блокированной застройки")]
        [DefaultValue(false)]
        public virtual bool IsBlockedBuilding { get; set; }

        /// <summary>
        /// Индивидуальный
        /// </summary>
        [GkhConfigProperty(DisplayName = "Индивидуальный")]
        [DefaultValue(false)]
        public virtual bool IsIndividual { get; set; }

        /// <summary>
        /// Общежитие
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общежитие")]
        [DefaultValue(false)]
        public virtual bool IsSocialBehavior { get; set; }

        /// <summary>
        /// Не задано
        /// </summary>
        [GkhConfigProperty(DisplayName = "Не задано")]
        [DefaultValue(true)]
        public virtual bool IsNotSet { get; set; }
    }
}