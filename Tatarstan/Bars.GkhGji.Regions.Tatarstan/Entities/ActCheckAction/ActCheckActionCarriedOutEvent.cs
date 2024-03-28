namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Выполненное мероприятие действия акта проверки
    /// </summary>
    public class ActCheckActionCarriedOutEvent : BaseEntity
    {
        /// <summary>
        /// Действие акта проверки
        /// </summary>
        public virtual ActCheckAction ActCheckAction { get; set; }

        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public virtual ActCheckActionCarriedOutEventType EventType { get; set; }
    }
}