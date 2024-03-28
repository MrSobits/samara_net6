using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Migration.Version_2018082101
{
    [Migration("2018082101")]
    [MigrationDependsOn(typeof(Version_2018.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_REQUESTSTATE", new Column("NOTIFIED_USER", DbType.Boolean, ColumnProperty.None, true));
            Database.AddColumn("REGOP_REQUESTSTATE", new Column("NOTIFIED_RUSTAMCHIK", DbType.Boolean, ColumnProperty.None, true));

        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_REQUESTSTATE", "NOTIFIED_RUSTAMCHIK");
            this.Database.RemoveColumn("REGOP_REQUESTSTATE", "NOTIFIED_USER");
        }
    }
}