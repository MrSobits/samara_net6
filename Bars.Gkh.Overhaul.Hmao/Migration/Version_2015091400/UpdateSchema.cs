namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2015091400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2015070300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_DIVIDED_REC", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("PUBLISH_YEAR_FOR_DIV_REC", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_DIVIDED_REC");
            Database.RemoveColumn("OVRHL_VERSION_REC", "PUBLISH_YEAR_FOR_DIV_REC");
        }
    }
}