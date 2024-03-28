namespace Bars.GkhGji.Migrations._2023.Version_2023052300
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023052300")]
    [MigrationDependsOn(typeof(_2023.Version_2023051500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("CONCLUSION_FILE_ID", "MKD_LIC_CONCLUSION_FILE", "B4_FILE_INFO", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("REQUEST_FILE_ID", "MKD_LIC_REQUEST_FILE", "B4_FILE_INFO", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("ZONAINSP_ID", "MKD_LIC_ZONAINSP", "GKH_DICT_ZONAINSP", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("PREVIOUS_REQUEST_ID", "MKD_LIC_PREVIOUS_REQUEST", "GJI_MKD_LIC_STATEMENT", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("GJI_DICT_KIND_ID", "MKD_LIC_GJI_DICT_KIND", "GJI_DICT_KINDSTATEMENT", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("GJI_REDTAPE_FLAG_ID", "MKD_LIC_GJI_REDTAPE_FLAG", "GJI_DICT_ACREDT_FLAG", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("REGISTRATOR_ID", "MKD_LIC_REGISTRATOR", "GKH_DICT_INSPECTOR", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("EXECUTANT_ID", "MKD_LIC_EXECUTANT", "GKH_DICT_INSPECTOR", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("SURETY_ID", "MKD_LIC_SURETY", "GKH_DICT_INSPECTOR", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("SURETY_RESOLVE_ID", "MKD_LIC_SURETY_RESOLVE", "GJI_DICT_RESOLVE", "ID"));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new RefColumn("APPROVALCONTRAGENT_ID", "MKD_LIC_APPROVALCONTRAGENT", "GKH_CONTRAGENT", "ID"));

            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("QUESTION_STATE", DbType.Int16));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("SSTU", DbType.Int16));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("CHECK_TIME", DbType.DateTime));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("EXTENS_TIME", DbType.DateTime));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("CONTROL_DATE_GIS_GKH", DbType.DateTime));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("GJI_NUMBER", DbType.String));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("REQUEST_STATUS", DbType.Int16));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("AMOUNT_PAGES", DbType.Int64));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("QUESTION_COUNT", DbType.Int32));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("PLANNED_EXEC_DATE", DbType.DateTime));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("EXEC_TAKE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("COMMENT", DbType.String, 1000));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("YEAR", DbType.Int32));
            Database.AddColumn("GJI_MKD_LIC_STATEMENT", new Column("CHANGE_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "CHANGE_DATE");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "YEAR");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "COMMENT");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "EXEC_TAKE_DATE");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "PLANNED_EXEC_DATE");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "QUESTION_COUNT");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "AMOUNT_PAGES");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "REQUEST_STATUS");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "GJI_NUMBER");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "CONTROL_DATE_GIS_GKH");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "EXTENS_TIME");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "CHECK_TIME");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "SSTU");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "QUESTION_STATE");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "APPROVALCONTRAGENT_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "SURETY_RESOLVE_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "SURETY_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "EXECUTANT_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "REGISTRATOR_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "GJI_REDTAPE_FLAG_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "GJI_DICT_KIND_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "PREVIOUS_REQUEST_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "ZONAINSP_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "REQUEST_FILE_ID");
            this.Database.RemoveColumn("GJI_MKD_LIC_STATEMENT", "CONCLUSION_FILE_ID");
        }
    }
}