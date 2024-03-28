using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Migration.Version_2018
{
    [Migration("2018082100")]
    [MigrationDependsOn(typeof(Bars.Gkh.RegOperator.Regions.Tyumen.Migration.Version_1.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "REGOP_REQUESTSTATE_PERSON",
               new Column("EMAIL", DbType.String, 100, ColumnProperty.NotNull),
               new Column("NAME", DbType.String, 200, ColumnProperty.NotNull),
               new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.Null),
               new Column("POSITION", DbType.String, 300, ColumnProperty.Null),
               new Column("STATUS", DbType.Int32, ColumnProperty.NotNull, 10)
               );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_REQUESTSTATE_PERSON");
        }
    }
}