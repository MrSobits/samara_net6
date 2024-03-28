namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017020300
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <inheritdoc />
    [Migration("2017020300")]
    [MigrationDependsOn(typeof(_2016.Version_2016122000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                @"
                 delete from b4_role_permission perm
                 where perm.permission_id like 'GkhRegOp.PersonalAccountOwner.Field.Individ%';
                        
                  insert into b4_role_permission (permission_id, role_id)
                  select replace(perm.permission_id, 'GkhRegOp.PersonalAccountOwner.Field', 'GkhRegOp.PersonalAccountOwner.Field.Individ'),
                         role_id
                  from b4_role_permission perm
                  where perm.permission_id like 'GkhRegOp.PersonalAccountOwner.Field.%';");

            var newPermissioins = new List<string>
            {
                "GkhRegOp.PersonalAccountOwner.Field.Individ.Surname_View",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.Surname_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.FirstName_View",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.FirstName_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.SecondName_View",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.SecondName_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.BirthDate_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.BirthPlace_View",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.BirthPlace_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityType_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.IdentitySerial_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityNumber_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.BillingAddress_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.Gender_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Individ.DateDocumentIssuance_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Contragent_View",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Contragent_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Inn_View",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Inn_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Kpp_View",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Kpp_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.PrintAct_View",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.PrintAct_Edit",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Address_View",
                "GkhRegOp.PersonalAccountOwner.Field.Legal.Address_Edit"
            };

            foreach (var newPermisson in newPermissioins)
            {
                this.InsertPermission(newPermisson);
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ExecuteNonQuery(
                @"delete
                from b4_role_permission
                where permission_id like 'GkhRegOp.PersonalAccountOwner.Field.Individ%';

                delete
                from b4_role_permission
                where permission_id like 'GkhRegOp.PersonalAccountOwner.Field.Legal%';");
        }

        private void InsertPermission(string permissionId)
        {
            this.Database.ExecuteNonQuery(
                $@"INSERT INTO B4_ROLE_PERMISSION (PERMISSION_ID, ROLE_ID)
                    SELECT '{permissionId}', ID  FROM B4_ROLE R
                    WHERE NOT EXISTS(
                        SELECT PERMISSION_ID 
                        FROM B4_ROLE_PERMISSION P
                        WHERE P.PERMISSION_ID = '{permissionId}' AND P.ROLE_ID = R.ID)
                AND ID IN (
                            SELECT DISTINCT ROLE_ID
                            FROM B4_ROLE_PERMISSION PERM
                            WHERE PERM.PERMISSION_ID LIKE 'GkhRegOp.PersonalAccountOwner.Field.%'
                          )");
        }
    }
}
