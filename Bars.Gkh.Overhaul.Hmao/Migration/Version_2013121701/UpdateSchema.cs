namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121701
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_SUBSIDY_REC", new RefColumn("MUNICIPALITY_ID", ColumnProperty.Null, "OVRHL_SUB_REC_MO", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_REC", "MUNICIPALITY_ID");
        }
    }
}