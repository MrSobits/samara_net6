using System.Data;

namespace Bars.GkhGji.Migrations._2014.Version_2014121601
{
    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddColumn("GJI_ACTCHECK_ROBJECT", new Column("PERSONS_WHO_HAVE_VIOLATED", DbType.String, 1000));
			Database.AddColumn("GJI_ACTREMOVAL_VIOLAT", new Column("CIRCUMSTANCES_DESCRIPTION", DbType.String, 500));
        }

        public override void Down()
        {
			Database.RemoveColumn("GJI_ACTCHECK_ROBJECT", "PERSONS_WHO_HAVE_VIOLATED");
			Database.RemoveColumn("GJI_ACTREMOVAL_VIOLAT", "CIRCUMSTANCES_DESCRIPTION");
        }
    }
}