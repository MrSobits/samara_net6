namespace Bars.B4.Modules.Analytics.Reports.Map
{
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.B4.Modules.Analytics.Reports.Entities.StoredReport"</summary>
    public class StoredReportMap : BaseEntityMap<StoredReport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public StoredReportMap() : 
                base("Bars.B4.Modules.Analytics.Reports.Entities.StoredReport", "AL_STORED_REPORT")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Category, "Категория.").Column("CATEGORY_ID");
            this.Property(x => x.Code, "Код").Column("CODE").Length(250);
            this.Property(x => x.Name, "Системное наименование").Column("NAME").Length(250);
            this.Property(x => x.DisplayName, "Отоброжаемое наименование (может меняться в регионах)").Column("DISPLAY_NAME").Length(250);
            this.Property(x => x.StoredReportType, "ReportType").Column("REPORT_TYPE");
            this.Property(x => x.ReportEncoding, "кодировка").Column("REPORT_ENCODING");
            this.Property(x => x.TemplateFile, "TemplateFile").Column("TEMPLATE");
            this.Property(x => x.ForAll, "Доступен этот отчет для всех ролей или нет").Column("FOR_ALL");
            this.Property(x => x.GenerateOnCalcServer, "Генерировать отчёт на сервере расчетов").Column("ON_CALC_SERVER");
            this.Property(x => x.UseTemplateConnectionString, "Не переопределять определенные в шаблоне строки подключения").Column("USE_TPL_CONN");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
        }
    }

    /// <summary>
    /// Маппигш
    /// </summary>
    public class StoredReportNHibernateMapping : ClassMapping<StoredReport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public StoredReportNHibernateMapping()
        {
            this.Bag(
                x => x.DataSources,
                collectionMapping =>
                    {
                        collectionMapping.Table("AL_REPORT_DATASOURCE");
                        collectionMapping.Cascade(Cascade.None);
                        collectionMapping.Key(k => k.Column("STORED_REPORT_ID"));
                    },
                map => map.ManyToMany(p => p.Column("DATA_SOURCE_ID")));

            this.Bag<ReportParamGkh>(
                "reportParams",
                mapper =>
                    {
                        mapper.Table("AL_REPORT_PARAM");
                        mapper.Cascade(Cascade.DeleteOrphans);
                        mapper.Key(k => k.Column("REPORT_ID"));
                        mapper.Inverse(true);
                    },
                relation => relation.OneToMany());
        }
    }
}
