namespace Bars.GkhGji.Entities.SurveyPlan
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    /// <summary>
    ///     План проверки
    /// </summary>
    public class SurveyPlan : BaseEntity, IStatefulEntity
    {
        /// <summary>
        ///     Наименование плана
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     План проверок юр лиц
        /// </summary>
        public virtual PlanJurPersonGji PlanJurPerson { get; set; }

        /// <summary>
        ///     Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}