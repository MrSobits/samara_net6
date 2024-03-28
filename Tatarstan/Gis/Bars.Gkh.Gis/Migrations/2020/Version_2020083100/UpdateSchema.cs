namespace Bars.Gkh.Gis.Migrations._2020.Version_2020083100
{
    using Bars.B4.Application;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Gis.DomainService.BilConnection;

    using Bars.B4.IoC;
    using Bars.Gkh.Gis.Enum;

    [MigrationDependsOn(typeof(Version_2020062100.UpdateSchema))]
    [Migration("2020083100")]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Execute(@"
                ALTER TABLE public.executorinfo ADD COLUMN amount_exec NUMERIC(14,2) DEFAULT 0 NOT NULL; -- сумма к оплате по исполнителю

                ALTER TABLE public.charge ADD COLUMN service_cost NUMERIC(14,2); -- стоимость услуги
                ALTER TABLE public.charge ADD COLUMN energobil_accrual SMALLINT; -- начисление от Энергобиллинга
                ALTER TABLE public.charge ADD COLUMN cons_accrual SMALLINT; -- признак сводного начисления (с учётом настроек – КП 6.0)

                ALTER TABLE public.parameters ADD COLUMN penalty_epd SMALLINT; -- справочное отображение услуг Пени в ЕПД
                ALTER TABLE public.parameters ADD COLUMN energobil_address CHARACTER(100); -- Энергобиллинг: Адрес доставки
                ALTER TABLE public.parameters ADD COLUMN ps_info SMALLINT; -- информация об отображении сообщения для частного сектора
                ALTER TABLE public.parameters ADD COLUMN tenant_fio CHARACTER(100) DEFAULT '' NOT NULL; -- Ф.И.О. Квартиросъемщика / Собственника / Нанимателя (Сокращенное)
                ALTER TABLE public.parameters ADD COLUMN barcode NUMERIC(28,0); -- штрихкод квитанции
                ALTER TABLE public.parameters ADD COLUMN debt_overpay NUMERIC(14,2); -- долг (переплата) по пеням

                ALTER TABLE public.counters ADD COLUMN nonres_cost NUMERIC(14,5); -- расход по нежилым помещениям
                ALTER TABLE public.counters ADD COLUMN liv_cost NUMERIC(14,5); -- расход по жилым помещениям
                
                CREATE TABLE IF NOT EXISTS public.revalservices
                (
                    line_type     smallint default 10,     -- тип строки
                    dat_month     date           not null, -- год и месяц начисления
                    pkod          bigint         not null, -- платежный код (или номер лицевого счета)
                    serv_code     int            not null, -- код услуги
                    serv_name     character(100) not null, -- наименование услуги
                    reason_recalc character(100) not null, -- основание для перерасчета
                    sum_recalc    numeric(14, 2) not null  -- сумма перерасчета
                );
                CREATE INDEX ind_revalservices_pkod ON public.revalservices (pkod);
                ANALYZE public.revalservices;");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Execute(@"
                ALTER TABLE public.executorinfo DROP COLUMN amount_exec;
                ALTER TABLE public.charge DROP COLUMN service_cost;
                ALTER TABLE public.charge DROP COLUMN energobil_accrual;
                ALTER TABLE public.charge DROP COLUMN cons_accrual;
                ALTER TABLE public.parameters DROP COLUMN penalty_epd;
                ALTER TABLE public.parameters DROP COLUMN energobil_address;
                ALTER TABLE public.parameters DROP COLUMN ps_info;
                ALTER TABLE public.parameters DROP COLUMN tenant_fio;
                ALTER TABLE public.parameters DROP COLUMN barcode;
                ALTER TABLE public.parameters DROP COLUMN debt_overpay;
                ALTER TABLE public.counters DROP COLUMN nonres_cost;
                ALTER TABLE public.counters DROP COLUMN liv_cost;
                DROP TABLE IF EXISTS public.revalservices;");
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