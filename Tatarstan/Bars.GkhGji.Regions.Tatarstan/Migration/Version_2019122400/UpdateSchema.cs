namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019122400
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019122400")]
    [MigrationDependsOn(typeof(Version_2019121300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            var query = @"drop table if exists tmp_disp;
                          create temp table tmp_disp as
                            select disp.id from public.gji_tat_disposal tat_disp
                              join public.gji_disposal disp on tat_disp.id = disp.id
                            where exists (select 1 from public.GJI_DICT_KIND_CHECK kc where disp.kind_check_id = kc.id and kc.code in (2,4,5))
                            and exists (select 1 from  public.gji_dict_inspection_base_type t1 where tat_disp.inspection_base_type_id = t1.id and t1.code = 'RSN_PP_OTHER');
                          
                          update public.gji_tat_disposal disp set inspection_base_type_id = (select id from public.gji_dict_inspection_base_type where code = 'RSN_VP_OTHER')
                          where exists(select 1 from tmp_disp td where td.id = disp.id);";

            this.Database.ExecuteQuery(query);
        }
    }
}
