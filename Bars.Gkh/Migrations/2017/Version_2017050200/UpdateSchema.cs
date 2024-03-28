namespace Bars.Gkh.Migrations._2017.Version_2017050200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    [Migration("2017050200")]
    [MigrationDependsOn(typeof(Version_2017041700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017042000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017042100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.AddContragentId(FormatDataExportSequences.ContragentExportId);
            this.AddServiceId(FormatDataExportSequences.DictUslugaExportId);
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.RemoveContragentId(FormatDataExportSequences.ContragentExportId);
            this.RemoveServiceId(FormatDataExportSequences.DictUslugaExportId);
        }

        private void AddContragentId(string sequenceName)
        {
            this.Database.AddSequence(sequenceName);

            this.AddExportId("GKH_CONTRAGENT", sequenceName);
            this.AddExportId("OVRHL_CREDIT_ORG", sequenceName);
        }

        private void RemoveContragentId(string sequenceName)
        {
            this.RemoveExportId("GKH_CONTRAGENT");
            this.RemoveExportId("OVRHL_CREDIT_ORG");

            this.Database.RemoveSequence(sequenceName);
        }

        private void AddServiceId(string sequenceName)
        {
            this.Database.AddSequence(sequenceName);

            this.AddExportId("GKH_MAN_ORG_ADD_SERVICE", sequenceName);
            this.AddExportId("GKH_MAN_ORG_AGR_SERVICE", sequenceName);
            this.AddExportId("GKH_MAN_ORG_COM_SERVICE", sequenceName);
            this.AddExportId("GKH_RO_PUBRESORG_SERVICE", sequenceName);
        }

        private void RemoveServiceId(string sequenceName)
        {
            this.RemoveExportId("GKH_MAN_ORG_ADD_SERVICE");
            this.RemoveExportId("GKH_MAN_ORG_AGR_SERVICE");
            this.RemoveExportId("GKH_MAN_ORG_COM_SERVICE");
            this.RemoveExportId("GKH_RO_PUBRESORG_SERVICE");

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