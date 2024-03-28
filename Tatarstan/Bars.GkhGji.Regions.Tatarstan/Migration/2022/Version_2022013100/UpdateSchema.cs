namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022013100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022013100")]
    [MigrationDependsOn(typeof(Version_2022012800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName taskActionIsolatedArticleLawTable =
            new SchemaQualifiedObjectName { Name = "GJI_TASK_ACTIONISOLATED_ARTICLE_LAW", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.taskActionIsolatedArticleLawTable.Name,
                new RefColumn("TASK_ID",
                    this.taskActionIsolatedArticleLawTable.Name + "_TASK",
                    "GJI_TASK_ACTIONISOLATED",
                    "ID"),
                new RefColumn("ARTICLELAW_ID",
                    this.taskActionIsolatedArticleLawTable.Name + "_ARTICLELAW",
                    "GJI_DICT_ARTICLELAW",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.taskActionIsolatedArticleLawTable);
        }
    }
}
