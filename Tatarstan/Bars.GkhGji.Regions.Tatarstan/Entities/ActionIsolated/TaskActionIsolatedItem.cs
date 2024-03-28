namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Предмет задания КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public class TaskActionIsolatedItem : BaseEntity
    {
        /// <summary>
        /// Задание мероприятия
        /// </summary>
        public virtual TaskActionIsolated Task { get; set; }

        /// <summary>
        /// Предмет мероприятия
        /// </summary>
        public virtual SurveySubject Item { get; set; }
    }
}
