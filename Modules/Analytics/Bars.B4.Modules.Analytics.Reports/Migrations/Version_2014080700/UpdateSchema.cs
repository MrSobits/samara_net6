namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_STORED_REPORT", new Column("DISPLAY_NAME", DbType.String, 100, ColumnProperty.Null));
            Database.AddColumn("AL_STORED_REPORT", new Column("REPORT_TYPE", DbType.Int16, ColumnProperty.Null, 1));
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_STORED_REPORT", "REPORT_TYPE");
            Database.RemoveColumn("AL_STORED_REPORT", "DISPLAY_NAME");
        }
    }
}
