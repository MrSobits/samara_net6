namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014073000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_STORED_REPORT", new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_STORED_REPORT", "DESCRIPTION");
        }
    }
}
