namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021081100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021081100")]
    [MigrationDependsOn(typeof(Version_2021072800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_APPCIT_PRESCRIPTION_FOND",    
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("PERFORMANCE_DATE", DbType.DateTime),
                new Column("PERFORMANCE_FACT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NAME", DbType.String),
                new Column("DOCUMENT_NUM", DbType.String, 30),
                new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new RefColumn("BC_ID", "GJI_PR_FOND_BC_ID", "CR_OBJ_MASS_BUILD_CONTRACT", "ID"),
                new RefColumn("APPCIT_ID", "GJI_PR_FOND_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("CONTRAGENT_ID", "GJI_PR_FOND_CONTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("INSPECTOR_ID", "GJI_PR_FOND_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("EXECUTOR_ID", "GJI_PR_FOND_INSPECTOR_EXECUTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("FILE_INFO_ID", "GJI_APPCIT_PR_FOND_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("ANSWERFILE_INFO_ID", "GJI_APPCIT_PR_FOND_ANSWERFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_FILE_ID", "GJI_APPCIT_PR_FOND_SIGNED_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_FILE_ID", "GJI_APPCIT_PR_FOND_SIGNATURE", "B4_FILE_INFO", "ID"),
                new RefColumn("CERTIFICATE_FILE_ID", "GJI_APPCIT_PR_FOND_CERTIFICATE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_ANSWERFILE_ID", "GJI_APPCIT_PR_FOND_SIGNED_ANSWERFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_ANSWERFILE_ID", "GJI_APPCIT_PR_FOND_ANSWER_SIGNATURE", "B4_FILE_INFO", "ID"),
                new RefColumn("CERTIFICATE_ANSWERFILE_ID", "GJI_APPCIT_PR_FOND_ANSWER_CERTIFICATE", "B4_FILE_INFO", "ID"));

         

            this.Database.AddEntityTable("GJI_APPCIT_PR_FOND_VIOLATION",
                new Column("PLANED_DATE", DbType.DateTime),
                new Column("FACT_DATE", DbType.DateTime),
                new RefColumn("APPCIT_PR_FOND_ID", "GJI_PR_FOND", "GJI_APPCIT_PRESCRIPTION_FOND", "ID"),
                new RefColumn("VIOLATION_ID", "GJI_PR_FOND_VIOL", "GJI_DICT_VIOLATION", "ID"));           

          
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GJI_APPCIT_PR_FOND_VIOLATION");
            Database.RemoveTable("GJI_APPCIT_PRESCRIPTION_FOND");
        }
    }
}


