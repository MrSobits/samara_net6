namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015052700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Migrations;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015051900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_CALC_ACC", new GuidColumn("TRANSFER_GUID", ColumnProperty.NotNull));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("LOAN", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.RemoveColumn("REGOP_RO_LOAN_WALLET", "SOURCE_WALLET_ID");
            
            Database.RemoveColumn("REGOP_RO_LOAN", "MO_ID");
            Database.RemoveColumn("REGOP_RO_LOAN", "TYPE_SOURCE_LOAN");

            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("SOURCE_CALC_ACC_ID", "REGOP_RO_LOAN_SRC", "REGOP_CALC_ACC_REGOP", "ID"));

            Database.RemoveColumn("regop_ro_loan", "cdirection");
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_CALC_ACC", "TRANSFER_GUID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "LOAN");

            Database.AddRefColumn("REGOP_RO_LOAN_WALLET",
                new RefColumn("SOURCE_WALLET_ID", "REGOP_RO_LOAN_WALLET_SW", "REGOP_WALLET", "ID"));

            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("MO_ID", "REGOP_LOAN_MO", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddColumn("REGOP_RO_LOAN", new Column("TYPE_SOURCE_LOAN", DbType.Int16, ColumnProperty.NotNull, 0));

            Database.RemoveColumn("REGOP_RO_LOAN", "SOURCE_CALC_ACC_ID");

            //Database.AddColumn("regop_ro_loan", new Column("cdirection", DbType.Int32));
        }
    }
}
