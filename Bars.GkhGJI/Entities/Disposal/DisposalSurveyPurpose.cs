namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Цель проверки приказа ГЖИ
    /// </summary>
    public class DisposalSurveyPurpose : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение ГЖИ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Цель проверки
        /// </summary>
        public virtual SurveyPurpose SurveyPurpose { get; set; }

        /// <summary>
        /// Редактируемое поле цели проверки
        /// </summary>
        public virtual string Description { get; set; }
    }
}