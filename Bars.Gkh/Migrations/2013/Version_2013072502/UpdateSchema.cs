namespace Bars.Gkh.Migrations.Version_2013072502
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072502")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_DICT_CONEL_GROUP", new RefColumn("GROUP_EL_OBJ_ID", "GKH_CONEL_GROUP_GREL", "GKH_DICT_GROUP_ELEM_OBJ", "ID"));
        }

        public override void Down()
        {
        }
    }
}