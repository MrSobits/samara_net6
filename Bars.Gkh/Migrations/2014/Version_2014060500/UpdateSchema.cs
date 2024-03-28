namespace Bars.Gkh.Migrations._2014.Version_2014060500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2014.Version_2014060100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_BUILDING_FEATURE",
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.NotNull));       
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_BUILDING_FEATURE");
        }
    }
}