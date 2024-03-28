namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197
{
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Этап наказания за нарушение в протоколе 19.7
    /// </summary>
	public class Protocol197Violation : InspectionGjiViolStage
    {
        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}