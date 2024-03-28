namespace Bars.Gkh.Migrations._2017.Version_2017052000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017052000")]
    [MigrationDependsOn(typeof(Version_2017051100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.AddContragentId("GKH_CONTRAGENTRSCHET_EXPORT_ID_SEQ");
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.RemoveContragentId("GKH_CONTRAGENTRSCHET_EXPORT_ID_SEQ");
        }

        private void AddContragentId(string sequenceName)
        {
            this.Database.AddSequence(sequenceName);

            this.AddExportId("GKH_CONTRAGENT_BANK", sequenceName);
            this.AddExportId("REGOP_DEC_NOTIF", sequenceName);
        }

        private void RemoveContragentId(string sequenceName)
        {
            this.RemoveExportId("GKH_CONTRAGENT_BANK");
            this.RemoveExportId("REGOP_DEC_NOTIF");

            this.Database.RemoveSequence(sequenceName);
        }

        private void AddExportId(string tableName, string sequenceName)
        {
            if (this.Database.TableExists(tableName))
            {
                this.Database.AddColumn(tableName,
                    "EXPORT_ID",
                    DbType.Int64,
                    ColumnProperty.Unique | ColumnProperty.NotNull,
                    $"nextval('{sequenceName}'::regclass)");

                this.Database.AddIndex($"{tableName}_EXPORT_ID_IDX", true, tableName, "EXPORT_ID");
            }
        }

        private void RemoveExportId(string tableName)
        {
            if (this.Database.TableExists(tableName))
            {
                this.Database.RemoveColumn(tableName, "EXPORT_ID");
            }
        }
    }
}