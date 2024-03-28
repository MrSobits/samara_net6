namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014060800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014060701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_RO_LOAN_PAYMENT",
                new RefColumn("LOAN_ID", ColumnProperty.NotNull, "REGOP_RO_LOAN_PAYMENT_LO", "REGOP_RO_LOAN", "ID"),
                new RefColumn("INCOME_ID", ColumnProperty.NotNull, "REGOP_RO_LOAN_PAYMENT_IN", "REGOP_RO_PAYMENT_ACC_OP", "ID"),
                new RefColumn("OUTCOME_ID", ColumnProperty.NotNull, "REGOP_RO_LOAN_PAYMENT_OUT", "REGOP_RO_PAYMENT_ACC_OP", "ID"));

            Database.AddEntityTable("REGOP_UNACCEPT_PAY_LOAN",
                new RefColumn("LOAN_PAYMENT_ID", ColumnProperty.NotNull, "REGOP_UNACCEPT_PAY_LOAN_LO", "REGOP_RO_LOAN_PAYMENT", "ID"),
                new RefColumn("UNACC_PAYMENT_ID", ColumnProperty.NotNull, "REGOP_UNACCEPT_PAY_LOAN_UP", "REGOP_UNACCEPT_PAY", "ID"),
                new Column("PSUM", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_UNACCEPT_PAY_LOAN");

            Database.RemoveTable("REGOP_RO_LOAN_PAYMENT");
        }
    }
}
