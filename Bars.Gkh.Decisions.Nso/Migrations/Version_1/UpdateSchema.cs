namespace Bars.Gkh.Decisions.Nso.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //gkh.migration_2014020600
            if (!Database.TableExists("GKH_OBJ_D_PROTOCOL"))
            {
                Database.AddEntityTable("GKH_OBJ_D_PROTOCOL",
                    new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_RO_D_PROT_RO", "GKH_REALITY_OBJECT", "ID"),
                    new RefColumn("FILE_ID", ColumnProperty.NotNull, "GKH_RO_D_PROT_FILE", "B4_FILE_INFO", "ID"),
                    new Column("DESCR", DbType.String, 500, ColumnProperty.Null),
                    new Column("DOCUMENT_NAME", DbType.String, 300, ColumnProperty.Null),
                    new Column("DOCUMENT_NUM", DbType.String, 50, ColumnProperty.Null),
                    new Column("PROTOCOL_DATE", DbType.DateTime, ColumnProperty.NotNull),
                    new Column("V_TOTAL_COUNT", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m),
                    new Column("V_PART_COUNT", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m),
                    new Column("PART_SHARE", DbType.Decimal.WithSize(3, 2), ColumnProperty.NotNull, 0m),
                    new Column("HAS_QUORUM", DbType.Int16, ColumnProperty.NotNull, 20),
                    new Column("POS_V_COUNT", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m),
                    new Column("DECIDED_SHARE", DbType.Decimal.WithSize(8, 2), ColumnProperty.NotNull, 0m));
            }

            //gkh.migration_2014021200
            if (!Database.ColumnExists("GKH_OBJ_D_PROTOCOL", "AUTHORIZED_PERSON"))
            {
                Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("AUTHORIZED_PERSON", DbType.String, 200, ColumnProperty.Null));
            }

            //gkh.migration_2014030601
            if (!Database.ColumnExists("GKH_OBJ_D_PROTOCOL", "STATE_ID"))
            {
                Database.AddRefColumn("GKH_OBJ_D_PROTOCOL", new RefColumn("STATE_ID", "GKH_OBJ_D_PROT_STATE", "B4_STATE", "ID"));
            }

            //gkh.migration_2014020601
            if (!Database.TableExists("GKH_GENERIC_DECISION"))
            {
                Database.AddEntityTable("GKH_GENERIC_DECISION",
                    new Column("DECISION_CODE", DbType.String, 100, ColumnProperty.NotNull),
                    new Column("IS_ACTUAL", DbType.Boolean, ColumnProperty.NotNull),
                    new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                    //gkh.migration_2014020602
                    new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GEN_DECISION_PROT", "GKH_OBJ_D_PROTOCOL", "ID"));
            }

            //gkh.migration_2014020604
            if (!Database.ColumnExists("GKH_GENERIC_DECISION", "JSON_OBJECT"))
            {
                Database.AddColumn("GKH_GENERIC_DECISION", new Column("JSON_OBJECT", DbType.String, 20000, ColumnProperty.Null));
            }

            //gkh.migration_2014020604
            if (!Database.ColumnExists("GKH_GENERIC_DECISION", "FILE_ID"))
            {
                Database.AddRefColumn("GKH_GENERIC_DECISION", new RefColumn("FILE_ID", ColumnProperty.Null, "GKH_GEN_DEC_FILE", "B4_FILE_INFO", "ID"));
            }

            // Базовый
            Database.AddEntityTable("DEC_ULTIMATE_DECISION",
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("IS_CHECKED", DbType.Boolean, ColumnProperty.NotNull),
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "ULT_DEC_PROT", "GKH_OBJ_D_PROTOCOL", "ID"));

            // Владелец счета
            Database.AddTable("DEC_ACCOUNT_OWNER",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("MANORG_ID", DbType.Int64, ColumnProperty.Null));
            Database.AddForeignKey("FK_ACC_OWNER_UD", "DEC_ACCOUNT_OWNER", "ID", "DEC_ULTIMATE_DECISION", "ID");
            Database.AddForeignKey("FK_ACC_OWNER_MANORG", "DEC_ACCOUNT_OWNER", "ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddIndex("IND_ACC_OWNER_MANORG", false, "DEC_ACCOUNT_OWNER", "MANORG_ID");

            // Перевод средств
            Database.AddTable("DEC_ACCUM_TRANSFER",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DECISION_VALUE", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_ACCUM_TRANSFER_UD", "DEC_ACCUM_TRANSFER", "ID", "DEC_ULTIMATE_DECISION", "ID");

            // Выбор кредитной организации
            Database.AddTable("DEC_CREDIT_ORG",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("CREDIT_ORG_ID", DbType.Int64, ColumnProperty.Null),
                new Column("FILE_ID", DbType.Int64, ColumnProperty.Null));
            Database.AddForeignKey("FK_CREDIT_ORG_UD", "DEC_CREDIT_ORG", "ID", "DEC_ULTIMATE_DECISION", "ID");
            Database.AddForeignKey("FK_CREDIT_ORG_CREDORG", "DEC_CREDIT_ORG", "CREDIT_ORG_ID", "OVRHL_CREDIT_ORG", "ID");
            Database.AddIndex("IND_CREDIT_ORG_CREDORG", false, "DEC_CREDIT_ORG", "CREDIT_ORG_ID");
            Database.AddForeignKey("FK_CREDIT_ORG_FILE", "DEC_CREDIT_ORG", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddIndex("IND_CREDIT_ORG_FILE", false, "DEC_CREDIT_ORG", "FILE_ID");

            // Способ формирования фонда
            Database.AddTable("DEC_CR_FUND",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DECISION_VALUE", DbType.Int32, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_CR_FUND_UD", "DEC_CR_FUND", "ID", "DEC_ULTIMATE_DECISION", "ID");

            // Минимальное значение фонда
            Database.AddTable("DEC_MIN_FUND_AMOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DECISION_VALUE", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_MIN_FUND_AMOUNT_UD", "DEC_MIN_FUND_AMOUNT", "ID", "DEC_ULTIMATE_DECISION", "ID");

            // Выбор способа управления
            Database.AddTable("DEC_MKD_MANAGE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("MANORG_ID", DbType.Int64, ColumnProperty.Null),
                new Column("DECISION_TYPE", DbType.Int32, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_MKD_MANAGE_UD", "DEC_MKD_MANAGE", "ID", "DEC_ULTIMATE_DECISION", "ID");
            Database.AddForeignKey("FK_MKD_MANAGE_MANORG", "DEC_MKD_MANAGE", "MANORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddIndex("IND_MKD_MANAGE_MANORG", false, "DEC_MKD_MANAGE", "MANORG_ID");

            // Месячный взнос
            Database.AddTable("DEC_MONTHLY_FEE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DECISION_VALUE", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_MONTHLY_FEE_UD", "DEC_MONTHLY_FEE", "ID", "DEC_ULTIMATE_DECISION", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_MONTHLY_FEE");
            Database.RemoveTable("DEC_MKD_MANAGE");
            Database.RemoveTable("DEC_MIN_FUND_AMOUNT");
            Database.RemoveTable("DEC_CR_FUND");
            Database.RemoveTable("DEC_CREDIT_ORG");
            Database.RemoveTable("DEC_ACCUM_TRANSFER");
            Database.RemoveTable("DEC_ACCOUNT_OWNER");
            Database.RemoveTable("DEC_ULTIMATE_DECISION");

            if (Database.TableExists("GKH_OBJ_D_PROTOCOL"))
            {
                Database.RemoveTable("GKH_OBJ_D_PROTOCOL");
            }
        }
    }
}