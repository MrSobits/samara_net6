namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2017042700
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017042700")]
    [MigrationDependsOn(typeof(Version_2016020300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveTable("GJI_TOMSK_DISP_VERIFSUBJ");
        }

        public override void Down()
        {
        }
    }
}