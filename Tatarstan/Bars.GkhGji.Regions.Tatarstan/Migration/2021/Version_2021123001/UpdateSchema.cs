namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021123001
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021123001")]
    [MigrationDependsOn(typeof(Version_2021123000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_TASKS_PREVENTIVE_ACTION_TASK",
                new RefColumn("PREVENTIVE_ACTION_TASK_ID", ColumnProperty.NotNull, "FK_TASK", "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK", "ID"),
                new RefColumn("TASKS_PREVENTIVE_MEASURES_ID", ColumnProperty.NotNull, "FK_DICT_TASK", "GJI_DICT_TASKS_PREVENTIVE_MEASURES", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_TASKS_PREVENTIVE_ACTION_TASK");
        }
    }
}