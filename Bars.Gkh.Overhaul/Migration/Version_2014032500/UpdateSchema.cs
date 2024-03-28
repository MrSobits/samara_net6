namespace Bars.Gkh.Overhaul.Migration.Version_2014032500
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014013100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_DICT_WORK_PRICE", new RefColumn("CAPITAL_GROUP_ID", "OV_WRK_PR_CAP_GROUP", "GKH_DICT_CAPITAL_GROUP", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_DICT_WORK_PRICE", "CAPITAL_GROUP_ID");
        }
    }
}