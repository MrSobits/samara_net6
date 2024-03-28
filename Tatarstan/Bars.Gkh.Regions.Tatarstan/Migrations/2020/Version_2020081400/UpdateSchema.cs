namespace Bars.Gkh.Regions.Tatarstan.Migrations._2020.Version_2020081400
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020081400")]
    [MigrationDependsOn(typeof(Version_2020020300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"update public.gkh_reality_object ro set number_lifts = tpv.value::integer
from public.TP_TEH_PASSPORT tp
  join public.tp_teh_passport_value tpv on tp.id = tpv.teh_passport_id 
                                           and tpv.form_code = 'Form_4_2' and tpv.cell_code = '1:1'
where tp.reality_obj_id = ro.id;");
        }
    }
}