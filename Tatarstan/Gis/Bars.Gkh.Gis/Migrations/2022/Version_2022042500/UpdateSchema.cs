namespace Bars.Gkh.Gis.Migrations._2022.Version_2022042500
{
    using System.Linq;
    using System.Web;

    using Bars.B4.Application;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.Enum;

    using Dapper;

    using Microsoft.AspNetCore.Http;

    using Npgsql;

    [MigrationDependsOn(typeof(_2022.Version_2022041900.UpdateSchema))]
    [Migration("2022042500")]
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
                WHERE  app_url = '{ currentContext.Request.Host.Value}'
                       AND connection_type = {(int)ConnectionType.GisConnStringReports}";
            var connectionString = this.Database.Connection.Query<string>(sql).FirstOrDefault();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                
                sql = @"
                        do $$
                        declare
                            rec record;
                        begin
                            -- Не все таблицы наследуются от raj_nach
                            FOR rec IN SELECT tablename FROM pg_tables WHERE tablename ~ 'raj\d+_nach'
                            LOOP
                                EXECUTE 'ALTER TABLE gkh.'|| rec.tablename ||' ADD COLUMN IF NOT EXISTS type_dom integer;';
                            END LOOP;
                        end $$;";

                connection.Query(sql);
            }
        }
    }
}