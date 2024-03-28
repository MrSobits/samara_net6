namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Задача проверки приказа ГЖИ
    /// </summary>
    public class DisposalSurveyObjective : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение ГЖИ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Задача проверки
        /// </summary>
        public virtual SurveyObjective SurveyObjective { get; set; }

        /// <summary>
        /// Редактируемое поле задачи проверки
        /// </summary>
        public virtual string Description { get; set; }
    }
}