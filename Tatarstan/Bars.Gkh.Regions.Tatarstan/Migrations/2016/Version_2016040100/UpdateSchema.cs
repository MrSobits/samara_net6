namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016040100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016040100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2016033000.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddColumn("GKH_CONSTRUCT_OBJ_TYPEWORK", new Column("DEADLINE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONSTRUCT_OBJ_TYPEWORK", "DEADLINE");
        }
    }
}
