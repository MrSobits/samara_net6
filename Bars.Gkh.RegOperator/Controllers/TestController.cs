namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.Repository.Wallets;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports;
    using Bars.Gkh.RegOperator.Quartz;
    using Bars.Gkh.RegOperator.Report.ReportManager;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2;
    using Bars.Gkh.RegOperator.Tasks.Period.Providers;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using global::Quartz;

    public class TestController : BaseController
    {
        public GkhCache Cache { get; set; }

        public ISessionProvider Sessions { get; set; }

        public IDomainService<RealityObject> RobjectDomain { get; set; }

        public ILogger LogManager { get; set; }

        public ActionResult Test()
        {
            var exec = Container.Resolve<ITaskExecutor>(PhysicalOwnerDocumentExecutor.Id);

            var prms = new BaseParams
                           {
                               Params =
                                   DynamicDictionary.Create()
                                       .SetValue("accounts", new[] { 2412, 3216, 2013 })
                                       .SetValue("accountNumbers", new[] { "000002733", "000002754" })
                                       .SetValue("periodDto", new PeriodDto { Id = 3618, Name = "lol" })
                                       .SetValue("path", "")
                                       .SetValue("owners", new[] { 1217 })
                           };
            
            var reportCopyRepo = Container.ResolveRepository<PaymentDocumentTemplate>();
            var chargeRepo = Container.ResolveRepository<ChargePeriod>();

            var period = chargeRepo.Load((long)3618);

            var manager = new PaymentDocReportManager(reportCopyRepo);
            manager.CreateTemplateCopyIfNotExist(period);

            exec.Execute(prms, null, null, CancellationToken.None);

            return null;
        }

        public ActionResult SetAreaShare()
        {
            var roomRepo = Container.ResolveRepository<Room>();
            var accountRepo = Container.ResolveRepository<BasePersonalAccount>();
            var llRepo = Container.ResolveRepository<EntityLogLight>();

            var accountQuery = accountRepo.GetAll();

            var roomsHasOneAccount =
                roomRepo.GetAll()
                    .Where(z => accountQuery.Any(x => x.Room.Id == z.Id && x.AreaShare == 0))
                    .Select(x => new { Room = x, Count = accountQuery.Count(z => z.Room.Id == x.Id) })
                    .AsEnumerable()
                    .Where(x => x.Count == 1)
                    .Select(x => x.Room)
                    .ToArray();

            var roomIds = roomsHasOneAccount.Select(x => x.Id).ToArray();

            var accounts = accountQuery.Where(x => roomIds.Contains(x.Room.Id)).ToDictionary(x => x.Room.Id);

            Container.InTransaction(
                () =>
                    {
                        foreach (var room in roomsHasOneAccount)
                        {
                            var acc = accounts.Get(room.Id);

                            if (acc == null)
                                continue;

                            acc.AreaShare = 1;

                            llRepo.Save(
                                new EntityLogLight
                                    {
                                        ClassName = "BasePersonalAccount",
                                        EntityId = acc.Id,
                                        PropertyName = "AreaShare",
                                        PropertyValue = 1.ToStr(),
                                        DateActualChange = new DateTime(2014, 10, 1),
                                        DateApplied = DateTime.UtcNow,
                                        Document = null,
                                        ParameterName = "area_share",
                                        User = "anonymous"
                                    });

                            accountRepo.Update(acc);
                        }
                    });

            return JsonNetResult.Success;
        }

        public ActionResult CreatePersonalAccounts()
        {
            var roomRepo = Container.ResolveRepository<Room>();
            var accountRepo = Container.ResolveRepository<BasePersonalAccount>();
            var indivOwnerRepo = Container.ResolveRepository<IndividualAccountOwner>();

            var accountQuery = accountRepo.GetAll();

            var roomHasNoAccounts = roomRepo.GetAll().Where(z => !accountQuery.Any(x => x.Room.Id == z.Id)).ToArray();

            var stateProvider = Container.Resolve<IStateProvider>();
            var accountNumberService = Container.Resolve<IAccountNumberService>();

            var accountIds = new List<long>();

            var acc = new BasePersonalAccount();

            stateProvider.SetDefaultState(acc);

            var defaultState = acc.State;

            foreach (var rooms in roomHasNoAccounts.Section(100))
            {
                IEnumerable<Room> rooms1 = rooms;
                Container.InTransaction(
                    () =>
                        {
                            foreach (var room in rooms1)
                            {
                                var owner = new IndividualAccountOwner
                                                {
                                                    FirstName = " ",
                                                    SecondName = " ",
                                                    Surname = " ",
                                                    OwnerType = PersonalAccountOwnerType.Individual,
                                                    IdentityNumber = "",
                                                    IdentitySerial = ""
                                                };

                                var account = new BasePersonalAccount
                                                  {
                                                      AccountOwner = owner,
                                                      AreaShare = 1,
                                                      Room = room,
                                                      OpenDate = DateTime.Today,
                                                      State = defaultState
                                                  };

                                accountNumberService.Generate(account);

                                indivOwnerRepo.Save(owner);
                                accountRepo.Save(account);

                                accountIds.Add(account.Id);
                            }
                        });
            }

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            session.CreateSQLQuery(@"
update gkh_entity_log_light
set date_actual=to_date('01.10.2014', 'dd.mm.yyyy')
where entity_id in (:ids) and param_name='area_share'").SetParameterList("ids", accountIds).ExecuteUpdate();

            return JsonNetResult.Success;
        }

        public ActionResult ActivateDecisions()
        {
            var crfundDecisionDomain = Container.ResolveDomain<CrFundFormationDecision>();
            var accountOwnerDecisionDomain = Container.ResolveDomain<AccountOwnerDecision>();
            var govDecisionDomain = Container.ResolveDomain<GovDecision>();
            var specCalcService = Container.Resolve<ISpecialCalcAccountService>();
            var govDecService = Container.Resolve<IGovDecisionAccountService>();

            var crFundDecisionCache =
                crfundDecisionDomain.GetAll()
                    .Where(x => x.Protocol.State.FinalState)
                    .Where(x => x.Protocol.DateStart <= DateTime.Today)
                    .Select(x => new { x.Protocol.RealityObject.Id, x.Protocol.DateStart, Decision = x })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

            var accountOwnerDecisionCache =
                accountOwnerDecisionDomain.GetAll()
                    .Where(x => x.Protocol.State.FinalState)
                    .Where(x => x.Protocol.DateStart <= DateTime.Today)
                    .Select(x => new { x.Protocol.RealityObject.Id, x.Protocol.DateStart, Decision = x })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

            var govDecisionCache =
                govDecisionDomain.GetAll()
                    .Where(x => x.State.FinalState)
                    .Where(x => x.DateStart <= DateTime.Today)
                    .Where(x => x.FundFormationByRegop)
                    .Select(x => new { x.RealityObject.Id, x.DateStart, Decision = x })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, z => z.OrderByDescending(x => x.DateStart).Select(x => x.Decision).First());

            var robjectsToUpdate = crFundDecisionCache.Select(x => x.Key).Union(govDecisionCache.Select(x => x.Key)).ToHashSet();

            foreach (var roId in robjectsToUpdate)
            {
                UpdateRoDecisions(roId, specCalcService, govDecService, crFundDecisionCache, accountOwnerDecisionCache, govDecisionCache);
            }

            return JsonNetResult.Success;
        }

        private void UpdateRoDecisions(
            long roId,
            ISpecialCalcAccountService specAccService,
            IGovDecisionAccountService govDecService,
            Dictionary<long, CrFundFormationDecision> crFundCache,
            Dictionary<long, AccountOwnerDecision> accOwnCache,
            Dictionary<long, GovDecision> govdecCache)
        {
            var ro = RobjectDomain.Get(roId);

            LogManager.LogDebug("Обновление протоколов, обрабатывается {0}", ro.Address);

            var activeProtocol = GetActiveProtocol(roId, crFundCache, govdecCache);

            var protocol = activeProtocol as GovDecision;
            if (protocol != null)
            {
                govDecService.SetPersonalAccountStateIfNeeded(protocol);

                specAccService.HandleSpecialAccountByProtocolChange(null, null, protocol, new RealityObject { Id = roId });
            }
            else
            {
                var decision = activeProtocol as CrFundFormationDecision;
                if (decision != null)
                {
                    specAccService.HandleSpecialAccountByProtocolChange(accOwnCache.Get(roId), decision, null, new RealityObject { Id = roId });
                }
            }
        }

        public ActionResult UpdateAll(long? accountId, string accountNumber)
        {
            try
            {
                IQueryable<RealityObject> query;

                if (accountId.HasValue || !accountNumber.IsEmpty())
                {
                    var repo = Container.ResolveRepository<CalcAccountRealityObject>();
                    query =
                        repo.GetAll()
                            .WhereIf(accountId.HasValue, x => x.Account.Id == accountId.Value)
                            .WhereIf(!accountNumber.IsEmpty(), x => x.Account.AccountNumber == accountNumber)
                            .Where(x => x.DateStart <= DateTime.Today)
                            .Where(x => !x.DateEnd.HasValue || x.DateEnd >= DateTime.Today)
                            .Select(x => x.RealityObject);
                }
                else
                {
                    var repo = Container.ResolveRepository<RealityObject>();
                    query = repo.GetAll();
                }

                Container.Resolve<IRealityObjectAccountUpdater>().UpdateBalance(query);
                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        private object GetActiveProtocol(long roId, Dictionary<long, CrFundFormationDecision> crFundCache, Dictionary<long, GovDecision> govdecCache)
        {
            var anotherProtocol = crFundCache.Get(roId);

            var oneMoreProtocol = govdecCache.Get(roId);

            if (anotherProtocol != null && oneMoreProtocol == null)
            {
                return anotherProtocol;
            }

            if (anotherProtocol == null && oneMoreProtocol != null)
            {
                return oneMoreProtocol;
            }

            if (anotherProtocol == null)
                return null;

            if (anotherProtocol.Protocol.DateStart > oneMoreProtocol.ProtocolDate)
            {
                return anotherProtocol;
            }

            return oneMoreProtocol;
        }

        public ActionResult FillWallets()
        {
            var logger = Container.Resolve<ILogger>();
            try
            {
                var refactorConvertor = new RefactorConvertor(Container);
                refactorConvertor.Process();
            }
            catch (Exception e)
            {
                logger.LogDebug("Ошибка при миграции данных", e);
            }

            return JsonNetResult.Success;
        }

        public ActionResult RollbackPeriod(BaseParams baseParams)
        {
            var periodToDeleteId = baseParams.Params.GetAsId("periodToDeleteId");
            var periodToRollbackId = baseParams.Params.GetAsId("periodToRollbackId");

            var periodDomain = Container.ResolveDomain<ChargePeriod>();

            var periodToDelete = periodDomain.Get(periodToDeleteId);
            var periodToRollback = periodDomain.Get(periodToRollbackId);

            if (periodToDeleteId == 0 && periodToRollbackId == 0)
                return JsonNetResult.Failure("Не указаны периоды. параметры: periodToDeleteId, periodToRollbackId");

            if (periodToDelete.Return(x => x.StartDate) != DateTime.MinValue)
            {
                if (periodDomain.GetAll().Where(x => x.Id != periodToDeleteId).Any(x => x.StartDate >= periodToDelete.StartDate))
                {
                    return JsonNetResult.Failure("Удаляемый период не является последним");
                }
            }

            if (periodToRollback.Return(x => x.StartDate) != DateTime.MinValue)
            {
                var cnt = periodDomain.GetAll().Where(x => x.Id != periodToRollbackId).Count(x => x.StartDate >= periodToRollback.StartDate);

                if (periodToDelete == null && cnt > 0)
                {
                    return JsonNetResult.Failure(@"Откатываемый период не является последним");
                }

                if (periodToDelete != null && cnt > 1)
                {
                    return JsonNetResult.Failure("Откатываемый период не является последним или предпоследним");
                }
            }

            IDataResult result;

            using (var openService = new ChargePeriodOpenService())
            {
                result = openService.RollbackPeriod(periodToRollback, periodToDelete);
            }

            return result.ToJsonResult();
        }

        public ActionResult Charge()
        {
            var repo = Container.ResolveRepository<BasePersonalAccount>();
            var period = Container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();
            var factory = Container.Resolve<IChargeCalculationImplFactory>();
            var cache = Container.Resolve<ICalculationGlobalCache>();

            UnacceptedCharge charge;

            using (Container.Using(repo))
            {
                var acc = repo.GetAll().First(x => x.PersonalAccountNum == "010037051");

                cache.Initialize(period, new[] { acc }.AsQueryable());

                charge = acc.CreateCharge(period, new UnacceptedChargePacket(), factory);
            }

            return
                new JsonNetResult(
                    new
                        {
                            charge.Charge,
                            charge.ChargeTariff,
                            charge.TariffOverplus,
                            charge.Penalty,
                            charge.RecalcPenalty,
                            charge.RecalcByBaseTariff,
                            charge.RecalcByDecision
                        });
        }

        public ActionResult CreateLostPersonalAccountSummaries(BaseParams baseParams)
        {
            var service = Container.Resolve<IRealtyObjectRegopOperationService>();
            var periodRepo = Container.Resolve<IChargePeriodRepository>();
            try
            {
                var period = periodRepo.GetCurrentPeriod();
                if (period == null)
                {
                    return JsonNetResult.Failure("Не удалось получить текущий открытый период начислений");
                }

                var roId = baseParams.Params.GetAs("roId", 0L);
                service.CreatePersonalAccountSummaries(period, roId > 0 ? new RealityObject { Id = roId } : null);

                return JsonNetResult.Success;
            }
            finally
            {
                Container.Release(service);
                Container.Release(periodRepo);
            }
        }

        public ActionResult Restart()
        {
            try
            {
                var session = Sessions.GetCurrentSession();

                session.CreateSQLQuery(@"
                    truncate RF_TRANSFER_CTR cascade;
                    truncate REGOP_MONEY_LOCK cascade;
                    truncate REGOP_PERS_ACC_NUM_VALUE cascade;
                    truncate gkh_entity_log_light cascade;
                    truncate regop_money_operation cascade;
                    truncate regop_wallet cascade;
                    truncate regop_suspense_account cascade;
                    truncate CR_OBJ_PER_ACT_PAYMENT cascade;
                    truncate gkh_room cascade;
                    truncate regop_period cascade;
                    truncate REGOP_UNACCEPT_C_PACKET cascade;
                    truncate REGOP_UNACCEPT_PAY_PACKET cascade;").ExecuteUpdate();

                CreatePeriod();
                CreateWallets();

                var r1 = CreateRoom("д. 1");
                var r2 = CreateRoom("д. 2");
                var r3 = CreateRoom("д. 2");

                var owner = GetOwner();

                CreatePersonalAccount(r1, owner, 0.5m);
                CreatePersonalAccount(r1, owner, 0.5m);
                CreatePersonalAccount(r2, owner, 1m);
                CreatePersonalAccount(r3, owner, 1m);
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }

            return new JsonNetResult(new BaseDataResult());
        }

        private void CreatePeriod()
        {
            var periodService = Container.Resolve<IChargePeriodService>();

            periodService.CreateFirstPeriod();
        }

        private void CreateWallets()
        {
            Container.Resolve<IExecutionAction>("RoAccountsGeneratorAction").Action();
        }

        private int _roomNumber;

        private Room CreateRoom(string address)
        {
            var ro = Container.ResolveRepository<RealityObject>().GetAll().FirstOrDefault(x => x.Address.Contains(address));

            var room = new Room(ro)
                           {
                               RoomNum = (++_roomNumber).ToString(new CultureInfo("ru-RU")),
                               OwnershipType = RoomOwnershipType.Private,
                               Area = 100m
                           };

            var romDomain = Container.ResolveRepository<Room>();
            romDomain.Save(room);

            return room;
        }

        private void CreatePersonalAccount(Room room, PersonalAccountOwner owner, decimal areaShare)
        {
            var factory = Container.Resolve<IPersonalAccountFactory>();

            var account = owner.CreateAccount(factory, room, new DateTime(2014, 6, 1), areaShare);

            account.PersAccNumExternalSystems = account.PersonalAccountNum;

            Container.ResolveDomain<BasePersonalAccount>().Save(account);

            //return account;
        }

        private PersonalAccountOwner GetOwner(string name = null)
        {
            var domain = Container.ResolveRepository<PersonalAccountOwner>();

            var owner = domain.GetAll().FirstOrDefault(x => name != null || x.Name.Contains(name));

            if (owner == null)
            {
                owner = new IndividualAccountOwner
                            {
                                Surname = "Бессмертный",
                                SecondName = "Кирдыкбабаевич",
                                FirstName = "Кощей",
                                IdentityNumber = "",
                                IdentitySerial = ""
                            };

                domain.Save(owner);
            }

            return owner;
        }

        public ActionResult TestImport()
        {
            var import = Container.Resolve<IGkhImport>(typeof(ChargesToClosedPeriodsImport).FullName);
            this.Container.Release(import);

            return JsSuccess();
        }

        public ActionResult GeneratePayments()
        {
            Container.InTransaction(
                () =>
                    {
                        var packetRepo = Container.ResolveRepository<UnacceptedPaymentPacket>();
                        var paymentRepo = Container.ResolveRepository<UnacceptedPayment>();
                        var accountRepo = Container.ResolveRepository<BasePersonalAccount>();
                        var periodRepo = Container.Resolve<IChargePeriodRepository>();

                        var accounts = accountRepo.GetAll().ToArray();

                        var packet = new UnacceptedPaymentPacket();

                        packetRepo.Save(packet);

                        var rnd = new Random();

                        var currentPeriod = periodRepo.GetCurrentPeriod();

                        var paymentDate = currentPeriod.StartDate.AddDays(5);

                        for (int i = 0; i < 100000; i++)
                        {
                            var number = rnd.Next(0, accounts.Length - 1);

                            var payment = new UnacceptedPayment
                                              {
                                                  Packet = packet,
                                                  PersonalAccount = accounts[number],
                                                  PaymentType = PaymentType.Basic,
                                                  PaymentDate = paymentDate,
                                                  Sum = (rnd.Next(0, 10000) / 100m),
                                                  Accepted = false
                                              };

                            paymentRepo.Save(payment);
                        }
                    });

            return JsSuccess();
        }

        /// <summary>
        /// Обновление баланса кошелька
        /// </summary>
        /// обновит баланс на всех кошельков значения wallets.HasNewOperations = true
        /// после обновления баланса кошелька переводит wallets.HasNewOperations = false
        /// <returns></returns>
        public ActionResult UpdateBalance()
        {

            var walletsDomain = this.Container.ResolveDomain<Wallet>();
            var walletRepository = this.Container.Resolve<IWalletRepository>();

            var wallets = walletsDomain.GetAll().Where(x => x.HasNewOperations);

            var portionSize = 1000;
            var totalCount = wallets.Count();
            var processedCount = 0;
            var walletsToProcess = new List<string>();

            try
            {
                while (processedCount <= totalCount)
                {
                    walletsToProcess = wallets.Skip(processedCount).Take(portionSize).Select(x => x.WalletGuid).ToList();

                    walletRepository.UpdateWalletBalance(walletsToProcess);

                    processedCount += portionSize;

                    walletsToProcess.Clear();
                }
            }
            finally
            {
                this.Container.Release(walletRepository);
                this.Container.Release(walletsDomain);
            }

            return this.JsSuccess();
        }

        /// <summary>
        /// Запуск закрытия периода с определенного этапа
        /// </summary>
        /// <param name="step">Этап</param>
        /// <returns></returns>
        public ActionResult StartClosePeriodStep(int step = 1)
        {
            var taskMan = this.Container.Resolve<ITaskManager>();

            using (this.Container.Using(taskMan))
            {
                switch (step)
                {
                    case 1:
                        {
                            taskMan.CreateTasks(new PeriodCloseTaskProvider_Step1(this.Container), new BaseParams());
                            break;
                        }
                    case 2:
                        {
                            taskMan.CreateTasks(new PeriodCloseTaskProvider_Step2(), new BaseParams());
                            break;
                        }
                    case 3:
                        {
                            taskMan.CreateTasks(new PeriodCloseTaskProvider_Step3(), new BaseParams());
                            break;
                        }
                }
            }

            return this.JsSuccess();
        }

        /// <summary>
        /// Запуск запланированной задачи Quartz
        /// </summary>
        public ActionResult StartScheduledTrigger(string triggerName, string triggerGroup)
        {
            var scheduler = this.Container.Resolve<IScheduler>();

            try
            {
                var key = new JobKey(triggerName, triggerGroup);

                if (scheduler.CheckExists(key).GetResultWithoutContext())
                {
                    scheduler.TriggerJob(key);
                }
                else
                {
                    // временно так, для тестов (ручного запуска), т.к. задачи нет в планировщике
                    if (triggerGroup == "RegopGroup" && triggerName == "SyncEmergencyObjects")
                    {
                        var job = JobBuilder.Create<TaskJob<SyncEmergencyObjectsTask>>().Build();
                        var trigger = TriggerBuilder.Create().WithIdentity(triggerName, triggerGroup).StartNow().Build();
                        scheduler.ScheduleJob(job, trigger);
                    }
                    else
                    {
                        return this.JsFailure("Указанный триггер не зарегистрирован в планировщике");
                    }
                }

                return this.JsSuccess();
            }
            catch (Exception exception)
            {
                return this.JsFailure(exception.Message);
            }
            finally
            {
                this.Container.Release(scheduler);
            }
        }

        /// <summary>
        /// Тестирование зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="id">Идентификатор ЛС</param>
        /// <returns>Успешность</returns>
        public ActionResult PerformedWorkDistributionApply(long id = 0)
        {
            var perfWorkDistribService = this.Container.Resolve<IPerformedWorkDistribution>();
            var accDomain = this.Container.ResolveDomain<BasePersonalAccount>();

            try
            {
                var accounts = accDomain.GetAll().Where(x => x.Id == id);

                perfWorkDistribService.DistributeForAccountPacket(accounts);

                return this.JsSuccess();
            }
            catch (Exception exception)
            {
                return this.JsFailure(exception.Message);
            }
            finally
            {
                this.Container.Release(perfWorkDistribService);
                this.Container.Release(accDomain);
            }
        }
    }
}