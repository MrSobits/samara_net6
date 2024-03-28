namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Этап указания к устранению нарушения в предписании
    /// </summary>
    public class ProtocolViol : InspectionGjiViolStage
    {
        /// <summary>
        /// Мероприятие по устранению
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}