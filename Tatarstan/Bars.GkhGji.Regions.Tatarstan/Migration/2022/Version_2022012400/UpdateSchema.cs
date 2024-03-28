namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022012400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022012400")]
    [MigrationDependsOn(typeof(Version_2022012200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_TASK_ACTIONISOLATED_SURVEY_PURP",
                new RefColumn("TASK_ACTIONISOLATED_ID", ColumnProperty.NotNull, "FK_TASK_ACTIONISOLATED", "GJI_TASK_ACTIONISOLATED", "ID"),
                new RefColumn("SURVEY_PURP_ID", ColumnProperty.NotNull, "FK_DICT_SURVEY_PURP", "GJI_DICT_SURVEY_PURP", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_TASK_ACTIONISOLATED_SURVEY_PURP");
        }
    }
}