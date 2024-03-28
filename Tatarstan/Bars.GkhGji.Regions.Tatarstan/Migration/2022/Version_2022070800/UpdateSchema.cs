namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022070800
{
    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(Version_2022070500.UpdateSchema))]
    [Migration("2022070800")]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_WARNING_DOC_VIOLATIONS";
        
        public override void Up()
        {
            this.Database.ChangeColumnNotNullable(TableName, "NORMATIVE_DOC_ID", false);
        }

        public override void Down()
        {
            this.Database.ChangeColumnNotNullable(TableName, "NORMATIVE_DOC_ID", true);
        }
    }
}