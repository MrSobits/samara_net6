namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016121200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

	[Migration("2016121200")]
    [MigrationDependsOn(typeof(_2016.Version_2016030500.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016030502.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016030700.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016032900.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016033100.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016041900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddTable("CLW_UTILITY_DEBTOR_CLAIM_WORK",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("ACCOUNT_OWNER", DbType.String, 150),
                new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull, 20),
                new Column("CHARGE_DEBT", DbType.Decimal),
                new Column("PENALTY_DEBT", DbType.Decimal),
                new Column("ACCOUNT_NUM", DbType.String, 50),
                new Column("ACCOUNT_STATE", DbType.String, 150));

            this.Database.AddTable("CLW_EXECUTORY_PROCESS",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("ACCOUNT_OWNER", DbType.String, 150),
                new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull, 20),
                new RefColumn("RO_ID", "CLW_EXEC_PROC_RO", "GKH_REALITY_OBJECT", "ID"),

                new RefColumn("JUR_INSTITUTION_ID", "CLW_EXEC_PROC_JUR_INST", "CLW_JUR_INSTITUTION", "ID"),
                new RefColumn("FILE_ID", "CLW_EXEC_PROC_FILE", "B4_FILE_INFO", "ID"),
                new Column("REG_NUMBER", DbType.String, 50),
                new Column("DOCUMENT", DbType.String, 50),
                new Column("DEBT_SUM", DbType.Decimal),
                new Column("PAID_SUM", DbType.Decimal),
                new Column("DATE_START", DbType.DateTime),
                new Column("IS_TERMINATED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DATE_END", DbType.DateTime),
                new Column("TERM_REASON_TYPE", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("NOTATION", DbType.String, 255),
                new Column("CREDITOR", DbType.String, 255),
                new RefColumn("LEGAL_OWNER_RO_ID", "CLW_EXEC_PROC_LEGAL_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("INN", DbType.String, 50),
                new Column("CLAUSE", DbType.String, 50),
                new Column("PARAGRAPH", DbType.String, 50),
                new Column("SUBPARAGRAPH", DbType.String, 50));

            this.Database.AddTable("CLW_PROP_SEIZURE",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("ACCOUNT_OWNER", DbType.String, 150),
                new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull, 20),
                new RefColumn("JUR_INSTITUTION_ID", "CLW_EXEC_PROC_JUR_INST", "CLW_JUR_INSTITUTION", "ID"),

                new Column("YEAR", DbType.String, 4),
                new Column("NUMBER", DbType.String, 50),
                new Column("SUBNUMBER", DbType.String, 10),
                new Column("DOCUMENT", DbType.String, 50),
                new Column("DELIVERY_DATE", DbType.DateTime),
                new Column("IS_CANCELED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("CANCEL_REASON", DbType.String, 150),
                new Column("OFFICIAL", DbType.String, 255),
                new Column("LOCATION", DbType.String, 255),
                new Column("CREDITOR", DbType.String, 255),
                new Column("BANK_DETAILS", DbType.String, 255));

            this.Database.AddTable("CLW_DEPARTURE_RESTRICTION",
               new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
               new Column("ACCOUNT_OWNER", DbType.String, 150),
               new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull, 20),
               new RefColumn("JUR_INSTITUTION_ID", "CLW_EXEC_PROC_JUR_INST", "CLW_JUR_INSTITUTION", "ID"),

               new Column("YEAR", DbType.String, 4),
               new Column("NUMBER", DbType.String, 50),
               new Column("SUBNUMBER", DbType.String, 10),
               new Column("DOCUMENT", DbType.String, 50),
               new Column("DELIVERY_DATE", DbType.DateTime),
               new Column("IS_CANCELED", DbType.Boolean, ColumnProperty.NotNull, false),
               new Column("CANCEL_REASON", DbType.String, 150),
               new Column("OFFICIAL", DbType.String, 255),
               new Column("LOCATION", DbType.String, 255),
               new Column("CREDITOR", DbType.String, 255),
               new Column("BANK_DETAILS", DbType.String, 255));

            this.Database.AddEntityTable("CLW_EXEC_PROC_DOCUMENT",
                    new RefColumn("EXEC_PROC_ID", ColumnProperty.NotNull, "CLW_EXEC_PROC_DOC_D", "CLW_EXECUTORY_PROCESS", "ID"),
                    new RefColumn("FILE_ID", ColumnProperty.NotNull, "CLW_EXEC_PROC_DOC_FILE", "B4_FILE_INFO", "ID"),
                    new Column("NUMBER", DbType.String, ColumnProperty.NotNull, 50),
                    new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
                    new Column("DOC_TYPE", DbType.Int32, ColumnProperty.NotNull, 10),
                    new Column("NOTATION", DbType.String, 255));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CLW_EXEC_PROC_DOCUMENT");
            this.Database.RemoveTable("CLW_DEPARTURE_RESTRICTION");
            this.Database.RemoveTable("CLW_PROP_SEIZURE");
            this.Database.RemoveTable("CLW_EXECUTORY_PROCESS");
            this.Database.RemoveTable("CLW_UTILITY_DEBTOR_CLAIM_WORK");
        }
    }
}