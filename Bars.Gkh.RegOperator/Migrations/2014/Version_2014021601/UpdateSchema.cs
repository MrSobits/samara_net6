namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021601
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("REGOP_RO_LOAN", "PLAN_END_DATE", true);
            Database.RemoveColumn("REGOP_RO_LOAN", "LOAN_SUM");
            Database.RemoveColumn("REGOP_RO_LOAN", "LOAN_RETURNED_SUM");
            Database.AddColumn("REGOP_RO_LOAN", new Column("LOAN_SUM", DbType.Decimal, ColumnProperty.NotNull, "0"));
            Database.AddColumn("REGOP_RO_LOAN", new Column("LOAN_RETURNED_SUM", DbType.Decimal, ColumnProperty.NotNull, "0"));
        }

        public override void Down()
        {
        }
    }
}
