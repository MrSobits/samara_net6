namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_CALC_ACC",
                new RefColumn("ACCOUNT_OWNER_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_OWN", "GKH_CONTRAGENT", "ID"),
                new RefColumn("CREDIT_ORG_ID", "REGOP_CALC_ACC_CRO", "OVRHL_CREDIT_ORG", "ID"),
                new Column("ACCOUNT_NUMBER", DbType.String, 100),
                new Column("DATE_OPEN", DbType.Date, ColumnProperty.NotNull),
                new Column("DATE_CLOSE", DbType.Date),
                new Column("TYPE_ACCOUNT", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("TYPE_OWNER", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("TOTAL_OUT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("TOTAL_IN", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("ABALANCE", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("ABALANCE_IN", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("ABALANCE_OUT", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.AddJoinedSubclassTable("REGOP_CALC_ACC_REGOP", "REGOP_CALC_ACC", "REGOP_CALC_ACC_RGP",
                new RefColumn("CONTR_CREDIT_ORG_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_REGOP_CCRO", "GKH_CONTR_BANK_CR_ORG", "ID"));

            Database.AddJoinedSubclassTable("REGOP_CALC_ACC_SPEC", "REGOP_CALC_ACC", "REGOP_CALC_ACC_SPC",
                new Column("IS_ACTIVE", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddEntityTable("REGOP_CALC_ACC_CREDIT",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_CREDIT_ACC", "REGOP_CALC_ACC", "ID"),
                new RefColumn("DOCUMENT_ID", "REGOP_CALC_ACC_CREDIT_DOC", "B4_FILE_INFO", "ID"),
                new Column("DATE_START", DbType.Date, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.Date),
                new Column("CREDIT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PERCENT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CREDIT_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PERCENT_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PERCENT_RATE", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CREDIT_PERIOD", DbType.Int32, ColumnProperty.NotNull, 0m));

            Database.AddEntityTable("REGOP_CALC_ACC_OVERDRAFT",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_OVERDRAFT_AC", "REGOP_CALC_ACC", "ID"),
                new Column("DATE_START", DbType.Date, ColumnProperty.NotNull),
                new Column("OVERDRAFT_LIMIT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("OVERDRAFT_PERIOD", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PERCENT_RATE", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("AVAILABLE_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.AddEntityTable("REGOP_CALC_ACC_RO",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_RO_ACC", "REGOP_CALC_ACC", "ID"),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_RO_ROBJ", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CALC_ACC_RO");
            Database.RemoveTable("REGOP_CALC_ACC_OVERDRAFT");
            Database.RemoveTable("REGOP_CALC_ACC_CREDIT");

            Database.RemoveTable("REGOP_CALC_ACC_SPEC");
            Database.RemoveTable("REGOP_CALC_ACC_REGOP");
            Database.RemoveTable("REGOP_CALC_ACC");
        }
    }
}
