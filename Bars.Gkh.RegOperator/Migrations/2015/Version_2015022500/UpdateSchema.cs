namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015022500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015022500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015022200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_BANK_ACC_STMNT", new RefColumn("PAYER_CONTRAGENT_ID", "PAYER_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddRefColumn("REGOP_BANK_ACC_STMNT", new RefColumn("RECIPIENT_CONTRAGENT_ID", "RECIPIENT_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddRefColumn("REGOP_BANK_ACC_STMNT", new RefColumn("FILE_ID", "ACC_STMNT_FILE", "B4_FILE_INFO", "ID"));

            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("MONEY_DIRECTION", DbType.Int16, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("STATE", DbType.Int16, ColumnProperty.NotNull, 20));

            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("PAYER_CORR_ACC", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("PAYER_INN", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("PAYER_KPP", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("PAYER_BIK", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("PAYER_BANK", DbType.String, 200));

            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("RECIPIENT_CORR_ACC", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("RECIPIENT_INN", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("RECIPIENT_KPP", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("RECIPIENT_BIK", DbType.String, 200));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("RECIPIENT_BANK", DbType.String, 200));

            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("LINKED_DOCUMENTS", DbType.String, 2000));

            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("DISTR_CODE", DbType.String, 100));

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "uuid_generate_v4()::text"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "RAWTOHEX(sys_guid())"));
            }

            Database.AddEntityTable("REGOP_BANK_ACC_STMNT_LINK",
                new RefColumn("STATEMENT_ID", ColumnProperty.NotNull, "REGOP_BANK_ACC_STMNT_LNK_ST", "REGOP_BANK_ACC_STMNT", "ID"),
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "REGOP_BANK_ACC_STMNT_LNK_BD", "REGOP_BANK_DOC_IMPORT", "ID"));

            Database.AddEntityTable("REGOP_DISTR_DETAIL",
                new Column("ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("DOBJECT", DbType.String, 500, ColumnProperty.NotNull),
                new Column("DISTR_SOURCE", DbType.Int16, ColumnProperty.NotNull),
                new Column("DSUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DESTINATION", DbType.String, 500, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_CONTRAGENT_ID");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECIPIENT_CONTRAGENT_ID");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "FILE_ID");

            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "MONEY_DIRECTION");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "STATE");

            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_CORR_ACC");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_INN");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_KPP");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_BIK");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_BANK");

            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECIPIENT_CORR_ACC");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECIPIENT_INN");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECIPIENT_KPP");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECIPIENT_BIK");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "RECIPIENT_BANK");
            
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "TRANSFER_GUID");

            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "LINKED_DOCUMENTS");

            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "DISTR_CODE");

            Database.RemoveTable("REGOP_BANK_ACC_STMNT_LINK");

            Database.RemoveTable("REGOP_DISTR_DETAIL");
        }
    }
}
