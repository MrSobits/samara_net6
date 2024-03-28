namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Приложение задания КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public class TaskActionIsolatedArticleLaw: BaseEntity
    {
        /// <summary>
        /// Задание КНМ без взаимодействия с контролируемыми лицами
        /// </summary>
        public virtual TaskActionIsolated Task { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }
    }
}