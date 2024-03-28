namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014081200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014081200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014080800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_STORED_REPORT", new Column("TEMPLATE", DbType.Binary, ColumnProperty.NotNull));
            Database.RemoveColumn("AL_STORED_REPORT", "TPL_FILE_ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_STORED_REPORT", "TEMPLATE");
            Database.AddRefColumn("AL_STORED_REPORT", new RefColumn("TPL_FILE_ID", "STORED_REPORT_TPL", "B4_FILE_INFO", "ID"));
        }
    }
}
