namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061203
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061203")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061202.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_DEBTOR",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "R_DEBTOR_PA", "REGOP_PERS_ACC", "ID"),
                new Column("PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("DAYS_COUNT", DbType.Int16, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_DEBTOR");
        }
    }
}
