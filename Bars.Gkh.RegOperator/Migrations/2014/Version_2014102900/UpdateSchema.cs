namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014102900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014102801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_CALC_ACC_RO", new Column("DATE_START", DbType.Date));
            Database.AddColumn("REGOP_CALC_ACC_RO", new Column("DATE_END", DbType.Date));

            Database.ExecuteNonQuery(@"
update REGOP_CALC_ACC_RO caro 
set date_start =
    (select
        date_start
    from gkh_obj_d_protocol p
        join b4_state s on s.id = p.state_id
    where p.ro_id = caro.ro_id and s.final_state
    order by date_start desc
    limit 1)
");
            Database.AlterColumnSetNullable("REGOP_CALC_ACC_RO", "DATE_START", true);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_CALC_ACC_RO", "DATE_START");
            Database.RemoveColumn("REGOP_CALC_ACC_RO", "DATE_END");
        }
    }
}
