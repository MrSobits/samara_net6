namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Этап выявления нарушения в доме для акта без взаимодействия
    /// </summary>
    public class ActIsolatedRealObjViolation : InspectionGjiViolStage
    {
        /// <summary>
        /// Дом акта без взаимодействия
        /// </summary>
        public virtual ActIsolatedRealObj ActIsolatedRealObj { get; set; }

        /// <summary>
        /// Результаты проведения мероприятия
        /// </summary>
        public virtual string EventResult { get; set; }

        /// <summary>
        /// Нехранимое поле c id нарушения ГЖИ
        /// </summary>
        public virtual long ViolationGjiId { get; set; }
    }
}