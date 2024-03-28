namespace Bars.Gkh.Regions.Tatarstan.Migrations.Version_2014022400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Tatarstan.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_REALITY_OBJECT", new Column("HAS_PRIV_FLATS", DbType.Boolean, ColumnProperty.Null, null));
        }

        public override void Down()
        {
            Database.ChangeColumn("GKH_REALITY_OBJECT", new Column("HAS_PRIV_FLATS", DbType.Boolean, ColumnProperty.NotNull, false));
        }
    }
}