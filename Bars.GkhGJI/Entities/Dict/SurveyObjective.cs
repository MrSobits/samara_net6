namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочники - ГЖИ - Задачи проверки
    /// </summary>
    public class SurveyObjective : BaseEntity
    {
        /// <summary>
        /// Код.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public virtual string Name { get; set; }
    }
}