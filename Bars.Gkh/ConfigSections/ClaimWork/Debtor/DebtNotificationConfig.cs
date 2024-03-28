namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class DebtNotificationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Печать уведомления
        /// </summary>
        [GkhConfigProperty(DisplayName = "Печать уведомления")]
        [DefaultValue(PrintType.Print)]
        public virtual PrintType PrintType { get; set; }

        /// <summary>
        /// Карточка уведомления
        /// </summary>
        [GkhConfigProperty(DisplayName = "Карточка уведомления")]
        [DefaultValue(DocumentFormationType.Form)]
        public virtual DocumentFormationType NotifFormationKind { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сумма задолженности")]
        [DefaultValue(NotifSumType.WithoutPenalty)]
        public virtual NotifSumType NotifDebtSumType { get; set; }

        /// <summary>
        /// Размер задолженности (руб.)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Размер задолженности (руб.)")]
        public virtual decimal NotifDebtSum { get; set; }
    }
}