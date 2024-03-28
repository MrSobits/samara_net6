namespace Bars.GkhGji.Migrations._2024.Version_2024030104
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using System.Data;

    [Migration("2024030104")]
    [MigrationDependsOn(typeof(Version_2024030103.UpdateSchema))]
    /// Является Version_2018080300 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddTable("GJI_ACTISOLATED",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("AREA", DbType.Decimal, ColumnProperty.Null),
                new Column("FLAT", DbType.String, 10, ColumnProperty.Null),
                new Column("DOCUMENT_TIME", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("DOCUMENT_PLACE_FIAS_ID", ColumnProperty.Null, "GJI_ACTISOLATED_DOCUMENT_PLACE_FIAS", "B4_FIAS_ADDRESS", "ID"));
            this.Database.AddForeignKey("FK_GJI_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID", "GJI_DOCUMENT", "ID");

            this.Database.AddEntityTable("GJI_ACTISOLATED_ANNEX",
                new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("NAME", DbType.String, 300, ColumnProperty.Null),
                new Column("DESCRIPTION", DbType.String, 500, ColumnProperty.Null),
                new RefColumn("ACTISOLATED_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_ANNEX_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.Null, "GJI_ACTISOLATED_ANNEX_FILE_ID", "B4_FILE_INFO", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_DEFINITION",
                new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("EXECUTION_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("DOC_NUMBER", DbType.Int32, ColumnProperty.Null),
                new Column("DOCUMENT_NUM", DbType.String, 50, ColumnProperty.Null),
                new Column("DESCRIPTION", DbType.String, 500, ColumnProperty.Null),
                new Column("TYPE_DEFINITION", DbType.String, ColumnProperty.NotNull),
                new RefColumn("ACTISOLATED_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_DEFINITION_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID"),
                new RefColumn("ISSUED_DEFINITION_ID", ColumnProperty.Null, "GJI_ACTISOLATED_DEFINITION_ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_INSPECTPART",
                new Column("CHARACTER", DbType.String, 300, ColumnProperty.Null),
                new Column("DESCRIPTION", DbType.String, 500, ColumnProperty.Null),
                new RefColumn("ACTISOLATED_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_INSPECTPART_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID"),
                new RefColumn("INSPECTIONPART_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_INSPECTPART_INSPECTIONPART_ID", "GJI_DICT_INSPECTEDPART", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_PERIOD",
                new Column("DATE_CHECK", DbType.DateTime, ColumnProperty.Null),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.Null),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("ACTISOLATED_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_PERIOD_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_PROVDOC",
                new Column("DATE_PROVIDED", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("ACTISOLATED_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_PROVDOC_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID"),
                new RefColumn("PROVDOC_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_PROVDOC_PROVDOC_ID", "GJI_DICT_PROVIDEDDOCUMENT", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_WITNESS",
                new Column("FIO", DbType.String, 300, ColumnProperty.Null),
                new Column("POSITION", DbType.String, 300, ColumnProperty.Null),
                new Column("IS_FAMILIAR", DbType.Boolean, ColumnProperty.NotNull),
                new RefColumn("ACTISOLATED_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_WITNESS_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_ROBJECT",
                new Column("DESCRIPTION", DbType.String, 2000, ColumnProperty.Null),
                new Column("HAVE_VIOLATION", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("ACTISOLATED_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_ROBJECT_ACTISOLATED_ID", "GJI_ACTISOLATED", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_ROBJECT_REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_ROBJECT_MEASURE",
                new Column("MEASURE", DbType.String, 300, ColumnProperty.NotNull),
                new RefColumn("ACTISOLATED_RO_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_ROBJECT_MEASURE_ACT_RO_ID", "GJI_ACTISOLATED_ROBJECT", "ID"));

            this.Database.AddEntityTable("GJI_ACTISOLATED_ROBJECT_EVENT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("TERM", DbType.Int32, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("ACTISOLATED_RO_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_ROBJECT_EVENT_ACT_RO_ID", "GJI_ACTISOLATED_ROBJECT", "ID"));

            this.Database.AddTable("GJI_ACTISOLATED_ROBJECT_VIOLATION",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("EVENT_RESULT", DbType.String, 500, ColumnProperty.Null),
                new RefColumn("ACTISOLATED_RO_ID", ColumnProperty.NotNull, "GJI_ACTISOLATED_ROBJECT_VIOLATION_ACT_RO_ID", "GJI_ACTISOLATED_ROBJECT", "ID"));
            this.Database.AddForeignKey("FK_GJI_ACTISOLATED_ROBJECT_VIOLATION_ID", "GJI_ACTISOLATED_ROBJECT_VIOLATION", "ID", "GJI_INSPECTION_VIOL_STAGE", "ID");

            //ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActisolatedCountRobject");
            //ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActisolatedHasViolation");
            //ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActisolatedRobject");
            //ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActisolatedRobjectMuName");
            //ViewManager.Create(this.Database, "GkhGji", "CreateFunctionGetActisolatedRobjectMuId");
            //ViewManager.Create(this.Database, "GkhGji", "CreateViewActisolated");
        }

        public override void Down()
        {
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteViewActisolated");
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteFunctionGetActisolatedRobjectMuId");
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteFunctionGetActisolatedRobjectMuName");
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteFunctionGetActisolatedRobject");
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteFunctionGetActisolatedHasViolation");
            //ViewManager.Drop(this.Database, "GkhGji", "DeleteFunctionGetActisolatedCountRobject");

            this.Database.RemoveTable("GJI_ACTISOLATED_ROBJECT_VIOLATION");
            this.Database.RemoveTable("GJI_ACTISOLATED_ROBJECT_MEASURE");
            this.Database.RemoveTable("GJI_ACTISOLATED_ROBJECT_EVENT");

            this.Database.RemoveTable("GJI_ACTISOLATED_ROBJECT");
            this.Database.RemoveTable("GJI_ACTISOLATED_ANNEX");
            this.Database.RemoveTable("GJI_ACTISOLATED_DEFINITION");
            this.Database.RemoveTable("GJI_ACTISOLATED_INSPECTPART");
            this.Database.RemoveTable("GJI_ACTISOLATED_PERIOD");
            this.Database.RemoveTable("GJI_ACTISOLATED_PROVDOC");
            this.Database.RemoveTable("GJI_ACTISOLATED_WITNESS");

            this.Database.RemoveTable("GJI_ACTISOLATED");
        }
    }
}