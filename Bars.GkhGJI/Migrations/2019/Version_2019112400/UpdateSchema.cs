namespace Bars.GkhGji.Migrations._2019.Version_2019112400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019112400")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019080500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddEntityTable(
          "GJI_PRESCRIPTION_OFFICIAL_REPORT",
              new RefColumn("PRESCRIPTION_ID", "FK_GJI_PRESCR_OR_DOC", "GJI_PRESCRIPTION", "ID"),
              new RefColumn("FILE_ID", "FK_GJI_PRESCRIPTION_OFFICIAL_REPORT_STATE", "B4_FILE_INFO", "ID"),
              new RefColumn("INSPECTOR_ID", "FK_GJI_PRESCR_OR_FILE", "GKH_DICT_INSPECTOR", "ID"),
              new RefColumn("STATE_ID", "GJI_ACT_TSJ_STATE", "B4_STATE", "ID"),
              new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.None),
              new Column("VIOLATION_DATE", DbType.DateTime, ColumnProperty.None),
              new Column("NAME", DbType.String, 300, ColumnProperty.None),
              new Column("NUMBER", DbType.String, 300, ColumnProperty.NotNull),
              new Column("DESCRIPTION", DbType.String, 2000),
              new Column("VIOLATION_REMOVED", DbType.Int32, 4, ColumnProperty.NotNull, 10),
              new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddEntityTable(
            "GJI_PRESCR_OFF_REPORT_VIOLATION",
                new RefColumn("OFFICIAL_REPORT_ID", "FK_GJI_PRESCR_OR_VIOL_DOC", "GJI_PRESCRIPTION_OFFICIAL_REPORT", "ID"),
                new RefColumn("VIOL_STAGE_ID", "FK_GJI_PRESCR_OFF_REPORT_VIOLATION", "GJI_PRESCRIPTION_VIOLAT", "ID"));
            //GJI_INSPECTION_VIOL_STAGE
            Database.AddColumn("GJI_INSPECTION_VIOL_STAGE", new Column("DATE_PLAN_EXTENSION", DbType.DateTime, ColumnProperty.None));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PRESCR_OFF_REPORT_VIOLATION");
            Database.RemoveTable("GJI_PRESCRIPTION_OFFICIAL_REPORT");
        }
    }
}