using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.Utils;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020030600
{
    [Migration("2020030600")]
    [MigrationDependsOn(typeof(_2020.Version_2020030300.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CR_OBJ_CMP_BUILD_CONTR_FILE",          
                new Column("DESCRIPTION", DbType.String, 1500),
                new RefColumn("SK_ID", "FK_BKSMR_SK", "CR_OBJ_CMP_BUILD_CONTR", "Id"),
                new FileColumn("FILE_ID", "FK_BKSMR_SK_FILE"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CR_OBJ_CMP_BUILD_CONTR_FILE");
        }
    }
}