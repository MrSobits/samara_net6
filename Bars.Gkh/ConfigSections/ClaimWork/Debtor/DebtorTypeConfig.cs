namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    public class DebtorTypeConfig : IGkhConfigSection
    {
        /// <summary>
        /// Уведомление о задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Уведомление о задолженности")]
        [StringValue("Код формы", "NotificationClw")]
        public virtual DebtNotificationConfig DebtNotification { get; set; }

        /// <summary>
        /// Претензия
        /// </summary>
        [GkhConfigProperty(DisplayName = "Претензия")]
        [StringValue("Код формы", "Pretension")]
        public virtual PretensionConfig Pretension { get; set; }

        /// <summary>
        /// Заявление о выдаче судебного приказа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Заявление о выдаче судебного приказа")]
        [StringValue("Код формы", "CourtClaim")]
        public virtual CourtOrderClaimConfig CourtOrderClaim { get; set; }

        /// <summary>
        /// Исковое заявление
        /// </summary>
        [GkhConfigProperty(DisplayName = "Исковое заявление")]
        [StringValue("Код формы", "LawSuit")]
        public virtual LawsuitConfig Lawsuit { get; set; }

        /// <summary>
        /// Реструктуризация долга
        /// </summary>
        [GkhConfigProperty(DisplayName = "Реструктуризация долга")]
        [StringValue("Код формы", "RestructDebt")]
        public virtual RestructDebtConfig RestructDebt { get; set; }

        /// <summary>
        /// Реструктуризация по мировому соглашению
        /// </summary>
        [GkhConfigProperty(DisplayName = "Реструктуризация по мировому соглашению")]
        [StringValue("Код формы", "RestructDebtAmicAgr")]
        public virtual RestructDebtConfig RestructDebtAmicAgr { get; set; }
    }
}