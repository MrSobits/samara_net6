namespace Bars.Gkh.ConfigSections.ClaimWork.BuilderContract
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;

    public class PretensionConfig : IGkhConfigSection
    {
        /// <summary>
        /// Единица периода просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Единица периода просрочки")]
        [DefaultValue(PeriodKind.Day)]
        public virtual PeriodKind PretensionPeriodKind { get; set; }

        /// <summary>
        /// Период просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период просрочки")]
        public virtual int PretensionDelayDaysCount { get; set; }

        /// <summary>
        /// Срок ответа на претензию(дни)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Срок ответа на претензию(дни)")]
        public virtual int PretensionAnswerDaysCount { get; set; }

        /// <summary>
        /// Срок оплаты задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Срок оплаты задолженности")]
        public virtual int PaymentPlannedPeriod { get; set; }
    }
}