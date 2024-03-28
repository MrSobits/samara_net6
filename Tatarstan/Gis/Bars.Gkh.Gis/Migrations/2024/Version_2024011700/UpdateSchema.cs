namespace Bars.Gkh.Gis.Migrations._2024.Version_2024011700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.Enum;
    using Dapper;
    using Npgsql;
    using System.Linq;
    using System.Web;

    using Bars.B4.Application;

    using Microsoft.AspNetCore.Http;

    [MigrationDependsOn(typeof(_2022.Version_2022042500.UpdateSchema))]
    [Migration("2024011700")]
    public class UpdateSchema : Migration
    {
        private readonly string table = "gkh.raj_opl_serv";
        private const string newColumn = "transfer";

        /// <inheritdoc />
        public override void Up()
        {
            using (var connection = new NpgsqlConnection(this.GetConnectionString()))
            {
                connection.Open();
                connection.Query($"ALTER TABLE {table} ADD COLUMN {newColumn} integer");
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            using (var connection = new NpgsqlConnection(this.GetConnectionString()))
            {
                connection.Open();
                connection.Query($"ALTER TABLE {table} DROP COLUMN {newColumn}");
            }
        }

        /// <summary>
        /// Получить строку подключения для загрузчика stat_opl
        /// </summary>
        /// <returns>Строка подключения</returns>
        private string GetConnectionString()
        {
            var httpContextAccessor = ApplicationContext.Current.Container.Resolve<IHttpContextAccessor>();
            var currentContext = httpContextAccessor.HttpContext;
            var sql = $@"
                SELECT connection
                FROM bil_connection
                WHERE app_url = '{currentContext.Request.Host.Value}'
                    AND connection_type = {(int)ConnectionType.GisConnStringReports}";
            return this.Database.Connection.Query<string>(sql).FirstOrDefault();
        }
    }
}