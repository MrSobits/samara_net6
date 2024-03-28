namespace Bars.Gkh.Gis.DomainService.ImportData.Impl
{
    using System.Linq;
    using B4.Config;
    using B4.Modules.Security;
    using Entities.Kp50;
    using System.Collections.Generic;
    using B4.Modules.FIAS;
    using Dapper;
    using Entities.Register.HouseRegister;
    using Entities.Register.HouseServiceRegister;
    using GisAddressMatching;
    using NHibernate;
    using Utils;
    using System;
    using System.Data;
    using System.Data.Common;

    using B4.Utils;
    using B4;
    using B4.DataAccess;
    using B4.IoC;

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Gis.KP_legacy;

    using Castle.Windsor;
    using Entities.Register.LoadedFileRegister;
    using Enum;
    using Gkh.Entities;

    /// <summary>
    /// Загрузка данных из БД биллинга
    /// </summary>
    public class LoadFromBillingBasesService : ILoadFromBillingBasesService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container;

        /// <summary>
        /// Справочник префиксов схем баз данных биллинга
        /// </summary>
        protected IRepository<BilDictSchema> BilDictSchemaRepository;

        /// <summary>
        /// Реестр домов МЖФ
        /// </summary>
        protected IRepository<RealityObject> RealityObject;

        /// <summary>
        /// Справочник услуг биллинга
        /// </summary>
        protected IRepository<BilServiceDictionary> BilServiceDictionary;
        /// <summary>
        /// Справочник УО биллинга
        /// </summary>
        protected IRepository<BilManOrgStorage> BilManOrgStorage;

        /// <summary>
        /// Тариф биллинга
        /// </summary>
        protected IRepository<BilTarifStorage> BilTarifRepository;

        /// <summary>
        /// Норматив биллинга
        /// </summary>
        protected IRepository<BilNormativStorage> BilNormativeRepository;


        protected IDomainService<ServiceDictionary> GisServiceDictionaryRepository;
        protected IRepository<User> UserRepository;
        protected IDomainService<HouseRegister> GisHouseRegister;
        protected IAddressMatcherService AddressMatcherService;
        protected IRepository<Fias> FiasRepository;
        protected IRepository<FiasAddress> FiasAddressRepository;


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="bilDictSchemaRepository">Справочник префиксов схем баз данных биллинга</param>
        /// <param name="fiasRepository"></param>
        /// <param name="fiasAddressRepository"></param>
        /// <param name="bilServiceDictionary"></param>
        /// <param name="bilTarifRepository">Реестр тарифов биллинга</param>
        /// <param name="bilNormativeStorage">Реест нормативов биллинга</param>
        /// <param name="realityObject">Реестр домов МЖФ</param>
        /// <param name="gisHouseRegister"></param>
        /// <param name="gisServiceDictionary"></param>
        /// <param name="userRepository"></param>
        /// <param name="addressMatcherService"></param>
        public LoadFromBillingBasesService(
            IWindsorContainer container,
            IRepository<BilDictSchema> bilDictSchemaRepository,
             IDomainService<HouseRegister> gisHouseRegister,
            IDomainService<ServiceDictionary> gisServiceDictionary,
             IRepository<User> userRepository,
             IAddressMatcherService addressMatcherService,
            IRepository<Fias> fiasRepository,
            IRepository<FiasAddress> fiasAddressRepository,
            IRepository<BilServiceDictionary> bilServiceDictionary,
            IRepository<BilTarifStorage> bilTarifRepository,
            IRepository<BilNormativStorage> bilNormativeStorage,
            IRepository<BilManOrgStorage> bilManOrgStorage,
            IRepository<RealityObject> realityObject
            )
        {
            Container = container;
            BilDictSchemaRepository = bilDictSchemaRepository;
            BilServiceDictionary = bilServiceDictionary;
            BilTarifRepository = bilTarifRepository;
            BilNormativeRepository = bilNormativeStorage;
            RealityObject = realityObject;
            Container = container;
            GisHouseRegister = gisHouseRegister;
            GisServiceDictionaryRepository = gisServiceDictionary;
            UserRepository = userRepository;
            AddressMatcherService = addressMatcherService;
            FiasRepository = fiasRepository;
            FiasAddressRepository = fiasAddressRepository;
            BilDictSchemaRepository = bilDictSchemaRepository;
            BilManOrgStorage = bilManOrgStorage;
        }


        /// <summary>
        /// Тип тарифа в биллинге
        /// </summary>
        private enum BillingTarifType
        {
            [Display("Тариф на ЛС")] Ls = 1,

            [Display("Тариф на дом")] House = 2,

            [Display("Тариф на поставщика")] Supplier = 3,

            [Display("Тариф на БД")] DataBase = 4
        }


        /// <summary>
        /// Новый метод переноса данных для аналитики из нижних банков  биллинга 
        /// в разрезе одного месяца
        /// </summary>
        /// <param name="yy">Расчетный год</param>
        /// <param name="mm">Расчетный месяц</param>
        /// <returns></returns>
        public IDataResult GetAnalyzeDataFromBillingBases(int yy, int mm)
        {
            if (yy == 0 || mm < 1 || mm > 12)
            {
                return BaseDataResult.Error("Неверные параметры месяца");
            }

            var user = Container.Resolve<IUserIdentity>(); //текущий юзер
            if (user.UserId == 0)
            {
                return BaseDataResult.Error("Не определен пользователь. Вам необходимо зарегистрироваться в системе");
            }

            var provider = Container.Resolve<ISessionProvider>();
            using (Container.Using(provider))
            {
                var session = provider.OpenStatelessSession();
                session.SetBatchSize(5000);

                try
                {
                    //список активных баз данных биллинга получаем из репозитория
                    foreach (
                        var billingLocalSchema in
                            BilDictSchemaRepository.GetAll()
                                .Where(x => x.IsActive == 1)
                                .OrderBy(x => x.ConnectionString))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {
                                //создать запись файл загрузки
                                var lf = new LoadedFileRegister
                                {
                                    FileName = "Billing" + yy + "_" + mm,
                                    B4User = UserRepository.Load(user.UserId),
                                    ObjectCreateDate = DateTime.Now,
                                    ObjectEditDate = DateTime.Now,
                                    TypeStatus = TypeStatus.Done
                                };
                                session.Insert(lf);

                                GetAnalyzeDataFromBillingBases(billingLocalSchema.ConnectionString,
                                    billingLocalSchema.LocalSchemaPrefix, yy, mm, session, lf);

                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                finally
                {
                    provider.CloseCurrentSession();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Заполнение справочника bil_manorg_storage УО из нижних банков биллинга
        /// </summary>
        /// <returns></returns>
        public IDataResult FillManOrgStorageFromBillingBasesOld()
        {
            var provider = Container.Resolve<ISessionProvider>();
            using (Container.Using(provider))
            {
                var session = provider.GetCurrentSession();
                session.SetBatchSize(5000);

                try
                {
                    //список активных баз данных биллинга получаем из репозитория
                    foreach (
                        var billingLocalSchema in
                            BilDictSchemaRepository.GetAll()
                                .Where(x => x.IsActive == 1)
                                .OrderBy(x => x.ConnectionString))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {
                                //FillManOrgStorageFromBillingBases(billingLocalSchema, session);
                               transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                finally
                {
                    provider.CloseCurrentSession();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Новый метод переноса данных для аналитики из одного нижнего банка  биллинга 
        /// в разрезе одного месяца
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="localSchemaPrefix"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="session"></param>
        /// <param name="lf"></param>
        private void GetAnalyzeDataFromBillingBases(string connectionString, string localSchemaPrefix, int year,
            int month, IStatelessSession session, LoadedFileRegister lf)
        {
            var chargeSchema = string.Format("{0}{1}_{2}", localSchemaPrefix, DataBankPostfixContainer.ChargePostfix,
                year % 100);

            var dataSchema = localSchemaPrefix + DataBankPostfixContainer.DataPostfix;
            var chargeTable = chargeSchema + ".CHARGE_" + month.ToString("00");
            var countersTable = chargeSchema + ".COUNTERS_" + month.ToString("00");

            var houseTable = dataSchema + ".dom ";
            var accountTable = dataSchema + ".kvar ";
            var areaTable = dataSchema + ".s_area ";
            var statTable = dataSchema + ".s_stat ";
            var townTable = dataSchema + ".s_town ";
            var regionTable = dataSchema + ".s_rajon ";
            var streetTable = dataSchema + ".s_ulica ";
            var prm4Table = dataSchema + ".prm_4 ";
            var prm5Table = dataSchema + ".prm_5 ";
            var prm11Table = dataSchema + ".prm_11 ";
            var prm2Table = dataSchema + ".prm_2 ";

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            #region Запрос на получения данных от УК

            //Тариф ГКал на всю базу
            var sqlGkal5 = @" 
                    SELECT max(val_prm) as ""Value"" FROM " + prm5Table + @" p 
                    WHERE  p.nzp_prm = 252 and p.is_actual <> 100 and p.dat_s <= :d2 and p.dat_po >= :d1 ";

            //Тариф ГКал на поставщика
            var sqlGkal11 = @" 
                    SELECT nzp as ""SupplierId"", max(val_prm) as ""Value"" FROM " + prm11Table + @" p 
                    WHERE  p.nzp_prm = 339 and p.is_actual <> 100 and p.dat_s <= :d2 and p.dat_po >= :d1 
                    GROUP BY 1 ";

            //Тариф ГКал на дом
            var sqlGkal2 = @" 
                    SELECT nzp as ""HouseId"", max(val_prm) as ""Value"" FROM " + prm2Table + @" p 
                    WHERE  p.nzp_prm = 1062 and p.is_actual <> 100 and p.dat_s <= :d2 and p.dat_po >= :d1 
                    GROUP BY 1 ";


            var sqlCounters = @" 
                    Select nzp_dom as ""HouseId"", nzp_serv as ""ServiceId"", 

                        val2 as ""VolumeIndividualCounter"", 
                        val1 as ""VolumeNormative"", 
                        kf307 as ""CoefOdn"", 
                        dop87 as ""VolumeDistributed"", 
                        val3-val2-val1-dop87 as ""VolumeNotDistributed"", 
                        case when cnt_stage>0 then dop87 else 0 end as ""VolumeOdnIndividualCounter"", 
                        case when cnt_stage<1 then dop87 else 0 end as ""VolumeOdnNormative""

                    From " + countersTable + @" 

                    Where nzp_dom = :nzp_dom and nzp_kvar = 0  and stek = 3 and nzp_type = 1 ";

            var sqlCharge = @" 
                    Select k.nzp_dom as ""HouseId"", ch.nzp_serv as ""ServiceId"", 

                         max( ch.nzp_supp ) as ""SupplierId"", 
                         max( ch.tarif ) as ""Tariff"", 
                         sum( ch.rsum_tarif ) as ""Charge"", 
                         sum( ch.sum_money  ) as ""Payment"", 
                         sum( ch.c_calc  ) as ""TotalVolume""

                    From " + chargeTable + @" ch INNER JOIN " + accountTable + @" k ON ch.nzp_kvar = k.nzp_kvar 

                    Where ch.nzp_serv in (14, 6,7,8,9,25,210, 510,511,512,513,514,515,516)
                      and ch.dat_charge is null and ( abs(rsum_tarif) > 0 or abs(sum_money) > 0 )

                    Group by 1,2 Order by 1 ";

            var sqlHouse = @" 
                    Select d.nzp_dom as ""Id"", 

                           max(trim(a.area)) as ""ManOrgs"", 
                           max(trim(st.stat)) as ""Region"",
                           max(trim(raj.rajon)) as ""Area"",
                           max(trim(t.town)) as ""City"",
                           max(trim(ul.ulica)) as ""Street"",
                           max(trim(d.ndom)) as ""HouseNum"",
                           max(trim(d.nkor)) as ""BuildNum"",

                           max(trim(p.val_prm)) AS ""CodeErc"",
                           max(trim(ul.ulicareg)) as ""StreetAddName""

                    From " + houseTable + @" d

                        LEFT OUTER JOIN " + prm4Table +
                           @" p ON d.nzp_dom = p.nzp and p.nzp_prm = 890 and p.is_actual <> 100 and p.dat_s <= now() and p.dat_po>=now()
                        LEFT OUTER JOIN " + areaTable + @" a ON d.nzp_area = a.nzp_area
                        LEFT OUTER JOIN " + streetTable + @" ul ON d.nzp_ul = ul.nzp_ul
                        LEFT OUTER JOIN " + regionTable + @" raj ON ul.nzp_raj = raj.nzp_raj
                        LEFT OUTER JOIN " + townTable + @" t ON raj.nzp_town = t.nzp_town
                        LEFT OUTER JOIN " + statTable + @" st ON t.nzp_stat = st.nzp_stat
                     
                    Group by 1 Order by 1 ";

            #endregion

            //выбираем соединение и подключаемся к БД, где лежит дом
            var provider = new ConnectionProvider(Container.Resolve<IConfigProvider>());
            provider.Init(connectionString);
            var dbConnection = provider.CreateConnection();
            dbConnection.Open();


            //вытащить все начисления по домам сразу в список 
            //var charges = session
            //    .CreateSQLQuery(sqlCharge)
            //    .SetTimeout(900000)
            //    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(HouseServiceRegister)))
            //    .List<HouseServiceRegister>();

            var charges = dbConnection.Query<HouseServiceRegister>(sqlCharge);

            //вытащить все дома сразу в список 
            //var houses = session
            //    .CreateSQLQuery(sqlHouse)
            //    .SetTimeout(900000)
            //    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(HouseRegister)))
            //    .List<HouseRegister>();

            var houses = dbConnection.Query<HouseRegister>(sqlHouse);

            //вытащить ГКалы
            //var gkal5 = session
            //    .CreateSQLQuery(sqlGkal5) //на всю базу
            //    .SetParameter("d1", d1)
            //    .SetParameter("d2", d2)
            //    .SetTimeout(900000)
            //    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(GkalProxy)))
            //    .List<GkalProxy>();

            var gkal5 = dbConnection
                .Query<GkalProxy>(
                    sqlGkal5.Replace(":d1", " '" + startDate + "' ").Replace(":d2", "'" + endDate + "'")
                ); //на всю базу


            //var gkal11 = session
            //    .CreateSQLQuery(sqlGkal11) //на поставщика
            //    .SetParameter("d1", d1)
            //    .SetParameter("d2", d2)
            //    .SetTimeout(900000)
            //    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(GkalProxy)))
            //    .List<GkalProxy>();


            var gkal11 = dbConnection
                .Query<GkalProxy>(
                    sqlGkal11.Replace(":d1", " '" + startDate + "' ").Replace(":d2", "'" + endDate + "'")
                ); //на поставщика

            //var gkal2 = session
            //    .CreateSQLQuery(sqlGkal2) //на дом
            //    .SetParameter("d1", d1)
            //    .SetParameter("d2", d2)
            //    .SetTimeout(900000)
            //    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(GkalProxy)))
            //    .List<GkalProxy>();


            var gkal2 =
                dbConnection
                    .Query<GkalProxy>(
                        sqlGkal2.Replace(":d1", " '" + startDate + "' ").Replace(":d2", "'" + endDate + "'")
                    ); //на дом


            var services = GisServiceDictionaryRepository.GetAll().ToList();

            long curHouse = 0;
            HouseRegister gisHouse = null;
            IList<HouseServiceRegister> counters = null;

            //откроем цикл по домовым начислениям
            foreach (var charge in charges)
            {
                if (charge.HouseId != curHouse)
                {
                    RealityObject gkhHouse = null;
                    gisHouse = null;
                    curHouse = charge.HouseId;

                    //вытащить данные дома
                    var house1 = curHouse;
                    var billHouse = houses.FirstOrDefault(x => x.Id == house1);
                    if (billHouse == null)
                    {
                        continue;
                    }

                    //определить дом в ЖКХ по коду ЕРЦ
                    if (!string.IsNullOrEmpty(billHouse.CodeErc))
                    {
                        var item = billHouse;
                        gkhHouse = RealityObject.GetAll().FirstOrDefault(x => x.CodeErc.Contains(item.CodeErc));
                    }

                    if (gkhHouse != null && gkhHouse.FiasAddress != null)
                    {
                        //определить дом в ГИС
                        var item = gkhHouse;
                        gisHouse = GisHouseRegister.GetAll()
                            .FirstOrDefault(x => x.FiasAddress.Id == item.FiasAddress.Id);
                    }

                    // пробуем сопоставить напрямую
                    var fiasHouse = FiasAddressRepository
                        .GetAll()
                        .Where(x => x.PlaceName.ToUpper() == billHouse.City
                                    ||
                                    !billHouse.City.Trim().IsEmpty() &&
                                    x.PlaceName.ToUpper() == billHouse.City.Trim().ToUpper())
                        .Where(x => x.StreetName.ToUpper() == billHouse.Street.ToUpper())
                        .Where(x => x.House.ToUpper() == billHouse.HouseNum.ToUpper())
                        .WhereIf(!billHouse.BuildNum.IsEmpty(),
                            x => x.Housing == null || x.Housing.ToUpper() == billHouse.BuildNum.ToUpper())
                        .FirstOrDefault();

                    if (fiasHouse != null)
                    {
                        gisHouse = GisHouseRegister.GetAll().FirstOrDefault(x => x.FiasAddress.Id == fiasHouse.Id);
                    }

                    if (gisHouse == null)
                    {
                        // получение адреса из ФИАСа
                        var fiasAddress = gkhHouse.Return(x => x.FiasAddress)
                                          ?? fiasHouse
                                          ?? AddressMatcherService.MatchAddress(billHouse);

                        if (fiasAddress != null)
                        {
                            gisHouse = new HouseRegister
                            {
                                ManOrgs = billHouse.ManOrgs,
                                HouseNum = fiasAddress.House,
                                BuildNum = fiasAddress.Housing,
                                FiasAddress = fiasAddress
                            };

                            HouseRegisterUtils.FillHouseByFiasAddress(gisHouse, fiasAddress.StreetGuidId, FiasRepository);
                            HouseRegisterUtils.CopyParam(gisHouse, gkhHouse);
                        }
                        else
                        {
                            //нет данных в gis_house_register - вставим
                            gisHouse = new HouseRegister
                            {
                                ManOrgs = billHouse.ManOrgs,
                                FiasAddress = null,
                                Region = billHouse.Region,
                                Area = billHouse.Area == "-"
                                    ? ""
                                    : billHouse.Area,
                                City = billHouse.City,
                                Street = string.Format("{0}{1}",
                                    billHouse.StreetAddName.IsEmpty() ? "" : billHouse.StreetAddName + ". ",
                                    billHouse.Street),
                                HouseNum = billHouse.HouseNum,
                                BuildNum = billHouse.BuildNum
                            };
                        }

                        session.Insert(gisHouse);
                    }

                    //вытащить расходы
                    //counters = session
                    //    .CreateSQLQuery(sqlCounters)
                    //    .SetParameter("nzp_dom", curHouse)
                    //    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(HouseServiceRegister)))
                    //    .List<HouseServiceRegister>();


                    counters = dbConnection
                        .Query<HouseServiceRegister>(
                            sqlCounters.Replace(":nzp_dom", curHouse.ToString())).ToList();
                }

                if (gisHouse == null)
                {
                    continue; //дом не найден или не вставлен
                }

                var charge1 = charge;
                if (counters != null && counters.Any())
                {
                    foreach (var item in counters.Where(item => item.ServiceId == charge1.ServiceId))
                    {
                        //заполним данные по ОДН
                        charge.VolumeIndividualCounter = item.VolumeIndividualCounter;
                        charge.VolumeNormative = item.VolumeNormative;
                        charge.CoefOdn = item.CoefOdn;
                        charge.VolumeDistributed = item.VolumeDistributed;
                        charge.VolumeNotDistributed = item.VolumeNotDistributed < 0 ? 0 : item.VolumeNotDistributed;
                        charge.VolumeOdnIndividualCounter = item.VolumeOdnIndividualCounter;
                        charge.VolumeOdnNormative = item.VolumeOdnNormative;

                        break;
                    }
                }

                var gisService = services.FirstOrDefault(x => x.Code == charge1.ServiceId);
                if (gisService == null)
                {
                    throw new Exception("Не найдена услуга " + charge1.ServiceId);
                }

                //отопление: перевести объемы по квадратуру в гКал
                if (charge.ServiceId == 8)
                {
                    var gkal = gkal2.FirstOrDefault(x => x.HouseId == charge1.HouseId); //на дом

                    if (gkal == null)
                    {
                        gkal = gkal11.FirstOrDefault(x => x.SupplierId == charge1.SupplierId); //на поставщика
                    }
                    if (gkal == null)
                    {
                        gkal = gkal5.FirstOrDefault(); //на всю базу
                    }

                    if (gkal != null)
                    {
                        decimal gkalTariff;
                        decimal gkalVolume = 0;

                        decimal.TryParse(gkal.Value, out gkalTariff);

                        if (gkalTariff > 0 && charge.Charge > 0)
                        {
                            gkalVolume = (decimal)charge.Charge / gkalTariff;
                            //charge.TotalVolume = gkalVolume;
                        }
                    }
                }

                //вставка начислений
                var insertCh = new HouseServiceRegister
                {
                    Service = gisService,
                    House = gisHouse,
                    CalculationDate = new DateTime(year, month, 1),
                    HouseAddress = gisHouse.FiasAddress.Return(x => x.AddressName, gisHouse.GetFullAddress()),
                    ManOrgs = gisHouse.ManOrgs,

                    Tariff = charge.Tariff,
                    Charge = charge.Charge,
                    Payment = charge.Payment,
                    TotalVolume = charge.TotalVolume,

                    VolumeIndividualCounter = charge.VolumeIndividualCounter,
                    VolumeNormative = charge.VolumeNormative,
                    CoefOdn = charge.CoefOdn,
                    VolumeDistributed = charge.VolumeDistributed,
                    VolumeNotDistributed = charge.VolumeNotDistributed,
                    VolumeOdnIndividualCounter = charge.VolumeOdnIndividualCounter,
                    VolumeOdnNormative = charge.VolumeOdnNormative,

                    LoadedFile = lf
                };

                session.Insert(insertCh);
            }
            dbConnection.Close();
            dbConnection.Dispose();
        }

        /// <summary>
        /// Заполнение справочника bil_dict_service услугами из нижних банков биллинга 
        /// </summary>
        /// <returns></returns>
        public IDataResult FillServicesDictionaryFromBillingBases()
        {
            var provider = Container.Resolve<ISessionProvider>();
            using (Container.Using(provider))
            {
                var session = provider.OpenStatelessSession();
                session.SetBatchSize(5000);

                try
                {
                    //список активных баз данных биллинга получаем из репозитория
                    foreach (
                        var billingLocalSchema in
                            BilDictSchemaRepository.GetAll()
                                .Where(x => x.IsActive == 1)
                                .OrderBy(x => x.ConnectionString))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {
                                FillServicesDictionaryFromBillingBases(billingLocalSchema, session);

                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                finally
                {
                    provider.CloseCurrentSession();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Заполнение справочника bil_dict_service услугами из одного нижнего банка биллинга 
        /// </summary>
        /// <param name="localSchema"></param>
        /// <param name="session"></param>
        private void FillServicesDictionaryFromBillingBases(BilDictSchema localSchema, IStatelessSession session)
        {
            var kernelSchema = localSchema.LocalSchemaPrefix + DataBankPostfixContainer.KernelPostfix;

            //выбираем соединение и подключаемся к БД, где лежит дом
            var provider = new ConnectionProvider(Container.Resolve<IConfigProvider>());
            provider.Init(localSchema.ConnectionString);

            using (var dbConnection = provider.CreateConnection())
            {
                dbConnection.Open();
                var sqlQuery =
                    string.Format(

                        " SELECT serv.nzp_serv AS ServiceCode, " +
                            "    TRIM(serv.service_name) AS ServiceName, " +
                            "    s.nzp_grpserv AS ServiceTypeCode,  " +
                            "    TRIM(s.name_grpserv) AS ServiceTypeName, " +
                            "    m.nzp_measure AS MeasureCode,  " +
                            "    TRIM(m.measure) AS MeasureName,  " +

                            //порядок сортировки (в  ЕПД)
                            "    serv.ordering AS OrderNumber," +

                            //Признак "Услуга предоставляется на общедомовые нужды"(true - ОДН, false - не ОДН)
                            "     CASE WHEN serv.nzp_serv > 500 THEN true ELSE false END AS IsOdnService, " +

                            //cсылка на родительскую услугу
                            "     u.nzp_serv_base AS ParentServiceCode " +
                            " FROM " +

                            //справочник услуг
                            " {0}.services serv   " +

                            //справочник методик расчета (нужно для получения единиц измерения)
                            " LEFT OUTER JOIN {0}.formuls frm ON frm.nzp_frm = serv.nzp_frm " +

                            //справочник единииц измерений
                            " LEFT OUTER JOIN {0}.s_measure m ON m.nzp_measure = frm.nzp_measure " +

                            //таблица связки типов услуг и справочника услуг
                            " LEFT OUTER JOIN {0}.grpserv grp_serv ON grp_serv.nzp_serv = serv.nzp_serv " +

                            //справочник типов услуг
                            " LEFT OUTER JOIN {0}.s_grpserv s ON s.nzp_grpserv = grp_serv.nzp_grpserv " +

                            //связка услуг(базовая и дочерняя)
                            " LEFT OUTER JOIN {0}.service_union u ON (u.nzp_serv_uni = serv.nzp_serv AND u.nzp_serv_base != u.nzp_serv_uni) ",
                        kernelSchema);

                var billServicesList = dbConnection.Query<BilServiceDictionary>(sqlQuery).ToList();

                billServicesList
                    //исключаем услуги, которые загружены ранее из этого банка данных
                    .Except(BilServiceDictionary
                        .GetAll()
                        .Where(x => x.Schema.Id == localSchema.Id), new BilServiceComparer())
                    .ForEach(x =>
                    {
                        x.Schema = localSchema;
                        session.Insert(x);
                    }
                    );

                var servicesList = BilServiceDictionary.GetAll().Where(x => x.Schema.Id == localSchema.Id).ToList();


                (from s in servicesList
                    join b in billServicesList
                        on s.ServiceCode equals b.ServiceCode
                    select
                        new BilServiceDictionary
                        {
                            Id = s.Id,
                            ServiceCode = b.ServiceCode,
                            ServiceName = b.ServiceName,
                            ServiceTypeCode = b.ServiceTypeCode,
                            ServiceTypeName = b.ServiceTypeName,
                            IsOdnService = b.IsOdnService,
                            MeasureCode = b.MeasureCode,
                            MeasureName = b.MeasureName,
                            OrderNumber = b.OrderNumber,
                            ParentServiceCode = b.ParentServiceCode,
                            Schema = s.Schema,
                        })
                    .ForEach(session.Update);
            }

        }

        /// <summary>
        /// Компаратор услуг биллинга
        /// </summary>
        private class BilServiceComparer : IEqualityComparer<BilServiceDictionary>
        {
            public bool Equals(BilServiceDictionary x, BilServiceDictionary y)
            {
                return
                    (x.ServiceCode == y.ServiceCode);
            }

            public int GetHashCode(BilServiceDictionary obj)
            {
                return obj.ServiceCode;
            }
        }

        /// <summary>
        /// Компаратор УО биллинга
        /// </summary>
        private class BilManOrgComparer : IEqualityComparer<BilManOrgStorage>
        {
            public bool Equals(BilManOrgStorage x, BilManOrgStorage y)
            {
                return
                    (x.ManOrgCode == y.ManOrgCode);
            }

            public int GetHashCode(BilManOrgStorage obj)
            {
                return obj.ManOrgCode;
            }
        }

        /// <summary>
        /// Заполнение нормативов из нижних банков биллинга
        /// </summary>
        /// <returns></returns>
        public IDataResult FillNormativeStorageFromBillingBases()
        {
            var provider = Container.Resolve<ISessionProvider>();
            using (Container.Using(provider))
            {
                var session = provider.OpenStatelessSession();
                session.SetBatchSize(5000);

                try
                {
                    //список активных баз данных биллинга получаем из репозитория
                    foreach (
                        var billingLocalSchema in
                            BilDictSchemaRepository.GetAll()
                                .Where(x => x.IsActive == 1)
                                .OrderBy(x => x.ConnectionString))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {
                                FillNormativeStorageFromBillingBases(billingLocalSchema, session);

                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                finally
                {
                    provider.CloseCurrentSession();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Заполнение справочника bil_normative_storage нормативами  из одного нижнего банка биллинга 
        /// </summary>
        /// <param name="localSchema"></param>
        /// <param name="session"></param>
        private void FillNormativeStorageFromBillingBases(BilDictSchema localSchema, IStatelessSession session)
        {
            var kernelSchema = localSchema.LocalSchemaPrefix + DataBankPostfixContainer.KernelPostfix;
            var dataSchema = localSchema.LocalSchemaPrefix + DataBankPostfixContainer.DataPostfix;

            //выбираем соединение и подключаемся к БД, где лежит дом
            var provider = new ConnectionProvider(Container.Resolve<IConfigProvider>());
            provider.Init(localSchema.ConnectionString);

            using (var dbConnection = provider.CreateConnection())
            {
                dbConnection.Open();

                //удаляем все нормативы, которые пришли ранее из этого банка данных
                BilNormativeRepository
                    .GetAll()
                    .Where(x => x.BilService.Schema.Id == localSchema.Id)
                    .ForEach(session.Delete);



                var sqlQuery =
                    string.Format(
                        " SELECT						" +
                        " CASE                          " +
                        " WHEN p.nzp_prm = 172 THEN 6   " +
                        " WHEN p.nzp_prm = 173 THEN 10  " +
                        " WHEN p.nzp_prm = 177 THEN 9   " +
                        " WHEN p.nzp_prm = 180 THEN 25  " +
                        " WHEN p.nzp_prm = 181 THEN 25  " +
                        " WHEN p.nzp_prm = 182 THEN 25  " +
                        " WHEN p.nzp_prm = 183 THEN 25  " +
                        " WHEN p.nzp_prm = 184 THEN 515 " +
                        " WHEN p.nzp_prm = 185 THEN 513 " +
                        " WHEN p.nzp_prm = 186 THEN 512 " +
                        " END AS ServiceCode,               " +
                        " p.nzp_prm as NormativeTypeCode, 									" +
                        " TRIM(p.name_prm) AS NormativeTypeName,                              " +
                        " TRIM(p13.val_prm) AS NormativeCode,                                  " +
                        " '\"'||TRIM(x.name_x)||'\" - \"'|| TRIM(y.name_y)||'\"' AS NormativeName, " +
                        " TRIM(res.name_res) AS NormativeDescription,                          " +
                        " TRIM(v.value) AS NormativeValue,                                     " +
                        " p13.dat_s AS NormativeStartDate,                                    " +
                        " p13.dat_po AS NormativeEndDate                                      " +
                        " FROM {1}.prm_name p                                          " +
                        " LEFT OUTER JOIN {0}.prm_13 p13 ON (p13.nzp_prm = p.nzp_prm)    " +
                        " LEFT OUTER JOIN {1}.resolution res ON (res.nzp_res = CAST(p13.val_prm as INTEGER)) " +
                        " LEFT OUTER JOIN {1}.res_x x ON (x.nzp_res = res.nzp_res)" +
                        " LEFT OUTER JOIN {1}.res_y y ON (y.nzp_res = res.nzp_res)" +
                        " LEFT OUTER JOIN {1}.res_values v ON " +
                        " 				(v.nzp_res = res.nzp_res AND v.nzp_y = y.nzp_y AND v.nzp_x = x.nzp_x) " +
                        " WHERE p.prm_num = 13 " +
                        " ORDER BY 1,3,5,6,7; ",
                        dataSchema,
                        kernelSchema);


                //вставляем в БД МЖФ
                (from billingNormative in dbConnection.Query<BilNormativ>(sqlQuery)

                 //сопоставляем с услугами для полученных нормативов
                 join billingService in BilServiceDictionary.GetAll()
                     .Where(x => x.Schema.Id == localSchema.Id)
                     on new { billingNormative.ServiceCode }
                     equals new { billingService.ServiceCode }

                 select new BilNormativStorage()
                 {
                     BilService = billingService,
                     NormativeTypeCode = billingNormative.NormativeTypeCode,
                     NormativeTypeName = billingNormative.NormativeTypeName,
                     NormativeCode = billingNormative.NormativeCode,
                     NormativeName = billingNormative.NormativeName,
                     NormativeDescription = billingNormative.NormativeDescription,
                     NormativeValue = billingNormative.NormativeValue,
                     NormativeStartDate = billingNormative.NormativeStartDate,
                     NormativeEndDate = billingNormative.NormativeEndDate
                 }).ForEach(x => session.Insert(x));
            }

        }

        /// <summary>
        /// Заполнение справочника тарифов из нижних банков биллинга 
        /// за один год
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public IDataResult FillTarifsDictionaryFromBillingBases(int year)
        {
            //контейнер с входными параметрами
            var finder = new PrmTarifFinder
            {
                NzpUser = 1,
                DateFrom = new DateTime(year, 1, 1),
                DateTo = new DateTime(year, 12, 31),
                ShowAll = true
            };

            //перебираем по всем типам тарифов
            foreach (BillingTarifType type in System.Enum.GetValues(typeof (BillingTarifType)))
            {
                GetTarifByType(type, finder);
            }

            return new BaseDataResult();
        }
        
        /// <summary>
        /// Получаем тариф определенного типа за выбранный период
        /// по всем нижним банкам биллинга
        /// </summary>
        /// <param name="type"></param>
        /// <param name="finder"></param>
        private void GetTarifByType(BillingTarifType type, PrmTarifFinder finder)
        {
            //список активных баз данных биллинга получаем из репозитория
            foreach (
                var billingLocalSchema in
                    BilDictSchemaRepository.GetAll()
                        .Where(x => x.IsActive == 1)
                        .OrderBy(x => x.ConnectionString))
            {
                try
                {
                    var provider = new ConnectionProvider(Container.Resolve<IConfigProvider>());

                    provider.Init(billingLocalSchema.ConnectionString);


                    using (var dbConnection = new SqlExecutor.SqlExecutor(billingLocalSchema.ConnectionString))
                    {

                        #region Создание временных таблиц

                        //список ЛС и домов, по которым вытаскиваем тарифы
                        var tableSelectedLs = string.Format("t{0}_tariff_spls", finder.NzpUser);

                        dbConnection.ExecuteSql("DROP TABLE IF EXISTS " + tableSelectedLs);


                        var sql = string.Format(
                            " SELECT nzp_kvar, nzp_dom, CAST('{1}' AS CHAR(20)) " +
                            " INTO TEMP {0}" +
                            " FROM {1}_data.kvar ",
                            tableSelectedLs,
                            billingLocalSchema.LocalSchemaPrefix.Trim());

                        dbConnection.ExecuteSql(sql);

                        sql =
                            " CREATE INDEX ix1_" + tableSelectedLs + " ON " + tableSelectedLs +
                            "(nzp_kvar)";
                        dbConnection.ExecuteSql(sql);

                        sql =
                            " CREATE INDEX ix2_" + tableSelectedLs + " ON " + tableSelectedLs +
                            "(nzp_dom)";
                        dbConnection.ExecuteSql(sql);


                        //временная таблица с тарифами
                        var tableTarifs = "bil_temp_table_tarifs_" + finder.NzpUser + DateTime.Now.Ticks;
                        dbConnection.ExecuteSql("DROP TABLE IF EXISTS " + tableTarifs);
                        sql = string.Format(
                            " CREATE TEMP TABLE {0} " +
                            " (nzp_wp integer," +
                            " nzp_dom integer, " +
                            " nzp_kvar integer," +
                            " nzp_serv integer, " +
                            " nzp_supp integer, " +
                            " nzp_frm integer, " +
                            " nzp_frm_typ integer," +
                            " nzp_prm_tarif integer)",
                            tableTarifs);

                        dbConnection.ExecuteSql(sql);

                        sql =
                            " CREATE INDEX ix1_" + tableTarifs + " ON " + tableTarifs +
                            "(nzp_kvar,nzp_serv,nzp_supp,nzp_frm,nzp_prm_tarif,nzp_wp)";
                        dbConnection.ExecuteSql(sql);

                        #endregion Создание временных таблиц

                        //Подготовка данных в зависимости от типа тарифа
                        PrepareDataByTarifType(dbConnection, type, finder, billingLocalSchema.LocalSchemaPrefix, tableTarifs);

                        //Перенос тарифов в БД МЖФ
                        TransferTarifsIntoMgfBase(dbConnection,
                            finder,
                            billingLocalSchema,
                            tableTarifs,
                            type);

                        dbConnection.ExecuteSql("DROP TABLE IF EXISTS " + tableSelectedLs);
                        dbConnection.ExecuteSql("DROP TABLE IF EXISTS " + tableTarifs);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Подготавливка данных в зависимости от типа тарифа
        /// </summary>
        /// <param name="sqlExecutor"></param>
        /// <param name="type"></param>
        /// <param name="finder"></param>
        /// <param name="pref"></param>
        /// <param name="tableTarifs"></param>
        /// <param name="addWhere"></param>
        /// <param name="nzpPrm"></param>
        /// <returns></returns>
        private void PrepareDataByTarifType(SqlExecutor.SqlExecutor sqlExecutor, BillingTarifType type,
            PrmTarifFinder finder,
            string pref,
            string tableTarifs, string addWhere = "", int nzpPrm = 0)
        {
            string whereFrmType;
            string prmTarifField;
            switch (type)
            {
                case BillingTarifType.Ls:
                {
                    #region

                    whereFrmType = "nzp_frm_typ not in (312,912,2) " + addWhere;
                    prmTarifField = "nzp_prm_tarif_ls";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);


                    //для этого типа тарифов получаем данные если на лс есть параметр "Ставки ЭОТ по лиц.счетам"
                    whereFrmType = "nzp_frm_typ=2 " + addWhere;
                    whereFrmType += string.Format(" AND EXISTS (SELECT 1 FROM {0}.prm_1 p1 " +
                                                  " WHERE p1.nzp=t.nzp_kvar AND p1.nzp_prm=335 " +
                                                  " AND p1.is_actual<>100 AND p1.dat_s<='{1}' AND p1.dat_po>='{2}')",
                        pref + DataBankPostfixContainer.DataPostfix, finder.DateTo.ToShortDateString(),
                        finder.DateFrom.ToShortDateString());
                    prmTarifField = "nzp_prm_tarif_ls";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);

                    whereFrmType = "nzp_frm_typ in (312,912)" + addWhere;
                    prmTarifField = "nzp_prm_tarif_dm";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);

                    whereFrmType = "nzp_frm_typ in (312,912)" + addWhere;
                    prmTarifField = "nzp_prm_tarif_su";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);

                    break;

                    #endregion
                }
                case BillingTarifType.House:
                {
                    #region

                    whereFrmType = "nzp_frm_typ not in (312,912 ,12,412)" + addWhere;
                    prmTarifField = "nzp_prm_tarif_dm";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);


                    whereFrmType = "nzp_frm_typ in (26)" + addWhere;
                    prmTarifField = "nzp_prm_tarif_su";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);


                    break;

                    #endregion
                }
                case BillingTarifType.Supplier:
                {
                    #region

                    //если включен параметр "Ставки ЭОТ по поставщикам"
                    if (GetParamValueInPeriod(sqlExecutor, pref, 336, 5, finder.DateFrom, finder.DateTo))
                    {
                        whereFrmType = "nzp_frm_typ not in (312,912 ,12,412 ,26)" + addWhere;
                        prmTarifField = "nzp_prm_tarif_su";
                        if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                        PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);
                    }

                    break;

                    #endregion
                }
                case BillingTarifType.DataBase:
                {
                    #region

                    whereFrmType = "nzp_frm_typ not in (312,912 ,12,412 ,26)" + addWhere;
                    prmTarifField = "nzp_prm_tarif_bd";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);


                    whereFrmType = "nzp_frm_typ in (12,412)" + addWhere;
                    prmTarifField = "nzp_prm_tarif_dm";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);


                    whereFrmType = "nzp_frm_typ in (12,412)" + addWhere;
                    prmTarifField = "nzp_prm_tarif_su";
                    if (nzpPrm > 0) whereFrmType += " AND " + prmTarifField + "=" + nzpPrm;
                    PrepareDataParams(sqlExecutor, finder, pref, tableTarifs, whereFrmType, prmTarifField);

                    break;

                    #endregion
                }
            }
        }

        /// <summary>
        /// Получить значение параметра в периоде (на пересечение)
        /// </summary>
        /// <typeparam name="T">тип параметра</typeparam>
        /// <param name="sqlExecutor">соединение</param>
        /// <param name="pref">префикс</param>
        /// <param name="nzpPrm">номер параметра</param>
        /// <param name="prmNum">номер таблицы</param>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        private static bool GetParamValueInPeriod(SqlExecutor.SqlExecutor sqlExecutor, string pref, int nzpPrm,
            int prmNum,
            DateTime dateFrom, DateTime dateTo)
        {
            var tableName = pref + DataBankPostfixContainer.DataPostfix + ".prm_" + prmNum;

            var sql = string.Format(
                " SELECT max(case when val_prm='1' then 1 else 0 end) " +
                " FROM {0} p " +
                " WHERE p.nzp_prm =  {1}" +
                "  AND p.is_actual <> 100 " +
                "  AND p.dat_s<='{3}' " +
                "  AND p.dat_po>='{2}'",
                tableName,
                nzpPrm,
                dateFrom.ToShortDateString(),
                dateTo.ToShortDateString());

            var result = sqlExecutor.ExecuteSql<bool>(sql);
            return result != null && result.FirstOrDefault();
        }


        private void PrepareDataParams(SqlExecutor.SqlExecutor sqlExecutor, PrmTarifFinder finder, string pref,
            string tableTarifs, string whereFrmType, string prmTarifField)
        {
            const int nzpWp = 0;

            //список ЛС и домов, по которым вытаскиваем тарифы
            var tableSelectedLs = string.Format("t{0}_tariff_spls", finder.NzpUser);


            var sql =
                " INSERT INTO " + tableTarifs +
                " (nzp_wp,nzp_dom,nzp_kvar,nzp_serv,nzp_supp,nzp_frm,nzp_frm_typ,nzp_prm_tarif)" +
                " SELECT  " + nzpWp +
                ", k.nzp_dom,k.nzp_kvar,t.nzp_serv,t.nzp_supp,f.nzp_frm,f.nzp_frm_typ,f." + prmTarifField +
                " FROM  " + pref + DataBankPostfixContainer.DataPostfix + ".tarif t, " +
                pref + DataBankPostfixContainer.KernelPostfix +
                ".formuls_opis f, " +
                tableSelectedLs + " k " +
                " WHERE k.nzp_kvar=t.nzp_kvar and f.nzp_frm=t.nzp_frm  " +
                " AND f." + whereFrmType + " AND f." + prmTarifField + ">0 " +
                " AND t.is_actual<>100 AND t.dat_s<='" + finder.DateTo.ToShortDateString() + "'" +
                " AND t.dat_po>='" + finder.DateFrom.ToShortDateString() + "'" +
                " GROUP BY 2,3,4,5,6,7,8";

            sqlExecutor.ExecuteSql(sql);
        }


        /// <summary>
        /// Класс для фильтрации получения данных по ведению тарифов
        /// </summary>
        private class PrmTarifFinder
        {
            /// <summary>
            /// Дата окончания периода действия тарифа
            /// </summary>
            private DateTime _dateTo;

            /// <summary>
            /// Код пользователя необходимый для получения имени таблицы
            /// </summary>
            public int NzpUser { get; set; }

            /// <summary>
            /// Показывать полный перечень тарифов, включая tarif is null
            /// </summary>
            public bool ShowAll { get; set; }

            /// <summary>
            /// Дата начала периода действия тарифа
            /// </summary>
            public DateTime DateFrom { get; set; }


            public DateTime DateTo
            {
                get { return _dateTo; }
                set
                {
                    if (value >= DateFrom)
                    {
                        _dateTo = value;
                    }
                }
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            public PrmTarifFinder()
            {
                DateFrom = new DateTime(2000, 1, 1);
                _dateTo = new DateTime(3000, 1, DateTime.DaysInMonth(3000, 1));
            }
        }

        /// <summary>
        /// Перенос тарифов в БД МЖФ
        /// </summary>
        private void TransferTarifsIntoMgfBase(SqlExecutor.SqlExecutor sqlExecutor, PrmTarifFinder finder,
            BilDictSchema billingLocalSchema,
            string tableTarifs, BillingTarifType tarifType)
        {

            var provider = Container.Resolve<ISessionProvider>();
            using (Container.Using(provider))
            {
                var session = provider.OpenStatelessSession();
                session.SetBatchSize(5000);



                const int nzpWp = 0;
                var fieldKey = "";
                var prmNum = 0;

                switch (tarifType)
                {
                    case BillingTarifType.Ls:
                    {
                        fieldKey = "p1.nzp = t.nzp_kvar";
                        prmNum = 1;
                    }

                        break;
                    case BillingTarifType.House:
                    {
                        fieldKey = "p1.nzp = t.nzp_dom";
                        prmNum = 2;
                    }
                        break;
                    case BillingTarifType.Supplier:
                    {
                        fieldKey = "p1.nzp = t.nzp_supp";
                        prmNum = 11;
                    }
                        break;
                    case BillingTarifType.DataBase:
                    {
                        fieldKey = "1=1";
                        prmNum = 5;
                    }
                        break;
                }

                var resultTable = string.Format("tem_tarif_result_{0}_{1}", finder.NzpUser, DateTime.Now.Ticks);
                sqlExecutor.ExecuteSql("DROP TABLE IF EXISTS " + resultTable);
                sqlExecutor.ExecuteSql("ANALYZE " + tableTarifs);


                //вытаскиваем все тарифы, которые собрали
                var sqlQuery =

                    string.Format(
                        " SELECT " +

                        //услуга
                        "   t.nzp_serv AS ServiceCode, " +
                        "   MAX(TRIM(s.service)) as ServiceName," +

                        //единица измерения
                        //"  m.nzp_measure AS MeasureCode, " +
                        //"  MAX(trim(m.measure)) AS MeasureName, " +

                        //поставщик (агентский договор)
                        "   sp.nzp_supp AS SupplierCode, " +
                        "   MAX(TRIM(sp.name_supp)) as SupplierName," +

                        //формула
                        "   t.nzp_frm_typ AS FormulaTypeCode, " +
                        "   t.nzp_frm AS FormulaCode, " +
                        "   MAX(ff.name_frm) AS FormulaName,  " +

                        //тариф
                        "   t.nzp_prm_tarif AS TarifCode," +
                        "   CAST(replace(p1.val_prm,',','.') as numeric) AS TarifValue," +
                        "   MAX(p.name_prm) AS TarifName, " +
                        " {9} AS TarifTypeCode," +
                        "'{2}' AS TarifTypeName," +

                        //период действия тарифа
                        "   p1.dat_s AS TarifStartDate, " +
                        "   p1.dat_po AS TarifEndDate, " +

                        //код дома МЖФ
                        "   CAST(MAX(trim(p4.val_prm)) AS NUMERIC(20)) AS BillingHouseCode," +

                        //кол-во ЛС, у которых проставлен данный тариф
                        "  COUNT(DISTINCT t.nzp_kvar) as LsCount " +

                        //" INTO TEMP {10} " +
                        " FROM " +
                        //получаем услугу
                        "  {0}.services s, " +
                        //получаем формулу
                        "  {0}.formuls ff, " +

                        //получаем название тарифа
                        "  {0}.prm_name p," +

                        //получаем поставщика
                        "  {0}.supplier sp, " +

                        "  {3} t" +

                        //получаем значение тарифа
                        " LEFT OUTER JOIN {1}.prm_{4} p1" +
                        "         ON ({5} AND p1.nzp_prm = t.nzp_prm_tarif " +
                        "                AND p1.is_actual<>100 " +
                        "                AND p1.dat_s <= '{6}'" +
                        "                AND p1.dat_po >= '{7}')" +

                        //получаем код дома МЖФ
                        " LEFT OUTER JOIN {1}.prm_4 p4" +
                        "         ON (p4.nzp = T .nzp_dom " +
                        "             AND p4.nzp_prm = 890 " +
                        "             AND p4.is_actual<>100 " +
                        "             AND p4.dat_s <= '{6}'" +
                        "             AND p4.dat_po >= '{7}')" +

                        " WHERE t.nzp_frm = ff.nzp_frm " +
                        " AND t.nzp_serv = s.nzp_serv " +
                        " AND t.nzp_prm_tarif = p.nzp_prm " +
                        //получить полный перечень тарифов, включая tarif is null
                        (finder.ShowAll ? "" : " AND p1.val_prm IS NOT NULL ") +
                        " AND t.nzp_wp = {8}" +
                        " AND t.nzp_supp = sp.nzp_supp" +
                        " GROUP BY 1,3,5,6,8,9,11,12,13,14 ",
                        billingLocalSchema.LocalSchemaPrefix + DataBankPostfixContainer.KernelPostfix,
                        billingLocalSchema.LocalSchemaPrefix + DataBankPostfixContainer.DataPostfix,
                        tarifType.GetDisplayName(),
                        tableTarifs,
                        prmNum,
                        fieldKey,
                        finder.DateTo.ToShortDateString(),
                        finder.DateFrom.ToShortDateString(),
                        nzpWp,
                        tarifType.GetHashCode(),
                        resultTable
                        );

                //считываем в промежуточный объект
                var tarifs = sqlExecutor.ExecuteSql<BilTarif>(sqlQuery);
                if (tarifs.Count() == 0)
                    return;
                try
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            //удаляем тарифы данного типа из данного локального банка
                            //за выбранный период
                            BilTarifRepository.GetAll()
                                .Where(x => x.BilService.Schema.Id == billingLocalSchema.Id
                                            && x.TarifTypeCode == tarifType.GetHashCode()
                                            && x.TarifStartDate <= finder.DateTo
                                            && x.TarifEndDate >= finder.DateFrom)
                                .ForEach(session.Delete);

                            //вставляем в БД МЖФ
                            (from billingTarif in tarifs

                                //сопоставляем с домами МЖФ для полученных тарифов
                                join mgfHouse in RealityObject.GetAll()
                                    .Where(x => x.CodeErc != null)
                                    on billingTarif.BillingHouseCode.ToString() equals mgfHouse.CodeErc

                                //сопоставляем с услугами для полученных тарифов
                                join billingService in BilServiceDictionary.GetAll()
                                    .Where(x => x.Schema.Id == billingLocalSchema.Id)
                                    on new {billingTarif.ServiceCode}
                                    equals new {billingService.ServiceCode}

                                select new BilTarifStorage
                                {
                                    BilService = billingService,
                                    RealityObject = mgfHouse,
                                    FormulaCode = billingTarif.FormulaCode,
                                    FormulaName = billingTarif.FormulaName,
                                    FormulaTypeCode = billingTarif.FormulaTypeCode,
                                    LsCount = billingTarif.LsCount,
                                    SupplierCode = billingTarif.SupplierCode,
                                    SupplierName = billingTarif.SupplierName,
                                    TarifCode = billingTarif.TarifCode,
                                    TarifName = billingTarif.TarifName,
                                    TarifTypeCode = billingTarif.TarifTypeCode,
                                    TarifTypeName = billingTarif.TarifTypeName,
                                    TarifValue = billingTarif.TarifValue,
                                    TarifStartDate = billingTarif.TarifStartDate,
                                    TarifEndDate = billingTarif.TarifEndDate
                                }).ForEach(x => session.Insert(x));

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                finally
                {
                    provider.CloseCurrentSession();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }



        /// <summary>
        /// Заполнение справочника bil_manorg_storage УО из нижних банков биллинга
        /// </summary>
        /// <returns></returns>
        public IDataResult FillManOrgStorageFromBillingBases()
        {
            var billingSchemasInfo = Container.Resolve<IRepository<BilDictSchema>>().GetAll();
            var provider = new ConnectionProvider(Container.Resolve<IConfigProvider>());
            var connectionString = "";
            DbConnection dbConnection = provider.CreateConnection();
            
                foreach (var schema in billingSchemasInfo.Where(x => x.IsActive==1).OrderBy(x => x.ConnectionString))
                {
                    if (connectionString != schema.ConnectionString)
                    {
                        if (dbConnection.State == ConnectionState.Open)
                        {
                            dbConnection.Close();
                            dbConnection.Dispose();
                        }
                        connectionString = schema.ConnectionString;
                        provider.Init(connectionString);
                        dbConnection = provider.CreateConnection();
                        dbConnection.Open();
                    }

                var dataSchema = schema.LocalSchemaPrefix + DataBankPostfixContainer.DataPostfix;
                var tempTableName = "t_bil_manorg" + DateTime.Now.Ticks;

                var sqlQuery = string.Format(
                    "DROP TABLE IF EXISTS {0}; " +
                        " CREATE TEMP TABLE {0} AS " +
                        " SELECT a.nzp_area AS manorg_code,  " +
                        " prm.nzp_prm AS prm_code,  " +
                        " MAX(prm.val_prm)::varchar AS prm_value " +

                        //справочник УО
                        "  FROM {1}.s_area a " +

                        //реестр параметров УО
                        " INNER JOIN {1}.prm_7 prm  " +
                        " 	ON (prm.nzp = a.nzp_area  " +
                        " 	AND prm.is_actual = 1  " +
                        " 	AND now() BETWEEN prm.dat_s AND prm.dat_po " +

                        //берем только 4 параметра (соответственно адрес, наименование, ИНН, КПП) 
                        " 	AND prm.nzp_prm IN (296,70,876,877)) " +
                        " GROUP BY 1,2 " +
                        " ORDER BY 1,2; ",
                    tempTableName,
                    dataSchema
                    );
                dbConnection.Execute(sqlQuery);



                sqlQuery =
                    string.Format(
                        " SELECT mo.manorg_code AS ManOrgCode, " +

                            //наименование УО
                            " mo.prm_value AS ManOrgName,  " +

                            //ИНН УО
                            " inn.prm_value AS ManOrgInn,  " +

                            //КПП УО
                            " kpp.prm_value AS ManOrgKpp,  " +

                            //адрес УО (юр. или факт. - неизвестно)
                            " address.prm_value  AS ManOrgAddress " +

                            " FROM {0} mo " +

                            //ИНН (параметр номер 876)
                            " INNER JOIN {0} inn ON (inn.manorg_code = mo.manorg_code AND inn.prm_code = 876) " +

                            //КПП (параметр номер 877)
                            " INNER JOIN {0} kpp ON (kpp.manorg_code = mo.manorg_code AND kpp.prm_code = 877) " +

                            //адрес (параметр номер 296)
                            " INNER JOIN {0} address ON (address.manorg_code = mo.manorg_code AND address.prm_code = 296) " +

                            //наименование (параметр номер 70)
                            " WHERE mo.prm_code = 70 " +
                            " GROUP BY 1,2,3,4,5 " +
                            " ORDER BY 1,2,3,4,5; ",
                        tempTableName);


                dbConnection.Query<BilManOrgStorage>(sqlQuery, commandTimeout:3600)

                    //исключаем УО, которые загружены ранее из этого банка данных
                    .Except(
                        BilManOrgStorage
                            .GetAll()
                            .Where(x => x.Schema.Id == schema.Id),
                        new BilManOrgComparer())
                    .ForEach(
                        x =>
                        {
                            x.Schema = schema;
                            BilManOrgStorage.Save(x);
                        }
                    );

                dbConnection.Execute("DROP TABLE IF EXISTS " + tempTableName);
            }
            return null;
        }



        /// <summary>
        /// Вспомогательная сущность для получения тарифа
        /// </summary>
        private class BilTarif
        {
            /// <summary>
            /// Код услуги в биллинге
            /// </summary>
            public virtual int ServiceCode { get; set; }

            
           
            /// <summary>
            /// Код дома биллинга
            /// </summary>
            public virtual long BillingHouseCode { get; set; }

            /// <summary>
            /// Код поставщика в биллинге
            /// </summary>
            public virtual int SupplierCode { get; set; }

            /// <summary>
            /// Наименование поставщика
            /// </summary>
            public virtual string SupplierName { get; set; }

            /// <summary>
            /// Код методики расчета в биллинге
            /// </summary>
            public virtual int FormulaCode { get; set; }

            /// <summary>
            /// Код типа получения тарифа методики расчета в биллинге
            /// </summary>
            public virtual int FormulaTypeCode { get; set; }

            /// <summary>
            /// Наименование методики расчета
            /// </summary>
            public virtual string FormulaName { get; set; }

            /// <summary>
            /// Код тарифа в биллинге
            /// </summary>
            public virtual int TarifCode { get; set; }

            /// <summary>
            /// Наименование тарифа
            /// </summary>
            public virtual string TarifName { get; set; }

            /// <summary>
            /// Код типа тарифа в биллинге
            /// </summary>
            public virtual int TarifTypeCode { get; set; }

            /// <summary>
            /// Наименование типа тарифа в биллинге
            /// </summary>
            public virtual string TarifTypeName { get; set; }

            /// <summary>
            /// Значение тарифа
            /// </summary>
            public virtual decimal TarifValue { get; set; }

            /// <summary>
            /// Дата начала действия тарифа
            /// </summary>
            public virtual DateTime? TarifStartDate { get; set; }

            /// <summary>
            /// Дата окончания действия тарифа
            /// </summary>
            public virtual DateTime? TarifEndDate { get; set; }

            /// <summary>
            /// Количество лицевых счетов
            /// </summary>
            public virtual long LsCount { get; set; }

        }

        /// <summary>
        /// Вспомогательная сущность для получения норматива
        /// </summary>
        private class BilNormativ
        {
            /// <summary>
            /// Код услуги в биллинге
            /// </summary>
            public virtual int ServiceCode { get; set; }

            /// <summary>
            /// Код типа норматива в биллинге
            /// </summary>
            public virtual int NormativeTypeCode { get; set; }

            /// <summary>
            /// Наименование типа норматива в биллинге
            /// </summary>
            public virtual string NormativeTypeName { get; set; }

            /// <summary>
            /// Код норматива в биллинге
            /// </summary>
            public virtual int NormativeCode { get; set; }

            /// <summary>
            /// Наименование норматива
            /// </summary>
            public virtual string NormativeName { get; set; }

            /// <summary>
            /// Описание наименования норматива
            /// </summary>
            public virtual string NormativeDescription { get; set; }

            /// <summary>
            /// Значение норматива
            /// </summary>
            public virtual string NormativeValue { get; set; }

            /// <summary>
            /// Дата начала действия норматива
            /// </summary>
            public virtual DateTime? NormativeStartDate { get; set; }

            /// <summary>
            /// Дата окончания действия норматива
            /// </summary>
            public virtual DateTime? NormativeEndDate { get; set; }

        }

        /// <summary>
        /// Вспомогательная сущность для вытаскивания расходов в ГКал
        /// </summary>
        private class GkalProxy
        {
            public int HouseId { get; set; }
            public int SupplierId { get; set; }
            public string Value { get; set; }
        }
    }
}

