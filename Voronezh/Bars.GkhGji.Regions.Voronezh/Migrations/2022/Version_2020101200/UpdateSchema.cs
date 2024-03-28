namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020101200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020101200")]
    [MigrationDependsOn(typeof(Version_2020100600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_APPCIT_ADMONITION",    
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("PERFORMANCE_DATE", DbType.DateTime),
                new Column("PERFORMANCE_FACT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NAME", DbType.String),
                new Column("DOCUMENT_NUM", DbType.String, 30),
                new RefColumn("APPCIT_ID", "GJI_ADMONITION_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("CONTRAGENT_ID", "GJI_ADMONITION_CONTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("INSPECTOR_ID", "GJI_ADMONITION_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("EXECUTOR_ID", "GJI_ADMONITION_INSPECTOR_EXECUTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("FILE_INFO_ID", "GJI_APPCIT_ADMON_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("ANSWERFILE_INFO_ID", "GJI_APPCIT_ADMON_ANSWERFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_FILE_ID", "GJI_APPCIT_ADMONITION_SIGNED_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_FILE_ID", "GJI_APPCIT_ADMONITION_SIGNATURE", "B4_FILE_INFO", "ID"),
                new RefColumn("CERTIFICATE_FILE_ID", "GJI_APPCIT_ADMONITION_CERTIFICATE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_ANSWERFILE_ID", "GJI_APPCIT_ADMONITION_SIGNED_ANSWERFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_ANSWERFILE_ID", "GJI_APPCIT_ADMONITION_ANSWER_SIGNATURE", "B4_FILE_INFO", "ID"),
                new RefColumn("CERTIFICATE_ANSWERFILE_ID", "GJI_APPCIT_ADMONITION_ANSWER_CERTIFICATE", "B4_FILE_INFO", "ID"));

         

            this.Database.AddEntityTable("GJI_APPCIT_ADMON_VIOLATION",
                new Column("PLANED_DATE", DbType.DateTime),
                new Column("FACT_DATE", DbType.DateTime),
                new RefColumn("APPCIT_ADMONITION_ID", "GJI_AAV_ADMONITION", "GJI_APPCIT_ADMONITION", "ID"),
                new RefColumn("VIOLATION_ID", "GJI_AAV_VIOL", "GJI_DICT_VIOLATION", "ID"));           

          
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GJI_APPCIT_ADMON_VIOLATION");
            Database.RemoveTable("GJI_APPCIT_ADMONITION");
        }
    }
}


