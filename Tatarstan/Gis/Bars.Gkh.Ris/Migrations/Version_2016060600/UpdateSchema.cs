namespace Bars.Gkh.Ris.Migrations.Version_2016060600
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016060600")]
    [MigrationDependsOn(typeof(Version_2015101200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.UpExamination();

            //this.UpPrecept();

            //this.UpOffence();
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.DownExamination();

            //this.DownPrecept();

            //this.DownOffence();
        }

        //private void UpOffence()
        //{
        //    this.Database.AddColumn("RIS_INSPECTION_OFFENCE", new Column("IS_CANCELLED", DbType.Boolean));
        //    this.Database.AddColumn("RIS_INSPECTION_OFFENCE", new Column("CANCEL_REASON", DbType.String, 2000));
        //    this.Database.AddColumn("RIS_INSPECTION_OFFENCE", new Column("CANCEL_DATE", DbType.DateTime));
        //    this.Database.AddColumn("RIS_INSPECTION_OFFENCE", new Column("CANCEL_DECISION_NUM", DbType.String, 255));

        //    this.Database.AddEntityTable("RIS_INSPECTION_OFFENCE_ATTACH",
        //        new RefColumn("OFFENCE_ID", ColumnProperty.NotNull, "RIS_INSP_OFFENCE_ATTACH_OFFENCE", "RIS_INSPECTION_OFFENCE", "ID"),
        //        new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "RIS_INSP_OFFENCE_ATTACH_ATTACH", "RIS_ATTACHMENT", "ID"));
        //}

        //private void DownOffence()
        //{
        //    this.Database.RemoveColumn("RIS_INSPECTION_OFFENCE", "IS_CANCELLED");
        //    this.Database.RemoveColumn("RIS_INSPECTION_OFFENCE", "CANCEL_REASON");
        //    this.Database.RemoveColumn("RIS_INSPECTION_OFFENCE", "CANCEL_DATE");
        //    this.Database.RemoveColumn("RIS_INSPECTION_OFFENCE", "CANCEL_DECISION_NUM");

        //    this.Database.RemoveTable("RIS_INSPECTION_OFFENCE_ATTACH");
        //}

        //private void UpPrecept()
        //{
        //    this.Database.AddColumn("RIS_INSPECTION_PRECEPT", new Column("FIAS_HOUSE_GUID", DbType.String, 200));
        //    this.Database.AddColumn("RIS_INSPECTION_PRECEPT", new Column("IS_CANCELLED", DbType.Boolean));

        //    this.Database.AddEntityTable("RIS_INSPECTION_PRECEPT_ATTACH",
        //        new RefColumn("PRECEPT_ID", ColumnProperty.NotNull, "RIS_INSP_PRECEPT_ATTACH_PRECEPT", "RIS_INSPECTION_PRECEPT", "ID"),
        //        new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "RIS_INSP_PRECEPT_ATTACH_ATTACH", "RIS_ATTACHMENT", "ID"),
        //        new Column("IS_CANCEL_ATTACH", DbType.Boolean));
        //}

        //private void DownPrecept()
        //{
        //    this.Database.RemoveColumn("RIS_INSPECTION_PRECEPT", "FIAS_HOUSE_GUID");
        //    this.Database.RemoveColumn("RIS_INSPECTION_PRECEPT", "IS_CANCELLED");

        //    this.Database.RemoveTable("RIS_INSPECTION_PRECEPT_ATTACH");
        //}

        //private void UpExamination()
        //{
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "IS_PHYS_PERSON");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "COUNT_DAYS");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "CONTRAGENT_ID");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "OBJECT_CODE");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "OBJECT_GUID");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "ORDER_GKH_ID");

        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("SUBJECT_TYPE", DbType.Int32));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("ORG_ROOT_ENTITY_GUID", DbType.String, 36));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("ACTIVITY_PLACE", DbType.String, 2000));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("EVENT_DESC", DbType.String, 2000));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("HAS_RESULT", DbType.Boolean));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("RESULT_DOC_TYPE_CODE", DbType.String, 20));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("RESULT_DOC_TYPE_GUID", DbType.String, 36));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("RESULT_DOC_NUMBER", DbType.String, 255));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("RESULT_DOC_DATETIME", DbType.DateTime));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("HAS_OFFENCE", DbType.Boolean));

        //    this.Database.AddEntityTable("RIS_INSPECTION_EXAM_ATTACH",
        //        new RefColumn("EXAMINATION_ID", ColumnProperty.NotNull, "RIS_INSP_EXAM_ATTACH_EXAM", "RIS_INSPECTION_EXAMINATION", "ID"),
        //        new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "RIS_INSP_EXAM_ATTACH_ATTACH", "RIS_ATTACHMENT", "ID"));

        //    this.Database.AddEntityTable("RIS_INSPECTION_EXAM_PLACE",
        //        new RefColumn("EXAMINATION_ID", ColumnProperty.NotNull, "RIS_INSP_EXAM_PLACE_EXAM", "RIS_INSPECTION_EXAMINATION", "ID"),
        //        new Column("ORDER_NUM", DbType.Int32),
        //        new Column("FIAS_HOUSE_GUID", DbType.String, 200));
        //}

        //private void DownExamination()
        //{
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("IS_PHYS_PERSON", DbType.Boolean));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("COUNT_DAYS", DbType.Double));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new RefColumn("CONTRAGENT_ID", "EXAM_CONTRAGENT", "RIS_CONTRAGENT", "ID"));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("OBJECT_CODE", DbType.String, 50));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("OBJECT_GUID", DbType.String, 50));
        //    this.Database.AddColumn("RIS_INSPECTION_EXAMINATION", new Column("ORDER_GKH_ID", DbType.Int64));

        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "SUBJECT_TYPE");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "ORG_ROOT_ENTITY_GUID");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "ACTIVITY_PLACE");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "EVENT_DESC");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "HAS_RESULT");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "RESULT_DOC_TYPE_CODE");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "RESULT_DOC_TYPE_GUID");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "RESULT_DOC_NUMBER");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "RESULT_DOC_DATETIME");
        //    this.Database.RemoveColumn("RIS_INSPECTION_EXAMINATION", "HAS_OFFENCE");

        //    this.Database.RemoveTable("RIS_INSPECTION_EXAM_ATTACH");
        //    this.Database.RemoveTable("RIS_INSPECTION_EXAM_PLACE");
        //}
    }
}