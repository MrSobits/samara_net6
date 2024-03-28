namespace Bars.GkhGji.Migrations.Version_2013040700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013040700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013040400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----таблица связи основания проверки и документа гжи
            Database.AddEntityTable("GJI_INSPECTION_DOC_REF",
                new Column("INSPECTION_ID", DbType.Int32, 22, ColumnProperty.NotNull),
                new Column("DOCUMENT_ID", DbType.Int32, 22, ColumnProperty.NotNull),
                new Column("TYPE_REFERENCE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddIndex("IND_GJI_INSP_DOC_REF_INSP", false, "GJI_INSPECTION_DOC_REF", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_INSP_DOC_REF_DOC", false, "GJI_INSPECTION_DOC_REF", "DOCUMENT_ID");

            Database.AddForeignKey("FK_GJI_INSP_DOC_REF_INSP", "GJI_INSPECTION_DOC_REF", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_INSP_DOC_REF_DOC", "GJI_INSPECTION_DOC_REF", "DOCUMENT_ID", "GJI_DOCUMENT", "ID");
            //-----

            //удаление колонки предыдущего документа у основания проверки по поручению руководства
            Database.RemoveConstraint("GJI_INSPECTION_DISPHEAD", "FK_GJI_INSPECT_DH_DOC");
            Database.RemoveColumn("GJI_INSPECTION_DISPHEAD", "PREV_DOCUMENT_ID");
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_GJI_INSP_DOC_REF_INSP", "GJI_INSPECTION_DOC_REF");
            Database.RemoveIndex("IND_GJI_INSP_DOC_REF_INSP", "GJI_INSPECTION_DOC_REF");

            Database.RemoveConstraint("GJI_INSPECTION_DOC_REF", "FK_GJI_INSP_DOC_REF_INSP");
            Database.RemoveConstraint("GJI_INSPECTION_DOC_REF", "FK_GJI_INSP_DOC_REF_DOC");

            Database.RemoveTable("GJI_INSPECTION_DOC_REF");

            //возвращаем колонку на место
            Database.AddColumn("GJI_INSPECTION_DISPHEAD", new Column("PREV_DOCUMENT_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GJI_INSPECT_DH_DOC", false, "GJI_INSPECTION_DISPHEAD", "PREV_DOCUMENT_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_DH_DOC", "GJI_INSPECTION_DISPHEAD", "PREV_DOCUMENT_ID", "GJI_DOCUMENT", "ID");
        }
    }
}