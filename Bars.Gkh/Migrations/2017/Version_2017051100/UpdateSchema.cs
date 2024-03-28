namespace Bars.Gkh.Migrations._2017.Version_2017051100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017051100")]
    [MigrationDependsOn(typeof(Version_2017050200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_FORMAT_DATA_EXPORT_TASK",
                new Column("IS_DELETE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("START_TIME_HOUR", DbType.Int32, ColumnProperty.NotNull),
                new Column("START_TIME_MINUTES", DbType.Int32, ColumnProperty.NotNull),
                new Column("PERIOD_TYPE", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("START_DAY_OF_WEEK_LIST", DbType.Binary),
                new Column("START_DAYS_LIST", DbType.Binary),
                new Column("START_MONTH_LIST", DbType.Binary),
                new Column("ENTITY_GROUP_CODE_LIST", DbType.Binary),
                new RefColumn("OPERATOR_ID", "GKH_FORMAT_DATA_EXPORT_TASK_OPERATOR", "GKH_OPERATOR", "ID")
            );

            this.Database.AddEntityTable("GKH_FORMAT_DATA_EXPORT_RESULT",
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("PROGRESS", DbType.Single, ColumnProperty.NotNull, 0f),
                new Column("STATUS", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("ENTITY_CODE_LIST", DbType.Binary),

                new RefColumn("LOG_OPERATION_ID", "GKH_FORMAT_DATA_EXPORT_RESULT_LOG", "GKH_LOG_OPERATION", "ID"),
                new RefColumn("TASK_ID", "GKH_FORMAT_DATA_EXPORT_RESULT_TASK", "GKH_FORMAT_DATA_EXPORT_TASK", "ID")
            );
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_FORMAT_DATA_EXPORT_RESULT");
            this.Database.RemoveTable("GKH_FORMAT_DATA_EXPORT_TASK");
        }
    }
}