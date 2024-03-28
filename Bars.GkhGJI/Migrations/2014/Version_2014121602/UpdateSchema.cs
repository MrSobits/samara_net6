using System.Data;

namespace Bars.GkhGji.Migrations._2014.Version_2014121602
{
    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014121601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddColumn("GJI_ACTCHECK_ROBJECT", new Column("OFFICIALS_GUILTY_ACTIONS", DbType.String, 1000));
        }

        public override void Down()
        {
			Database.RemoveColumn("GJI_ACTCHECK_ROBJECT", "OFFICIALS_GUILTY_ACTIONS");
        }
    }
}