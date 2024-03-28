namespace Bars.GkhGji.Migrations._2020.Version_2020121800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020121800")]
    [MigrationDependsOn(typeof(Version_2020121500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        { 
            Database.AddEntityTable(
                         "GJI_MKD_LIC_REQUEST_QUERY",
                         new RefColumn("REQUEST_ID", "GJI_REQ_REQUEST_ID", "GJI_MKD_LIC_STATEMENT", "ID"),
                         new Column("COMPETENT_ORG_ID", DbType.Int64, 22),
                         new Column("DOCUMENT_NAME", DbType.String, 300),
                         new Column("DOCUMENT_NUM", DbType.String, 300),
                         new Column("DOCUMENT_DATE", DbType.Date),
                         new Column("DESCRIPTION", DbType.String, 2000),
                         new Column("PERFORMANCE_DATE", DbType.Date),
                         new Column("PERFORMANCE_FACT_DATE", DbType.Date),
                         new RefColumn("FILE_INFO_ID", "GJI_MKD_LIC_REQUEST_QUERY_FI", "B4_FILE_INFO", "ID"),
                         new RefColumn("FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_FILE", "B4_FILE_INFO", "ID"),
                         new RefColumn("SIGNED_FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_SIG_FILE", "B4_FILE_INFO", "ID"),
                         new RefColumn("SIGNATURE_FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_SIGNATURE", "B4_FILE_INFO", "ID"),
                         new RefColumn("CERTIFICATE_FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_CERT", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
                "GJI_MKD_LIC_REQUEST_QUERY_ANSWER",
                new RefColumn("REQUEST_QUERY_ID", "GJI_MKD_LIC_REQUEST_ANSW", "GJI_MKD_LIC_REQUEST_QUERY", "ID"),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 2000),
                new RefColumn("FILE_INFO_ID", "GJI_GJI_MKD_LIC_REQUEST_QUERY_ANSWER_FI", "B4_FILE_INFO", "ID"),
                new RefColumn("FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_ANSWER_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_ANSWER_SIG_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_ANSWER_SIGNATURE", "B4_FILE_INFO", "ID"),
                new RefColumn("CERTIFICATE_FILE_ID", "FK_GJI_MKD_LIC_REQUEST_QUERY_ANSWER_CERT", "B4_FILE_INFO", "ID"));


           

            Database.AddIndex("IND_GJI_GJI_MKD_LIC_REQUEST_QUERY_CORG", false, "GJI_MKD_LIC_REQUEST_QUERY", "COMPETENT_ORG_ID");
            Database.AddForeignKey("FK_GJI_MKD_LIC_REQUEST_QUERY_CORG", "GJI_MKD_LIC_REQUEST_QUERY", "COMPETENT_ORG_ID", "GJI_DICT_COMPETENT_ORG", "ID");

        }

        public override void Down()
        {          
            
            Database.RemoveTable("GJI_MKD_LIC_REQUEST_QUERY_ANSWER");
            Database.RemoveTable("GJI_MKD_LIC_REQUEST_QUERY");
            Database.RemoveIndex("IND_GJI_GJI_MKD_LIC_REQUEST_QUERY_CORG", "GJI_MKD_LIC_REQUEST_QUERY");
            Database.RemoveConstraint("GJI_MKD_LIC_REQUEST_QUERY", "FK_GJI_MKD_LIC_REQUEST_QUERY_CORG");
        }
    }
}