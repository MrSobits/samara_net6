namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111400
{
	using System.Data;

	using Bars.B4.Modules.Ecm7.Framework;
	using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019111400")]
	[MigrationDependsOn(typeof(Version_2019111301.UpdateSchema))]
	public class UpdateSchema : Migration
	{
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_DICT_EFFEC_PERF_INDEX",
                new RefColumn("CONTROL_ORG_ID", ColumnProperty.Null, "EFFEC_PERF_INDEX_CONTROL_ORG_ID", "GKH_CONTROL_ORGANIZATION", "ID"));

            this.Database.AddColumn("GKH_CONTROL_ORGANIZATION",
                new Column("TOR_ID", DbType.Guid, ColumnProperty.Null));

            this.Database.AddColumn("GJI_EFFEC_PERF_INDEX_VALUE",
                new Column("TOR_ID", DbType.Guid, ColumnProperty.Null));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICT_EFFEC_PERF_INDEX", "CONTROL_ORG_ID");
            this.Database.RemoveColumn("GKH_CONTROL_ORGANIZATION", "TOR_ID");
            this.Database.RemoveColumn("GJI_EFFEC_PERF_INDEX_VALUE", "TOR_ID");
        }
	}
}
