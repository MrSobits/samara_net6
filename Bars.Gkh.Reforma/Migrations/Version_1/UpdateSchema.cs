namespace Bars.Gkh.Reforma.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "RFRM_SESSION_LOG",
                new Column("SESSION_ID", DbType.String, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("START_TIME", DbType.DateTime, ColumnProperty.NotNull));

            Database.AddEntityTable(
                "RFRM_ACTION_LOG",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("ERROR_CODE", DbType.String, ColumnProperty.Null),
                new Column("ERROR_NAME", DbType.String, ColumnProperty.Null),
                new Column("ERROR_DESCRIPTION", DbType.String, 1000, ColumnProperty.Null),
                new Column("PARAMETERS", DbType.Binary, ColumnProperty.NotNull),
                new Column("REQUEST_TIME", DbType.DateTime, ColumnProperty.NotNull),
                new Column("RESPONSE_TIME", DbType.DateTime, ColumnProperty.NotNull),
                new Column("SUCCESS", DbType.Boolean, ColumnProperty.NotNull),
                new RefColumn("SESSION_LOG_ITEM_ID", ColumnProperty.NotNull, "RFRM_ACT_LOG_SESS", "RFRM_SESSION_LOG", "ID"),
                new RefColumn("PACKETS_FILE_ID", ColumnProperty.Null, "RFRM_ACT_PACKET", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("RFRM_ACTION_LOG");

            Database.RemoveIndex("RFRM_SESS_GUID", "RFRM_SESSION_LOG");
            Database.RemoveTable("RFRM_SESSION_LOG");
        }
    }
}