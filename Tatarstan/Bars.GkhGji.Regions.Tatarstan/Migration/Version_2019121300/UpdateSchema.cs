namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019121300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019121300")]
    [MigrationDependsOn(typeof(Version_2019112900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_EFFEC_PERF_INDEX_VALUE", new Column("VALUE", DbType.String));
        }
    }
}
