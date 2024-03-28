namespace Bars.Gkh.Migrations._2017.Version_2017020100
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017020100"), MigrationDependsOn(typeof(Version_2017012700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string RequirementId = "GkhCr.ObjectCR.Protocol.Field.File";

        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                $"INSERT INTO GKH_FIELD_REQUIREMENT (object_version, object_create_date, object_edit_date, REQUIREMENTID) VALUES (0, now(), now(), '{UpdateSchema.RequirementId}')");
        }

        public override void Down()
        {
            this.Database.ExecuteNonQuery($"DELETE FROM GKH_FIELD_REQUIREMENT WHERE REQUIREMENTID = '{UpdateSchema.RequirementId}'");
        }
    }
}