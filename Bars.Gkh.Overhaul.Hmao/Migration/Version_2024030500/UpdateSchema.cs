namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030500")]
    [MigrationDependsOn(typeof(Version_2024030400.UpdateSchema))]
    // Является Version_2019061300 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_VERSION_REC", "IS_CHANGED_PUBLISH_YEAR", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CHANGED_PUBLISH_YEAR");
        }
    }
}