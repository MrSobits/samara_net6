namespace Bars.Gkh.Map.HousingInspection
{
    using Bars.Gkh.Entities.HousingInspection;

    public class HousingInspectionMap : BaseImportableEntityMap<HousingInspection>
    {
        public HousingInspectionMap()
            : base("Жилищная инспекция", "GKH_HOUSING_INSPECTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull();
        }
    }
}