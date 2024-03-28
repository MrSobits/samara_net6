namespace Bars.GisIntegration.Base.Migrations.Version_2016090100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GisIntegration.Base.Enums;

    [Migration("2016090100")]
    [MigrationDependsOn(typeof(Version_2016083100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GI_CONTEXT_SETTINGS",
                new Column("FILE_STORAGE_NAME", DbType.Int16, ColumnProperty.NotNull),
                new Column("CONTEXT", DbType.String, 100, ColumnProperty.NotNull));

            this.Database.RemoveColumn("GI_SERVICE_SETTINGS", "NAME");
        }

        public override void Down()
        {
            this.Database.AddColumn("GI_SERVICE_SETTINGS", new Column("NAME", DbType.String, 255));

            this.Database.RemoveTable("GI_CONTEXT_SETTINGS");
        }
    }
}