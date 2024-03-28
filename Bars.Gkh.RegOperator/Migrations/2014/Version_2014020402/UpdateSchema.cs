namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014020402
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020402")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014020400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERS_ACC_PERIOD_SUMM",
                new Column("CHARGE_TARIFF", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PENALTY", DbType.Decimal, ColumnProperty.NotNull),
                new Column("RECALC", DbType.Decimal, ColumnProperty.NotNull),
                new Column("SALDO_IN", DbType.Decimal, ColumnProperty.NotNull),
                new Column("SALDO_OUT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PAYMENT", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "ROP_PERS_ACC_PER_SUM_P", "REGOP_PERIOD", "ID"),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "ROP_PERS_ACC_PER_SUM_ACC", "REGOP_PERS_ACC", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERS_ACC_PERIOD_SUMM");
        }
    }
}
