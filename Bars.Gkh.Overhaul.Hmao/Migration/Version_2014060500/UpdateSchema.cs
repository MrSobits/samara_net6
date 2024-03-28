namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014060500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014051200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CHANGED_YEAR");
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_CHANGED_YEAR", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CHANGED_YEAR");
        }
    }
}