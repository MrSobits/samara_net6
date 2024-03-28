namespace Bars.Gkh.Gis.Migrations._2022.Version_2022041900
{
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.Enum;

    using Dapper;

    using Microsoft.AspNetCore.Http;

    using Npgsql;

    [MigrationDependsOn(typeof(_2022.Version_2022020700.UpdateSchema))]
    [Migration("2022041900")]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            var httpContextAccessor = ApplicationContext.Current.Container.Resolve<IHttpContextAccessor>();
            var currentContext = httpContextAccessor.HttpContext;
            var sql = $@"
                SELECT connection
                FROM   bil_connection
                WHERE  app_url = '{currentContext.Request.Host.Value}'
                       AND connection_type = {(int)ConnectionType.GisConnStringReports}";
            var connectionString = this.Database.Connection.Query<string>(sql).FirstOrDefault();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                
                sql = @"CREATE TABLE IF NOT EXISTS gkh.raj_opl_serv
                        (
                            pref         char(10),
                            nzp_graj     integer,
                            nzp_headbank integer,
                            bank         char(100),
                            nzp_bank     integer,
                            nzp_pack_ls  integer,
                            pkod         numeric(10,0),
                            kod_sum      integer,
                            paysource    integer,
                            dat_uchet    date,
                            dat_vvod     date,
                            dat_month    date,
                            sum_oplat    numeric(14,2),
                            type_serv    integer
                        );
                        CREATE INDEX IF NOT EXISTS indx_ros_nzp_headbank ON gkh.raj_opl_serv (nzp_headbank);
                        CREATE INDEX IF NOT EXISTS indx_ros_nzp_bank ON gkh.raj_opl_serv (nzp_bank);
                        CREATE INDEX IF NOT EXISTS indx_ros_pkod ON gkh.raj_opl_serv (pkod);
                        CREATE INDEX IF NOT EXISTS indx_ros_dat_uchet ON gkh.raj_opl_serv (dat_uchet);
                        
                        ALTER TABLE gkh.raj_nach
                        ADD COLUMN IF NOT EXISTS type_dom integer;
                        ";

                connection.Query(sql);
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            var httpContextAccessor = ApplicationContext.Current.Container.Resolve<IHttpContextAccessor>();
            var currentContext = httpContextAccessor.HttpContext;
            var sql = $@"
                SELECT connection
                FROM   bil_connection
                WHERE  app_url = '{currentContext.Request.Host.Value}'
                       AND connection_type = {(int)ConnectionType.GisConnStringReports}";
            var connectionString = this.Database.Connection.Query<string>(sql).FirstOrDefault();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                connection.Query
                    (
                        @"DROP TABLE IF EXISTS gkh.raj_opl_serv; 
                        ALTER TABLE gkh.raj_nach 
                        DROP COLUMN type_dom;"
                    );
            }
        }
    }
}