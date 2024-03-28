namespace Bars.Gkh.Migrations.Version_2013102400
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013102300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.IndexExists("IND_B4_FILEINFO_CHECKSUM", "B4_FILE_INFO"))
            {
                Database.AddIndex("IND_B4_FILEINFO_CHECKSUM", false, "B4_FILE_INFO", "CHECKSUM");
            }
        }

        public override void Down()
        {
            if (Database.IndexExists("IND_B4_FILEINFO_CHECKSUM", "B4_FILE_INFO"))
            {
                Database.RemoveIndex("IND_B4_FILEINFO_CHECKSUM", "B4_FILE_INFO");
            }
        }
    }
}