namespace Bars.Gkh.Migrations.Version_2015042300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015042300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015042200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_CONFIG_PARAMETER",
                new Column("KEY", DbType.String, 500, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("VALUE", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CONFIG_PARAMETER");
        }
    }
}