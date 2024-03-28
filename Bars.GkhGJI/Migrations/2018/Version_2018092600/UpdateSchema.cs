namespace Bars.GkhGji.Migrations._2018.Version_2018092600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018092600")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018061300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            //-----Добавляем документ протокол РСО
            Database.AddTable(
                "GJI_PROTOCOLRSO",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("RSO_ID", DbType.Int64, 22),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("TYPE_SUPPLIER", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DATE_SUPPLY", DbType.DateTime));

            Database.AddIndex("IND_GJI_PROTOCOLRSO_RSO", false, "GJI_PROTOCOLRSO", "RSO_ID");
            Database.AddIndex("IND_GJI_PROTOCOLRSO_EXE", false, "GJI_PROTOCOLRSO", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_PROTOCOLRSO_CTR", false, "GJI_PROTOCOLRSO", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_CTR", "GJI_PROTOCOLRSO", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_EXE", "GJI_PROTOCOLRSO", "EXECUTANT_ID", "GJI_DICT_EXECUTANT", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_RSO", "GJI_PROTOCOLRSO", "RSO_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_DOC", "GJI_PROTOCOLRSO", "ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Приложения в протоколе РСО
            Database.AddEntityTable(
                "GJI_PROTOCOLRSO_ANNEX",
                new Column("PROTOCOLRSO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTOCOLRSO_ANNEX_F", false, "GJI_PROTOCOLRSO_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_PROTOCOLRSO_ANNEX_D", false, "GJI_PROTOCOLRSO_ANNEX", "PROTOCOLRSO_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_ANNEX_D", "GJI_PROTOCOLRSO_ANNEX", "PROTOCOLRSO_ID", "GJI_PROTOCOLRSO", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_ANNEX_F", "GJI_PROTOCOLRSO_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Статья закона в протоколе РСО
            Database.AddEntityTable(
                "GJI_PROTOCOLRSO_ARTLAW",
                new Column("PROTOCOLRSO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ARTICLELAW_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTOCOLRSO_ARTLAW_D", false, "GJI_PROTOCOLRSO_ARTLAW", "PROTOCOLRSO_ID");
            Database.AddIndex("IND_GJI_PROTOCOLRSO_ARTLAW_A", false, "GJI_PROTOCOLRSO_ARTLAW", "ARTICLELAW_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_ARTLAW_D", "GJI_PROTOCOLRSO_ARTLAW", "PROTOCOLRSO_ID", "GJI_PROTOCOLRSO", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_ARTLAW_A", "GJI_PROTOCOLRSO_ARTLAW", "ARTICLELAW_ID", "GJI_DICT_ARTICLELAW", "ID");
            //-----

            //-----Дома в протоколе РСО
            Database.AddEntityTable(
                "GJI_PROTOCOLRSO_ROBJECT",
                new Column("PROTOCOLRSO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTOCOLRSO_RO_P", false, "GJI_PROTOCOLRSO_ROBJECT", "PROTOCOLRSO_ID");
            Database.AddIndex("IND_GJI_PROTOCOLRSO_RO_O", false, "GJI_PROTOCOLRSO_ROBJECT", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_RO_P", "GJI_PROTOCOLRSO_ROBJECT", "PROTOCOLRSO_ID", "GJI_PROTOCOLRSO", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_RO_O", "GJI_PROTOCOLRSO_ROBJECT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Определения протокола РСО
            Database.AddEntityTable(
                "GJI_PROTOCOLRSO_DEFINITION",
                new Column("PROTOCOLRSO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ISSUED_DEFINITION_ID", DbType.Int64, 22),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 2000),
                new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROTOCOLRSO_DEF_D", false, "GJI_PROTOCOLRSO_DEFINITION", "PROTOCOLRSO_ID");
            Database.AddIndex("IND_GJI_PROTOCOLRSO_DEF_I", false, "GJI_PROTOCOLRSO_DEFINITION", "ISSUED_DEFINITION_ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_DEF_I", "GJI_PROTOCOLRSO_DEFINITION", "ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_PROTOCOLRSO_DEF_D", "GJI_PROTOCOLRSO_DEFINITION", "PROTOCOLRSO_ID", "GJI_PROTOCOLRSO", "ID");
            //-----

            //-----Основание проверки - постановления РСО
            Database.AddTable(
                "GJI_INSPECTION_PROTRSO",
                   new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));
            Database.AddForeignKey("FK_GJI_INSPECT_PROTOCOLMHC_I", "GJI_INSPECTION_PROTRSO", "ID", "GJI_INSPECTION", "ID");
            //-----



        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
	    public override void Down()
        {
            Database.RemoveTable("GJI_PROTOCOLRSO_DEFINITION");
            Database.RemoveTable("GJI_PROTOCOLRSO_ROBJECT");
            Database.RemoveTable("GJI_PROTOCOLRSO_ARTLAW");
            Database.RemoveTable("GJI_PROTOCOLRSO_ANNEX");
            Database.RemoveTable("GJI_PROTOCOLRSO");
            Database.RemoveTable("GJI_INSPECTION_PROTRSO");
        }
    }
}