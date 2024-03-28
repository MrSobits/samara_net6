namespace Bars.GkhGji.Migrations._2023.Version_2023101600
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023101600")]
    [MigrationDependsOn(typeof(Version_2023091800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("PDF_SIGN_INFO",
                new RefColumn("ORIGINAL_PDF_ID", "PDF_SIGN_INFO_ORIGINAL_PDF", "B4_FILE_INFO", "ID"),
                new RefColumn("STAMPED_PDF_ID", "PDF_SIGN_INFO_STAMPED_PDF", "B4_FILE_INFO", "ID"),
                new RefColumn("FOR_HASH_FILE_ID", "PDF_SIGN_INFO_FOR_HASH_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_PDF_ID", "PDF_SIGN_INFO_SIGNED_PDF", "B4_FILE_INFO", "ID"),
                new Column("CONTEXT_GUID", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveTable("PDF_SIGN_INFO");
        }
    }
}