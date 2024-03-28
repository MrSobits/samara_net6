namespace Bars.Gkh.Gis.Migrations._2020.Version_2020101600
{
    using Bars.B4.Application;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.DomainService.BilConnection;

    using Bars.B4.IoC;
    using Bars.Gkh.Gis.Enum;

    [MigrationDependsOn(typeof(Version_2020092400.UpdateSchema))]
    [Migration("2020101600")]
    public class UpdateSchema : Migration
    {
        private const string TableName = "public.additionalinfo";
        private const string InfoEpd = "epd_info";

        /// <inheritdoc />
        public override void Up()
        {
            // Добавление поля 25. Информационная часть ЕПД (Номер телефона контакт центра)
            this.Execute($@"
                ALTER TABLE {TableName} ADD COLUMN {InfoEpd} CHARACTER(40);
            ");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Execute($@"
                ALTER TABLE {TableName} DROP COLUMN {InfoEpd};
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