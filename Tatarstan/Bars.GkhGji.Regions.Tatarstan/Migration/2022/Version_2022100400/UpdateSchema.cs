namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022100400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022100400")]
    [MigrationDependsOn(typeof(Version_2022100300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_TASK_ACTIONISOLATED_PLANNED_ACTION" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.table.Name,
                new RefColumn(
                    "TASK_ACTIONISOLATED_ID",
                    ColumnProperty.NotNull,
                    $"{this.table.Name}_TASK",
                    "GJI_TASK_ACTIONISOLATED",
                    "ID"),
                new RefColumn(
                    "PLANNED_ACTION_ID",
                    ColumnProperty.NotNull,
                    $"{this.table.Name}_PLANNED_ACTION",
                    "GJI_DICT_KNM_ACTION",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.table);
        }
    }
}