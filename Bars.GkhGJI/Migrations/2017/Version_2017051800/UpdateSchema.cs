namespace Bars.GkhGji.Migrations._2017.Version_2017051800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017051800")]
    [MigrationDependsOn(typeof(Version_2017051500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"insert into b4_role_permission(permission_id, role_id)
	                                            select 'GkhGji.AppealCitizensState.Field.Department_View', id
		                                            from b4_role
	                                            union
	                                            select 'GkhGji.AppealCitizensState.Field.Department_Edit', id
		                                            from b4_role");

            this.Database.ExecuteNonQuery(@"insert into b4_state_role_permission(object_version, object_create_date, object_edit_date, permission_id, role_id, state_id)
	                                            select 0, current_date, current_date, 'GkhGji.AppealCitizensState.Field.Department_View', r.id, s.id
		                                            from b4_role r
			                                            inner join b4_state s
				                                            on s.type_id = 'gji_appeal_citizens'
	                                            union
	                                            select 0, current_date, current_date, 'GkhGji.AppealCitizensState.Field.Department_Edit', r.id, s.id
		                                            from b4_role r
			                                            inner join b4_state s
				                                            on s.type_id = 'gji_appeal_citizens'");

            this.Database.ExecuteNonQuery(@"insert into gkh_field_requirement(object_version, object_create_date, object_edit_date, requirementid) values
                                                (0, current_date, current_date, 'GkhGji.AppealCits.Field.Department_Rqrd')");
        }

        public override void Down()
        {
            this.Database.ExecuteNonQuery(@"delete
	                                            from b4_role_permission
	                                            where permission_id in ('GkhGji.AppealCitizensState.Field.Department_View', 'GkhGji.AppealCitizensState.Field.Department_Edit')");

            this.Database.ExecuteNonQuery(@"delete
	                                            from b4_state_role_permission
	                                            where permission_id in ('GkhGji.AppealCitizensState.Field.Department_View', 'GkhGji.AppealCitizensState.Field.Department_Edit')");

            this.Database.ExecuteNonQuery(@"delete
	                                            from gkh_field_requirement
	                                            where requirementid = 'GkhGji.AppealCits.Field.Department_Rqrd'");
        }
    }
}