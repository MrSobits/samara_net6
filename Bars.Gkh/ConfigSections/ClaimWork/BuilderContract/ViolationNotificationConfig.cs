namespace Bars.Gkh.ConfigSections.ClaimWork.BuilderContract
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class ViolationNotificationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Акт выявления нарушений
        /// </summary>
        [GkhConfigProperty(DisplayName = "Акт выявления нарушений")]
        public virtual DocumentFormationKind ViolActFormationKind { get; set; }

        /// <summary>
        /// Единица периода просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Единица периода просрочки")]
        [DefaultValue(PeriodKind.Day)]
        public virtual PeriodKind ViolActPeriodKind { get; set; }

        /// <summary>
        /// Период просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период просрочки")]
        public virtual int ViolActDelayDaysCount { get; set; }

        /// <summary>
        /// Документ уведомления
        /// </summary>
        [GkhConfigProperty(DisplayName = "Документ уведомления")]
        public virtual DocumentFormationKind NotifFormationKind { get; set; }

        /// <summary>
        /// Единица периода просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Единица периода просрочки")]
        [DefaultValue(PeriodKind.Day)]
        public virtual PeriodKind NotifFormPeriodKind { get; set; }

        /// <summary>
        /// Период просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период просрочки")]
        public virtual int NotifFormDelayDaysCount { get; set; }
    }
}