namespace Bars.GkhGji.Regions.Nso.Entities
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