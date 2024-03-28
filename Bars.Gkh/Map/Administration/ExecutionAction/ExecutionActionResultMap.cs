namespace Bars.Gkh.Map.Administration.ExecutionAction
{
    using Bars.B4;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Entities.Administration.ExecutionAction;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ExecutionActionResultMap : GkhBaseEntityMap<ExecutionActionResult>
    {
        /// <inheritdoc />
        public ExecutionActionResultMap() : base("GKH_EXECUTION_ACTION_RESULT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.StartDate).Column("START_DATE");
            this.Property(x => x.EndDate).Column("END_DATE");
            this.Property(x => x.Status).Column("STATUS");
            this.Property(x => x.Result).Column("RESULT");

            this.Reference(x => x.Task).Column("TASK_ID");
        }

        public class FormatDataExportResultMapNHibernateMapping : ClassMapping<ExecutionActionResult>
        {
            public FormatDataExportResultMapNHibernateMapping()
            {
                this.Property(x => x.Result, m =>
                {
                    m.Type<ImprovedBinaryJsonType<LoggingDataResult>>();
                });
            }
        }
    }
}