using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020051400
{
    [Migration("2020051200")]
    [MigrationDependsOn(typeof(Version_2020032000.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CR_OBJ_TW_STAGE",
                new Column("UNDER_CONTROL", DbType.Boolean, false),
                new Column("DATE_START_WORK", DbType.DateTime),
                new Column("DATE_END_WORK", DbType.DateTime),
                new RefColumn("TYPEWORK_ID", ColumnProperty.NotNull, "FK_CR_OBJ_TWSTAGE_TYPE_WORK", "CR_OBJ_TYPE_WORK", "ID"),
                new RefColumn("ADDIT_WORK_ID", ColumnProperty.NotNull, "FK_CR_OBJ_TWSTAGE_ADDIT_WORK", "GKH_DICT_ADDIT_WORK", "ID"));

            Database.AddRefColumn("CR_OBJ_CMP_BUILD_CONTR", new RefColumn("STAGE_ID", ColumnProperty.None, "FK_CR_OBJ_CMP_BUILD_CONTR_STAGE", "CR_OBJ_TW_STAGE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_CMP_BUILD_CONTR", "STAGE_ID");
            this.Database.RemoveTable("CR_OBJ_TW_STAGE");
        }
    }
}