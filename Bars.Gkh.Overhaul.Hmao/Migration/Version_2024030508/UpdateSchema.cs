namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030508
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030508")]
    [MigrationDependsOn(typeof(Version_2024030507.UpdateSchema))]
    // Является Version_2023080100 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "OVRHL_DPKR_DOCUMENT_REAL_OBJ" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(table.Name,
                new RefColumn("DPKR_DOCUMENT_ID", ColumnProperty.NotNull, $"{table.Name}_DPKR_DOCUMENT", "OVRHL_DPKR_DOCUMENTS", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, $"{table.Name}_REALITY_OBJECT", "GKH_REALITY_OBJECT", "ID"),
                new Column("IS_EXCLUDED", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(table);
        }
    }
}