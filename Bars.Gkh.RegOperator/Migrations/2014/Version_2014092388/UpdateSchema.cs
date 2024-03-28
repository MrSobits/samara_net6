namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092388
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092388")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092387.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
                Database.AddColumn("REGOP_UNACCEPT_PAY", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "uuid_generate_v4()::text"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_UNACCEPT_PAY", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "RAWTOHEX(sys_guid())"));
            }
            Database.AddIndex("IDX_REGOP_UNACC_PAY_TRG", true, "REGOP_UNACCEPT_PAY", "TRANSFER_GUID");

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AddColumn("REGOP_UNACCEPT_PAY_PACKET", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "uuid_generate_v4()::text"));
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AddColumn("REGOP_UNACCEPT_PAY_PACKET", new Column("TRANSFER_GUID", DbType.String, 40, ColumnProperty.NotNull, "RAWTOHEX(sys_guid())"));
            }
            Database.AddIndex("IDX_REGOP_UNACC_PAYP_TRG", true, "REGOP_UNACCEPT_PAY_PACKET", "TRANSFER_GUID");
        }

        public override void Down()
        {
            Database.RemoveIndex("IDX_REGOP_UNACC_PAYP_TRG", "REGOP_UNACCEPT_PAY_PACKET");
            Database.RemoveColumn("REGOP_UNACCEPT_PAY_PACKET", "TRANSFER_GUID");

            Database.RemoveIndex("IDX_REGOP_UNACC_PAY_TRG", "REGOP_UNACCEPT_PAY");
            Database.RemoveColumn("REGOP_UNACCEPT_PAY", "TRANSFER_GUID");
        }
    }
}
