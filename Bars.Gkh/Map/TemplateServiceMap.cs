namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhDi.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.GkhDi.Entities.TemplateService"</summary>
    public class TemplateServiceMap : BaseImportableEntityMap<TemplateService>
    {
        public TemplateServiceMap()
            :
            base("Bars.GkhDi.Entities.TemplateService", "DI_DICT_TEMPL_SERVICE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "ExportId").Column("EXPORT_ID").NotNull();
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Name").Column("NAME").Length(300);
            this.Property(x => x.Code, "Code").Column("CODE").Length(300);
            this.Property(x => x.Characteristic, "Characteristic").Column("CHARACTERISTIC").Length(300);
            this.Property(x => x.Changeable, "Changeable").Column("CHANGEABLE").NotNull();
            this.Property(x => x.IsMandatory, "IsMandatory").Column("IS_MANDATORY").NotNull();
            this.Property(x => x.TypeGroupServiceDi, "TypeGroupServiceDi").Column("TYPE_GROUP_SERVICE").NotNull();
            this.Property(x => x.KindServiceDi, "KindServiceDi").Column("KIND_SERVICE").NotNull();
            this.Property(x => x.CommunalResourceType, "CommunalResourceType").Column("COMMUNAL_RESOURCE");
            this.Property(x => x.HousingResourceType, "HousingResourceType").Column("HOUSING_RESOURCE");
            this.Property(x => x.ActualYearStart, "YearStartActual").Column("ACTUAL_START_YEAR");
            this.Property(x => x.ActualYearEnd, "YearEndActual").Column("ACTUAL_END_YEAR");
            this.Property(x => x.IsConsiderInCalc, "IsConsiderInCalc").Column("IS_CONSIDER_IN_CALC");
            this.Reference(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE_ID").Fetch();
        }

        /// <summary>ReadOnly ExportId</summary>
        public class TemplateServiceNhMapping : BaseHaveExportIdMapping<TemplateService>
        {
        }
    }
}