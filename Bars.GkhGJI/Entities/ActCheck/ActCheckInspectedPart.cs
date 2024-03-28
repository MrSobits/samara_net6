namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Инспектируемая часть в акте проверки
    /// </summary>
    public class ActCheckInspectedPart : BaseGkhEntity
    {
        /// <summary>
        /// Акт обследования
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

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