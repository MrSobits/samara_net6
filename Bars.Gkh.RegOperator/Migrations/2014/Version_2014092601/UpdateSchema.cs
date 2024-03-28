namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092601
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            }

            Database.AddEntityTable("REGOP_RO_LOAN_WALLET", new RefColumn("REGOP_RO_LOAN_ID", ColumnProperty.NotNull, "REGOP_RO_LOAN_WALLET_L", "REGOP_RO_LOAN", "ID"), new RefColumn("SOURCE_WALLET_ID", ColumnProperty.NotNull, "REGOP_RO_LOAN_WALLET_SW", "REGOP_WALLET", "ID"), new RefColumn("TARGET_WALLET_ID", ColumnProperty.NotNull, "REGOP_RO_LOAN_WALLET_TW", "REGOP_WALLET", "ID"), new Column("SUM", DbType.Decimal, ColumnProperty.NotNull), new Column("RETURNED_SUM", DbType.Decimal, ColumnProperty.NotNull), new Column("TYPE_SOURCE_LOAN", DbType.Int16, ColumnProperty.NotNull));

            Database.AddColumn("REGOP_RO_LOAN", new Column("C_GUID", DbType.String, 40, ColumnProperty.Null));

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("update REGOP_RO_LOAN set c_guid = uuid_generate_v4()::text where c_guid is null or c_guid = ''");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("update REGOP_RO_LOAN set c_guid = RAWTOHEX(sys_guid()) where c_guid is null");
            }
            
            Database.AlterColumnSetNullable("regop_ro_loan", "c_guid", false);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_LOAN", "C_GUID");
            Database.RemoveTable("REGOP_RO_LOAN_WALLET");
        }
    }
}
