using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2021.Version_2021042200
{
    [Migration("2021042200")]
    [MigrationDependsOn(typeof(_2020.Version_2021011900.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_CMP_BUILD_CONTR", new Column("DEADLINE_MISSED", DbType.Boolean, ColumnProperty.None, false));
            Database.AddColumn("CR_OBJECT", new Column("DEADLINE_MISSED", DbType.Boolean, ColumnProperty.None, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJECT", "DEADLINE_MISSED");
            Database.RemoveColumn("CR_OBJ_CMP_BUILD_CONTR", "DEADLINE_MISSED");
        }
    }
}