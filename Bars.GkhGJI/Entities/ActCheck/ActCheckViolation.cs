namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Этап выявления нарушения в акте проверки
    /// Данная таблица служит связтю между нарушением и Актом проверки
    /// Чтобы понимать какие Нарушения были выявлены входе проверки
    /// Для выявленных нарушений ставится плановая дата устранения
    /// </summary>
    public class ActCheckViolation : InspectionGjiViolStage
    {
        /// <summary>
        /// Дом акта проверки
        /// </summary>
        public virtual ActCheckRealityObject ActObject { get; set; }
    }
}