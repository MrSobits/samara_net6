namespace Bars.GkhGji.Entities.SurveyPlan
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.Dict;

    using Newtonsoft.Json;

    /// <summary>
    ///     Контрагент плана проверки
    /// </summary>
    public class SurveyPlanContragent : BaseEntity
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
        ///     Причина исключения
        /// </summary>
        public virtual string ExclusionReason { get; set; }

        /// <summary>
        ///     Признак расчета даты на основании предыдущей проверки
        /// </summary>
        public virtual bool FromLastAuditDate { get; set; }

        /// <summary>
        ///     Сформированная проверка
        /// </summary>
        [JsonIgnore]
        public virtual BaseJurPerson Inspection { get; set; }

        /// <summary>
        ///     Признак исключеия из плана
        /// </summary>
        public virtual bool IsExcluded { get; set; }

        /// <summary>
        ///     Плановый месяц проверки
        /// </summary>
        public virtual Month PlanMonth { get; set; }

        /// <summary>
        ///     Плановый год проверки
        /// </summary>
        public virtual int PlanYear { get; set; }

        /// <summary>
        ///     Причина включения в план
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        ///     План проверки
        /// </summary>
        public virtual SurveyPlan SurveyPlan { get; set; }
    }
}