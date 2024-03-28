namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CCURR_BALANCE");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CTOTAL_DEBIT");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "CTOTAL_CREDIT");
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("PACC_BALANCE", DbType.Decimal, ColumnProperty.NotNull, "0"));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("SUBCIDY_BALANCE", DbType.Decimal, ColumnProperty.NotNull, "0"));

            Database.RemoveColumn("REGOP_RO_SUPP_ACC", "BALANCE");
            
            Database.RemoveColumn("REGOP_RO_LOAN", "LOAN_DEBT");
            Database.RemoveColumn("REGOP_RO_LOAN", "PROVIDER_ID");
            Database.RemoveColumn("REGOP_RO_LOAN", "RECEIVER_ID");
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("RO_SUBJ_ID", ColumnProperty.Null, "REGOP_LOAN_SUBJ", "GKH_REALITY_OBJECT", "ID"));
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("RO_ID", ColumnProperty.NotNull, "REGOP_LOAN_RO", "GKH_REALITY_OBJECT", "ID"));
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("MO_ID", ColumnProperty.NotNull, "REGOP_LOAN_MO", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddColumn("REGOP_RO_LOAN", new Column("CEO_NAMES", DbType.String, 1000, ColumnProperty.Null));
            Database.AddColumn("REGOP_RO_LOAN", new Column("CDIRECTION", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_LOAN", "RO_ID");
            Database.RemoveColumn("REGOP_RO_LOAN", "MO_ID");
            Database.RemoveColumn("REGOP_RO_LOAN", "RO_SUBJ_ID");
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("RECEIVER_ID", ColumnProperty.Null, "REGOP_LOAN_SUBJ", "GKH_REALITY_OBJECT", "ID"));
            Database.AddRefColumn("REGOP_RO_LOAN", new RefColumn("PROVIDER_ID", ColumnProperty.Null, "REGOP_LOAN_SUBJ", "GKH_REALITY_OBJECT", "ID"));
            Database.AddColumn("REGOP_RO_LOAN", new Column("LOAN_DEBT", DbType.Decimal.WithSize(18, 2), ColumnProperty.NotNull, 0m));
            
            Database.AddColumn("REGOP_RO_SUPP_ACC", new Column("BALANCE", DbType.AnsiString));

            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "SUBCIDY_BALANCE");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "PACC_BALANCE");
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CCURR_BALANCE", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CTOTAL_DEBIT", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("CTOTAL_CREDIT", DbType.DateTime, ColumnProperty.NotNull));
        }
    }
}
