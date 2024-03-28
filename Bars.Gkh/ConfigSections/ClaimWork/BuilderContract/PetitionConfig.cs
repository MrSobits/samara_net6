namespace Bars.Gkh.ConfigSections.ClaimWork.BuilderContract
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;

    public class PetitionConfig : IGkhConfigSection
    {
        /// <summary>
        /// Единица периода просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Единица периода просрочки")]
        [DefaultValue(PeriodKind.Day)]
        public virtual PeriodKind PetitionPeriodKind { get; set; }

        /// <summary>
        /// Период просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период просрочки")]
        public virtual int PetitionDelayDaysCount { get; set; }
    }
}