namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Связь цели проверки с заданием КНМ
    /// </summary>
    public class TaskActionIsolatedSurveyPurpose : BaseEntity
    {
        /// <summary>
        /// Задание КНМ
        /// </summary>
        public virtual TaskActionIsolated TaskActionIsolated { get; set; }
        
        /// <summary>
        /// Цели проверки
        /// </summary>
        public virtual SurveyPurpose SurveyPurpose { get; set; }
    }
}