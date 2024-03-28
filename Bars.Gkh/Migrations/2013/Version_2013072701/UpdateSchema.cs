namespace Bars.Gkh.Migrations.Version_2013072701
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_DICT_CONST_ELEMENT", "GROUP_ID");
            Database.AddRefColumn("GKH_DICT_CONST_ELEMENT", new RefColumn("GROUP_ID", "GKH_CONST_EL_GROUP", "GKH_DICT_CONEL_GROUP", "ID"));
        }

        public override void Down()
        {
        }
    }
}