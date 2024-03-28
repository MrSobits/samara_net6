namespace Bars.GkhGji.Migrations._2017.Version_2017040500
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017040500
    /// </summary>
    [Migration("2017040500")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017040300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"insert into public.b4_role_permission(permission_id, role_id)
	                                            select 'GkhGji.ActivityTsj.Register.Member.Field.File', id
		                                            from public.b4_role");

            this.Database.ExecuteNonQuery(@"insert into public.b4_state_role_permission(object_version, object_create_date, object_edit_date, permission_id, role_id, state_id)
                                                select 0, current_date, current_date, 'GkhGji.ActivityTsj.Register.Member.Field.File', r.id, s.id
                                                    from public.b4_role r
                                                        inner join public.b4_state s
                                                            on s.type_id = 'gji_activity_tsj_member'");

            this.Database.ExecuteNonQuery(@"insert into public.gkh_field_requirement(object_version, object_create_date, object_edit_date, requirementid) values
                                                (0, current_date, current_date, 'GkhGji.ActivityTsj.Members.Field.File')");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.ExecuteNonQuery(@"delete from public.b4_role_permission where permission_id = 'GkhGji.ActivityTsj.Register.Member.Field.File'");

            this.Database.ExecuteNonQuery(@"delete from public.b4_state_role_permission where permission_id = 'GkhGji.ActivityTsj.Register.Member.Field.File'");

            this.Database.ExecuteNonQuery(@"delete from public.gkh_field_requirement where requirementid = 'GkhGji.ActivityTsj.Members.Field.File'");
        }
    }
}