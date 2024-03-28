namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015062400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015061000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_STORED_REPORT", "FOR_ALL", DbType.Boolean, ColumnProperty.NotNull, true);
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_STORED_REPORT", "FOR_ALL");
        }
    }
}