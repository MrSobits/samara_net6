namespace Bars.GkhGji.Regions.Stavropol.Migrations.Version_2018011900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2018011900")]
    [MigrationDependsOn(typeof(Version_2014102900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(this.Database, "GkhGjiStavropol");
        }

        public override void Down()
        {
        }
    }
}