namespace Bars.Gkh.Migrations._2017.Version_2017080900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2017080900")]
    [MigrationDependsOn(typeof(Version_2017080400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                delete from GKH_ACTIVITY_STAGE 
                where DATE_START is null;");

            this.Database.AlterColumnSetNullable("GKH_ACTIVITY_STAGE", "DATE_START", false);
        }

        public override void Down()
        {
            this.Database.AlterColumnSetNullable("GKH_ACTIVITY_STAGE", "DATE_START", true);
        }
    }
}