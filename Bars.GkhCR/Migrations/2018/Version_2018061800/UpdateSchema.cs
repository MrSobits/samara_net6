using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.GkhCr.Migrations._2018.Version_2018061800
{
    [Migration("2018061800")]
    [MigrationDependsOn(typeof(_2017.Version_2017072400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "CR_OBJ_PSD_WORK",
               new RefColumn("WORK_ID", ColumnProperty.NotNull, "CR_OBJ_PSD_WORK_CR_OBJ_TYPE_WORK_WORK_ID_ID", "CR_OBJ_TYPE_WORK", "ID"),
               new RefColumn("PSDWORK_ID", ColumnProperty.NotNull, "CR_OBJ_PSD_WORK_CR_OBJ_TYPE_WORK_PSDWORK_ID_ID", "CR_OBJ_TYPE_WORK", "ID")
               );
        }

        public override void Down()
        {
            Database.RemoveTable("CR_OBJ_PSD_WORK");
        }
    }
}