namespace Bars.GkhGji.Migrations._2024.Version_2024030126
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using System.Data;

    [Migration("2024030126")]
    [MigrationDependsOn(typeof(Version_2024030125.UpdateSchema))]
    /// Является Version_2022041900 из ядра
    public class UpdateSchema : Migration
    {
        private Column column = new Column("ERKNM_GUID", DbType.String.WithSize(36));

        private SchemaQualifiedObjectName[] tables =
        {
            new SchemaQualifiedObjectName { Name = "GJI_DISPOSAL_EXPERT" },
            new SchemaQualifiedObjectName { Name = "GJI_DISPOSAL_PROVDOC" },
            new SchemaQualifiedObjectName { Name = "GJI_INSPECTION_ROBJECT" },
            new SchemaQualifiedObjectName { Name = "GJI_DOCUMENT_INSPECTOR" },
            new SchemaQualifiedObjectName { Name = "GJI_DICT_PLANJURPERSON" }
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.tables.ForEach(table => this.Database.AddColumn(table, column));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.tables.ForEach(table => this.Database.RemoveColumn(table, column.Name));
        }
    }
}