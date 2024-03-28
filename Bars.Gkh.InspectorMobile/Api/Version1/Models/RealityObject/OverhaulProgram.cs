namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.RealityObject
{
    /// <summary>
    /// Модель программы капитального ремонта
    /// </summary>
    public class OverhaulProgram
    {
        /// <summary>
        /// Уникальный идентификатор объекта КР
        /// </summary>
        public long ObjectId { get; set; }
        
        /// <summary>
        /// Наименование программы
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// Период программы
        /// </summary>
        public string ProgramPeriod { get; set; }
    }
}