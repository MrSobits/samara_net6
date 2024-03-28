namespace Bars.GkhGji.Regions.Nso.Entities
{
	using Bars.Gkh.Entities;
	using Bars.GkhGji.Entities;

	/// <summary>
    /// Инспектируемая часть в акте проверки предписания
    /// </summary>
    public class ActRemovalInspectedPart : BaseGkhEntity
    {
        /// <summary>
        /// Акт проверки предписания
        /// </summary>
        public virtual ActRemoval ActRemoval { get; set; }

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