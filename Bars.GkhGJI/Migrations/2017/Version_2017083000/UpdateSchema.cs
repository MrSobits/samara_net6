namespace Bars.GkhGji.Migrations._2017.Version_2017083000
{
    using System.Data;
    using Bars.Gkh.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Enums;

    [Migration("2017083000")]
    [MigrationDependsOn(typeof(Version_2017080800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC", new RefColumn("DISPOSAL_ID", "GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC_ST", "GJI_DISPOSAL", "ID"));

            this.Database.AlterColumnSetNullable("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC", "FOUND_CHECK_ID", true);
            this.Database.AlterColumnSetNullable("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC", "DISPOSAL_ID", true);
        }

        public override void Down()
        {
            this.Database.AlterColumnSetNullable("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC", "FOUND_CHECK_ID", false);
            this.Database.RemoveColumn("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC", "DISPOSAL_ID");
        }
    }
}