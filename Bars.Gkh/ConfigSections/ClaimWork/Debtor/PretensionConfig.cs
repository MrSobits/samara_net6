namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class PretensionConfig : IGkhConfigSection
    {
        /// <summary>
        /// Печать претензии
        /// </summary>
        [GkhConfigProperty(DisplayName = "Печать претензии")]
        [DefaultValue(PrintType.Print)]
        public virtual PrintType PrintType { get; set; }

        /// <summary>
        /// Карточка претензии
        /// </summary>
        [GkhConfigProperty(DisplayName = "Карточка претензии")]
        [DefaultValue(DocumentFormationType.Form)]
        public virtual DocumentFormationType PretensionFormationKind { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сумма задолженности")]
        [DefaultValue(DebtSumType.WithoutPenalty)]
        public virtual DebtSumType DebtSumType { get; set; }

        /// <summary>
        /// Размер задолженности (руб.)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Размер задолженности (руб.)")]
        public virtual decimal DebtSum { get; set; }

        /// <summary>
        /// Срок оплаты задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Срок оплаты задолженности")]
        public virtual int PaymentPlannedPeriod { get; set; }
    }
}