namespace Bars.Gkh.Gis.DomainService.House.Impl
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities.House;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Entities.Kp50;
    using Entities.PersonalAccount;
    using Gkh.Entities;

    /// <summary>
    /// Сервис для работы с данными о доме, 
    /// загруженном из ППП "Коммунальные платежи"
    /// </summary>
    public class KpHouseService : IGisHouseService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public KpHouseService(IWindsorContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Получение информации о хранилище дома
        /// </summary>
        public GisHouseProxy GetHouseStorage(long realityObjectId)
        {
            var house = new GisHouseProxy();
            try
            {
                //ищем хранилище дома
                var houseStorage = (
                    from s in Container.Resolve<IRepository<BilDictSchema>>().GetAll()
                    join l in Container.Resolve<IRepository<BilHouseCodeStorage>>().GetAll()
                        on s.Id equals l.Schema.Id
                    where
                        l.BillingHouseCode ==
                        Convert.ToInt64(Container.Resolve<IRepository<RealityObject>>().Get(realityObjectId).CodeErc)
                    select
                        new {s.Id, s.CentralSchemaPrefix, s.LocalSchemaPrefix, s.ConnectionString, l.BillingHouseCode})
                    .SingleOrDefault();

                using (var sqlExecutor = new SqlExecutor.SqlExecutor(houseStorage.ConnectionString))
                {
                    //ищем внутренний код дома в разрезе одной БД (nzp_dom)
                    var sqlQuery = String.Format(
                        " SELECT DISTINCT CAST(nzp AS INTEGER)  AS HouseId " +
                        " FROM {0}_data.prm_4 " +
                        " WHERE nzp_prm = 890 " +
                        " AND TRIM(val_prm) = '{1}'",
                        houseStorage.LocalSchemaPrefix.Trim(),
                        houseStorage.BillingHouseCode);

                    house.Id = sqlExecutor.ExecuteSql<long>(sqlQuery).Single();
                }

                house.DataBankStorage = new DataBankStogare
                {
                    SchemaName = houseStorage.LocalSchemaPrefix,
                    ConnectionString = houseStorage.ConnectionString,
                    DataBankId = 0
                    };
                return house;
            }
            catch
            {
                //не нашли хранилище дома
                return null;
            }
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
                " SELECT " +
                "    nzp_key AS Id, " +
                " 	 {1} AS HouseId, " +
                "    TRIM(n.name_prm) AS Name, " +
                "    TRIM(n.type_prm) AS Type, " +
                "    n.nzp_res AS NzpRes, " +
                "    n.prm_num AS PrmNum, " +
                "    TRIM(p.val_prm) AS ValPrm, " +
                "    TRIM('prm_' || n.prm_num) AS NameTab, " +
                "    p.dat_s AS DateBegin, " +
                "    p.dat_po AS DateEnd " +
                " FROM {0}_data.prm_2 p, {0}_kernel.prm_name n " +
                " WHERE p.nzp_prm = n.nzp_prm " +
                "   AND p.nzp = {1} " +
                "   AND p.is_actual = 1 ",
                house.DataBankStorage.SchemaName.Trim(),
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


            //подключаемся к БД, где лежит дом
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

                //получаем услуги дома
                var sqlQuery = String.Format(
                    " SELECT " +
                     " MAX(s.nzp_serv) AS Id, " +
                    " k.nzp_dom AS HouseId, " +
                    " su.name_supp AS Supplier," +
                    " s.service AS Service," +
                    " f.name_frm AS Formula," +
                    " COUNT(DISTINCT c.num_ls) AS  LsCount " +
                    " FROM {0} c " +
                    " INNER JOIN {1}_data.kvar k ON k.nzp_kvar = c.nzp_kvar  " +
                    " INNER JOIN {1}_kernel.services s ON s.nzp_serv = c.nzp_serv " +
                    " INNER JOIN {1}_kernel.supplier su ON su.nzp_supp = c.nzp_supp " +
                    " INNER JOIN {1}_kernel.formuls f ON f.nzp_frm = c.nzp_frm " +
                    " WHERE k.nzp_dom =  {2} " +
                    " GROUP BY 2,3,4,5 ",
                    chargeTableFullName,
                    house.DataBankStorage.SchemaName.Trim(),
                    house.Id);
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
                //получаем приборы учета дома
                var sqlQuery =
                    String.Format(
                        " SELECT " +
                        "  a.nzp_crd AS Id, " +
                        "  {1} AS HouseId, " +
                        "  s.service as Service,  " +
                        "  s.nzp_serv as ServiceId,  " +
                        //единица измерения
                        "  s.ed_izmer," +
                        //код услуги
                        "  a.nzp_serv,  " +
                        "  a.num_cnt AS CounterNumber,  " +
                        "  s1.name_type AS CounterType,  " +
                        "  TRUE AS ManualModeMetering, " +
                        "  a.dat_when AS ReadoutDate, " +
                        "  CAST(a.dat_uchet AS DATE) AS StatementDate,  " +
                        "  a.val_cnt AS CounterValue,  " +
                        "  CAST(b.dat_uchet AS DATE) AS PrevStatementDate,  " +
                        "  b.val_cnt AS PrevCounterValue " +
                        " FROM {0}_kernel.services s, " +
                        "   {0}_data.counters_dom a  " +
                        " LEFT OUTER JOIN {0}_data.counters_dom b ON  " +
                        " 	(a.nzp_dom=b.nzp_dom AND a.nzp_serv=b.nzp_serv AND a.num_cnt=b.num_cnt AND a.is_actual=b.is_actual),  " +
                        "  {0}_kernel.s_counttypes s1 " +
                        "  WHERE a.nzp_serv=s.nzp_serv AND a.nzp_cnttype=s1.nzp_cnttype " +
                        "  AND a.is_gkal = 0 " +
                        "  AND b.is_gkal = 0 " +
                        "  AND a.nzp_cnttype=b.nzp_cnttype  " +
                        "  AND a.dat_uchet<=mdy({2},1,{3}) " +
                        "  AND COALESCE(a.dat_prov,mdy(1,1,1980))<>mdy(1,1,1990)  " +
                        "  AND COALESCE(b.dat_prov,mdy(1,1,1980))<>mdy(1,1,1990)  " +
                        "  AND b.dat_uchet<a.dat_uchet   " +
                        "  AND a.is_actual=1  " +
                        "  AND a.nzp_dom = {1}  " +
                        "  AND a.nzp_serv in (6,8,9,25,210,253)  " +
                        "  AND a.dat_uchet=(  " +
                        "  SELECT max(dat_uchet)  " +
                        "  FROM {0}_data.counters_dom c  " +
                        "  WHERE a.nzp_dom=c.nzp_dom  " +
                        "  AND a.nzp_serv=c.nzp_serv  " +
                        "  AND a.nzp_cnttype=c.nzp_cnttype  " +
                        "  AND a.num_cnt=c.num_cnt AND c.is_gkal = 0 " +
                        "  AND c.dat_uchet<=mdy({2},1,{3}) " +
                        "  AND a.is_actual=c.is_actual )  " +
                        "  AND b.dat_uchet=(  " +
                        "  SELECT max(dat_uchet) FROM {0}_data.counters_dom c  " +
                        "  WHERE a.nzp_dom=c.nzp_dom  " +
                        "  AND a.nzp_serv=c.nzp_serv  " +
                        "  AND a.nzp_cnttype=c.nzp_cnttype  " +
                        "  AND a.num_cnt=c.num_cnt AND c.is_gkal = 0  " +
                        "  AND a.dat_uchet>c.dat_uchet  " +
                        "  AND a.is_actual=c.is_actual )  " +
                        "  AND 0=(  " +
                        "  SELECT count(*) FROM {0}_data.counters_dom d  " +
                        "  WHERE a.nzp_dom=d.nzp_dom  " +
                        "  AND a.nzp_serv=d.nzp_serv  " +
                        "  AND a.nzp_cnttype=d.nzp_cnttype  " +
                        "  AND a.num_cnt=d.num_cnt AND d.is_gkal = 0 " +
                        "  AND a.is_actual=d.is_actual AND d.dat_close is not null); ",
                        house.DataBankStorage.SchemaName.Trim(),
                        house.Id,
                        date.Month,
                        date.Year);
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


            //подключаемся к БД, где лежит дом
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

                //получаем начисления дома
                var sqlQuery =
                    String.Format(
                        " SELECT " +
                        " MAX(c.nzp_charge) AS Id, " +
                        " k.nzp_dom AS HouseId, " +
                        " s.service AS  Service, " +
                        " SUM(sum_insaldo) AS BalanceIn, " +
                        " SUM(rsum_tarif) AS TariffAmount, " +
                        " SUM(sum_nedop) AS BackorderAmount, " +
                        " SUM(reval) AS RecalcAmount, " +
                        " SUM(money_to) AS ErcAmount, " +
                        " SUM(money_from) AS SupplierAmount, " +
                        " SUM(sum_outsaldo) AS BalanceOut" +
                        " FROM {0} c " +
                        " INNER JOIN {1}_data.kvar k ON k.nzp_kvar = c.nzp_kvar  " +
                        " INNER JOIN {1}_kernel.services s ON s.nzp_serv = c.nzp_serv " +
                        " WHERE k.nzp_dom =  {2} " +
                        " GROUP BY 2,3 ",
                        chargeTableFullName,
                        house.DataBankStorage.SchemaName.Trim(),
                        house.Id);
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
                    " SELECT                                               " +
                    "     {2} AS RealityObjectId, " +
                    "     kv.nzp_kvar AS Id,                       " +
                    "     kv.nzp_dom AS HouseId,                                 " +
                    "     kv.num_ls AS AccountId,                       " +
                    "     kv.typek AS AccountType,                       " + 
                    "     CAST (p15.val_prm AS DECIMAL(20, 0)) AS PSS, " +
                    "     kv.pkod AS PaymentCode,                  " +
                    "     kv.nkvar AS ApartmentNumber,                         " +
                    "     kv.nkvar_n AS RoomNumber,                       " +
                    "     '{0}' AS Prefix,                     " +
                    "     CASE WHEN p3.val_prm = '1'                        " +
                    "           THEN 1 ELSE 0 END AS IsOpen,        " +
                    "     TRIM(kv.fio) AS Tenant,                                  " +
                    "     kv.num_ls AS AccountNumber,                   " +
                    "     p3.dat_s AS DateBegin, " +
                    "     p3.dat_po AS DateEnd, " +
                    "     kv.remark AS Remark,                            " +
                    "     kv.porch AS Porch,                              " +
                    "     kv.phone AS Phone                               " +
                    " FROM                                              " +
                    "     {0}_data.kvar kv                                         " +
                //параметр "ПСС" 
                    " LEFT OUTER JOIN {0}_data.prm_15 p15 ON ( " +
                    "                                       kv.nzp_kvar = p15.nzp " +
                    "                                       AND p15.nzp_prm = 162 " +
                    "                                       AND p15.is_actual = 1) " +
                //параметр "Состояние ЛС (открыт, закрыт)" 
                    " LEFT OUTER JOIN {0}_data.prm_3 p3 ON " +
                    "                                       ( kv.nzp_kvar = p3.nzp " +
                    "                                       AND p3.nzp_prm = 51 " +
                    "                                       AND p3.is_actual = 1) " +
                    " WHERE                                             " +
                    " kv.nzp_dom = {1}                               ",
                    house.DataBankStorage.SchemaName,
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
