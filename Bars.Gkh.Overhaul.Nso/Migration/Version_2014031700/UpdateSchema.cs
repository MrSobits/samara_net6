using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014031700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014031400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC",  new Column("IS_CHANGED_YEAR", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("DOC_NAME", DbType.String, 300));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("DOC_NUM", DbType.String, 50));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("DOC_DATE", DbType.DateTime));
            Database.AddRefColumn("OVRHL_VERSION_REC", new RefColumn("FILE_ID", "OV_VERS_REC_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CHANGED_YEAR");
            Database.RemoveColumn("OVRHL_VERSION_REC", "DOC_NAME");
            Database.RemoveColumn("OVRHL_VERSION_REC", "DOC_NUM");
            Database.RemoveColumn("OVRHL_VERSION_REC", "DOC_DATE");
            Database.RemoveColumn("OVRHL_VERSION_REC", "FILE_ID");
        }
    }
}