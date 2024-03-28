namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120201
{
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120201")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_LOADED_FILE_REGISTER", "FORMAT", DbType.Int64);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "FORMAT");
        }
    }
}