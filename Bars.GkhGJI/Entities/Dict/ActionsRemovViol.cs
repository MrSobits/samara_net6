namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Мероприятия по устранению нарушений
    /// </summary>
    public class ActionsRemovViol : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// ДНаименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}