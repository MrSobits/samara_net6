namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.GkhDi.Entities;

    public class TemplateOtherServiceMap : BaseEntityMap<TemplateOtherService>
    {
        public TemplateOtherServiceMap() : base("Bars.GkhDi.Entities.TemplateOtherService", "DI_DICT_TEMPL_OTHER_SERVICE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(300);
            this.Property(x => x.Code, "Code").Column("CODE").Length(300);
            this.Property(x => x.Characteristic, "Characteristic").Column("CHARACTERISTIC").Length(300);
            this.Reference(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE_ID").Fetch();
        }
    }
}
