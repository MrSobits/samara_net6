namespace Bars.Gkh.Overhaul.Hmao.ConfigSections.OverhaulHmao
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Дома в программе
    /// </summary>
    public class HouseTypesConfig : IGkhConfigSection
    {
        /// <summary>
        /// Многоквартирный
        /// </summary>
        [GkhConfigProperty(DisplayName = "Многоквартирный")]
        [DefaultValue(false)]
        public virtual bool UseManyApartments { get; set; }

        /// <summary>
        /// Блокированной застройки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Блокированной застройки")]
        [DefaultValue(false)]
        public virtual bool UseBlockedBuilding { get; set; }

        /// <summary>
        /// Индивидуальный
        /// </summary>
        [GkhConfigProperty(DisplayName = "Индивидуальный")]
        [DefaultValue(false)]
        public virtual bool UseIndividual { get; set; }

        /// <summary>
        /// Общежитие
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общежитие")]
        [DefaultValue(false)]
        public virtual bool UseSocialBehavior { get; set; }
    }
}