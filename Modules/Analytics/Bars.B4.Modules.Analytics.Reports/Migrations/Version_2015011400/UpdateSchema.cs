namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015011400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015011400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014082100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_REPORT_PARAM", new Column("SQL_QUERY", DbType.String));
            Database.AddColumn("AL_REPORT_PARAM", new Column("MULTISELECT", DbType.Boolean));
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_REPORT_PARAM", "MULTISELECT");
            Database.RemoveColumn("AL_REPORT_PARAM", "SQL_QUERY");
        }
    }
}