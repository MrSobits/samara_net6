namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120300
{
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120300")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.RemoveConstraint("GIS_HOUSE_REGISTER", "FK_GIS_HOUSE_REGISTER_HOUSETYPE_GIS_REAL_ESTATE_TYPE");
        }

        public override void Down()
        {
            this.Database.AddForeignKey(
                "FK_GIS_HOUSE_REGISTER_HOUSETYPE_GIS_REAL_ESTATE_TYPE",
                "GIS_HOUSE_REGISTER",
                "HOUSETYPE",
                "GIS_REAL_ESTATE_TYPE",
                "ID");
        }
    }
}