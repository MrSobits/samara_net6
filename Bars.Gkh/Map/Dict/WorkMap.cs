namespace Bars.Gkh.Map.Dicts
{
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Работы"</summary>
    public class WorkMap : BaseImportableEntityMap<Work>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public WorkMap() : base("Работы", "GKH_DICT_WORK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.Code, "Код").Column("CODE").Length(10);
            this.Property(x => x.ReformCode, "Код реформы").Column("REFORM_CODE").Length(10);
            this.Property(x => x.WorkAssignment, "Назначение работ").Column("WORK_ASSIGNMENT");
            this.Property(x => x.Consistent185Fz, "Соответсвие 185 ФЗ").Column("CONSISTENT185FZ").NotNull();
            this.Property(x => x.IsAdditionalWork, "Дополнительная работа, нужно для ДПКР").Column("IS_ADDITIONAL_WORK").NotNull();
            this.Property(x => x.IsConstructionWork, "Работа (услуга) по строительству (Татарстан)").Column("IS_CONSTRUCTION_WORK").NotNull();
            this.Property(x => x.IsPSD, "Работа (услуга) по ПСД").Column("IS_PSD");
            this.Property(x => x.Normative, "Норматив").Column("NORMATIVE");
            this.Property(x => x.TypeWork, "Тип работ").Column("TYPE_WORK").NotNull();
            this.Reference(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE_ID").Fetch();
            this.Property(x => x.GisGkhCode, "Код ГИС ЖКХ").Column("GIS_GKH_CODE");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
        }
    }

    public class WorkNhMap : BaseHaveExportIdMapping<Work>
    {
    }
}
