namespace Bars.Gkh.Gis.DomainService.House.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using BilConnection;
    using Enum;
    using Castle.Windsor;
    using Entities.House;
    using Entities.ImportAddressMatching;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Entities.PersonalAccount;
    using Gkh.Entities;

    /// <summary>
    /// Сервис для работы с данными о доме
    /// </summary>
    public class GisHouseService : IGisHouseService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container;

        /// <summary>
        /// Строки соединения к серверам БД биллинга
        /// </summary>
        public IBilConnectionService BilConnectionService { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public GisHouseService(IWindsorContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Получение информации о хранилище дома
        /// </summary>
        public GisHouseProxy GetHouseStorage(long realityObjectId)
        {
            var house = new GisHouseProxy();
            var houseInfo = (
                //таблица bil_import_address
                from ia in Container.Resolve<IRepository<ImportedAddressMatch>>().GetAll()
                join fa in Container.Resolve<IRepository<FiasAddressUid>>().GetAll() on ia.FiasAddress.Address.Id equals
                    fa.Address.Id
                join ro in Container.Resolve<IRepository<RealityObject>>().GetAll() on fa.Address.Id equals
                    ro.FiasAddress.Id
                where ro.Id == realityObjectId
                select ia).FirstOrDefault();

            //если данные не найдены
            if (houseInfo == null || houseInfo.DataBankId <= 0) return null;

            //подключаемся к мастер-БД, чтоб найти хранилище дома
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringInc)))
            {
                //получаем строку соединения к хранилищу, где находится дом
                var sqlQuery = String.Format(
                    " SELECT {0} AS DataBankId, TRIM(store.pref) AS SchemaName, TRIM(db.db_connect) AS ConnectionString " +
                    " FROM master.data_bank b, " +
                    " master.data_storage store, " +
                    " master.data_base db " +
                    " WHERE b.data_bank_id = {0} " +
                    " AND store.data_storage_id = b.data_storage_id " +
                    " AND store.db_id = db.db_id; ",
                    houseInfo.DataBankId);
                house.DataBankStorage = sqlExecutor.ExecuteSql<DataBankStogare>(sqlQuery).FirstOrDefault();
                house.Id = houseInfo.HouseCode;
            }

            return house.DataBankStorage == null ? null : house;
        }

        /// <summary>
        /// Получение параметров дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<HouseParam> GetHouseParams(long realityObjectId, DateTime date)
        {
            var house = GetHouseStorage(realityObjectId);
            if (house == null) return null;


            //собираем запрос для получения параметров дома
            var sqlQuery = String.Format(
                 " SELECT p.nzp_key AS Id, " +
                " nzp AS HouseId, " +
                " p.val_prm AS ValPrm, " +
                " TRIM(pn.name_prm) AS Name, " +
                " p.dat_s AS DateBegin, " +
                " p.dat_po AS DateEnd  " +
                " FROM {0}.prm_2 p, " +
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
                house.Id);

            //подключаемся к БД, где лежит дом и получаем его параметры
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<HouseParam>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение услуг дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<HouseService> GetHouseServices(long realityObjectId, DateTime date)
        {
            var house = GetHouseStorage(realityObjectId);
            if (house == null) return null;
            
            //собираем запрос для получения услуг дома
            var sqlQuery = String.Format(
                " SELECT " +
                " c.id AS Id, " +
                " k.nzp_dom AS HouseId, " +
                " p.npayer AS Supplier, " +
                " s.service AS Service, " +
                " f.name_frm AS Formula, " +
                " COUNT(DISTINCT c.nzp_kvar) AS  LsCount  " +
                " FROM {0}.charge c  " +
                " INNER JOIN {0}.kvar k ON  " +
                " 	(k.nzp_kvar = c.nzp_kvar   " +
                " 		AND k.report_month = '{1}' " +
                " 		AND k.data_bank_id = {2}) " +
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
                " AND c.data_bank_id = {2} " +
                " AND k.nzp_dom =  {3} " +
                " GROUP BY 1,2,3,4,5; ",
                house.DataBankStorage.SchemaName,
                date.ToShortDateString(),
                house.DataBankStorage.DataBankId,
                house.Id);

            //подключаемся к БД, где лежит дом
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<HouseService>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение показаний приборов учета дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<HouseCounter> GetHouseCounterValues(long realityObjectId, DateTime date)
        {
            var house = GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //подключаемся к БД, где лежит дом
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                var tempTableName = "tmp_dom_counter_val" + DateTime.Now.Ticks;

                var sqlQuery = " DROP TABLE IF EXISTS " + tempTableName;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = String.Format(
                    " CREATE TEMP TABLE {0} AS " +
                    " SELECT cc.id, sp.nzp as house_id, ss.service,  " +
                    " sp.num_cnt , cc.dat_uchet, cc.val_cnt,  " +
                    " sp.name_type, sp.nzp_counter,  " +
                    " '1900-1-1'::date as prev_dat_uchet, " +
                    " 0::real as prev_val_cnt " +
                    " FROM {1}.counters_spis sp, {1}.counters cc, {1}.services ss " +
                    " WHERE sp.nzp_counter = cc.nzp_counter " +
                    " AND sp.nzp_type = 1 " +
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
                    house.Id);
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
                    " house_id AS HouseId, " +
                    " service AS Service, " +
                    " num_cnt AS CounterNumber, " +
                    " name_type AS CounterType, " +
                    " dat_uchet AS StatementDate, " +
                    " val_cnt AS CounterValue, " +
                    " prev_dat_uchet AS PrevStatementDate, " +
                    " prev_val_cnt AS PrevCounterValue " +
                    " FROM  " + tempTableName;
                
                return sqlExecutor.ExecuteSql<HouseCounter>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение начислений дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<HouseAccruals> GetHouseAccruals(long realityObjectId, DateTime date)
        {
            var house = GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //собираем запрос для получения начислений дома
            var sqlQuery = String.Format(
                " SELECT  " +
                " c.id AS Id,   " +
                " k.nzp_dom AS HouseId,   " +
                " s.service AS  Service,   " +
                " SUM(c.sum_insaldo) AS BalanceIn,   " +
                " SUM(c.rsum_tarif) AS TariffAmount,   " +
                " SUM(c.sum_nedop) AS BackorderAmount,   " +
                " SUM(c.sum_tarif_p) AS RecalcAmount,   " +
                //" SUM(c.money_to) AS ErcAmount,   " + 
                //" SUM(c.money_from) AS SupplierAmount, " +   
                " SUM(c.sum_outsaldo) AS BalanceOut  " +
                " FROM {0}.charge c  " +
                " INNER JOIN {0}.kvar k ON (k.data_bank_id = {1} AND k.report_month = '{2}' AND k.nzp_kvar = c.nzp_kvar) " +
                " INNER JOIN {0}.services s ON (s.data_bank_id = {1} AND s.report_month = '{2}' AND s.nzp_serv = c.nzp_serv) " +
                " WHERE k.nzp_dom =  {3} " +
                " AND c.data_bank_id = {1} AND c.report_month = '{2}' " +
                " GROUP BY 1,2,3 ",
                house.DataBankStorage.SchemaName,
                house.DataBankStorage.DataBankId,
                date.ToShortDateString(),
                house.Id);

            //подключаемся к БД, где лежит дом
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<HouseAccruals>(sqlQuery);
            }
        }

        /// <summary>
        /// Получение лицевых счетов дома
        /// </summary>
        /// <param name="realityObjectId">Код дома в системе МЖФ</param>
        /// <param name="date">Дата, за которую надо получить данные (округлено до месяца)</param>
        /// <returns></returns>
        public IEnumerable<GisPersonalAccount> GetHousePersonalAccounts(long realityObjectId, DateTime date)
        {
            var house = GetHouseStorage(realityObjectId);
            if (house == null) return null;

            //собираем запрос для получения лицевых счетов дома
            var sqlQuery = String.Format(
                " SELECT " +
                " {4} AS RealityObjectId, " +
                " kv.nzp_kvar AS Id, " +
                " kv.nzp_dom AS HouseId, " +
                " kv.nzp_kvar AS AccountId, " +
                " NULL AS PSS, " +
                " NULL AS PaymentCode, " +
                " kv.nkvar AS ApartmentNumber, " +
                " kv.nkvar_n AS RoomNumber, " +
                " '{0}' AS Prefix, " +
                " CASE WHEN p3.val_prm = '1' " +
                "       THEN 1 ELSE 0 END AS IsOpen, " +
                " kv.fio AS Tenant, " +
                " kv.nzp_kvar AS AccountNumber, " +
                " p3.dat_s AS DateBegin, " +
                " p3.dat_po AS DateEnd " +
                " FROM " +
                " {0}.kvar kv " +
                //параметр "Состояние ЛС (открыт, закрыт)" 
                " LEFT OUTER JOIN {0}.prm_name p ON ( p.data_bank_id = {1} AND p.report_month = '{2}' AND p.nzp_prm_base = 51) " +
                " LEFT OUTER JOIN {0}.prm_3 p3 ON ( p3.data_bank_id = {1} AND p3.report_month = '{2}' AND  kv.nzp_kvar = p3.nzp AND p3.nzp_prm = p.nzp_prm) " +
                " WHERE " +
                " kv.nzp_dom = {3} " +
                " AND kv.data_bank_id = {1} AND kv.report_month = '{2}' ",
                house.DataBankStorage.SchemaName,
                house.DataBankStorage.DataBankId,
                date.ToShortDateString(),
                house.Id,
                realityObjectId);

            //подключаемся к БД, где лежит дом
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(house.DataBankStorage.ConnectionString))
            {
                return sqlExecutor.ExecuteSql<GisPersonalAccount>(sqlQuery);
            }
        }
    }
}

