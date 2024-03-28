namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Нарушение мотивированного представления КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public class MotivatedPresentationViolation : BaseGkhEntity
    {
        /// <summary>
        /// Дом мотивированного представления
        /// </summary>
        public virtual MotivatedPresentationRealityObject MotivatedPresentationRealityObject { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji Violation { get; set; }
    }
}