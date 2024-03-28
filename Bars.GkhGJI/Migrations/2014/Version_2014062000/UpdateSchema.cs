namespace Bars.GkhGji.Migration.Version_2014062000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014061500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Добавляем документ протокол МЖК
            Database.AddTable(
                "GJI_PROTOCOLMHC",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("MUNICIPALITY_ID", DbType.Int64, 22),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DATE_SUPPLY", DbType.DateTime),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_PROTOCOLMHC_MCP", false, "GJI_PROTOCOLMHC", "MUNICIPALITY_ID");
            Database.AddIndex("IND_GJI_PROTOCOLMHC_EXE", false, "GJI_PROTOCOLMHC", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_PROTOCOLMHC_CTR", false, "GJI_PROTOCOLMHC", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_CTR", "GJI_PROTOCOLMHC", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_EXE", "GJI_PROTOCOLMHC", "EXECUTANT_ID", "GJI_DICT_EXECUTANT", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_MCP", "GJI_PROTOCOLMHC", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_DOC", "GJI_PROTOCOLMHC", "ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Приложения в протоколе мжк
            Database.AddEntityTable(
                "GJI_PROTOCOLMHC_ANNEX",
                new Column("PROTOCOLMHC_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTOCOLMHC_ANNEX_F", false, "GJI_PROTOCOLMHC_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_PROTOCOLMHC_ANNEX_D", false, "GJI_PROTOCOLMHC_ANNEX", "PROTOCOLMHC_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_ANNEX_D", "GJI_PROTOCOLMHC_ANNEX", "PROTOCOLMHC_ID", "GJI_PROTOCOLMHC", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_ANNEX_F", "GJI_PROTOCOLMHC_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Статья закона в протоколе мжк
            Database.AddEntityTable(
                "GJI_PROTOCOLMHC_ARTLAW",
                new Column("PROTOCOLMHC_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ARTICLELAW_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTOCOLMHC_ARTLAW_D", false, "GJI_PROTOCOLMHC_ARTLAW", "PROTOCOLMHC_ID");
            Database.AddIndex("IND_GJI_PROTOCOLMHC_ARTLAW_A", false, "GJI_PROTOCOLMHC_ARTLAW", "ARTICLELAW_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_ARTLAW_D", "GJI_PROTOCOLMHC_ARTLAW", "PROTOCOLMHC_ID", "GJI_PROTOCOLMHC", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_ARTLAW_A", "GJI_PROTOCOLMHC_ARTLAW", "ARTICLELAW_ID", "GJI_DICT_ARTICLELAW", "ID");
            //-----

            //-----Дома в протоколе мжк
            Database.AddEntityTable(
                "GJI_PROTOCOLMHC_ROBJECT",
                new Column("PROTOCOLMHC_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTOCOLMHC_RO_P", false, "GJI_PROTOCOLMHC_ROBJECT", "PROTOCOLMHC_ID");
            Database.AddIndex("IND_GJI_PROTOCOLMHC_RO_O", false, "GJI_PROTOCOLMHC_ROBJECT", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_RO_P", "GJI_PROTOCOLMHC_ROBJECT", "PROTOCOLMHC_ID", "GJI_PROTOCOLMHC", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLMHC_RO_O", "GJI_PROTOCOLMHC_ROBJECT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Определения протокола
            Database.AddEntityTable(
                "GJI_PROTOCOLMHC_DEFINITION",
                new Column("PROTOCOLMHC_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ISSUED_DEFINITION_ID", DbType.Int64, 22),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 2000),
                new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTMHC_DEF_D", false, "GJI_PROTOCOLMHC_DEFINITION", "PROTOCOLMHC_ID");
            Database.AddIndex("IND_GJI_PROTMHC_DEF_I", false, "GJI_PROTOCOLMHC_DEFINITION", "ISSUED_DEFINITION_ID");
            Database.AddForeignKey("FK_GJI_PROTMHC_DEF_I", "GJI_PROTOCOLMHC_DEFINITION", "ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_PROTMHC_DEF_D", "GJI_PROTOCOLMHC_DEFINITION", "PROTOCOLMHC_ID", "GJI_PROTOCOLMHC", "ID");
            //-----

            //-----Основание проверки - постановления прокуратуры
            Database.AddTable(
                "GJI_INSPECTION_PROTMHC",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_INSPECT_PROTOCOLMHC_I", "GJI_INSPECTION_PROTMHC", "ID", "GJI_INSPECTION", "ID");
            //-----
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_PROTOCOLMHC", "FK_GJI_PROTOCOLMHC_CTR");
            Database.RemoveConstraint("GJI_PROTOCOLMHC", "FK_GJI_PROTOCOLMHC_EXE");
            Database.RemoveConstraint("GJI_PROTOCOLMHC", "FK_GJI_PROTOCOLMHC_MCP");
            Database.RemoveConstraint("GJI_PROTOCOLMHC", "FK_GJI_PROTOCOLMHC_DOC");

            Database.RemoveConstraint("GJI_PROTOCOLMHC_ANNEX", "FK_GJI_PROTOCOLMHC_ANNEX_D");
            Database.RemoveConstraint("GJI_PROTOCOLMHC_ANNEX", "FK_GJI_PROTOCOLMHC_ANNEX_F");

            Database.RemoveConstraint("GJI_PROTOCOLMHC_ARTLAW", "FK_GJI_PROTOCOLMHC_ARTLAW_D");
            Database.RemoveConstraint("GJI_PROTOCOLMHC_ARTLAW", "FK_GJI_PROTOCOLMHC_ARTLAW_A");

            Database.RemoveConstraint("GJI_PROTOCOLMHC_ROBJECT", "FK_GJI_PROTOCOLMHC_RO_P");
            Database.RemoveConstraint("GJI_PROTOCOLMHC_ROBJECT", "FK_GJI_PROTOCOLMHC_RO_O");

            Database.RemoveConstraint("GJI_PROTOCOLMHC_DEFINITION", "FK_GJI_PROTMHC_DEF_I");
            Database.RemoveConstraint("GJI_PROTOCOLMHC_DEFINITION", "FK_GJI_PROTMHC_DEF_D");

            Database.RemoveConstraint("GJI_INSPECTION_PROTMHC", "FK_GJI_INSPECT_PROTOCOLMHC_I");

            Database.RemoveTable("GJI_PROTOCOLMHC_DEFINITION");
            Database.RemoveTable("GJI_PROTOCOLMHC_ROBJECT");
            Database.RemoveTable("GJI_PROTOCOLMHC_ARTLAW");
            Database.RemoveTable("GJI_PROTOCOLMHC_ANNEX");
            Database.RemoveTable("GJI_PROTOCOLMHC");
            Database.RemoveTable("GJI_INSPECTION_PROTMHC");
        }
    }
}