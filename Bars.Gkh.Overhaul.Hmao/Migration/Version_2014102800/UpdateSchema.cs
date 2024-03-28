namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014102800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014100500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_ADD_ACTUALIZE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_CH_YEAR_ACTUALIZE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_CH_SUM_ACTUALIZE", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_ADD_ACTUALIZE");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CH_YEAR_ACTUALIZE");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CH_SUM_ACTUALIZE");
        }
    }
}