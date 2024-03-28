namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014120400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014120301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            }

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("RF_TRANSFER_CTR", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "uuid_generate_v4()::text"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("RF_TRANSFER_CTR", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "RAWTOHEX(sys_guid())"));
            }

            Database.AddColumn("RF_TRANSFER_CTR", new Column("CSUM", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("PAID_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.AlterColumnSetNullable("RF_TRANSFER_CTR", "FIN_SOURCE_ID", true);
        }

        public override void Down()
        {
            Database.RemoveColumn("RF_TRANSFER_CTR", "PAID_SUM");
            Database.RemoveColumn("RF_TRANSFER_CTR", "CSUM");

            Database.RemoveColumn("RF_TRANSFER_CTR", "TRANSFER_GUID");

        }
    }
}
