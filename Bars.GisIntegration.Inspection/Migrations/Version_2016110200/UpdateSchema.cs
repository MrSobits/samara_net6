namespace Bars.GisIntegration.Inspection.Migrations.Version_2016110200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Base.Extensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016110200")]
    [MigrationDependsOn(typeof(Version_2016101400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            //this.Database.AddRefColumn("GI_INSPECTION_EXAM_PLACE", new RefColumn("GI_CONTRAGENT_ID", "GI_EXAM_PLACE_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            //this.Database.AddColumn("GI_INSPECTION_EXAM_PLACE", new Column("GUID", DbType.String, 50));

            //this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            //this.Database.AddRefColumn("GI_INSPECTION_PRECEPT_ATTACH", new RefColumn("GI_CONTRAGENT_ID", "GI_PRECEPT_ATTACH_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            //this.Database.AddColumn("GI_INSPECTION_PRECEPT_ATTACH", new Column("GUID", DbType.String, 50));

            //this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            //this.Database.AddRefColumn("GI_INSPECTION_EXAM_ATTACH", new RefColumn("GI_CONTRAGENT_ID", "GI_EXAM_ATTACH_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            //this.Database.AddColumn("GI_INSPECTION_EXAM_ATTACH", new Column("GUID", DbType.String, 50));

            //this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull, "0"));
            //this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            //this.Database.AddRefColumn("GI_INSPECTION_OFFENCE_ATTACH", new RefColumn("GI_CONTRAGENT_ID", "GI_OFFENCE_ATTACH_CONTRAGENT", "GI_CONTRAGENT", "ID"));
            //this.Database.AddColumn("GI_INSPECTION_OFFENCE_ATTACH", new Column("GUID", DbType.String, 50));

            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("SHOULD_NOT_BE_REGISTERED", DbType.Boolean));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("FUNCTION_REGISTRY_NUMBER", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("AUTHORIZED_PERSONS", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("INVOLVED_EXPERTS", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("PRECEPT_GUID", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("OBJECT_CODE", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("OBJECT_GUID", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("IDENTIFIED_OFFENCES", DbType.String, 100000));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_FROM", DbType.DateTime));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_TO", DbType.DateTime));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("RESULT_PLACE", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("FAMILIARIZATION_DATE", DbType.DateTime));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("IS_SIGNED", DbType.Boolean));
            //this.Database.AddColumn("GI_INSPECTION_EXAMINATION", new Column("FAMILIARIZED_PERSON", DbType.String));

            //this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("DEADLINE", DbType.DateTime));
            //this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("IS_FULFILED_PRECEPT", DbType.Boolean));
            //this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("CANCEL_REASON_GUID", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("ORG_ROOT_ENTITY_GUID", DbType.String));
            //this.Database.AddColumn("GI_INSPECTION_PRECEPT", new Column("IS_CANCELLED_AND_IS_FULFILED", DbType.Boolean));

            //this.Database.AddColumn("GI_INSPECTION_OFFENCE", new Column("IS_FULFILED_OFFENCE", DbType.Boolean));

            //this.Database.AddRisEntityTable("GI_EXAMINATION_OTHER_DOCUMENT",
            //    new RefColumn("EXAMINATION_ID", ColumnProperty.NotNull, "GI_EXAM_OTHER_DOC_EXAM", "GI_INSPECTION_EXAMINATION", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "GI_EXAM_OTHER_DOC_ATTACH", "GI_ATTACHMENT", "ID"));

            //this.Database.AddRisEntityTable("GI_CANCEL_PRECEPT_ATTACH",
            //    new RefColumn("PRECEPT_ID", ColumnProperty.NotNull, "GI_CANCEL_PRECEPT_ATTACH_PRECEPT", "GI_INSPECTION_PRECEPT", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "GI_ANCEL_PRECEPT_ATTACH_ATT", "GI_ATTACHMENT", "ID"));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "SHOULD_NOT_BE_REGISTERED");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "FUNCTION_REGISTRY_NUMBER");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "AUTHORIZED_PERSONS");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "INVOLVED_EXPERTS");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "PRECEPT_GUID");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "OBJECT_CODE");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "OBJECT_GUID");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "IDENTIFIED_OFFENCES");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "RESULT_FROM");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "RESULT_TO");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "FAMILIARIZATION_DATE");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "IS_SIGNED");
            //this.Database.RemoveColumn("GI_INSPECTION_EXAMINATION", "FAMILIARIZED_PERSON");

            //this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "DEADLINE");
            //this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "IS_FULFILED_PRECEPT");
            //this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "CANCEL_REASON_GUID");
            //this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "ORG_ROOT_ENTITY_GUID");
            //this.Database.RemoveColumn("GI_INSPECTION_PRECEPT", "IS_CANCELLED_AND_IS_FULFILED");

            //this.Database.RemoveColumn("GI_INSPECTION_OFFENCE", "IS_FULFILED_OFFENCE");

            //this.Database.RemoveTable("GI_EXAMINATION_OTHER_DOCUMENT");
            //this.Database.RemoveTable("GI_CANCEL_PRECEPT_ATTACH");
        }
    }
}