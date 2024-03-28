namespace Bars.GkhGji.MigrationsVersion_2014100300
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014092400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_FEATUREVIOL", new RefColumn("PARENT_ID", ColumnProperty.Null, "PARENT_FEATUREVIOL", "GJI_DICT_FEATUREVIOL", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_FEATUREVIOL", "PARENT_ID");
        }
    }
}