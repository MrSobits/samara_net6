namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сущность связи между Нарушением и Мероприятиями по устранению нарушений
    /// </summary>
    public class ViolationActionsRemovGji : BaseEntity
    {
        /// <summary>
        /// Мероприятия по устранению нарушений
        /// </summary>
        public virtual ActionsRemovViol ActionsRemovViol { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji ViolationGji { get; set; }
    }
}