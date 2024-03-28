namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022011000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022011000")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021123100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName visitSheetAnnexTable =
            new SchemaQualifiedObjectName { Name = "GJI_VISIT_SHEET_ANNEX", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.visitSheetAnnexTable.Name,
                new RefColumn("VISIT_SHEET_ID",
                    this.visitSheetAnnexTable.Name + "_VISIT_SHEET",
                    "GJI_VISIT_SHEET",
                    "ID"),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String.WithSize(50)),
                new Column("DESCRIPTION", DbType.String.WithSize(255)),
                new RefColumn("FILE_ID",
                    this.visitSheetAnnexTable.Name + "_FILE",
                    "B4_FILE_INFO",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.visitSheetAnnexTable);
        }
    }
}