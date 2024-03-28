namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016080900
{ 
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016080900")]
    [MigrationDependsOn(typeof(Version_2016070401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
	        Database.AddColumn("REGOP_PERIOD_CLS_CHCK_RES", new RefColumn("FULL_LOG_FILE_ID", ColumnProperty.Null, "PER_CLS_CHK_RES_F_LOG", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
			Database.RemoveColumn("REGOP_PERIOD_CLS_CHCK_RES", "FULL_LOG_FILE_ID");
        }
    }
}
