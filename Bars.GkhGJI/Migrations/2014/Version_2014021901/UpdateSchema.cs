namespace Bars.GkhGji.Migrations.Version_2014021901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014021900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable(
                "GJI_PROTOCOL_MVD",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GJI_PROTMVD_MCP", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DATE_SUPPLY", DbType.DateTime),
                new Column("TYPE_EXECUTANT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_PROTMVD_DOC", "GJI_PROTOCOL_MVD", "ID", "GJI_DOCUMENT", "ID");

            Database.AddEntityTable(
                "GJI_PROT_MVD_ROBJECT",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new RefColumn("PROT_MVD_ID", ColumnProperty.NotNull, "GJI_PROTMVD_RO_DOC", "GJI_PROTOCOL_MVD", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GJI_PROTMVD_RO_OBJ", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable(
                "GJI_PROT_MVD_ARTLAW",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new RefColumn("PROT_MVD_ID", ColumnProperty.NotNull, "GJI_PROTMVD_ARTLAW_DOC", "GJI_PROTOCOL_MVD", "ID"),
                new RefColumn("ARTICLELAW_ID", ColumnProperty.NotNull, "GJI_PROTMVD_ARTLAW_ARL", "GKH_REALITY_OBJECT", "ID"),
                new Column("DESCRIPTION", DbType.String, 500));

            Database.AddEntityTable(
                "GJI_PROT_MVD_ANNEX",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new RefColumn("PROT_MVD_ID", ColumnProperty.NotNull, "GJI_PROTMVD_ANNEX_DOC", "GJI_PROTOCOL_MVD", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "GJI_PROTMVD_ANNEX_FILE", "B4_FILE_INFO", "ID"),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));

            Database.AddTable(
                "GJI_INSPECTION_PROTMVD",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_INSPECT_PROTMVD_INS", "GJI_INSPECTION_PROTMVD", "ID", "GJI_INSPECTION", "ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_MVD", "MUNICIPALITY_ID");
            Database.RemoveConstraint("GJI_PROTOCOL_MVD", "FK_GJI_PROTMVD_DOC");
            Database.RemoveTable("GJI_PROTOCOL_MVD");

            Database.RemoveColumn("GJI_PROT_MVD_ROBJECT", "PROT_MVD_ID");
            Database.RemoveColumn("GJI_PROT_MVD_ROBJECT", "REALITY_OBJECT_ID");
            Database.RemoveTable("GJI_PROT_MVD_ROBJECT");

            Database.RemoveColumn("GJI_PROT_MVD_ARTLAW", "PROT_MVD_ID");
            Database.RemoveColumn("GJI_PROT_MVD_ARTLAW", "ARTICLELAW_ID");
            Database.RemoveTable("GJI_PROT_MVD_ARTLAW");

            Database.RemoveColumn("GJI_PROT_MVD_ANNEX", "PROT_MVD_ID");
            Database.RemoveColumn("GJI_PROT_MVD_ANNEX", "FILE_ID");
            Database.RemoveTable("GJI_PROT_MVD_ANNEX");

            Database.RemoveConstraint("GJI_INSPECTION_PROTMVD", "FK_GJI_INSPECT_PROTMVD_INS");
            Database.RemoveTable("GJI_INSPECTION_PROTMVD");
        }
    }
}