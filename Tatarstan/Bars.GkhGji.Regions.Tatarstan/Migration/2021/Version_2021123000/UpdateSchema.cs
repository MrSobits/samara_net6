namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021123000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021123000")]
    [MigrationDependsOn(typeof(Version_2021122900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table =
            new SchemaQualifiedObjectName { Name = "PREVENTIVE_ACTION_TASK_REGULATION" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable
            (
                this.table.Name,
                new RefColumn("TASK_ID", ColumnProperty.NotNull, this.table.Name + "_TASK", "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK", "ID"),
                new RefColumn("NORMATIVE_DOC_ID", ColumnProperty.NotNull, this.table.Name + "_NORMATIVE_DOC", "GKH_DICT_NORMATIVE_DOC", "ID")
            );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.table);
        }
    }
}