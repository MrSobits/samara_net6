namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120400
{
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120400")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddForeignKey(
                "FK_GIS_HOUSE_REGISTER_FIASADDRESS_B4_FIAS_ADDRESS_ID",
                "GIS_HOUSE_REGISTER",
                "FIASADDRESS",
                "B4_FIAS_ADDRESS",
                "ID");
        }

        public override void Down()
        {
            this.Database.RemoveConstraint("GIS_HOUSE_REGISTER", "FK_GIS_HOUSE_REGISTER_FIASADDRESS_B4_FIAS_ADDRESS_ID");
        }
    }
}