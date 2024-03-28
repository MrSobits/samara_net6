namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030507
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030507")]
    [MigrationDependsOn(typeof(Version_2024030506.UpdateSchema))]
    // Является Version_2023071900 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "OVRHL_DPKR_DOCUMENT_PRG_VERSION" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(table.Name,
                new RefColumn("DPKR_DOCUMENT_ID", ColumnProperty.NotNull, $"{table.Name}_DPKR_DOCUMENT", "OVRHL_DPKR_DOCUMENTS", "ID"),
                new RefColumn("PRG_VERSION_ID", ColumnProperty.NotNull, $"{table.Name}_PRG_VERSION", "OVRHL_PRG_VERSION", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(table);
        }
    }
}