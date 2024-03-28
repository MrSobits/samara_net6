namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тип обследования рапоряжения ГЖИ
    /// </summary>
    public class DisposalTypeSurvey : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение ГЖИ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// тип обследования
        /// </summary>
        public virtual TypeSurveyGji TypeSurvey { get; set; }
    }
}