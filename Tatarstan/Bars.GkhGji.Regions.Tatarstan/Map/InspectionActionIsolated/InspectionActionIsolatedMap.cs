namespace Bars.GkhGji.Regions.Tatarstan.Map.InspectionActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;

    public class InspectionActionIsolatedMap: JoinedSubClassMap<InspectionActionIsolated>
    {
        /// <inheritdoc />
        public InspectionActionIsolatedMap()
            : base("Проверки по мероприятиям без взаимодействия с контролируемыми лицами", "GJI_INSPECTION_ACTIONISOLATED")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TypeForm, "Форма проверки").Column("TYPE_FORM");
            this.Reference(x => x.ActionIsolated, "КНМ без взаимодействия с контролируемыми лицами").Column("ACTION_ISOLATED_ID");
        }
    }
}