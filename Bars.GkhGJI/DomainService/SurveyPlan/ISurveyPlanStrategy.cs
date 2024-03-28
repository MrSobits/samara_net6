namespace Bars.GkhGji.DomainService.SurveyPlan
{
    using System.Collections.Generic;

    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    ///     Стратегия расчета плановой даты проверки
    /// </summary>
    public interface ISurveyPlanStrategy
    {
        /// <summary>
        ///     Код цели проверки
        /// </summary>
        string Code { get; }

        /// <summary>
        ///     Цель проверки
        /// </summary>
        AuditPurposeGji Purpose { get; }

        /// <summary>
        ///     Создание списка кандидатов, подходящих под критерии стратегии
        /// </summary>
        IEnumerable<SurveyPlanCandidateProxy> CreatePlanItems();
    }
}