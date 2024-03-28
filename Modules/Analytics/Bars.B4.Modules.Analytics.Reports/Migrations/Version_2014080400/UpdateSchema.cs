namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_REPORT_PARAM", new Column("ADDITIONAL", DbType.String, 300, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_REPORT_PARAM", "ADDITIONAL");
        }
    }
}
