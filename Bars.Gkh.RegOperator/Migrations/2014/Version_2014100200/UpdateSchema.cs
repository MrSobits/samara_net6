namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014100200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014100101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            }

            var update = "update REGOP_PERS_ACC_PAYMENT set PGUID={0} where PGUID is NULL or PGUID=''";

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(update.FormatUsing("uuid_generate_v4()::text"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(update.FormatUsing("RAWTOHEX(sys_guid())"));
            }

            Database.AddColumn("REGOP_PERS_ACC", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.Null));

            var updateAccs = "update REGOP_PERS_ACC set TRANSFER_GUID={0} where TRANSFER_GUID is NULL or TRANSFER_GUID=''";
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(updateAccs.FormatUsing("uuid_generate_v4()::text"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(updateAccs.FormatUsing("RAWTOHEX(sys_guid())"));
            }

            Database.AddIndex("IDX_REGOP_P_ACC_TR_G", true, "REGOP_PERS_ACC", "TRANSFER_GUID");

            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("AF_WALLET_ID", ColumnProperty.Null, "REGOP_ROACC_W_AF", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("PWP_WALLET_ID", ColumnProperty.Null, "REGOP_ROACC_W_PWP", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("R_WALLET_ID", ColumnProperty.Null, "REGOP_ROACC_W_R", "REGOP_WALLET", "ID"));
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("SS_WALLET_ID", ColumnProperty.Null, "REGOP_ROACC_W_SS", "REGOP_WALLET", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "SS_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "R_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "PWP_WALLET_ID");
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "AF_WALLET_ID");

            Database.RemoveIndex("IDX_REGOP_P_ACC_TR_G", "REGOP_PERS_ACC");
            Database.RemoveColumn("REGOP_PERS_ACC", "TRANSFER_GUID");
        }
    }
}
