namespace Bars.Gkh.Gis.Migrations._2014.Version_2014122200
{
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014122200")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014121800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddForeignKey(
                "FK_GIS_HOUSE_REGISTER_WALLMATERIAL_GKH_DICT_WALL_MATERIAL_ID",
                "GIS_HOUSE_REGISTER",
                "WALLMATERIAL",
                "GKH_DICT_WALL_MATERIAL", 
                "ID");

            this.Database.AddForeignKey(
                "FK_GIS_HOUSE_REGISTER_ROOFINGMATERIAL_GKH_DICT_ROOFING_MATERIAL_ID",
                "GIS_HOUSE_REGISTER",
                "ROOFINGMATERIAL",
                "GKH_DICT_ROOFING_MATERIAL",
                "ID");

            this.Database.AddForeignKey(
                "FK_GIS_HOUSE_REGISTER_TYPEPROJECT_GKH_DICT_TYPE_PROJ_ID",
                "GIS_HOUSE_REGISTER",
                "TYPEPROJECT",
                "GKH_DICT_TYPE_PROJ",
                "ID");


        }

        public override void Down()
        {
            this.Database.RemoveConstraint("GIS_HOUSE_REGISTER", "FK_GIS_HOUSE_REGISTER_WALLMATERIAL_GKH_DICT_WALL_MATERIAL_ID");
            this.Database.RemoveConstraint("GIS_HOUSE_REGISTER",
                "FK_GIS_HOUSE_REGISTER_ROOFINGMATERIAL_GKH_DICT_ROOFING_MATERIAL_ID");
            this.Database.RemoveConstraint("GIS_HOUSE_REGISTER", "FK_GIS_HOUSE_REGISTER_TYPEPROJECT_GKH_DICT_TYPE_PROJ_ID");
        }
    }
}