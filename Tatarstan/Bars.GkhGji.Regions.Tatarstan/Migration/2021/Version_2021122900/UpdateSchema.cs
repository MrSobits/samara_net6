namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021122900")]
    [MigrationDependsOn(typeof(Version_2021122800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName preventiveActionTaskConsultingQuestionTable =
            new SchemaQualifiedObjectName { Name = "PREVENTIVE_ACTION_TASK_CONSULTING_QUESTION", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.preventiveActionTaskConsultingQuestionTable.Name,
                new RefColumn("TASK_ID",
                    this.preventiveActionTaskConsultingQuestionTable.Name + "_TASK",
                    "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK",
                    "ID"),
                new Column("QUESTION", DbType.String.WithSize(1000)),
                new Column("ANSWER", DbType.String.WithSize(1000)),
                new Column("CONTROLLED_PERSON", DbType.String.WithSize(255)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.preventiveActionTaskConsultingQuestionTable);
        }
    }
}