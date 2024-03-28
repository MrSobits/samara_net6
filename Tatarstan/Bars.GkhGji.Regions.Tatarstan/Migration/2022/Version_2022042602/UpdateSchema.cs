namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042602
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022042602")]
    [MigrationDependsOn(typeof(Version_2022042601.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TypeDocumentsERKNMTableName = "GJI_DICT_ERKNM_TYPE_DOCUMENT";
        private const string TypeDocumentsERKNMKindCheckTableName = "GJI_DICT_ERKNM_TYPE_DOCUMENT_KIND_CHECK";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.TypeDocumentsERKNMTableName,
                new Column("DOCUMENT_TYPE",DbType.String),
                new Column("CODE", DbType.String),
                new Column("IS_BASIS_KNM", DbType.Boolean, ColumnProperty.NotNull, false)
            );
            this.Database.AddEntityTable(UpdateSchema.TypeDocumentsERKNMKindCheckTableName,
                new RefColumn("TYPE_DOCUMENT_ID",
                    "ERKNM_TYPE_DOCUMENT_KIND_CHECK_TYPE_DOCUMENT", UpdateSchema.TypeDocumentsERKNMTableName, "ID"),
                new RefColumn("KIND_CHECK_ID", "ERKNM_TYPE_DOCUMENT_KIND_CHECK_KIND",
                    "GJI_DICT_KIND_CHECK", "ID")
            );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.TypeDocumentsERKNMKindCheckTableName);
            this.Database.RemoveTable(UpdateSchema.TypeDocumentsERKNMTableName);
        }
    }
}