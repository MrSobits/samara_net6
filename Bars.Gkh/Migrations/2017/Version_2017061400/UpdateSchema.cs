namespace Bars.Gkh.Migrations._2017.Version_2017061400
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    [Migration("2017061400")]
    [MigrationDependsOn(typeof(Version_2017052500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        private const string PermissionKey = "Gkh.Dictionaries.Suggestion.CitizenSuggestionViewCreate.View";

        public override void Up()
        {
            this.Database.ExecuteNonQuery($@"INSERT INTO b4_role_permission (role_id, permission_id)
                SELECT r.id, '{UpdateSchema.PermissionKey}' from b4_role r
                    where not exists (select null from b4_role_permission rp where rp.role_id = r.id and rp.permission_id = '{UpdateSchema.PermissionKey}')");
        }

        public override void Down()
        {
        }
    }
}
