namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015050501
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015042800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameColumn("CLW_DEBTOR_CLAIM_WORK", "DEBT_SUM", "CUR_CHARGE_DEBT");
            Database.RenameColumn("CLW_DEBTOR_CLAIM_WORK", "PENALTY_DEBT", "CUR_PENALTY_DEBT");

            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("ORIG_CHARGE_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("ORIG_PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "ORIG_CHARGE_DEBT");
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "ORIG_PENALTY_DEBT");

            Database.RenameColumn("CLW_DEBTOR_CLAIM_WORK", "CUR_CHARGE_DEBT", "DEBT_SUM");
            Database.RenameColumn("CLW_DEBTOR_CLAIM_WORK", "CUR_PENALTY_DEBT", "PENALTY_DEBT");
        }
    }
}
