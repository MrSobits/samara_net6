namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016041900
{
	using Bars.Gkh.Utils;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2016041900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2016040801.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			this.Database.AlterColumnSetNullable("GKH_CONSTRUCT_OBJ_DOCUMENT", "NAME", true);
			this.Database.AlterColumnSetNullable("GKH_CONSTRUCT_OBJ_CONTRACT", "NAME", true);
			this.Database.AlterColumnSetNullable("GKH_CONSTRUCT_OBJ_TYPEWORK", "WORK_ID", true);
        }

        public override void Down()
        {
	        this.Database.ExecuteNonQuery("UPDATE GKH_CONSTRUCT_OBJ_DOCUMENT SET NAME = '' WHERE NAME is NULL");
	        this.Database.AlterColumnSetNullable("GKH_CONSTRUCT_OBJ_DOCUMENT", "NAME", false);

	        this.Database.ExecuteNonQuery("UPDATE GKH_CONSTRUCT_OBJ_CONTRACT SET NAME = '' WHERE NAME is NULL");
	        this.Database.AlterColumnSetNullable("GKH_CONSTRUCT_OBJ_CONTRACT", "NAME", false);
		}
    }
}
