namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Инспектируемая часть в акте обследования
    /// </summary>
    public class ActSurveyInspectedPart : BaseEntity
    {
        /// <summary>
        /// Акт обследования
        /// </summary>
        public virtual ActSurvey ActSurvey { get; set; }

        /// <summary>
        /// Инспектируемая часть
        /// </summary>
        public virtual InspectedPartGji InspectedPart { get; set; }

        /// <summary>
        /// Характер и местоположение
        /// </summary>
        public virtual string Character { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}