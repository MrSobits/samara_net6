namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022012200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022012200")]
    [MigrationDependsOn(typeof(Version_2022012100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName taskActionIsolatedItemTable =
            new SchemaQualifiedObjectName { Name = "GJI_TASK_ACTIONISOLATED_ITEM", Schema = "PUBLIC" };
        private const string TaskActionIsolatedTable = "GJI_TASK_ACTIONISOLATED";
        private const string DictItemTable = "GJI_DICT_SURVEY_SUBJ";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.taskActionIsolatedItemTable.Name,
                new RefColumn("TASK_ID",
                ColumnProperty.NotNull,
                    this.taskActionIsolatedItemTable.Name + "_" + TaskActionIsolatedTable,
                    TaskActionIsolatedTable,
                    "ID"),
                new RefColumn("ITEM_ID",
                ColumnProperty.NotNull,
                    this.taskActionIsolatedItemTable.Name + "_" + DictItemTable,
                    DictItemTable,
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.taskActionIsolatedItemTable);
        }
    }
}
