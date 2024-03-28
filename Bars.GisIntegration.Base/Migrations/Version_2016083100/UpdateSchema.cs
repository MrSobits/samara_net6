namespace Bars.GisIntegration.Base.Migrations.Version_2016083100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GisIntegration.Base.Enums;

    [Migration("2016083100")]
    [MigrationDependsOn(typeof(Version_2016082900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GI_PACKAGE_TRIGGER", new Column("STATE", DbType.Int16, ColumnProperty.NotNull, (int)PackageState.New));

            this.Database.ExecuteNonQuery("UPDATE GI_PACKAGE_TRIGGER PT SET STATE = (SELECT STATE FROM GI_PACKAGE P WHERE P.ID = PT.PACKAGE_ID)");

            this.Database.RemoveColumn("GI_PACKAGE", "STATE");
        }

        public override void Down()
        {
            this.Database.AddColumn("GI_PACKAGE", new Column("STATE", DbType.Int16, ColumnProperty.NotNull, (int)PackageState.New));

            this.Database.ExecuteNonQuery("UPDATE GI_PACKAGE P SET STATE = (SELECT STATE FROM GI_PACKAGE_TRIGGER PT WHERE PT.PACKAGE_ID = P.ID)");

            this.Database.RemoveColumn("GI_PACKAGE_TRIGGER", "STATE");
        }
    }
}