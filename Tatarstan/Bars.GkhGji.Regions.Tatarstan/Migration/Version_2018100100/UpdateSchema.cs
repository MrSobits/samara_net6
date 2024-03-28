namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018100100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018100100")]
    [MigrationDependsOn(typeof(Version_2018070500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GJI_TATARSTAN_RESOLUTION", "CHANGE_REASON", DbType.String, 255);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_TATARSTAN_RESOLUTION", "CHANGE_REASON");
        }
    }
}