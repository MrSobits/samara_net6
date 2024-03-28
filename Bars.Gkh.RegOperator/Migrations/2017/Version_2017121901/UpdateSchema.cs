namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017121901
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017121901")]
    [MigrationDependsOn(typeof(Version_2017121300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017121301.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017121900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            var sql = @"
alter table clw_claim_work alter column base_info type varchar(500);

WITH owners as (
    SELECT w.id as id, 'Абонент ' || o.name as name FROM clw_document d
    JOIN clw_claim_work w ON w.id = d.claimwork_id
    LEFT JOIN clw_debtor_claim_work dw ON dw.id = w.id
    LEFT JOIN regop_pers_acc_owner o ON o.id = dw.owner_id
    WHERE dw.id is not null
)
UPDATE clw_claim_work w SET base_info = owners.name
FROM owners
WHERE w.id = owners.id;

-- обнуление невалидных дат
ALTER TABLE regop_individual_acc_own ALTER COLUMN birth_date DROP NOT NULL;
UPDATE regop_individual_acc_own SET birth_date = null WHERE birth_date = '-infinity'::date;

-- добавление обязательности по умолчанию
INSERT INTO gkh_field_requirement (object_version, object_create_date, object_edit_date, requirementid) 
SELECT 0, now()::date, now()::date, id
FROM (
    SELECT UNNEST(ARRAY['GkhRegOp.PersonalAccountOwner.Field.BirthDate_Rqrd', 'GkhRegOp.PersonalAccountOwner.Field.RegistrationAddress_Rqrd']) as id
) s
WHERE id NOT IN (SELECT requirementid FROM gkh_field_requirement);
";

            ViewManager.Drop(this.Database, "Regop");
            this.Database.ExecuteNonQuery(sql);
            //ViewManager.Create(this.Database, "Regop");
        }
    }
}