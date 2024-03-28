namespace Bars.Gkh.Migrations._2017.Version_2017053100
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.Enums;

    [Migration("2017053100")]
    [MigrationDependsOn(typeof(Version_2017053000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            if (this.Database.TableExists("GKH_FORMAT_DATA_EXPORT_HISTORY"))
            {
                this.Database.RemoveTable("GKH_FORMAT_DATA_EXPORT_HISTORY");
            }

            this.Database.AddEntityTable("GKH_FORMAT_DATA_EXPORT_REMOTE_RESULT",
                new Column("FILE_ID", DbType.Int64, ColumnProperty.Null),
                new Column("TASK_ID", DbType.Int64, ColumnProperty.Null),
                new Column("LOG_ID", DbType.Int64, ColumnProperty.Null),
                new Column("STATUS", DbType.Int32, ColumnProperty.NotNull, (int) FormadDataExportRemoteStatus.Default),
                new Column("UPLOAD_RESULT", DbType.Binary, ColumnProperty.Null),
                new RefColumn("TASK_RESULT_ID", "GKH_FORMAT_DATA_EXPORT_REMOTE_RESULT_TASK_RESULT", "GKH_FORMAT_DATA_EXPORT_RESULT", "ID"));

        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_FORMAT_DATA_EXPORT_REMOTE_RESULT");
        }
    }
}
