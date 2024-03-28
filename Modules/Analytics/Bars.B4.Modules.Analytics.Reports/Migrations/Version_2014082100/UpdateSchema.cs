namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014082100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014082100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014081200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_REPORT_CUSTOM", new Column("TEMPLATE", DbType.Binary, ColumnProperty.Null));
            Database.RemoveColumn("AL_REPORT_CUSTOM", "TEMPLATE_ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_STORED_REPORT", "TEMPLATE");
            Database.AddRefColumn("AL_STORED_REPORT", new RefColumn("TEMPLATE_ID", "REPORT_CUSTOM_TPL", "B4_FILE_INFO", "ID"));
        }
    }
}
