namespace Bars.GkhGji.Regions.Tatarstan.Map.ConfigSections
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ConfigSections;

    public class GjiValidityDocPeriodMap : BaseEntityMap<GjiValidityDocPeriod>
    {
        public GjiValidityDocPeriodMap() :
            base("Период действия документа ГЖИ", "GJI_VALIDITY_DOC_PERIOD")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeDocument, "Тип документа").Column("TYPE_DOC");
            this.Property(x => x.StartDate, "Дата начала действия").Column("START_DATE");
            this.Property(x => x.EndDate, "Дата окончания действия").Column("END_DATE");
        }
    }
}
