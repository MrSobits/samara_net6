namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class CourtOrderClaimConfig : IGkhConfigSection
    {
        /// <summary>
        /// Печать заявления на выдачу судебного приказа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Печать заявления на выдачу судебного приказа")]
        [DefaultValue(PrintType.Print)]
        public virtual PrintType PrintType { get; set; }

        /// <summary>
        /// Карточка заявления на выдачу судебного приказа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Карточка заявления на выдачу судебного приказа")]
        [DefaultValue(DocumentFormationKind.Form)]
        public virtual DocumentFormationKind CourtOrderClaimFormationKind { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сумма задолженности")]
        [DefaultValue(DebtSumType.WithoutPenalty)]
        public virtual DebtSumType CourtOrderClaimDebtSumType { get; set; }

        /// <summary>
        /// Размер задолженности (руб.)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Размер задолженности (руб.)")]
        public virtual decimal CourtOrderClaimDebtSum { get; set; }
    }
}