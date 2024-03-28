namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042201
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022042201")]
    [MigrationDependsOn(typeof(Version_2022042200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery("ALTER TABLE GJI_DICT_CONTROL_TYPE_RISK_INDICATORS ALTER COLUMN NAME TYPE varchar(5000)");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ExecuteNonQuery("ALTER TABLE GJI_DICT_CONTROL_TYPE_RISK_INDICATORS ALTER COLUMN NAME TYPE varchar(500)");
        }
    }
}