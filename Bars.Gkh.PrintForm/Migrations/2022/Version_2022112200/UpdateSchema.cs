namespace Bars.Gkh.PrintForm.Migrations._2022.Version_2022112200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022112200")]
    public class UpdateSchema : Migration
    {
        private const string Table = "PDF_SIGN_INFO";

        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.Table, 
                new RefColumn("ORIGINAL_PDF_ID", $"{UpdateSchema.Table}_ORIGINAL_PDF", "B4_FILE_INFO", "ID"),
                new RefColumn("STAMPED_PDF_ID", $"{UpdateSchema.Table}_STAMPED_PDF", "B4_FILE_INFO", "ID"),
                new RefColumn("FOR_HASH_FILE_ID", $"{UpdateSchema.Table}_FOR_HASH_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_PDF_ID", $"{UpdateSchema.Table}_SIGNED_PDF", "B4_FILE_INFO", "ID"),
                new Column("CONTEXT_GUID", DbType.String));
        }

        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.Table);
        }
    }
}
