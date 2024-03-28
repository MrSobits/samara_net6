namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2018100500
{
    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Исправление миграции <see cref="Version_2018090700.UpdateSchema"/>
    /// </summary>
    [Migration("2018100500")]
    [MigrationDependsOn(typeof(Version_2018090700.UpdateSchema))]
    [MigrationReverseDependency(typeof(Gkh.Migrations._2018.Version_2018100500.UpdateSchema))]
    public sealed class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveExportId("OVRHL_SPECIAL_ACCOUNT");
        }
    }
}
