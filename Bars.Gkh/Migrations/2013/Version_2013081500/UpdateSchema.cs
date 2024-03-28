namespace Bars.Gkh.Migrations.Version_2013081500
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013081500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013081300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_DICT_INSPECTOR", new RefColumn("ZON_INSP_ID", "GKH_INS_ZON_INSP", "GKH_DICT_ZONAINSP", "ID"));

            Database.AddEntityTable("GKH_DICT_INSP_SUBSCRIP",
                new RefColumn("INSP_ID", "GKH_INS_SUBSCR_INS", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("SIGNED_INSP_ID", "GKH_INS_SUBSCR_SIGN", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "ZON_INSP_ID");
            Database.RemoveTable("GKH_DICT_INSP_SUBSCRIP");
        }
    }
}