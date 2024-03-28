using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.FormatDataExport.ExportableEntities;
using System.Data;
using Bars.Gkh.Utils;

namespace Bars.GkhCr.Migrations._2023.Version_2023123101
{
    [Migration("2023123101")]
    [MigrationDependsOn(typeof(_2023.Version_2023082100.UpdateSchema))]
    // Является Version_2018072400 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public const string PkrExportId = "GKH_PKR_EXPORT_ID_SEQ";
        public override void Up()
        {
            this.Database.RemoveSequence(PkrExportId);

            this.Database.AddSequence(PkrExportId);
            this.Database.AddExportId("CR_DICT_PROGRAM", PkrExportId);
            this.Database.AddExportId("OVRHL_PRG_VERSION", PkrExportId);
        }

        public override void Down()
        {
            this.Database.RemoveExportId("OVRHL_PRG_VERSION");
            this.Database.RemoveExportId("CR_DICT_PROGRAM");
            this.Database.RemoveSequence(PkrExportId);
        }
    }
}