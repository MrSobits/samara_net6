namespace Bars.Gkh.Entities.HousingInspection
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Муниципальное образование жилищной инспекции
    /// </summary>
    public class HousingInspectionMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Жилищная инспекция
        /// </summary>
        public virtual HousingInspection HousingInspection { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}