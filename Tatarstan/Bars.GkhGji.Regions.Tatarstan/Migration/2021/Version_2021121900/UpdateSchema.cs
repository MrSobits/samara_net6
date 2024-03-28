namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2021121900")]
    [MigrationDependsOn(typeof(Version_2021121800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiActCheckSurveyActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_SURVEY_ACTION", Schema = "PUBLIC" };
        
        private readonly SchemaQualifiedObjectName gjiActCheckSurveyActionQuestionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_SURVEY_ACTION_QUESTION", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.gjiActCheckSurveyActionTable.Name,
                "GJI_ACTCHECK_ACTION",
                this.gjiActCheckSurveyActionTable.Name + "_ACTION",
                new Column("CONTINUE_DATE", DbType.Date),
                new Column("CONTINUE_START_TIME", DbType.DateTime),
                new Column("CONTINUE_END_TIME", DbType.DateTime),
                new Column("PROTOCOL_READED", DbType.Int16, ColumnProperty.NotNull, (int)YesNoNotSet.NotSet),
                new Column("HAS_REMARK", DbType.Int16, ColumnProperty.NotNull, (int)HasValuesNotSet.NotSet));
            
            this.Database.AddEntityTable(this.gjiActCheckSurveyActionQuestionTable.Name,
                new RefColumn("SURVEY_ACTION_ID",
                    ColumnProperty.NotNull,
                    this.gjiActCheckSurveyActionQuestionTable.Name + "_SURVEY_ACTION",
                    this.gjiActCheckSurveyActionTable.Name,
                    "ID"),
                new Column("QUESTION", DbType.String.WithSize(500)),
                new Column("ANSWER", DbType.String.WithSize(500)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.gjiActCheckSurveyActionQuestionTable);
            this.Database.RemoveTable(this.gjiActCheckSurveyActionTable);
        }
    }
}