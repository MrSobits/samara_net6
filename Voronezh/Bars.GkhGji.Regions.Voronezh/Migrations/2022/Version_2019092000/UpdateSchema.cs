namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019092000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019092000")]
    [MigrationDependsOn(typeof(Version_2019081200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_VR_COURT_PRACTICE",
           new Column("CM_RESULT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("CM_TIME", DbType.DateTime, ColumnProperty.None),
           new Column("CP_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("IS_COST", DbType.Boolean),
           new Column("COST_FACT", DbType.Decimal, ColumnProperty.None),
           new Column("COST_PLAN", DbType.Decimal, ColumnProperty.None),
           new Column("CM_DATE", DbType.DateTime, ColumnProperty.NotNull),
           new Column("DEF_ADDRESS", DbType.String, 500),
           new Column("DEF_FIO", DbType.String, 100),
           new Column("DIFF_ADDRESS", DbType.String, 500),
           new Column("DIFF_FIO", DbType.String, 100),
           new Column("COMMENT", DbType.String, 500),
           new Column("DISP_CATEGORY", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("DISP_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("DOC_NUMBER", DbType.String, 20),
           new Column("IS_INLAW", DbType.Boolean),
           new Column("INLAW_DATE", DbType.DateTime, ColumnProperty.None),
           new Column("IS_MEASURES", DbType.Boolean),
           new Column("MEASURES_DATE", DbType.DateTime, ColumnProperty.None),
           new Column("PERF_LIST", DbType.String, 1500),
           new Column("PERF_PROC", DbType.String, 1500),
           new Column("PLANT_ADDRESS", DbType.String, 500),
           new Column("PLANT_FIO", DbType.String, 100),
           new RefColumn("TYPE_FACT_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_FV_ID", "GJI_DICT_TYPE_FACT_VIOL", "ID"),
           new RefColumn("CONTRAGENT_D_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_DCONTR_ID", "GKH_CONTRAGENT", "ID"),
           new RefColumn("CONTRAGENT_P_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_PCONTR_ID", "GKH_CONTRAGENT", "ID"),
           new RefColumn("CONTRAGENT_DIFF_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_DIFFCONTR_ID", "GKH_CONTRAGENT", "ID"),
           new RefColumn("STATE_ID", "FK_GJI_VR_COURT_PRACTICE_STATE", "B4_STATE", "ID"),
           new RefColumn("FILE_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_FILE_ID", "B4_FILE_INFO", "ID"),
           new RefColumn("JUR_INST_ID", ColumnProperty.NotNull, "FK_GJI_VR_COURT_PRACTICE_JURINST_ID", "CLW_JUR_INSTITUTION", "ID"));

            Database.AddEntityTable(
           "GJI_VR_COURT_PRACTICE_INSPECTOR",
           new Column("LAWYER_INSPECTOR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("DESCRIPTION", DbType.String, 1500),
           new RefColumn("CP_ID", ColumnProperty.NotNull, "FK_CPINSP_COURT_PRACTICE", "GJI_VR_COURT_PRACTICE", "ID"),
           new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "FK_CPINSP_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
           "GJI_VR_COURT_PRACTICE_RO",
           new RefColumn("CP_ID", ColumnProperty.NotNull, "FK_CPRO_COURT_PRACTICE", "GJI_VR_COURT_PRACTICE", "ID"),
           new RefColumn("RO_ID", ColumnProperty.NotNull, "FK_CPRO_REALITY_OBJECT", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable(
            "GJI_VR_COURT_PRACTICE_FILE",
            new Column("DOC_NAME", DbType.String, 1500),
            new Column("DESCRIPTION", DbType.String, 1500),
            new RefColumn("CP_ID", ColumnProperty.NotNull, "FK_CPFILE_COURT_PRACTICE", "GJI_VR_COURT_PRACTICE", "ID"),
            new RefColumn("FILE_ID", ColumnProperty.NotNull, "FK_CPFILE_FILE", "B4_FILE_INFO", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_VR_COURT_PRACTICE_FILE");
            Database.RemoveTable("GJI_VR_COURT_PRACTICE_RO");
            Database.RemoveTable("GJI_VR_COURT_PRACTICE_INSPECTOR");
            Database.RemoveTable("GJI_VR_COURT_PRACTICE");
        }
    }
}