namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061204
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061204")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061203.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_LOAN_PAY_ACC",
                new RefColumn("LOAN_ID", ColumnProperty.NotNull, "LOAN_PAY_ACC_LOAN", "REGOP_RO_LOAN", "ID"),
                new RefColumn("PAY_ACC_OP_ID", ColumnProperty.NotNull, "LOAN_PAY_ACC_PAY_ACC", "REGOP_RO_PAYMENT_ACC_OP", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_LOAN_PAY_ACC");
        }
    }
}
