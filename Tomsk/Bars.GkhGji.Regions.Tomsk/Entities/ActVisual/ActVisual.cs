namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Dict;

    /// <summary>
    /// Акт визуального осмотра
    /// </summary>
    public class ActVisual : DocumentGji
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Результат проверки
        /// </summary>
        public virtual string InspectionResult { get; set; }

        /// <summary>
        /// Вывод/заключение
        /// </summary>
        public virtual string Conclusion { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public virtual string Flat { get; set; }

        /// <summary>
        /// Час проверки
        /// </summary>
        public virtual int? Hour { get; set; }

        /// <summary>
        /// Минуты
        /// </summary>
        public virtual int? Minute { get; set; }

        /// <summary>
        /// Рамки проверки 
        /// </summary>
        public virtual FrameVerification FrameVerification { get; set; }
        
    }
}