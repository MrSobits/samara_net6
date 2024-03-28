namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013120500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013120300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PUBLISH_PRG", new Column("SIGN_DATE", DbType.DateTime));
            Database.AddRefColumn(
                "OVRHL_PUBLISH_PRG", new RefColumn("FILE_XML_ID", "OVRHL_PUBLISH_PRG_FXML", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn(
                "OVRHL_PUBLISH_PRG", new RefColumn("FILE_PDF_ID", "OVRHL_PUBLISH_PRG_FPDF", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn(
                "OVRHL_PUBLISH_PRG", new RefColumn("FILE_CER_ID", "OVRHL_PUBLISH_PRG_FCER", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PUBLISH_PRG", "FILE_XML_ID");
            Database.RemoveColumn("OVRHL_PUBLISH_PRG", "FILE_PDF_ID");
            Database.RemoveColumn("OVRHL_PUBLISH_PRG", "FILE_CER_ID");
            Database.RemoveColumn("OVRHL_PUBLISH_PRG", "SIGN_DATE");
        }
    }
}