namespace Bars.Gkh.Overhaul.Tat.Migration._2022.Version_2022020100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022020100")]
    [MigrationDependsOn(typeof(_2020.Version_2020042700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string DpkrDocument = "OVRHL_TAT_DPKR_DOCUMENTS";
        private const string DpkrDocumentRealityObject = "OVRHL_TAT_DPKR_DOCUMENT_REALITY_OBJECT";
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.DpkrDocument,
                new RefColumn("DOC_KIND_ID", ColumnProperty.NotNull, "DPKR_DOCUMENTS_DOC_KIND_ID",
                    "OVRHL_DICT_BASIS_OVERHAUL_DOC_KIND", "ID"),
                new Column("DOC_NAME", DbType.String, ColumnProperty.NotNull),
                new RefColumn("FILE_ID", ColumnProperty.Null, "DPKR_DOCUMENTS_FILE_ID",
                    "B4_FILE_INFO", "ID"),
                new Column("DOC_NUMBER", DbType.String.WithSize(10)),
                new Column("DOC_DATE", DbType.DateTime),
                new Column("DOC_DEPARTMENT", DbType.String),
                new Column("PUBLICATION_DATE", DbType.DateTime),
                new Column("OBLIGATION_BEFORE_2014", DbType.DateTime),
                new Column("OBLIGATION_AFTER_2014", DbType.DateTime));
            
            this.Database.AddEntityTable(UpdateSchema.DpkrDocumentRealityObject,
                new RefColumn("DPKR_DOCUMENTS_ID", ColumnProperty.NotNull, "DPKR_DOCUMENTS_ID",
                    UpdateSchema.DpkrDocument, "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "REALITY_OBJECT_ID",
                    "GKH_REALITY_OBJECT", "ID"),
                new Column("IS_INCLUDED", DbType.Boolean, ColumnProperty.NotNull, false), 
                new Column("IS_EXCLUDED", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.DpkrDocumentRealityObject);
            this.Database.RemoveTable(UpdateSchema.DpkrDocument);
        }
    }
}