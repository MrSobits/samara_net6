namespace Bars.Gkh.Migrations._2017.Version_2017041700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017041700")]
    [MigrationDependsOn(typeof(Version_2017021000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017030600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017030601.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017031300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017040800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017041000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_FORMAT_DATA_EXPORT_HISTORY",
               new Column("START_DATE", DbType.DateTime),
               new Column("END_DATE", DbType.DateTime),
               new Column("FILE_PATH", DbType.String, 512),
               new Column("REMOTE_FILE_ID", DbType.Int64),
               new Column("REMOTE_TASK_ID", DbType.Int64),
               new Column("REMOTE_LOG_ID", DbType.Int64),
               new Column("REMOTE_STATUS", DbType.Int32, ColumnProperty.NotNull, 0),
               new Column("SELECTED_SECTION", DbType.Binary),
               new Column("UPLOAD_RESULT", DbType.Binary),
               new RefColumn("OPERATOR_ID", "GKH_FORMAT_DATA_EXPORT_HISTORY_OPERATOR", "GKH_OPERATOR", "ID"),
               new RefColumn("LOG_OPERATION_ID", "GKH_FORMAT_DATA_EXPORT_HISTORY_OPERATOR_LOG", "GKH_LOG_OPERATION", "ID"));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_FORMAT_DATA_EXPORT_HISTORY");
        }
    }
}