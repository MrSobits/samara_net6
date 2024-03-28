namespace Bars.GkhGji.Entities
{
    using Enums;
    using Gkh.Entities;

    /// <summary>
    /// сущность связи основания проверки и документа гжи
    /// </summary>
    public class InspectionDocGjiReference : BaseGkhEntity
    {
        /// <summary>
        /// Тип связи
        /// </summary>
        public virtual TypeInspectionDocGjiReference TypeReference { get; set; }

        /// <summary>
        /// Основание проверки
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji Document { get; set; }
    }
}