namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_REPORT_PARAM", new Column("OWNER_TYPE", DbType.Int16, ColumnProperty.Null, 1));
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_REPORT_PARAM", "OWNER_TYPE");
        }
    }
}
