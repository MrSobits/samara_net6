namespace Bars.Gkh.ConfigSections.ClaimWork
{
    using System.ComponentModel;

    using Bars.Gkh.Enums.ClaimWork;

    /// <summary>
    ///     Настройки рассмотрения дел
    /// </summary>
    [DisplayName(@"Настройки рассмотрения дел")]
    public class CourtProceedingConfig
    {
        /// <summary>
        /// Тип суда
        /// </summary>
        [DisplayName(@"Тип суда")]
        [DefaultValue(CourtType.NotSet)]
        public virtual CourtType CourtType { get; set; }

        /// <summary>
        ///     Основание
        /// </summary>
        [DisplayName(@"Основание")]
        [DefaultValue(ReasonType.NotSet)]
        public virtual ReasonType ReasonType { get; set; }

        /// <summary>
        ///     Тип должника
        /// </summary>
        [DisplayName(@"Тип должника")]
        [DefaultValue(DebtorType.NotSet)]
        public virtual DebtorType DebtorType { get; set; }

        /// <summary>
        ///     Минимальная сумма иска
        /// </summary>
        [DisplayName(@"Минимальная сумма иска")]
        public virtual decimal MinClaimAmount { get; set; }

        /// <summary>
        ///     Максимальная сумма иска
        /// </summary>
        [DisplayName(@"Максимальная сумма иска")]
        public virtual decimal MaxClaimAmount { get; set; }
    }
}