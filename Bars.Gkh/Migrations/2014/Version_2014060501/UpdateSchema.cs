namespace Bars.Gkh.Migrations._2014.Version_2014060501
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2014.Version_2014060500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_RO_BUILD_FEATURE",
                new RefColumn("BUILDING_FEATURE_ID", ColumnProperty.NotNull, "RO_BFEATURE_BF", "GKH_DICT_BUILDING_FEATURE", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "RO_BFEATURE_RO", "GKH_REALITY_OBJECT", "ID"));       
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_RO_BUILD_FEATURE");
        }
    }
}