using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.FormatDataExport.ExportableEntities;
using System.Data;
using Bars.Gkh.Utils;

namespace Bars.GkhCr.Migrations._2023.Version_2023123102
{
    [Migration("2023123102")]
    [MigrationDependsOn(typeof(_2023.Version_2023123101.UpdateSchema))]
    // Является Version_2018081400 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public const string PkrDomExportId = "GKH_PKRDOM_EXPORT_ID_SEQ";
        public override void Up()
        {
            this.Database.RemoveSequence(PkrDomExportId);

            this.Database.AddSequence(PkrDomExportId);
            this.Database.AddExportId("CR_OBJECT", PkrDomExportId);
            this.Database.AddExportId("OVRHL_VERSION_REC", PkrDomExportId);
            this.Database.AddExportId("OVRHL_PUBLISH_PRG_REC", PkrDomExportId);
        }

        public override void Down()
        {
            this.Database.RemoveExportId("OVRHL_PUBLISH_PRG_REC");
            this.Database.RemoveExportId("OVRHL_VERSION_REC");
            this.Database.RemoveExportId("CR_OBJECT");
            this.Database.RemoveSequence(PkrDomExportId);
        }
    }
}