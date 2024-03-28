using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.FormatDataExport.ExportableEntities;
using System.Data;
using Bars.Gkh.Utils;

namespace Bars.GkhCr.Migrations._2023.Version_2023123105
{
    [Migration("2023123105")]
    [MigrationDependsOn(typeof(_2023.Version_2023123104.UpdateSchema))]
    // Является Version_2018092000 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public const string PkrDomExportId = "GKH_PKRDOM_EXPORT_ID_SEQ";

        public override void Up()
        {
            this.Database.RemoveExportId("OVRHL_PUBLISH_PRG_REC");
            this.Database.RemoveExportId("OVRHL_VERSION_REC");
            this.Database.RemoveExportId("CR_OBJECT");
            this.Database.RemoveSequence(PkrDomExportId);
        }

        public override void Down()
        {
        }
    }
}