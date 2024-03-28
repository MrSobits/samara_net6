namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019110100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Text;

    [Migration("2019110100")]
    [MigrationDependsOn(typeof(Version_2019103000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                update gji_tat_disposal as tat_disposal
                set inspection_base_type_id = case 
	                when dict_base_type.code in ('RSN_PP_I','RSN_PP_III','RSN_PP_II','4','5') then null
	                when dict_base_type.code in ('RSN_PP_OTHER') then (select id from gji_dict_inspection_base_type where code='RSN_VP_OTHER')
	                end 
                from gji_tat_disposal as tat_disposal_1
	                join  gji_disposal disposal on disposal.id=tat_disposal_1.id
	                join gji_document document on disposal.Id=document.Id
	                join gji_inspection inspection on document.inspection_id=inspection.Id
	                join gji_dict_inspection_base_type dict_base_type on tat_disposal_1.inspection_base_type_id = dict_base_type.id
                where inspection.type_base in (20,40,50) and dict_base_type.code in ('RSN_PP_I','RSN_PP_III','RSN_PP_II','4','5', 'RSN_PP_OTHER')
                and disposal.id=tat_disposal.id"
            );
        }
    }
}
