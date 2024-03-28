namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014110800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014110800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014110600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "OVERHAUL_PAYMENT");
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("OVERHAUL_PAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0M));

            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "RECRUITMENT_PAYMENT");            
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("RECRUITMENT_PAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0M));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "RECRUITMENT_PAYMENT");
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("RECRUITMENT_PAYMENT", DbType.Decimal));

            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "OVERHAUL_PAYMENT");
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("OVERHAUL_PAYMENT", DbType.Decimal));
        }
    }
}
