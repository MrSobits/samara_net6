namespace Bars.Gkh.Migrations.Version_2013052100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013052100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013051600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Органы государственной власти
            Database.AddEntityTable(
                "GKH_POLITIC_AUTHORITY",
                new RefColumn("CONTRAGENT_ID", "GKH_POLIT_AUTH_CTR", "GKH_CONTRAGENT", "ID"),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EMAIL", DbType.String, 50),
                new Column("NAME_DEP_GKH", DbType.String, 300),
                new Column("OFFICIAL_SITE", DbType.String, 50),
                new Column("PHONE", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            //------

            //-----МО органа государственной власти
            Database.AddEntityTable(
                "GKH_POLITIC_AUTH_MUN",
                new RefColumn("POLITIC_AUTH_ID", "GKH_POL_AUTH_MUN_POL", "GKH_POLITIC_AUTHORITY", "ID"),
                new RefColumn("MUNICIPALITY_ID", "GKH_POL_AUTH_MUN_MCP", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));
            //-----

            //-----"Режим работы органа государственной власти"
            Database.AddEntityTable(
                "GKH_POLITIC_AUTH_WORK",
                new Column("TYPE_MODE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("TYPE_DAY", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime),
                new Column("PAUSE", DbType.String, 50),
                new Column("AROUND_CLOCK", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn("POLITIC_AUTH_ID", "GKH_POL_AUTH_WRK_POL", "GKH_POLITIC_AUTHORITY", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));
            //-----
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_POLITIC_AUTHORITY");
            Database.RemoveTable("GKH_POLITIC_AUTH_MUN");
            Database.RemoveTable("GKH_POLITIC_AUTH_WORK");
        }
    }
}