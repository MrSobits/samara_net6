namespace Bars.Gkh.Gis.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using Castle.Windsor;
    using Entities.PersonalAccount;
    using House;

    /// <summary>
    /// Сервис для работы с данными о лицевом счете
    /// </summary>
    public class GisPersonalAccountService : IGisPersonalAccountService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public GisPersonalAccountService(IWindsorContainer container)
        {
            Container = container;
        }
        /// <summary>
        /// Получение параметров лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<PersonalAccountParam> GetPersonalAccountParams(long realityObjectId, long personalAccountId, DateTime date)
        {
            var house = Container.Resolve<IGisHouseService>("GisHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //собираем запрос для получения параметров лицевого счета
            var sqlQuery = String.Format(
                  " SELECT p.nzp_key AS Id, " +
                 " nzp AS HouseId, " +
                 " p.val_prm AS ValPrm, " +
                 " TRIM(pn.name_prm) AS Name, " +
                 " p.dat_s AS DateBegin, " +
                 " p.dat_po AS DateEnd  " +
                 " FROM {0}.prm_1 p, " +
                 " {0}.prm_name pn " +
                 " WHERE p.data_bank_id = {1} " +
                 " AND pn.data_bank_id = {1} " +
                 " AND p.report_month = '{2}' " +
                 " AND pn.report_month = '{2}' " +
                 " AND p.nzp_prm = pn.nzp_prm " +
                 " AND p.nzp = {3} ",
                 house.DataBankStorage.SchemaName,
                 house.DataBankStorage.DataBankId,
                 date.ToShortDateString(),
                 personalAccountId);

            //подключаемся к БД, где лежит дом и получаем его параметры
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<PersonalAccountParam>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение услуг лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<PersonalAccountService> GetPersonalAccountServices(long realityObjectId, long personalAccountId, DateTime date)
        {
            var house = Container.Resolve<IGisHouseService>("GisHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;
            
            //собираем запрос для получения услуг дома
            var sqlQuery = String.Format(
                " SELECT " +
                " c.id AS Id, " +
                " c.nzp_kvar AS ApartmentId, " +
                " p.npayer AS Supplier, " +
                " s.service AS Service, " +
                " f.name_frm AS Formula " +
                " FROM {0}.charge c  " +
                " INNER JOIN {0}.services s ON  " +
                " 	(s.nzp_serv = c.nzp_serv  " +
                " 		AND s.report_month = '{1}' " +
                " 		AND s.data_bank_id = {2}) " +
                " INNER JOIN {0}.supplier su ON  " +
                " 	(su.nzp_supp = c.nzp_supp  " +
                " 		AND su.report_month = '{1}' " +
                " 		AND su.data_bank_id = {2}) " +
                  " INNER JOIN {0}.s_payer p ON  " +
                " 	(p.nzp_payer = su.nzp_payer_supp  " +
                " 		AND p.report_month = '{1}' " +
                " 		AND p.data_bank_id = {2}) " +
                " INNER JOIN {0}.formuls f ON  " +
                " 	(f.nzp_frm = c.nzp_frm  " +
                " 		AND f.report_month = '{1}' " +
                " 		AND f.data_bank_id = {2}) " +
                " WHERE c.report_month = '{1}' " +
                " AND c.nzp_kvar =  {3} " +
                " AND c.data_bank_id = {2} ;",
                house.DataBankStorage.SchemaName,
                date.ToShortDateString(),
                house.DataBankStorage.DataBankId,
                personalAccountId);
            //подключаемся к БД, где лежит дом
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<PersonalAccountService>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение показаний приборов учета лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<PersonalAccountCounter> GetPersonalAccountCounterValues(long realityObjectId, long personalAccountId, DateTime date)
        {
            var house = Container.Resolve<IGisHouseService>("GisHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //подключаемся к БД, где лежит дом
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                var tempTableName = "tmp_dom_counter_val" + DateTime.Now.Ticks;

                var sqlQuery = " DROP TABLE IF EXISTS " + tempTableName;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = String.Format(
                    " CREATE TEMP TABLE {0} AS " +
                    " SELECT cc.id, sp.nzp as apartment_id, ss.service,  " +
                    " sp.num_cnt , cc.dat_uchet, cc.val_cnt,  " +
                    " sp.name_type, sp.nzp_counter,  " +
                    " '1900-1-1'::date as prev_dat_uchet, " +
                    " 0::real as prev_val_cnt " +
                    " FROM {1}.counters_spis sp, {1}.counters cc, {1}.services ss " +
                    " WHERE sp.nzp_counter = cc.nzp_counter " +
                    " AND sp.nzp_type = 3 " +
                    " AND sp.nzp_serv = ss.nzp_serv " +
                    " AND sp.nzp = {4} " +
                    " AND sp.data_bank_id = {2} AND sp.report_month = '{3}' " +
                    " AND cc.data_bank_id = {2} AND cc.report_month = '{3}' " +
                    " AND ss.data_bank_id = {2} AND ss.report_month = '{3}' " +
                    " AND cc.dat_uchet = '{3}'; ",
                    tempTableName,
                    house.DataBankStorage.SchemaName,
                    house.DataBankStorage.DataBankId,
                    date.ToShortDateString(),
                    personalAccountId);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = String.Format(
                    " UPDATE {0} t SET " +
                    " prev_dat_uchet =  " +
                    " (SELECT max(a.dat_uchet)  " +
                    " FROM {1}.counters a  " +
                    " WHERE a.nzp_counter = t.nzp_counter  " +
                    " AND a.dat_uchet < t.dat_uchet " +
                    " AND a.data_bank_id = {2}  " +
                    " AND a.report_month = '{3}'); ",
                    tempTableName,
                    house.DataBankStorage.SchemaName,
                    house.DataBankStorage.DataBankId,
                    date.ToShortDateString());
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = String.Format(
                    " UPDATE {0} t SET " +
                    " prev_val_cnt =  " +
                    " (SELECT a.val_cnt  " +
                    " FROM {1}.counters a  " +
                    " WHERE a.nzp_counter = t.nzp_counter  " +
                    " AND a.dat_uchet = t.prev_dat_uchet  " +
                    " AND t.prev_dat_uchet is not null " +
                    " AND a.data_bank_id = {2} " +
                    " AND a.report_month = '{3}'); ",
                    tempTableName,
                    house.DataBankStorage.SchemaName,
                    house.DataBankStorage.DataBankId,
                    date.ToShortDateString());
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT id AS Id, " +
                    " apartment_id AS ApartmentId, " +
                    " service AS Service, " +
                    " num_cnt AS CounterNumber, " +
                    " name_type AS CounterType, " +
                    " dat_uchet AS StatementDate, " +
                    " val_cnt AS CounterValue, " +
                    " prev_dat_uchet AS PrevStatementDate, " +
                    " prev_val_cnt AS PrevCounterValue " +
                    " FROM  " + tempTableName;

                return sqlExecutor.ExecuteSql<PersonalAccountCounter>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение начислений лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<PersonalAccountAccruals> GetPersonalAccountAccruals(long realityObjectId, long personalAccountId, DateTime date)
        {
            var house = Container.Resolve<IGisHouseService>("GisHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //собираем запрос для получения начислений лицевого счета
            var sqlQuery = String.Format(
                " SELECT " +
                " c.id AS Id, " +
                " c.nzp_kvar AS ApartmentId,   " +
                " s.service AS  Service,   " +
                " p.npayer AS  Supplier, " +
                " SUM(c.sum_insaldo) AS BalanceIn,   " +
                " SUM(c.rsum_tarif) AS TariffAmount,   " +
                " SUM(c.sum_nedop) AS BackorderAmount,   " +
                " SUM(c.sum_tarif_p) AS RecalcAmount,   " +
                //" SUM(c.money_to) AS ErcAmount,   " + 
                //" SUM(c.money_from) AS SupplierAmount,   " + 
                " SUM(c.sum_outsaldo) AS BalanceOut  " +
                " FROM {0}.charge c  " +
                " INNER JOIN {0}.services s ON (s.data_bank_id = {1} AND s.report_month = '{2}' AND s.nzp_serv = c.nzp_serv) " +
                " INNER JOIN {0}.supplier supp ON (s.data_bank_id = {1} AND s.report_month = '{2}' AND supp.nzp_supp = c.nzp_supp) " +
                " INNER JOIN {0}.s_payer p ON (s.data_bank_id = {1} AND s.report_month = '{2}' AND  p.nzp_payer = supp.nzp_payer_supp) " +
                " WHERE c.nzp_kvar =  {3} " +
                " AND c.data_bank_id = {1} AND c.report_month = '{2}' " +
                " GROUP BY 1,2,3,4 ",
                house.DataBankStorage.SchemaName,
                house.DataBankStorage.DataBankId,
                date.ToShortDateString(),
                personalAccountId);

            //подключаемся к БД, где лежит дом
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<PersonalAccountAccruals>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение собственников лицевого счета
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="personalAccountId">Код лицевого счета</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<PersonalAccountOwner> GetPersonalAccountOwners(long realityObjectId, long personalAccountId, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
