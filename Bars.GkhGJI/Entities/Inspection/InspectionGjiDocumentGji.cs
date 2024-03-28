namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Таблица связи Проверки ГЖИ и документов оснований
    /// </summary>
    public class InspectionGjiDocumentGji : BaseEntity
    {
        /// <summary>
        /// Основание обращение граждан ГЖИ
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji Document { get; set; }
    }
}