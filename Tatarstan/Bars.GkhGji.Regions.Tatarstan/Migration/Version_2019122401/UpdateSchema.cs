namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019122401
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019122401")]
    [MigrationDependsOn(typeof(Version_2019122400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_DICT_EFFEC_PERF_INDEX",
                new Column("TOR_ID", DbType.Guid, ColumnProperty.Null));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICT_EFFEC_PERF_INDEX", "TOR_ID");
        }
    }
}
