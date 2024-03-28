namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022112300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022112300")]
    [MigrationDependsOn(typeof(Version_2022111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_DOCUMENT_PDF_SIGN_INFO" };
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.table.Name, 
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, $"{this.table.Name}_DOCUMENT", "GJI_DOCUMENT", "ID"),
                new RefColumn("PDF_SIGN_INFO_ID", ColumnProperty.NotNull, $"{this.table.Name}_PDF_SIGN_INFO", "PDF_SIGN_INFO", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.table);
        }
    }
}