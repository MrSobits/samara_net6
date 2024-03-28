namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022051800
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022051800")]
    [MigrationDependsOn(typeof(Version_2021121400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_CH_COURT_PRACTICE_HISTORY",
           new Column("CM_RESULT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("CM_TIME", DbType.DateTime, ColumnProperty.None),
           new Column("CP_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           new Column("IS_COST", DbType.Boolean),
           new Column("COST_FACT", DbType.Decimal, ColumnProperty.None),
           new Column("COST_PLAN", DbType.Decimal, ColumnProperty.None),
           new Column("CM_DATE", DbType.DateTime, ColumnProperty.NotNull),        
           new Column("COMMENT", DbType.String, 1500),
           new Column("IS_INLAW", DbType.Boolean),
           new Column("INLAW_DATE", DbType.DateTime, ColumnProperty.None),
           new Column("IS_MEASURES", DbType.Boolean),
           new Column("MEASURES_DATE", DbType.DateTime, ColumnProperty.None),
           new Column("PERF_LIST", DbType.String, 1500),
           new Column("PERF_PROC", DbType.String, 1500),         
           new Column("PAUSED_COMMENT", DbType.String, 500),
           new Column("IS_DISPUTE", DbType.Boolean),
           new RefColumn("CP_ID", ColumnProperty.NotNull, "FK_CPHISTORY_COURT_PRACTICE", "GJI_CH_COURT_PRACTICE", "ID"),
           new RefColumn("INSTANCE_ID", ColumnProperty.None, "FK_GJI_CH_COURT_PRACTICEHISTORY_INSTANCE_ID", "GJI_DICT_INSTANCE", "ID"),            
           new RefColumn("FILE_ID", ColumnProperty.None, "FK_GJI_CH_COURT_PRACTICEHISTORY_FILE_ID", "B4_FILE_INFO", "ID"),
           new RefColumn("JUR_INST_ID", ColumnProperty.NotNull, "FK_GJI_CH_COURT_PRACTICEHISTORY_JURINST_ID", "CLW_JUR_INSTITUTION", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_COURT_PRACTICE_HISTORY");
        }
    }
}