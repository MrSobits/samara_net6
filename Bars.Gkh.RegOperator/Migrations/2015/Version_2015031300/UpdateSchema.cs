namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015031300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015031200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("RD_DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("RD_PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_DEBT_SUM");
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_PENALTY_DEBT");

        }
    }
}
