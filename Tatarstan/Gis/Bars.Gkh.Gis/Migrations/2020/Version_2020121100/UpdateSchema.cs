namespace Bars.Gkh.Gis.Migrations._2020.Version_2020121100
{
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;

    [MigrationDependsOn(typeof(Version_2020103000.UpdateSchema))]
    [Migration("2020121100")]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Execute("ALTER TABLE public.additionalinfo ALTER COLUMN epd_info TYPE CHARACTER VARYING(100);");
        }

        public override void Down()
        {
            this.Execute("ALTER TABLE public.additionalinfo ALTER COLUMN epd_info TYPE CHARACTER(40);");
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