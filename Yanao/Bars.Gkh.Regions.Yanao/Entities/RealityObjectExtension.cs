namespace Bars.Gkh.Regions.Yanao.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    public class RealityObjectExtension : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Отсканированный документ - технический паспорт объекта
        /// </summary>
        public virtual FileInfo TechPassportScanFile { get; set; }
    }
}