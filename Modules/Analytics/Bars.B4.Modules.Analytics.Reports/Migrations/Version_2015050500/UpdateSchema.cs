namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015050500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_2015011400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_STORED_REPORT", "USE_TPL_CONN", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_STORED_REPORT", "USE_TPL_CONN");
        }
    }
}