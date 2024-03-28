namespace Bars.Gkh.Regions.Tatarstan.Migrations._2023.Version_2023010900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2023010900")]
    [MigrationDependsOn(typeof(_2022.Version_2022052600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GKH_TAT_TARIFF_DATA_INTEGRATION_LOG";

        public override void Up()
        {
            this.Database.AddEntityTable(TableName,
                new Column("TARIFF_DATA_INTEGRATION_METHOD", DbType.Int32, ColumnProperty.NotNull),
                new Column("START_METHOD_TIME", DbType.DateTime),
                new Column("PARAMETERS", DbType.String),
                new Column("EXECUTION_STATUS", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("B4_USER_ID", "TARIFF_DATA_INTEGRATION_LOG_B4_USER", "B4_USER", "ID"),
                new FileColumn("LOG_FILE_ID", ColumnProperty.Null, "ETARIFF_DATA_INTEGRATION_LOG_FILE_LOG"));
        }
    }
}