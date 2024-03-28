namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Этап наказания за нарушение в протоколе
    /// </summary>
    public class ProtocolViolation : InspectionGjiViolStage
    {
        // тут пока нет никаких конкретных свойств касаемых наказания за нарушение
        // просто эта табличка служит таблицей связи нарушения с протоколом чтобы понимать за какие нарушения
        // идет наказание

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}