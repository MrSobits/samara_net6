using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.FormatDataExport.ExportableEntities;
using System.Data;
using Bars.Gkh.Utils;

namespace Bars.GkhCr.Migrations._2023.Version_2023123103
{
    [Migration("2023123103")]
    [MigrationDependsOn(typeof(_2023.Version_2023123102.UpdateSchema))]
    // Является Version_2018082700 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public const string PayDogovExprtId = "GKH_PAYDOGOV_EXPORT_ID_SEQ";
        public override void Up()
        {
            this.Database.RemoveExportId("CR_OBJ_PER_ACT_PAYMENT");
            this.Database.RemoveSequence(PayDogovExprtId);
        }

        public override void Down()
        {
        }
    }
}