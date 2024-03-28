namespace Bars.Gkh.Map.Administration.ExecutionAction
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Entities.Administration.ExecutionAction;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ExecutionActionTaskMap : GkhBaseEntityMap<ExecutionActionTask>
    {
        /// <inheritdoc />
        public ExecutionActionTaskMap() : base("GKH_EXECUTION_ACTION_TASK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.PeriodType).Column("PERIOD_TYPE");
            this.Property(x => x.StartDate).Column("START_DATE");
            this.Property(x => x.EndDate).Column("END_DATE");
            this.Property(x => x.StartTimeHour).Column("START_TIME_HOUR");
            this.Property(x => x.StartTimeMinutes).Column("START_TIME_MINUTES");
            this.Property(x => x.StartDayOfWeekList).Column("START_DAY_OF_WEEK_LIST");
            this.Property(x => x.StartMonthList).Column("START_MONTH_LIST");
            this.Property(x => x.StartDaysList).Column("START_DAYS_LIST");

            this.Property(x => x.ActionCode).Column("ACTION_CODE");
            this.Property(x => x.BaseParams).Column("BASE_PARAMS");
            this.Property(x => x.IsDelete).Column("IS_DELETE");

            this.Reference(x => x.User).Column("USER_ID");
        }

        public class ExecutionActionTaskMapNHibernateMapping : ClassMapping<ExecutionActionTask>
        {
            public ExecutionActionTaskMapNHibernateMapping()
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

                this.Property(x => x.BaseParams, m =>
                {
                    m.Type<ImprovedBinaryJsonType<BaseParams>>();
                });
            }
        }
    }
}