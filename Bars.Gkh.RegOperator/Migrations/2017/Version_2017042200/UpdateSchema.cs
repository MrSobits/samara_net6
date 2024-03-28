namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017042200
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017042200")]
    [MigrationDependsOn(typeof(_2017.Version_2017021000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string RequirementId = "GkhRegOp.Accounts.BankOperations.Field.RecipientAccountNum_Rqrd";

        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                $"INSERT INTO GKH_FIELD_REQUIREMENT (object_version, object_create_date, object_edit_date, REQUIREMENTID) VALUES (0, now(), now(), '{UpdateSchema.RequirementId}')");
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.ExecuteNonQuery($"DELETE FROM GKH_FIELD_REQUIREMENT WHERE REQUIREMENTID = '{UpdateSchema.RequirementId}'");
        }
    }
}