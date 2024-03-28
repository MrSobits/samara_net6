namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014111000
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014110800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery(@"
update REGOP_CALC_ACC_RO caro 
set date_start = GREATEST(
    (select
        date_start
    from gkh_obj_d_protocol p
        join b4_state s on s.id = p.state_id
    where p.ro_id = caro.ro_id and s.final_state
    order by date_start desc
    limit 1),
       (select
        date_start
    from dec_gov_decision g
        join b4_state s on s.id = g.state_id
    where g.ro_id = caro.ro_id and s.final_state
    order by date_start desc
    limit 1)
    )
");
        }

        public override void Down()
        {
        }
    }
}
