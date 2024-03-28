namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015060900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015060900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015060801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery(@"
alter table REGOP_UNACCEPT_CHARGE alter column CCHARGE          set data type numeric(19,2);
alter table REGOP_UNACCEPT_CHARGE alter column CCHARGE_TARIFF   set data type numeric(19,2);
alter table REGOP_UNACCEPT_CHARGE alter column CPENALTY         set data type numeric(19,2);
alter table REGOP_UNACCEPT_CHARGE alter column CRECALC          set data type numeric(19,2);
alter table REGOP_UNACCEPT_CHARGE alter column RECALC_DECISION  set data type numeric(19,2);
alter table REGOP_UNACCEPT_CHARGE alter column RECALC_PENALTY   set data type numeric(19,2);
alter table REGOP_UNACCEPT_CHARGE alter column TARIFF_OVERPLUS  set data type numeric(19,2);

alter table REGOP_PERS_ACC alter column TARIFF_CHARGE_BALANCE   set data type numeric(19,2);
alter table REGOP_PERS_ACC alter column DECISION_CHARGE_BALANCE set data type numeric(19,2);
alter table REGOP_PERS_ACC alter column PENALTY_CHARGE_BALANCE  set data type numeric(19,2);
");
        }

        public override void Down()
        {
            
        }
    }
}
