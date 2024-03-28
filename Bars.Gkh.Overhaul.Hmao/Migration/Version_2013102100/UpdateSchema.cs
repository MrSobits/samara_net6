namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013102100
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_CURR_PRIORITY", new RefColumn("MU_ID", ColumnProperty.Null, "OVRHL_CUR_PRM_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_CURR_PRIORITY", "MU_ID");
        }
    }
}