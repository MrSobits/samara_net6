using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.Utils;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020031000
{
    [Migration("2020031000")]
    [MigrationDependsOn(typeof(Version_2020030600.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_CMP_BUILD_CONTR", new StateColumn("STATE_ID", "FK_CROBJ_CMP_SK_STATE"));

            this.Database.AddEntityTable("CR_MASSBC_OBJ_CR_WORK",
               new Column("SUM", DbType.Decimal),
               new RefColumn("MASS_BC_OBJCR_ID", "FK_MASSBK_OBJCRWORK_OBJCR", "CR_MASS_BLD_CONTR_OBJ_CR", "Id"),
               new RefColumn("WORK_ID", "FK_MASSBK_OBJCRWORK_WORK", "GKH_DICT_WORK", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("CR_MASSBC_OBJ_CR_WORK");
            Database.RemoveColumn("CR_OBJ_CMP_BUILD_CONTR", "STATE_ID");
          
        }
    }
}