namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015101200
{
	using System.Data;
	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015101200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015081100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("AL_REPORT_PARAM", new Column("SQL_QUERY", DbType.String, 2000));
        }

        public override void Down()
        {
        }
    }
}