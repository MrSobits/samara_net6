namespace Bars.Gkh.Gis.Migrations._2020.Version_2020062100
{
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;

    [MigrationDependsOn(typeof(_2017.Version_2017101300.UpdateSchema))]
    [Migration("2020062100")]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Execute(@"
                ALTER TABLE public.counters ADD CONSTRAINT num_cnt_notnull
                CHECK ((virtual_pu != 1 AND num_cnt IS NULL) IS FALSE)
                NOT VALID;");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Execute(@"
                ALTER TABLE public.counters DROP CONSTRAINT IF EXISTS num_cnt_notnull;");
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
