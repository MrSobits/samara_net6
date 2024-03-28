namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061601
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SUSPACC_CREDIT",
                new RefColumn("CREDIT_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_CREDIT_CR", "REGOP_CALC_ACC_CREDIT", "ID"),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_CREDIT_ACC", "REG_OP_SUSPEN_ACCOUNT", "ID"),
                new Column("CPAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PPAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SUSPACC_CREDIT");
        }
    }
}
