namespace Bars.Gkh.Migrations._2017.Version_2017070300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Исправление миграции <see cref="Version_2017062600.UpdateSchema"/>
    /// </summary>
    [Migration("2017070300")]
    [MigrationDependsOn(typeof(Version_2017062600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.RemoveOldServiceId(FormatDataExportSequences.DictUslugaExportId);

            this.Database.AddSequence(FormatDataExportSequences.DictUslugaExportId);

            this.Database.AddExportId("GKH_DICT_MAN_CONTRACT_SERVICE", FormatDataExportSequences.DictUslugaExportId);
            this.Database.AddExportId("GIS_SERVICE_DICTIONARY", FormatDataExportSequences.DictUslugaExportId);
        }

        public override void Down()
        {
            this.Database.RemoveExportId("GKH_DICT_MAN_CONTRACT_SERVICE");
            this.Database.RemoveExportId("GIS_SERVICE_DICTIONARY");

            this.RemoveOldServiceId(FormatDataExportSequences.DictUslugaExportId);
        }

        private void RemoveOldServiceId(string sequenceName)
        {
            this.Database.RemoveExportId("GKH_MAN_ORG_ADD_SERVICE");
            this.Database.RemoveExportId("GKH_MAN_ORG_AGR_SERVICE");
            this.Database.RemoveExportId("GKH_MAN_ORG_COM_SERVICE");
            this.Database.RemoveExportId("GKH_RO_PUBRESORG_SERVICE");

            this.Database.RemoveExportId("GKH_DICT_MAN_CONTRACT_SERVICE");

            this.Database.RemoveSequence(sequenceName);
        }
    }
}