namespace Bars.Gkh.Gis.Migrations._2015.Version_2015081700
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081700")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015081200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GIS_INTEGR_REF_DICT", new RefColumn("DICT_ID", "GIS_INTEGR_REF_DICT", "GIS_INTEGR_DICT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_INTEGR_REF_DICT", "DICT_ID");
        }
    }
}