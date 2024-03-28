namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Основание проверки - административное дело
            Database.AddTable(
                "GJI_INSPECTION_ADMINCASE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_INSPECT_ADMINCASE_INS", "GJI_INSPECTION_ADMINCASE", "ID", "GJI_INSPECTION", "ID");
            //-----

            // Административное дело
            Database.AddTable("GJI_ADMINCASE",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("RO_ID", "GJI_ADMINCASE_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("CTR_ID", "GJI_ADMINCASE_CTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("INSPECTOR_ID", "GJI_ADMINCASE_INSP", "GKH_DICT_INSPECTOR", "ID"),
                new Column("TYPE_ADMIN_BASE", DbType.Int16, 4),
                new Column("DESC_QUESTION", DbType.String, 2000),
                new Column("DESC_SET", DbType.String, 2000),
                new Column("DESC_DEFINED", DbType.String, 2000));
            Database.AddForeignKey("FK_GJI_ADMINCASE_DOC", "GJI_ADMINCASE", "ID", "GJI_DOCUMENT", "ID");

            //-----Приложения адм дела
            Database.AddEntityTable(
                "GJI_ADMINCASE_ANNEX",
                new RefColumn("ADMINCASE_ID", ColumnProperty.NotNull, "GJI_ADMINCASE_ANNEX_AC", "GJI_ADMINCASE", "ID"),
                new RefColumn("FILE_ID", "GJI_ADMINCASE_ANNEX_F", "B4_FILE_INFO", "ID"),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            //-----

            //-----Статья закона в адм деле
            Database.AddEntityTable(
                "GJI_ADMINCASE_ARTLAW",
                new RefColumn("ADMINCASE_ID", ColumnProperty.NotNull, "GJI_ADMINCASE_ARTLAW_AC", "GJI_ADMINCASE", "ID"),
                new RefColumn("ARTICLELAW_ID", ColumnProperty.NotNull, "GJI_ADMINCASE_ARTLAW_L", "GJI_DICT_ARTICLELAW", "ID"));
            //-----
            
            //-----Предоставляемый документ адм дела
            Database.AddEntityTable(
                "GJI_ADMINCASE_PROVDOC",
                new RefColumn("ADMINCASE_ID", ColumnProperty.NotNull, "GJI_ADMINCASE_PROVDOC_AC", "GJI_ADMINCASE", "ID"),
                new RefColumn("PROVIDED_DOC_ID", ColumnProperty.NotNull, "GJI_ADMINCASE_PROVDOC_PR", "GJI_DISPOSAL_PROVDOC", "ID"));
            //-----

            //-----Приложения адм дела
            Database.AddEntityTable(
                "GJI_ADMINCASE_DOC",
                new RefColumn("ADMINCASE_ID", ColumnProperty.NotNull, "GJI_ADMINCASE_DOC_AC", "GJI_ADMINCASE", "ID"),
                new RefColumn("ENTITIED_ID", "GJI_ADMINCASE_DOC_ENI", "GKH_DICT_INSPECTOR", "ID"),
                new Column("TYPE_ADMIN_DOC", DbType.Int16, 4),
                new Column("DOCUMENT_NUMBER", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NEED_TERM", DbType.DateTime),
                new Column("RENEWAL_TERM", DbType.DateTime),
                new Column("DESCRIPTION_SET", DbType.String, 2000));
            //-----

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ADMINCASE_ANNEX");
            Database.RemoveTable("GJI_ADMINCASE_ARTLAW");
            Database.RemoveTable("GJI_ADMINCASE_PROVDOC");
            Database.RemoveTable("GJI_ADMINCASE_DOC");

            Database.RemoveConstraint("GJI_ADMINCASE", "FK_GJI_ADMINCASE_DOC");
            Database.RemoveTable("GJI_ADMINCASE");

            Database.RemoveConstraint("GJI_INSPECTION_ADMINCASE", "FK_GJI_INSPECT_ADMINCASE_INS");
            Database.RemoveTable("GJI_INSPECTION_ADMINCASE");
        }
    }
}
