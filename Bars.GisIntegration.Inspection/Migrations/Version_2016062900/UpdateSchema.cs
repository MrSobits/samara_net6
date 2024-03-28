namespace Bars.GisIntegration.Inspection.Migrations.Version_2016062900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GisIntegration.Base.Extensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016062900")]
    [MigrationDependsOn(typeof(Bars.GisIntegration.Base.Migrations.Version_2016062800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable("GI_INSPECTION_PLAN",
            //    new Column("YEAR", DbType.Int16),
            //    new Column("APPROVAL_DATE", DbType.DateTime));

            //this.Database.AddRisEntityTable("GI_INSPECTION_EXAMINATION",
            //    new RefColumn("PLAN_ID", ColumnProperty.Null, "INSP_EXAM_PLAN", "GI_INSPECTION_PLAN", "ID"),
            //    new Column("INSPECTIONNUMBER", DbType.Int32),
            //    new Column("EXAMFORM_CODE", DbType.String, 10),
            //    new Column("EXAMFORM_GUID", DbType.String, 50),
            //    new Column("ORDER_NUMBER", DbType.String, 300),
            //    new Column("ORDER_DATE", DbType.DateTime, 50),
            //    new Column("IS_SCHEDULED", DbType.Boolean),
            //    new Column("SUBJECT_TYPE", DbType.Int32),
            //    new Column("ORG_ROOT_ENTITY_GUID", DbType.String, 50),
            //    new Column("ACTIVITY_PLACE", DbType.String, 500),
            //    new Column("FIRSTNAME", DbType.String, 100),
            //    new Column("LASTNAME", DbType.String, 100),
            //    new Column("MIDDLENAME", DbType.String, 100),
            //    new Column("OVERSIGHT_ACT_CODE", DbType.String, 10),
            //    new Column("OVERSIGHT_ACT_GUID", DbType.String, 50),
            //    new Column("BASE_CODE", DbType.String, 10),
            //    new Column("BASE_GUID", DbType.String, 20),
            //    new Column("OBJECTIVE", DbType.String, 2000),
            //    new Column("DATE_FROM", DbType.DateTime),
            //    new Column("DATE_TO", DbType.DateTime),
            //    new Column("DURATION", DbType.Double),
            //    new Column("TASKS", DbType.String, 2000),
            //    new Column("EVENT_DESC", DbType.String, 2000),
            //    new Column("HAS_RESULT", DbType.Boolean),
            //    new Column("RESULT_DOC_TYPE_CODE", DbType.String, 10),
            //    new Column("RESULT_DOC_TYPE_GUID", DbType.String, 50),
            //    new Column("RESULT_DOC_NUMBER", DbType.String, 300),
            //    new Column("RESULT_DOC_DATETIME", DbType.DateTime),
            //    new Column("HAS_OFFENCE", DbType.Boolean)
            //    );

            //this.Database.AddEntityTable("GI_INSPECTION_EXAM_ATTACH",
            //    new RefColumn("EXAMINATION_ID", ColumnProperty.NotNull, "GI_INSP_EXAM_ATTACH_EXAM", "GI_INSPECTION_EXAMINATION", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "GI_INSP_EXAM_ATTACH_ATTACH", "GI_ATTACHMENT", "ID"));

            //this.Database.AddEntityTable("GI_INSPECTION_EXAM_PLACE",
            //    new RefColumn("EXAMINATION_ID", ColumnProperty.NotNull, "GI_INSP_EXAM_PLACE_EXAM", "GI_INSPECTION_EXAMINATION", "ID"),
            //    new Column("ORDER_NUM", DbType.Int32),
            //    new Column("FIAS_HOUSE_GUID", DbType.String, 50));

            //this.Database.AddRisEntityTable("GI_INSPECTION_OFFENCE",
            //    new RefColumn("EXAMINATION_ID", "GI_INSP_OFFENCE_EXAM", "GI_INSPECTION_EXAMINATION", "ID"),
            //    new Column("NUMBER", DbType.String, 300),
            //    new Column("DATE", DbType.DateTime),
            //    new Column("IS_CANCELLED", DbType.Boolean),
            //    new Column("CANCEL_REASON", DbType.String, 2000),
            //    new Column("CANCEL_DATE", DbType.DateTime),
            //    new Column("CANCEL_DECISION_NUM", DbType.String, 300));

            //this.Database.AddEntityTable("GI_INSPECTION_OFFENCE_ATTACH",
            //    new RefColumn("OFFENCE_ID", ColumnProperty.NotNull, "GI_INSP_OFFENCE_ATTACH_OFFENCE", "GI_INSPECTION_OFFENCE", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "GI_INSP_OFFENCE_ATTACH_ATTACH", "GI_ATTACHMENT", "ID"));

            //this.Database.AddRisEntityTable("GI_INSPECTION_PRECEPT",
            //    new RefColumn("EXAMINATION_ID", "GI_INSP_PREC_EXAM", "GI_INSPECTION_EXAMINATION", "ID"),
            //    new Column("NUMBER", DbType.String, 300),
            //    new Column("DATE", DbType.DateTime),
            //    new Column("CANCEL_REASON", DbType.String, 200),
            //    new Column("CANCEL_DATE", DbType.DateTime),
            //    new Column("FIAS_HOUSE_GUID", DbType.String, 50),
            //    new Column("IS_CANCELLED", DbType.Boolean));

            //this.Database.AddEntityTable("GI_INSPECTION_PRECEPT_ATTACH",
            //    new RefColumn("PRECEPT_ID", ColumnProperty.NotNull, "GI_INSP_PRECEPT_ATTACH_PRECEPT", "GI_INSPECTION_PRECEPT", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "GI_INSP_PRECEPT_ATTACH_ATTACH", "GI_ATTACHMENT", "ID"),
            //    new Column("IS_CANCEL_ATTACH", DbType.Boolean));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("GI_INSPECTION_PRECEPT_ATTACH");
            //this.Database.RemoveTable("GI_INSPECTION_PRECEPT");
            //this.Database.RemoveTable("GI_INSPECTION_OFFENCE_ATTACH");
            //this.Database.RemoveTable("GI_INSPECTION_OFFENCE");
            //this.Database.RemoveTable("GI_INSPECTION_EXAM_PLACE");
            //this.Database.RemoveTable("GI_INSPECTION_EXAM_ATTACH");
            //this.Database.RemoveTable("GI_INSPECTION_EXAMINATION");
            //this.Database.RemoveTable("GI_INSPECTION_PLAN");
        }
    }
}
