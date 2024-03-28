namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014091500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014090400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_TAT_PARAM",
                new Column("CPREFIX", DbType.String, 20, ColumnProperty.NotNull),
                new Column("CKEY", DbType.String, 50, ColumnProperty.NotNull),
                new Column("CVALUE", DbType.String, 2000));

            Database.AddIndex("GJI_TAT_PARAM_PREF", false, "GJI_TAT_PARAM", "CPREFIX");
            Database.AddIndex("GJI_TAT_PARAM_KEY", false, "GJI_TAT_PARAM", "CKEY");
        }

        public override void Down()
        {
            Database.RemoveIndex("GJI_TAT_PARAM_KEY", "GJI_TAT_PARAM");
            Database.RemoveIndex("GJI_TAT_PARAM_PREF", "GJI_TAT_PARAM");

            Database.RemoveTable("GJI_TAT_PARAM");
        }
    }
}