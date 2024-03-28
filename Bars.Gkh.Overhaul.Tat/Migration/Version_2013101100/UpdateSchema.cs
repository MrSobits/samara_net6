namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013101100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013101001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесен в модуль overhaul
            //Database.AddEntityTable(
            //    "OVRHL_ACCOUNT",
            //    new Column("ACC_NUMBER", DbType.String, 50, ColumnProperty.NotNull),
            //    new Column("OPEN_DATE", DbType.Date),
            //    new Column("CLOSE_DATE", DbType.Date),
            //    new Column("TOTAL_DEBIT", DbType.Double),
            //    new Column("TOTAL_CREDIT", DbType.Double),
            //    new Column("BALANCE", DbType.Double),
            //    new Column("LAST_OPERATION_DATE", DbType.Date));

            Database.AddTable(
                "OVRHL_SPECIAL_ACCOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),

                new Column("CREDIT_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ACCOUNT_OWNER_ID", DbType.Int64, 22, ColumnProperty.NotNull));

            //Database.AddForeignKey("FK_OVRHL_SPEC_ACC_CRED_ORG", "OVRHL_SPECIAL_ACCOUNT", "CREDIT_ORG_ID", "OVRHL_CREDIT_ORG", "ID");
            //Database.AddForeignKey("FK_OVRHL_SPEC_ACC_CTR", "OVRHL_SPECIAL_ACCOUNT", "ACCOUNT_OWNER_ID", "GKH_CONTRAGENT", "ID");

            Database.AddIndex("OVRHL_SPEC_ACC_CRED_ORG", false, "OVRHL_SPECIAL_ACCOUNT", "CREDIT_ORG_ID");
            Database.AddIndex("OVRHL_SPEC_ACC_CTR", false, "OVRHL_SPECIAL_ACCOUNT", "ACCOUNT_OWNER_ID");


            Database.AddTable(
                "OVRHL_REAL_ACCOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),

                new Column("REG_OPER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OVERDRAFT_LIMIT", DbType.Double));

            Database.AddForeignKey("FK_OVRHL_REAL_ACC_REG_OP", "OVRHL_REAL_ACCOUNT", "REG_OPER_ID", "OVRHL_REG_OPERATOR", "ID");
            Database.AddIndex("OVRHL_REAL_ACC_REG_OP", false, "OVRHL_REAL_ACCOUNT", "REG_OPER_ID");

            Database.AddTable(
                "OVRHL_ACCRUALS_ACCOUNT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),

                new Column("OPENING_BALANCE", DbType.Double));
        }

        public override void Down()
        {
           
        }
    }
}