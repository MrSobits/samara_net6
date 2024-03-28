namespace Bars.Gkh.Overhaul.Migration.Version_2017020100
{
    using System.Collections.Generic;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017020100")]
    [MigrationDependsOn(typeof(Version_2017011100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly List<string> permissionIds = new List<string>
        {
            "Gkh.RealityObject.Register.StructElem.Field.LastOverhaulYear_View",
            "Gkh.RealityObject.Register.StructElem.Field.LastOverhaulYear_Edit",
            "Gkh.RealityObject.Register.StructElem.Field.Wearout_View",
            "Gkh.RealityObject.Register.StructElem.Field.Wearout_Edit",
            "Gkh.RealityObject.Register.StructElem.Field.Volume_View",
            "Gkh.RealityObject.Register.StructElem.Field.Volume_Edit"
        };

        public override void Up()
        {
            foreach (var permissionId in this.permissionIds)
            {
                this.InsertPermission(permissionId);
                this.InsertStatePermission(permissionId, stateType: "gkh_real_obj");
            }
        }

        public override void Down()
        {
            foreach (var permissionId in this.permissionIds)
            {
                this.DeletePermission("b4_role_permission", permissionId);
                this.DeletePermission("b4_state_role_permission", permissionId);
            }
        }

        private void InsertPermission(string permissionId)
        {
            this.Database.ExecuteNonQuery(
                $@"INSERT INTO B4_ROLE_PERMISSION (PERMISSION_ID, ROLE_ID)
                    SELECT '{permissionId}', ID  FROM B4_ROLE R
                    WHERE NOT EXISTS(
                        SELECT PERMISSION_ID 
                        FROM B4_ROLE_PERMISSION P
                        WHERE P.PERMISSION_ID = '{permissionId}' AND P.ROLE_ID = R.ID)");
        }

        private void InsertStatePermission(string permissionId, string stateType)
        {
            this.Database.ExecuteNonQuery(
                $@"INSERT INTO B4_STATE_ROLE_PERMISSION (PERMISSION_ID, OBJECT_VERSION, OBJECT_CREATE_DATE, OBJECT_EDIT_DATE, ROLE_ID, STATE_ID)
                    SELECT '{permissionId}', 0, NOW(), NOW(), R.ID, S.ID
                    FROM B4_ROLE R, B4_STATE S
                    WHERE NOT EXISTS(
                        SELECT PERMISSION_ID 
                        FROM B4_STATE_ROLE_PERMISSION P 
                        WHERE P.PERMISSION_ID = '{permissionId}'
                        AND P.ROLE_ID = R.ID AND P.STATE_ID = S.ID AND S.TYPE_ID = '{stateType}')
                    AND S.TYPE_ID = '{stateType}'");
        }

        private void DeletePermission(string table, string permissionId)
        {
            this.Database.ExecuteNonQuery($"DELETE FROM {table} WHERE PERMISSION_ID = '{permissionId}'");
        }
    }
}
