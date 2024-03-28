namespace Bars.GkhGji.Migrations._2023.Version_2023041300
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2023041300")]
    [MigrationDependsOn(typeof(_2023.Version_2023033000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// <summary>
        public override void Up()
        {
            //-----Акт профвизита
            Database.AddTable(
             "GJI_PREVENTIVE_VISIT",
             new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
             new Column("TYPE_ACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
             new Column("PERSON_INSPECTION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
             new Column("PHYSICAL_PERSON", DbType.String, 300),
             new Column("ACT_ADDRESS", DbType.String, 300),
             new Column("PHYSICAL_PERSON_INFO", DbType.String, 1500),
             new Column("PHYSICAL_PERSON_ADDRESS", DbType.String, 1500),
             new Column("DISTANCE_DESCRIPTION", DbType.String, 1500),
             new Column("IS_DISTANCE", DbType.Boolean, ColumnProperty.NotNull, false),
             new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.None, 0),
             new Column("DISTANCE_DATE", DbType.DateTime, ColumnProperty.None),
             new Column("ERKNMID", DbType.String, 100),
             new Column("IS_SENT", DbType.Boolean, ColumnProperty.NotNull, false),
             new Column("VIDEOLINK", DbType.String, 1500),
             new Column("ERKNMGUID", DbType.String, 100),
             new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "GJI_PREVENTIVE_VISIT_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddForeignKey("FK_GJI_PREVENTIVE_VISIT_DOC", "GJI_PREVENTIVE_VISIT", "ID", "GJI_DOCUMENT", "ID");

            //-----Приложения Акта профвизита
            Database.AddEntityTable(
            "GJI_PREVENTIVE_VISIT_ANNEX",
            new Column("DOCUMENT_DATE", DbType.DateTime),
            new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
            new Column("DESCRIPTION", DbType.String, 500),
            new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36),
            new Column("ANNEX_TYPE", DbType.Int32, ColumnProperty.NotNull, 0),
            new RefColumn("FILE_ID", "GJI_PV_ANNEX_FILE", "B4_FILE_INFO", "ID"),
            new RefColumn("SIGNED_FILE_ID", "GJI_PV_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"),
            new RefColumn("SIGNATURE_FILE_ID", "GJI_PV_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"),
            new RefColumn("CERTIFICATE_FILE_ID", "GJI_PV_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"),
            new RefColumn("PREVENT_VISIT_ID", ColumnProperty.NotNull, "GJI_PV_ANNEX_PREVENTIVE_VISIT", "GJI_PREVENTIVE_VISIT", "ID"));

            Database.AddEntityTable(
              "GJI_PREVENTIVE_VISIT_WITNESS",
              new Column("POSITION", DbType.String, 300),
              new Column("IS_FAMILIAR", DbType.Boolean, ColumnProperty.NotNull, false),
              new Column("FIO", DbType.String, ColumnProperty.NotNull, 300),
              new RefColumn("PREVENT_VISIT_ID", ColumnProperty.NotNull, "GJI_PV_WITNESS_PREVENTIVE_VISIT", "GJI_PREVENTIVE_VISIT", "ID"));

            Database.AddEntityTable(
            "GJI_PREVENTIVE_VISIT_RO",
            new RefColumn("PREVENT_VISIT_ID", ColumnProperty.NotNull, "GJI_PV_RO_PREVENTIVE_VISIT", "GJI_PREVENTIVE_VISIT", "ID"),
            new RefColumn("RO_ID", ColumnProperty.NotNull, "GJI_PV_RO_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable(
           "GJI_PREVENTIVE_VISIT_PERIOD",
           new Column("DATE_CHECK", DbType.DateTime),
           new Column("DATE_START", DbType.DateTime),
           new Column("DATE_END", DbType.DateTime),
           new RefColumn("VISIT_ID", ColumnProperty.NotNull, "GJI_PV_PERIOD_PREVENTIVE_VISIT", "GJI_PREVENTIVE_VISIT", "ID"));


            Database.AddEntityTable(
              "GJI_PREVENTIVE_VISIT_RESULT",
              new Column("DESCRIPTION", DbType.String, 10000),
              new Column("RESULT_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 0),
              new RefColumn("RO_ID", ColumnProperty.Null, "GJI_PV_RESULT_RO", "GKH_REALITY_OBJECT", "ID"),
              new RefColumn("PREVENT_VISIT_ID", ColumnProperty.NotNull, "GJI_PV_RESULT_PREVENTIVE_VISIT", "GJI_PREVENTIVE_VISIT", "ID"));

            Database.AddEntityTable(
            "GJI_PREVENTIVE_VISIT_RESULT_VIOLATION",
            new RefColumn("RESULT_ID", ColumnProperty.NotNull, "GJI_PV_RESULT_VIOLATION_RESULT", "GJI_PREVENTIVE_VISIT_RESULT", "ID"),
            new RefColumn("VIOLATION_ID", ColumnProperty.NotNull, "GJI_PV_RESULT_VIOLATION_VIOLATION", "GJI_DICT_VIOLATION", "ID"));


        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PREVENTIVE_VISIT_RESULT_VIOLATION");
            Database.RemoveTable("GJI_PREVENTIVE_VISIT_RESULT");
            Database.RemoveTable("GJI_PREVENTIVE_VISIT_PERIOD");
            Database.RemoveTable("GJI_PREVENTIVE_VISIT_RO");
            Database.RemoveTable("GJI_PREVENTIVE_VISIT_WITNESS");
            Database.RemoveTable("GJI_PREVENTIVE_VISIT_ANNEX");
            Database.RemoveTable("GJI_PREVENTIVE_VISIT");
        }
    }
}