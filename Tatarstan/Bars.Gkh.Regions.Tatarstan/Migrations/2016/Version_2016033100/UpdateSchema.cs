namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016033100
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016033100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2016033000.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddRefColumn(
		        "GKH_CONSTRUCTION_OBJECT",
		        new RefColumn("RESETTLEMENT_PROGRAM_ID", "GKH_CONSTRUCT_OBJ_RESPROG", "GKH_DICT_RESETTLE_PROGRAM", "ID"));
        }

        public override void Down()
        {
			this.Database.RemoveColumn("GKH_CONSTRUCTION_OBJECT", "RESETTLEMENT_PROGRAM_ID");
		}
    }
}
