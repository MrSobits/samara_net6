namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022020400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022020400")]
    [MigrationDependsOn(typeof(Version_2022013100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName motivatedPresentationActionIsolatedAnnexTable =
            new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED_ANNEX", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.motivatedPresentationActionIsolatedAnnexTable.Name,
                new RefColumn("MOTIVATED_PRESENTATION_ID",
                    this.motivatedPresentationActionIsolatedAnnexTable.Name + "_MOTIVATED_PRESENTATION",
                    "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED",
                    "ID"),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String.WithSize(50)),
                new Column("DESCRIPTION", DbType.String.WithSize(255)),
                new RefColumn("FILE_ID",
                    this.motivatedPresentationActionIsolatedAnnexTable.Name + "_FILE",
                    "B4_FILE_INFO",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.motivatedPresentationActionIsolatedAnnexTable);
        }
    }
}