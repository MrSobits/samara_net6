namespace Bars.Gkh.Map.Administration.FormatDataExport
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Entities.Administration.FormatDataExport;

    using NHibernate.Mapping.ByCode.Conformist;

    public class FormatDataExportTaskMap : GkhBaseEntityMap<FormatDataExportTask>
    {
        /// <inheritdoc />
        public FormatDataExportTaskMap() : base("GKH_FORMAT_DATA_EXPORT_TASK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.IsDelete).Column("IS_DELETE");
            this.Property(x => x.StartDate).Column("START_DATE");
            this.Property(x => x.EndDate).Column("END_DATE");
            this.Property(x => x.StartTimeHour).Column("START_TIME_HOUR");
            this.Property(x => x.StartTimeMinutes).Column("START_TIME_MINUTES");
            this.Property(x => x.PeriodType).Column("PERIOD_TYPE");

            this.Property(x => x.StartDayOfWeekList).Column("START_DAY_OF_WEEK_LIST");
            this.Property(x => x.StartDaysList).Column("START_DAYS_LIST");
            this.Property(x => x.StartMonthList).Column("START_MONTH_LIST");
            this.Property(x => x.EntityGroupCodeList).Column("ENTITY_GROUP_CODE_LIST");
            this.Property(x => x.BaseParams).Column("BASE_PARAMS");

            this.Reference(x => x.User).Column("USER_ID");
        }

        public class FormatDataExportTaskMapNHibernateMapping : ClassMapping<FormatDataExportTask>
        {
            public FormatDataExportTaskMapNHibernateMapping()
            {
                this.Property(x => x.StartDayOfWeekList, m =>
                {
                    m.Type<ImprovedBinaryJsonType<List<byte>>>();
                });

                this.Property(x => x.StartDaysList, m =>
                {
                    m.Type<ImprovedBinaryJsonType<List<byte>>>();
                });

                this.Property(x => x.StartMonthList, m =>
                {
                    m.Type<ImprovedBinaryJsonType<List<byte>>>();
                });

                this.Property(x => x.EntityGroupCodeList, m =>
                {
                    m.Type<ImprovedBinaryJsonType<List<string>>>();
                });
                this.Property(x => x.BaseParams, m =>
                {
                    m.Type<ImprovedBinaryJsonType<BaseParams>>();
                });
            }
        }
    }
}