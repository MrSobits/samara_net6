namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014071800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014071600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_TOMSK_PROTOCOL_DESCR",
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "TOMSK_PROTO_DESCR", "GJI_PROTOCOL", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));

            Database.AddEntityTable(
                "GJI_ADMINCASE_DESCR",
                new RefColumn("ADMIN_CASE_ID", ColumnProperty.NotNull, "ADMIN_CASE_DESCR", "GJI_ADMINCASE", "ID"),
                new Column("DESCRIPTION_SET", DbType.Binary, ColumnProperty.Null));

            Database.AddEntityTable(
                "GJI_TOMSK_RESOLUTION_DESCR",
                new RefColumn("RESOLUTION_ID", ColumnProperty.NotNull, "RESOLUT_DESCR", "GJI_TOMSK_RESOLUTION", "ID"),
                new Column("RESOLUTION_TEXT", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TOMSK_RESOLUTION_DESCR");
            Database.RemoveTable("GJI_ADMINCASE_DESCR");
            Database.RemoveTable("GJI_TOMSK_PROTOCOL_DESCR");
        }
    }
}