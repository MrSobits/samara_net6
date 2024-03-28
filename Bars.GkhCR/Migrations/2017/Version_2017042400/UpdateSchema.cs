namespace Bars.GkhCr.Migrations._2017.Version_2017042400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017042400")]
    [MigrationDependsOn(typeof(Version_2017042000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "GUARANTEE_PERIOD", DbType.Int32);
            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "URL_RESULT_TRADING", DbType.String);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "GUARANTEE_PERIOD");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "URL_RESULT_TRADING");
        }
    }
}