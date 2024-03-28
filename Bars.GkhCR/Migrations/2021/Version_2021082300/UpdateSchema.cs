using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2021.Version_2021082300
{
    [Migration("2021082300")]
    [MigrationDependsOn(typeof(Version_2021060800.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_CMP_BUILD_CONTR", new Column("LATITUDE", DbType.Decimal, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_CMP_BUILD_CONTR", new Column("LONGITUDE", DbType.Decimal, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_CMP_BUILD_CONTR", "LONGITUDE");
            Database.RemoveColumn("CR_OBJ_CMP_BUILD_CONTR", "LATITUDE");
        }
    }
}