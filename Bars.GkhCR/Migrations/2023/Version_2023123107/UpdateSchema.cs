using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023123107
{
    [Migration("2023123107")]
    [MigrationDependsOn(typeof(_2023.Version_2023123106.UpdateSchema))]
    // Является Version_2018101500 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CR_DICT_PROGRAM", new Column("SPECIAL_ACC", DbType.Boolean, ColumnProperty.Null));
            this.Database.ExecuteNonQuery("update CR_DICT_PROGRAM set SPECIAL_ACC = false");
            //this.Database.ChangeColumnNotNullable("CR_DICT_PROGRAM", "SPECIAL_ACC", true);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_DICT_PROGRAM", "SPECIAL_ACC");
        }
    }
}