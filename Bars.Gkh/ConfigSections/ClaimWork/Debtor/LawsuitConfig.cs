namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class LawsuitConfig : IGkhConfigSection
    {
        /// <summary>
        /// Печать искового заявления
        /// </summary>
        [GkhConfigProperty(DisplayName = "Печать искового заявления")]
        [DefaultValue(PrintType.Print)]
        public virtual PrintType PrintType { get; set; }

        /// <summary>
        /// Карточка искового заявления
        /// </summary>
        [GkhConfigProperty(DisplayName = "Карточка искового заявления")]
        [DefaultValue(DocumentFormationKind.Form)]
        public virtual DocumentFormationKind LawsuitFormationKind { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сумма задолженности")]
        [DefaultValue(DebtSumType.WithoutPenalty)]
        public virtual DebtSumType LawsuitDebtSumType { get; set; }

        /// <summary>
        /// Размер задолженности (руб.)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Размер задолженности (руб.)")]
        public virtual decimal LawsuitDebtSum { get; set; }
    }
}