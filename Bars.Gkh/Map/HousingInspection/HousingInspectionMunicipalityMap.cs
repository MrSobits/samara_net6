namespace Bars.Gkh.Map.HousingInspection
{
    using Bars.Gkh.Entities.HousingInspection;

    public class HousingInspectionMunicipalityMap : BaseImportableEntityMap<HousingInspectionMunicipality>
    {
        public HousingInspectionMunicipalityMap()
            : base("Муниципальное образование жилищной инспекции", "GKH_HOUSING_INSPECTION_MUNICIPALITY")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.HousingInspection, "Жилищная инспекция").Column("HOUSING_INSPECTION_ID").NotNull();
            this.Reference(x => x.Municipality, "Муниципально образование").Column("MUNICIPALITY_ID").NotNull();
        }
    }
}