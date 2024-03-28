namespace Bars.Gkh.Gis.Migrations._2020.Version_2020092400
{
    using Bars.B4.Application;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.DomainService.BilConnection;

    using Bars.B4.IoC;
    using Bars.Gkh.Gis.Enum;

    [MigrationDependsOn(typeof(Version_2020083100.UpdateSchema))]
    [Migration("2020092400")]
    public class UpdateSchema : Migration
    {
        private const string TableName = "public.additionalinfo";
        private const string SectorName = "sector_name";
        private const string SectorAddress = "sector_address";
        private const string SectorPhone = "sector_phone";

        /// <inheritdoc />
        public override void Up()
        {
            this.Execute($@"
                ALTER TABLE {TableName} ADD COLUMN {SectorName} CHARACTER(100); -- Наименование территориального участка
                ALTER TABLE {TableName} ADD COLUMN {SectorAddress} CHARACTER(200); -- Адрес территориального участка
                ALTER TABLE {TableName} ADD COLUMN {SectorPhone} CHARACTER(40); -- Телефон территориального участка
            ");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Execute($@"
                ALTER TABLE {TableName} DROP COLUMN {SectorName};
                ALTER TABLE {TableName} DROP COLUMN {SectorAddress};
                ALTER TABLE {TableName} DROP COLUMN {SectorPhone};
            ");
        }

        private void Execute(string sqlQuery)
        {
            var container = ApplicationContext.Current.Container;
            var bilConnectionService = container.Resolve<IBilConnectionService>();

            using (container.Using(bilConnectionService))
            {
                using (var sqlExecutor = new SqlExecutor.SqlExecutor(bilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
                {
                    sqlExecutor.ExecuteSql(sqlQuery);
                }
            }
        }
    }

}