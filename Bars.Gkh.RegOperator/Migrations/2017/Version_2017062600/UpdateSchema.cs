namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017062600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2017062600")]
    [MigrationDependsOn(typeof(_2016.Version_2016061600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017042200.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            //this.Database.RemoveExportId("REGOP_DEC_NOTIF");
            //this.Database.AddExportId("REGOP_CALC_ACC", FormatDataExportSequences.ContragentRschetExportId);
        }

        /// <inheritdoc/>
        public override void Down()
        {
            //this.Database.AddExportId("REGOP_DEC_NOTIF", FormatDataExportSequences.ContragentRschetExportId);
            //this.Database.RemoveExportId("REGOP_CALC_ACC");
        }
    }
}