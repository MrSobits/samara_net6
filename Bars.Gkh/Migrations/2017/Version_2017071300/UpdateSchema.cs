namespace Bars.Gkh.Migrations._2017.Version_2017071300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2017071300")]
    [MigrationDependsOn(typeof(Version_2017070300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_EXECUTION_ACTION_TASK",
                new Column("PERIOD_TYPE", DbType.Int32, ColumnProperty.NotNull, (int) TaskPeriodType.NoPeriodicity),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("START_TIME_HOUR", DbType.Int32, ColumnProperty.NotNull),
                new Column("START_TIME_MINUTES", DbType.Int32, ColumnProperty.NotNull),
                new Column("START_DAY_OF_WEEK_LIST", DbType.Binary),
                new Column("START_MONTH_LIST", DbType.Binary),
                new Column("START_DAYS_LIST", DbType.Binary),
                new Column("ACTION_CODE", DbType.String, 64, ColumnProperty.NotNull),
                new Column("BASE_PARAMS", DbType.Binary),
                new Column("IS_DELETE", DbType.Boolean, ColumnProperty.NotNull, false),

                new RefColumn("USER_ID", "GKH_EXECUTION_ACTION_TASK_USER", "B4_USER", "ID")
            );

            this.Database.AddEntityTable("GKH_EXECUTION_ACTION_RESULT",
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("STATUS", DbType.Int32, ColumnProperty.NotNull, (int) ExecutionActionStatus.Initial),
                new Column("RESULT", DbType.Binary),

                new RefColumn("TASK_ID", "GKH_EXECUTION_ACTION_RESULT_TASK", "GKH_EXECUTION_ACTION_TASK", "ID")
            );

            this.FillData();
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_EXECUTION_ACTION_RESULT");
            this.Database.RemoveTable("GKH_EXECUTION_ACTION_TASK");
        }

        private void FillData()
        {
            var insertTasksSql = @"INSERT INTO GKH_EXECUTION_ACTION_TASK (
    OBJECT_VERSION,
    OBJECT_CREATE_DATE,
    OBJECT_EDIT_DATE,
    START_TIME_HOUR,
    START_TIME_MINUTES,
    ACTION_CODE,
    IS_DELETE
) SELECT 0, date_trunc('hour', now()), date_trunc('hour', now()), 0, 0, CODE, true
FROM GKH_EXECUTION_ACTION GROUP BY CODE;";

            var insertResultsSql = @"INSERT INTO GKH_EXECUTION_ACTION_RESULT (
    OBJECT_VERSION,
    OBJECT_CREATE_DATE,
    OBJECT_EDIT_DATE,
    START_DATE,
    END_DATE,
    STATUS,
    RESULT,
    TASK_ID
) SELECT a.OBJECT_VERSION,
    a.OBJECT_CREATE_DATE,
    a.OBJECT_EDIT_DATE,
    a.CREATE_DATE,
    case when a.END_DATE = '-infinity' then null else a.END_DATE end as END_DATE,
    a.STATUS,
    a.DATA_RESULT,
    t.ID
FROM GKH_EXECUTION_ACTION a
JOIN GKH_EXECUTION_ACTION_TASK t ON a.CODE = t.ACTION_CODE;";

            this.Database.ExecuteNonQuery(insertTasksSql);
            this.Database.ExecuteNonQuery(insertResultsSql);
        }
    }
}