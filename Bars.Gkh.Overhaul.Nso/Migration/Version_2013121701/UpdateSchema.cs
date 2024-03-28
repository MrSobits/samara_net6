namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121701
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Overhaul.Nso.Enum;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("OVRHL_PR_DEC_LIST_SERVICES",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_LIST_SERVICES", "OVRHL_PR_DEC_LIST_SERVICES", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");

            Database.AddTable("OVRHL_PR_DEC_OWNER_ACCOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OWNER_ACCOUNT_TYPE", DbType.Int32, ColumnProperty.NotNull, 10),
                new RefColumn("REG_OPERATOR_ID", ColumnProperty.Null, "OVRHL_OWNER_ACCOUNT_REGOPER", "OVRHL_REG_OPERATOR", "ID"),
                new Column("OWNER_SURNAME", DbType.String),
                new Column("OWNER_NAME", DbType.String),
                new Column("OWNER_PATRONYMIC", DbType.String),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_OWNER_ACCOUNT", "OVRHL_PR_DEC_OWNER_ACCOUNT", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");

            Database.AddTable("OVRHL_PR_DEC_CREDIT_ORG",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("CREDIT_ORG_ID", ColumnProperty.Null, "OVRHL_OWNER_ACCOUNT_CREDORG", "OVRHL_CREDIT_ORG", "ID"),
                new Column("SETTL_ACCOUNT", DbType.String),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_CREDIT_ORG", "OVRHL_PR_DEC_CREDIT_ORG", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");
            
            Database.AddTable("OVRHL_PR_DEC_MIN_FUND_SIZE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("SUBJ_MIN_FUND_SIZE", DbType.Int64, 40),
                new Column("OWN_MIN_FUND_SIZE", DbType.Int64),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_MIN_FUND_SIZE", "OVRHL_PR_DEC_MIN_FUND_SIZE", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");

            Database.AddTable("OVRHL_PR_DEC_ACCUM_AMOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("ACCUM_AMOUNT", DbType.Decimal.WithSize(18, 2), 0m),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_ACCUM_AMOUNT", "OVRHL_PR_DEC_ACCUM_AMOUNT", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("OVRHL_PR_DEC_LIST_SERVICES", "FK_OVRHL_PR_DEC_LIST_SERVICES");
            Database.RemoveTable("OVRHL_PR_DEC_LIST_SERVICES");

            Database.RemoveConstraint("OVRHL_PR_DEC_OWNER_ACCOUNT", "FK_OVRHL_PR_DEC_OWNER_ACCOUNT");
            Database.RemoveConstraint("OVRHL_PR_DEC_OWNER_ACCOUNT", "FK_OVRHL_OWNER_ACCOUNT_REGOPER");
            Database.RemoveIndex("IND_OVRHL_OWNER_ACCOUNT_REGOPER", "OVRHL_PR_DEC_OWNER_ACCOUNT");
            Database.RemoveTable("OVRHL_PR_DEC_OWNER_ACCOUNT");

            Database.RemoveConstraint("OVRHL_PR_DEC_CREDIT_ORG", "FK_OVRHL_PR_DEC_CREDIT_ORG");
            Database.RemoveConstraint("OVRHL_PR_DEC_CREDIT_ORG", "FK_OVRHL_OWNER_ACCOUNT_CREDORG");
            Database.RemoveIndex("IND_OVRHL_OWNER_ACCOUNT_CREDORG", "OVRHL_PR_DEC_CREDIT_ORG");
            Database.RemoveTable("OVRHL_PR_DEC_CREDIT_ORG");

            Database.RemoveConstraint("OVRHL_PR_DEC_MIN_FUND_SIZE", "FK_OVRHL_PR_DEC_MIN_FUND_SIZE");
            Database.RemoveTable("OVRHL_PR_DEC_MIN_FUND_SIZE");

            Database.RemoveConstraint("OVRHL_PR_DEC_ACCUM_AMOUNT", "FK_OVRHL_PR_DEC_ACCUM_AMOUNT");
            Database.RemoveTable("OVRHL_PR_DEC_ACCUM_AMOUNT");
        }
    }
}