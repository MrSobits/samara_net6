namespace Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2013120600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013120400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_SERVICE_PROVIDER", new Column("IS_ACTIVE", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_SERVICE_PROVIDER", "IS_ACTIVE");       
        }
    }
}