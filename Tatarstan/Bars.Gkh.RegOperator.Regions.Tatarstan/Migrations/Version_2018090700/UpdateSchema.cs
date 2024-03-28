namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Migrations.Version_2018090700
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Схема миграции
    /// </summary>
    [Migration("2018090700")]
    [MigrationDependsOn(typeof(Version_2015051300.UpdateSchema))]
    public sealed class UpdateSchema : Migration
    {
        public override void Up()
        {
            //this.Database.AddExportId("OVRHL_SPECIAL_ACCOUNT", FormatDataExportSequences.ContragentRschetExportId);
        }

        public override void Down()
        {
            //this.Database.RemoveExportId("OVRHL_SPECIAL_ACCOUNT");
        }
    }
}
