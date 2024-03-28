using System.Data;

namespace Bars.Gkh.Overhaul.Migration.Version_2014040400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014032500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.TableExists("OVRHL_ACCOUNT"))
            {
                Database.AddEntityTable(
                    "OVRHL_ACCOUNT",
                    new Column("ACC_NUMBER", DbType.String, 50),
                    new Column("OPEN_DATE", DbType.Date),
                    new Column("CLOSE_DATE", DbType.Date),
                    new Column("BALANCE", DbType.Decimal),
                    new Column("LAST_OPERATION_DATE", DbType.Date),
                    new Column("ACCOUNT_TYPE", DbType.Int32, ColumnProperty.NotNull, 10),
                    new Column("CREDIT_LIMIT", DbType.Decimal));
            }

            if (Database.ColumnExists("OVRHL_ACCOUNT", "TOTAL_DEBIT"))
            {
                Database.RenameColumn("OVRHL_ACCOUNT", "TOTAL_DEBIT", "TOTAL_INCOME");
            }

            if (!Database.ColumnExists("OVRHL_ACCOUNT", "TOTAL_INCOME"))
            {
                Database.AddColumn("OVRHL_ACCOUNT", new Column("TOTAL_INCOME", DbType.Decimal));
            }

            if (Database.ColumnExists("OVRHL_ACCOUNT", "TOTAL_CREDIT"))
            {
                Database.RenameColumn("OVRHL_ACCOUNT", "TOTAL_CREDIT", "TOTAL_OUT");
            }

            if (!Database.ColumnExists("OVRHL_ACCOUNT", "TOTAL_OUT"))
            {
                Database.AddColumn("OVRHL_ACCOUNT", new Column("TOTAL_OUT", DbType.Decimal));
            }

            if (!Database.ColumnExists("OVRHL_ACCOUNT", "ACCOUNT_TYPE"))
            {
                Database.AddColumn("OVRHL_ACCOUNT", new Column("ACCOUNT_TYPE", DbType.Int32, ColumnProperty.NotNull, 10));
            }

            if (!Database.ColumnExists("OVRHL_ACCOUNT", "CREDIT_LIMIT"))
            {
                Database.AddColumn("OVRHL_ACCOUNT", new Column("CREDIT_LIMIT", DbType.Decimal));
            }

            if (Database.ColumnExists("OVRHL_ACCOUNT", "LONG_TERM_OBJ_ID"))
            {
                AddColumn("OVRHL_ACCOUNT", "OVRHL_ACC_ROBJ", "LONG_TERM_OBJ_ID");
            }

            if (!Database.ColumnExists("OVRHL_ACCOUNT", "REALITY_OBJECT_ID"))
            {
                Database.AddRefColumn("OVRHL_ACCOUNT", new RefColumn("REALITY_OBJECT_ID", "OVRHL_ACC_ROBJ", "GKH_REALITY_OBJECT", "ID"));
            }

            if (!Database.TableExists("OVRHL_CREDIT_ORG"))
            {
                Database.AddEntityTable("OVRHL_CREDIT_ORG",
                    new RefColumn("FIAS_ID", "OVRHL_CREDIT_ORG_FIAS", "B4_FIAS_ADDRESS", "ID"),
                    new RefColumn("PARENT_ID", "OVRHL_CREDIT_ORG_CRORG", "OVRHL_CREDIT_ORG", "ID"),
                    new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                    new Column("IS_FILIAL", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("ADDRESS", DbType.String, 500),
                    new Column("ADDRESS_OUT_SUBJECT", DbType.String, 500),
                    new Column("IS_ADDRESS_OUT", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("INN", DbType.String, 20),
                    new Column("KPP", DbType.String, 20),
                    new Column("BIK", DbType.String, 20),
                    new Column("OKPO", DbType.String, 20),
                    new Column("CORR_ACCOUNT", DbType.String, 50)
                );
            }

            if (!Database.ColumnExists("OVRHL_CREDIT_ORG", "OGRN"))
            {
                Database.AddColumn("OVRHL_CREDIT_ORG", new Column("OGRN", DbType.String, 250));
            }

            if (!Database.ColumnExists("OVRHL_CREDIT_ORG", "MAILING_ADDRESS"))
            {
                Database.AddColumn("OVRHL_CREDIT_ORG", new Column("MAILING_ADDRESS", DbType.String, 500));
            }

            if (!Database.ColumnExists("OVRHL_CREDIT_ORG", "FIAS_MAIL_ADDRESS_ID"))
            {
                Database.AddRefColumn("OVRHL_CREDIT_ORG", new RefColumn("FIAS_MAIL_ADDRESS_ID", "OV_CR_ORG_FIAS_MAIL", "B4_FIAS_ADDRESS", "ID"));
            }

            if (!Database.ColumnExists("OVRHL_CREDIT_ORG", "OKTMO"))
            {
                Database.AddColumn("OVRHL_CREDIT_ORG", new Column("OKTMO", DbType.String));
            }
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_ACCOUNT");
            Database.RemoveTable("OVRHL_CREDIT_ORG");
        }

        private void AddColumn(string tableName, string indexFkName, string joinColumn)
        {
            var columnName = "REALITY_OBJECT_ID";

            Database.AddRefColumn(tableName, new RefColumn(columnName, indexFkName, "GKH_REALITY_OBJECT", "ID"));

            //перевешиваем существующие записи на жилые дома
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(string.Format("update {0} as t1 set REALITY_OBJECT_ID = t2.REALITY_OBJ_ID from OVRHL_LONGTERM_PR_OBJECT t2 where t2.id=t1.{1}", tableName, joinColumn));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(string.Format("update {0} t1 set t1.REALITY_OBJECT_ID = (select t2.REALITY_OBJ_ID from OVRHL_LONGTERM_PR_OBJECT t2 where t2.id=t1.{1})", tableName, joinColumn));
            }

            //удаляем те записи, для которых не нашелся жилой дом
            DeleteNullRecords(tableName, columnName);

            AddNotNull(tableName, columnName);

            Database.RemoveColumn(tableName, joinColumn);
        }

        private void DeleteNullRecords(string tableName, string column)
        {
            Database.ExecuteNonQuery(string.Format("delete from {0} where {1} is null", tableName, column));
        }

        private void AddNotNull(string tableName, string column)
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(string.Format("alter table {0} alter column {1} set not null", tableName, column));
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(string.Format("alter table {0} modify {1} not null", tableName, column));
            }
        }
    }
}