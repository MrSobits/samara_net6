namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022012100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022012100")]
    [MigrationDependsOn(typeof(Version_2022011900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName taskActionIsolatedAnnexTable =
            new SchemaQualifiedObjectName { Name = "GJI_TASK_ACTIONISOLATED_ANNEX", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.taskActionIsolatedAnnexTable.Name,
                new RefColumn("TASK_ID",
                    this.taskActionIsolatedAnnexTable.Name + "_TASK",
                    "GJI_TASK_ACTIONISOLATED",
                    "ID"),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String.WithSize(50)),
                new Column("DESCRIPTION", DbType.String.WithSize(255)),
                new RefColumn("FILE_ID",
                    this.taskActionIsolatedAnnexTable.Name + "_FILE",
                    "B4_FILE_INFO",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.taskActionIsolatedAnnexTable);
        }
    }
}