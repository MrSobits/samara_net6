namespace Bars.GkhGji.Entities.SurveyPlan
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    ///     Предрассчитанный кандидат на добавление в план
    /// </summary>
    public class SurveyPlanCandidate : BaseEntity
    {
        /// <summary>
        ///     Цель проверки
        /// </summary>
        public virtual AuditPurposeGji AuditPurpose { get; set; }

        /// <summary>
        ///     Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        ///     Признак расчета даты на основании предыдущей проверки
        /// </summary>
        public virtual bool FromLastAuditDate { get; set; }

        /// <summary>
        ///     Рассчитанный плановый месяц
        /// </summary>
        public virtual Month PlanMonth { get; set; }

        /// <summary>
        ///     Рассчитанный плановый год
        /// </summary>
        public virtual int PlanYear { get; set; }

        /// <summary>
        ///     Причина включения в план
        /// </summary>
        public virtual string Reason { get; set; }
    }
}