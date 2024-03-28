namespace Bars.GkhDi.Migrations._2019.Version_2019030400
{
    using Bars.B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019030400")]
    [MigrationDependsOn(typeof(_2017.Version_2017080800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn("DI_DISINFO_REALOBJ", new RefColumn("MAN_ORG_ID", "DI_DISINFO_REALOBJ_MAN_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("DI_DISINFO_REALOBJ", "MAN_ORG_ID");
        }
    }
}
