namespace Bars.Gkh.Regions.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Nso.Enums;

    /// <summary>
    /// Документ жилого дома
    /// </summary>
    public class RealityObjectDocument : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual RealityObjectDocumentType DocumentType { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}