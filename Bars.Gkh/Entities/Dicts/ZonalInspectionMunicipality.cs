namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Субтаблица зональной жилищной инспекции и Муниципального образования
    /// </summary>
    public class ZonalInspectionMunicipality : BaseGkhEntity
    {
        /// <summary>
        /// Зональная жилищная инспекция
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
