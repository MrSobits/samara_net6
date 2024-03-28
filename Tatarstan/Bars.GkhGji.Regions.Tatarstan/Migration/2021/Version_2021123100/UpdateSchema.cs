namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021123100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021123100")]
    [MigrationDependsOn(typeof(Version_2021123001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Table = "PREVENTIVE_ACTION_TASK_OBJECTIVE";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(Table,
                new RefColumn("PREVENTIVE_ACTION_TASK_ID", ColumnProperty.NotNull, "FK_ACTION_TASK_OBJECTIVE_TASK", "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK", "ID"),
                new RefColumn("OBJECTIVE_ID", ColumnProperty.NotNull, "FK_ACTION_TASK_OBJECTIVE_MEASURES", "GJI_DICT_OBJECTIVES_PREVENTIVE_MEASURES", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(Table);
        }
    }
}
