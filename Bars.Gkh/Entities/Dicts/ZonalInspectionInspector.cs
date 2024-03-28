namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Субтаблица зональной жилищной инспекции и инспекторов
    /// </summary>
    public class ZonalInspectionInspector : BaseGkhEntity
    {
        /// <summary>
        /// Зональная жилищная инспекция
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }
    }
}
