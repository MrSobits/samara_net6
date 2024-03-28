namespace Bars.GkhGji.Migrations._2017.Version_2017041100
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017041100
    /// </summary>
    [Migration("2017041100")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017040500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"insert into public.b4_role_permission(permission_id, role_id)
	                                            select 'GkhGji.AppealCitizensState.Answer.Executor_View', id
		                                            from public.b4_role");

            this.Database.ExecuteNonQuery(@"insert into public.b4_role_permission(permission_id, role_id)
	                                            select 'GkhGji.AppealCitizensState.Answer.Executor_Edit', id
		                                            from public.b4_role");

            this.Database.ExecuteNonQuery(@"insert into public.b4_state_role_permission(object_version, object_create_date, object_edit_date, permission_id, role_id, state_id)
                                                select 0, current_date, current_date, 'GkhGji.AppealCitizensState.Answer.Executor_View', r.id, s.id
                                                    from public.b4_role r
                                                        inner join public.b4_state s
                                                            on s.type_id = 'gji_appeal_citizens'");

            this.Database.ExecuteNonQuery(@"insert into public.b4_state_role_permission(object_version, object_create_date, object_edit_date, permission_id, role_id, state_id)
                                                select 0, current_date, current_date, 'GkhGji.AppealCitizensState.Answer.Executor_Увше', r.id, s.id
                                                    from public.b4_role r
                                                        inner join public.b4_state s
                                                            on s.type_id = 'gji_appeal_citizens'");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.ExecuteNonQuery(@"delete from public.b4_role_permission where permission_id = 'GkhGji.AppealCitizensState.Answer.Executor_View'");
            this.Database.ExecuteNonQuery(@"delete from public.b4_state_role_permission where permission_id = 'GkhGji.AppealCitizensState.Answer.Executor_View'");
            this.Database.ExecuteNonQuery(@"delete from public.b4_role_permission where permission_id = 'GkhGji.AppealCitizensState.Answer.Executor_Edit'");
            this.Database.ExecuteNonQuery(@"delete from public.b4_state_role_permission where permission_id = 'GkhGji.AppealCitizensState.Answer.Executor_Edit'");
        }
    }
}