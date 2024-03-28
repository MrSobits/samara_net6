namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021120801
{
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021120801")]

    [MigrationDependsOn(typeof(Version_2021120800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private List<Column> listColumn = new List<Column>
        {
            new Column("INN", DbType.String),
            new Column("TYPE_JUR_PERSON", DbType.Int32),
            new Column("TIME_START", DbType.DateTime),
            new Column("APPEAL_CITS_ID", DbType.Int64),
            new Column("ISSUED_TASK_ID", DbType.Int64),
            new Column("RESPONSIBLE_EXECUTION_ID", DbType.Int64),
            new Column("CONTROL_TYPE_ID", DbType.Int64),
            new Column("ZONAL_INSPECTION_ID", DbType.Int64),
            new Column("BASE_DOC_NAME", DbType.String),
            new Column("BASE_DOC_NUM", DbType.String),
            new Column("BASE_DOC_DATE", DbType.DateTime),
            new Column("BASE_DOC_FILE_ID", DbType.Int64)
        };

        public override void Up()
        {
            foreach (var column in listColumn)
            {
                this.Database.AddColumn("GJI_TASK_ACTIONISOLATED", column);
            }
            this.Database.AddForeignKey("FK_GJI_TASK_ACTION_APPEAL_CITS", "GJI_TASK_ACTIONISOLATED", "APPEAL_CITS_ID", "GJI_APPEAL_CITIZENS", "ID");
            this.Database.AddForeignKey("FK_GJI_TASK_ACTION_ISSUED_TASK", "GJI_TASK_ACTIONISOLATED", "ISSUED_TASK_ID", "GKH_DICT_INSPECTOR", "ID");
            this.Database.AddForeignKey("FK_GJI_TASK_ACTION_RESPONSIBLE_EXECUTION", "GJI_TASK_ACTIONISOLATED", "RESPONSIBLE_EXECUTION_ID", "GKH_DICT_INSPECTOR", "ID");
            this.Database.AddForeignKey("FK_GJI_TASK_ACTION_CONTROL_TYPE", "GJI_TASK_ACTIONISOLATED", "CONTROL_TYPE_ID", "GJI_DICT_CONTROL_TYPES", "ID");
            this.Database.AddForeignKey("FK_GJI_TASK_ACTION_ZONAL_INSPECTION", "GJI_TASK_ACTIONISOLATED", "ZONAL_INSPECTION_ID", "GKH_DICT_ZONAINSP", "ID");
            this.Database.AddForeignKey("FK_GJI_TASK_ACTION_BASE_DOC_FILE", "GJI_TASK_ACTIONISOLATED", "BASE_DOC_FILE_ID", "B4_FILE_INFO", "ID");
        }
        public override void Down()
        {
            foreach (var column in listColumn)
            {
                this.Database.RemoveColumn("GJI_TASK_ACTIONISOLATED", column.Name);
            }
        }
    }
}
