namespace Bars.Gkh.Gis.Migrations._2021.Version_2021012700
{
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;

    [MigrationDependsOn(typeof(_2020.Version_2020121100.UpdateSchema))]
    [Migration("2021012700")]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Execute(@"
                ALTER TABLE public.executorinfo ALTER COLUMN em_phone TYPE CHARACTER (200);
                ALTER TABLE public.additionalinfo ALTER COLUMN sector_phone TYPE CHARACTER(100);");
        }

        public override void Down()
        {
            this.Execute(@"
                ALTER TABLE public.executorinfo ALTER COLUMN em_phone TYPE CHARACTER (40);
                ALTER TABLE public.additionalinfo ALTER COLUMN sector_phone TYPE CHARACTER(40);");
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