namespace Bars.Gkh.Gis.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Windsor;
    using Entities.PersonalAccount;
    using House;

    /// <summary>
    /// Сервис для работы с данными о лицевом счете
    /// </summary>
    public class KpPersonalAccountService : IGisPersonalAccountService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public KpPersonalAccountService(IWindsorContainer container)
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
            var house = Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //собираем запрос для получения параметров лицевого счета
            var sqlQuery = String.Format(
                " SELECT " +
            "    nzp_key AS Id, " +
            "    {1} AS ApartmentId, " +
            "    TRIM(n.name_prm) AS Name, " +
            "    TRIM(n.type_prm) AS Type, " +
            "    n.nzp_res AS NzpRes, " +
            "    n.prm_num AS PrmNum, " +
            "    p.nzp_prm AS PrmCode," + 
            "    TRIM(p.val_prm) AS ValPrm, " +
            "    TRIM('prm_' || n.prm_num) AS NameTab, " +
            "    p.dat_s AS DateBegin, " +
            "    p.dat_po AS DateEnd " +
            " FROM {0}_data.prm_1 p, {0}_kernel.prm_name n " +
            " WHERE p.nzp_prm = n.nzp_prm " +
            "   AND p.nzp = {1} " +
            "   AND p.is_actual = 1 ",
                house.DataBankStorage.SchemaName.Trim(),
                personalAccountId);

            //подключаемся к БД, где лежит лицевой счет
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
            var house = Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;

           
            //подключаемся к БД, где лежит лицевой счет
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                var chargeTableFullName =
                    String.Format(
                        " {0}_charge_{1}.charge_{2} ",
                        house.DataBankStorage.SchemaName.Trim(),
                        (date.Year%100).ToString("00"),
                        date.Month.ToString("00"));

                if (
                    sqlExecutor.ExecuteSql<int>(
                        String.Format(
                            "SELECT  count(*) FROM information_schema.tables WHERE TRIM(table_schema)||'.'||TRIM(table_name) = '{0}' ",
                            chargeTableFullName.Trim())).First() == 0)
                {
                    return null;
                }

                //собираем запрос для получения услуг лицевого счета
                var sqlQuery = String.Format(
                   " SELECT " +
                    " MAX(c.nzp_charge) AS Id, " +
                    " {2} AS ApartmentId, " +
                    " s.service AS Service," +
                    " su.name_supp AS Supplier," +
                    " f.name_frm AS Tariff" +
                    " FROM {0} c " +
                    " INNER JOIN {1}_data.kvar k ON k.nzp_kvar = c.nzp_kvar  " +
                    " INNER JOIN {1}_kernel.services s ON s.nzp_serv = c.nzp_serv " +
                    " INNER JOIN {1}_kernel.supplier su ON su.nzp_supp = c.nzp_supp " +
                    " INNER JOIN {1}_kernel.formuls f ON f.nzp_frm = c.nzp_frm " +
                    " WHERE k.nzp_kvar =  {2} " +
                    " GROUP BY 2,3,4,5 ",
                    chargeTableFullName,
                    house.DataBankStorage.SchemaName.Trim(),
                    personalAccountId);

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
            var house = Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;
            
            //подключаемся к БД, где лежит лицевой счет
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {

                var tempTableName = "tmp_dom_counter_val" + DateTime.Now.Ticks;

                var sqlQuery = " DROP TABLE IF EXISTS " + tempTableName;
                sqlExecutor.ExecuteSql(sqlQuery);


                sqlQuery = String.Format(
                   " CREATE TEMP TABLE {0} AS " +
                   " SELECT cc.nzp_cr as id, sp.nzp as apartment_id, ss.service,  ss.nzp_serv AS service_id,  " +
                   " sp.num_cnt , cc.dat_uchet, cc.val_cnt,  " +
                   " s1.name_type, sp.nzp_counter,  " +
                   " '1900-1-1'::date as prev_dat_uchet, " +
                   " 0::real as prev_val_cnt, " +
                   " TRUE AS ManualModeMetering, " +
                   " cc.dat_when AS ReadoutDate " +
                   " FROM {1}_data.counters_spis sp, {1}_data.counters cc, " +
                   " {1}_kernel.services ss, {1}_kernel.s_counttypes s1 " +
                   " WHERE sp.nzp_counter = cc.nzp_counter " +
                   " AND sp.nzp_type = 3 " +
                   " AND sp.nzp_serv = ss.nzp_serv " +
                   " AND cc.nzp_cnttype = s1.nzp_cnttype  " +
                   " AND sp.nzp = {2} " +
                   " AND cc.dat_uchet = '{3}'; ",
                   tempTableName,
                   house.DataBankStorage.SchemaName.Trim(),
                   personalAccountId,
                   date.ToShortDateString()
                   );
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = String.Format(
                   " UPDATE {0} t SET " +
                   " prev_dat_uchet =  " +
                   " (SELECT max(a.dat_uchet)  " +
                   " FROM {1}_data.counters a  " +
                   " WHERE a.nzp_counter = t.nzp_counter  " +
                   " AND a.dat_uchet < t.dat_uchet); ",
                   tempTableName,
                   house.DataBankStorage.SchemaName.Trim());
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = String.Format(
                    " UPDATE {0} t SET " +
                    " prev_val_cnt =  " +
                    " (SELECT a.val_cnt  " +
                    " FROM {1}_data.counters a  " +
                    " WHERE a.nzp_counter = t.nzp_counter  " +
                    " AND a.dat_uchet = t.prev_dat_uchet  " +
                    " AND t.prev_dat_uchet is not null); ",
                    tempTableName,
                    house.DataBankStorage.SchemaName.Trim());
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT id AS Id, " +
                    " apartment_id AS ApartmentId, " +
                    " service AS Service, " +
                    " service_id AS ServiceId, " +
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
            var house = Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;


            //подключаемся к БД, где лежит лицевой счет
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                var chargeTableFullName =
                    String.Format(
                        " {0}_charge_{1}.charge_{2} ",
                        house.DataBankStorage.SchemaName.Trim(),
                        (date.Year % 100).ToString("00"),
                        date.Month.ToString("00"));

                if (
                    sqlExecutor.ExecuteSql<int>(
                        String.Format(
                            "SELECT  count(*) FROM information_schema.tables WHERE TRIM(table_schema)||'.'||TRIM(table_name) = '{0}' ",
                            chargeTableFullName.Trim())).First() == 0)
                {
                    return null;
                }

                //собираем запрос для получения начислений лицевого счета
                var sqlQuery = String.Format(
                   " SELECT " +
                        " MAX(c.nzp_charge) AS Id, " +
                        " {2} AS ApartmentId, " +
                        " s.service AS  Service, " +
                        " s.nzp_serv AS  ServiceId, " +
                        " su.name_supp AS  Supplier, " +
                        " su.nzp_supp AS  SupplierId, " +
                        " SUM(sum_insaldo) AS BalanceIn, " +
                        " SUM(rsum_tarif) AS TariffAmount, " +
                        " SUM(sum_nedop) AS BackorderAmount, " +
                        " SUM(sum_subsidy) AS Subsidy, " +
                        " SUM(CASE WHEN tarif != 0 THEN rsum_tarif/tarif  ELSE  0 END) AS Volume,  " +
                        " SUM(reval) AS RecalcAmount, " +
                        " SUM(money_to) AS ErcAmount, " +
                        " SUM(money_from) AS SupplierAmount, " +
                        " SUM(sum_outsaldo) AS BalanceOut " +
                        " FROM {0} c " +
                        " INNER JOIN {1}_data.kvar k ON k.nzp_kvar = c.nzp_kvar  " +
                        " INNER JOIN {1}_kernel.services s ON s.nzp_serv = c.nzp_serv " +
                        " INNER JOIN {1}_kernel.supplier su ON su.nzp_supp = c.nzp_supp " +
                        " WHERE k.nzp_kvar =  {2} " +
                        " GROUP BY 2,3,4,5,6 ",
                    chargeTableFullName,
                    house.DataBankStorage.SchemaName.Trim(),
                    personalAccountId);

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
            var house = Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //собираем запрос для получения параметров лицевого счета
            var sqlQuery =
                $@" SELECT nzp_sobstw AS ""Id"",
                nzp_kvar AS ""ApartmentId"",
                TRIM(fam) AS ""Surname"",
                TRIM(ima) AS ""Name"",
                TRIM(otch) AS ""Patronymic"",
                dat_rog AS ""BithDate"",
                d.nzp_dok AS ""IdentityDocumentId"",
                d.dok AS ""IdentityDocument"",
                TRIM(serij) AS ""IdentityDocumentSeries"",
                TRIM(nomer) AS ""IdentityDocumentNumber"",
                vid_dat AS ""IdentityDocumentIssuedDate""
                FROM {house.DataBankStorage.SchemaName.Trim()}_data.sobstw s
                LEFT OUTER JOIN n3_data.s_dok d ON (d.nzp_dok = s.nzp_dok)
                WHERE nzp_kvar = {personalAccountId}";

            //подключаемся к БД, где лежит лицевой счет
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<PersonalAccountOwner>(sqlQuery);
            }
        }
    }
}
