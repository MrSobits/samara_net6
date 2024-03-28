using System.Data;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014091400
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014091000.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_UNACCEPT_CHARGE", new Column("DESCR", DbType.String, 200));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "DESCR");
        }
    }
}
