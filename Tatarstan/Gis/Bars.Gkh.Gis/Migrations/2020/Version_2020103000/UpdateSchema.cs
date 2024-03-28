namespace Bars.Gkh.Gis.Migrations._2020.Version_2020103000
{
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;

    [MigrationDependsOn(typeof(Version_2020101600.UpdateSchema))]
    [Migration("2020103000")]
    public class UpdateSchema : Migration
    {
        private const string TableName = "public.parameters";
        private const string TenantFio = "tenant_fio";
        private const string TenantFioConstraint = "tenant_fio_notnull";

        /// <inheritdoc />
        public override void Up()
        {
            this.Execute($@"ALTER TABLE {TableName} ALTER COLUMN {TenantFio} DROP NOT NULL;");
            
            // Добавление условия на обязательность поля tenant_fio
            this.Execute($@"
                ALTER TABLE {TableName} ADD CONSTRAINT {TenantFioConstraint}
                CHECK ((COALESCE(show_full_name, 0) = 2 AND tenant_fio IS NULL) IS FALSE)
                NOT VALID;
            ");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Execute($"ALTER TABLE {TableName} ALTER COLUMN {TenantFio} SET NOT NULL;");

            this.Execute($"ALTER TABLE {TableName} DROP CONSTRAINT IF EXISTS {TenantFioConstraint};");
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