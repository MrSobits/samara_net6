namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2019091200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019091200")]
    [MigrationDependsOn(typeof(Version_2019080100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_DICT_OWNER_PROTOCOL_TYPE",
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500));

            Database.AddEntityTable("OVRHL_DICT_PROTTYPE_DECISION",
                new RefColumn("PROTOCOL_TYPE_ID", ColumnProperty.NotNull, "OVRHL_DICT_PROTTYPE_DECISION_TP", "OVRHL_DICT_OWNER_PROTOCOL_TYPE", "ID"),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull));

            Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("PROTOCOL_TYPE_ID", "OVRHL_PROP_OWN_PROTOCOLS_TP", "OVRHL_DICT_OWNER_PROTOCOL_TYPE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "PROTOCOL_TYPE_ID");
            Database.RemoveTable("OVRHL_DICT_PROTTYPE_DECISION");
            Database.RemoveTable("OVRHL_DICT_OWNER_PROTOCOL_TYPE");
        }
    }
}