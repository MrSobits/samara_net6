namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Import;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Domain;
    using Domain.Repository;
    using Domain.ValueObjects;
    using DomainModelServices;
    using Entities.ValueObjects;
    using Gkh.Domain;
    using Gkh.Entities;
    using Entities;
    using Entities.Dict;
    using Enums;
    using DataContracts;
    using Gkh.Utils;
    using Castle.Windsor;
    using NHibernate;
    using NHibernate.Linq;

    public class SyncChargePaymentRkcHelper
    {
        private readonly IWindsorContainer _container;
        private ILogImport _logImport;
        private readonly IPersonalAccountPaymentCommandFactory _paymentCommandFactory;
        private readonly IRepository<PersonalAccountCharge> _chargeRepo;
        private readonly IRepository<Transfer> _transferRepo;

        private readonly IChargePeriodRepository _periodRepo;
        private readonly IRealtyObjectPaymentSession _paymentSession;

        private readonly Dictionary<int, Dictionary<bool, List<string>>> _logDict;
        private readonly List<PersistentObject> _entitiesForSave;
        private readonly HashSet<PersistentObject> _entitiesForDelete;

        //счета начислений, по которым нужно обновить балансы
        private readonly HashSet<long> _roForUpdate;

        public SyncChargePaymentRkcHelper(IWindsorContainer container)
        {
            _container = container;
            _paymentCommandFactory = container.Resolve<IPersonalAccountPaymentCommandFactory>();
            _chargeRepo = container.ResolveRepository<PersonalAccountCharge>();
            _transferRepo = container.ResolveRepository<Transfer>();
            _paymentSession = container.Resolve<IRealtyObjectPaymentSession>();
            _periodRepo = container.Resolve<IChargePeriodRepository>();
            _logDict = new Dictionary<int, Dictionary<bool, List<string>>>();
            _entitiesForSave = new List<PersistentObject>();
            _entitiesForDelete = new HashSet<PersistentObject>();
            _roForUpdate = new HashSet<long>();
        }

        private Dictionary<long, Dictionary<long, PersonalAccountPeriodSummary>> _paSummaries;
        private Dictionary<long, Dictionary<long, RealityObjectChargeAccountOperation>> _rochargeSummaries;
        private Dictionary<string, BasePersonalAccount> _personalAccountsByRkcNumber;
        private Dictionary<string, ChargePeriod> _periodsByDate;
        private List<ChargePeriod> _needPeriods;

        private int _rowNum = 1;
        private bool _isNotFoundPeriod;
        private bool _isNotFoundPersAcc;

        public ILogImport SyncChargePaymentRkc(SyncChargePaymentRkcRecord record, CashPaymentCenter cashPaymentCenter, SyncRkcResult result, ILogImport logImport)
        {
            _logImport = logImport;

            var monthDict = new Dictionary<string, int>
            {
                {"январь", 1},
                {"февраль", 2},
                {"март", 3},
                {"апрель", 4},
                {"май", 5},
                {"июнь", 6},
                {"июль", 7},
                {"август", 8},
                {"сентябрь", 9},
                {"октябрь", 10},
                {"ноябрь", 11},
                {"декабрь", 12}
            };
            
            var chargePeriodDomain = _container.ResolveDomain<ChargePeriod>();
            var regopServiceLogDomain = _container.ResolveDomain<RegopServiceLog>();
            var persAccDomain = _container.ResolveDomain<BasePersonalAccount>();

            var session = _container.Resolve<ISessionProvider>().GetCurrentSession();
            session.FlushMode = FlushMode.Never;

            try
            {
                var periods =
                    record.Payments
                        .Select(x => "{0}.{1}".FormatUsing(monthDict.Get(x.Month.ToLower()).ToStr(), x.Year))
                        .Distinct()
                        .ToList();

                _periodsByDate = chargePeriodDomain.GetAll()
                    .AsEnumerable()
                    .ToDictionary(x => "{0}.{1}".FormatUsing(x.StartDate.Month, x.StartDate.Year));

                _needPeriods = _periodsByDate.Where(x => periods.Contains(x.Key)).Select(x => x.Value).ToList();

                InitDictionaries(_needPeriods, cashPaymentCenter);

                foreach (var payment in record.Payments)
                {
                    var month = (payment.Month ?? "").ToLower().Trim();

                    var period =
                        _periodsByDate.Get("{0}.{1}".FormatUsing(monthDict.Get(month).ToStr(), payment.Year));

                    if (period == null)
                    {
                        AddLog(payment.AccNumber,
                            "Не найден период: {0} {1}".FormatUsing(payment.Month, payment.Year), false);
                        _isNotFoundPeriod = true;
                        continue;
                    }

                    var acc =
                        _personalAccountsByRkcNumber.Get("{0}_{1}".FormatUsing(payment.AccNumber,
                            cashPaymentCenter.Id));

                    if (acc == null)
                    {
                        AddLog(payment.AccNumber, "Лицевой счет отсутствует в системе", false);
                        _isNotFoundPersAcc = true;
                        continue;
                    }

                    ProcessLine(payment, acc, period);

                    _rowNum++;
                }

                session.FlushMode = FlushMode.Auto;
                session.Clear();

                WriteLogs();

                Save();

                //иначе не создается мьютекс
                _container.InTransaction(() => _paymentSession.Complete());

                UpdateRobjects();

                if (_isNotFoundPeriod)
                {
                    result.Code = "04";
                    result.Description = "Не найден период";
                }

                if (_isNotFoundPersAcc)
                {
                    result.Code = "03";
                    result.Description = "Лицевой(ые) счет(а) не найдены в системе";
                }
            }
            catch (Exception e)
            {
                _paymentSession.Rollback();
                _logImport.Error("Критическая ошибка",
                    string.Format("message: {0};stacktrace: {1}", e.Message, e.StackTrace));
            }
            finally
            {
                _container.Release(regopServiceLogDomain);
                _container.Release(chargePeriodDomain);
                _container.Release(persAccDomain);
            }

            return _logImport;
        }

        private void InitDictionaries(List<ChargePeriod> needPeriods, CashPaymentCenter cashPaymentCenter)
        {
            var accountDomain = _container.ResolveDomain<BasePersonalAccount>();
            var paSummaryDomain = _container.ResolveDomain<PersonalAccountPeriodSummary>();
            var rochargeSummaryDomain = _container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var persAccCashCenterDomain = _container.ResolveDomain<CashPaymentCenterPersAcc>();
            var roCashCenterDomain = _container.ResolveDomain<CashPaymentCenterRealObj>();

            try
            {
                var needPeriodIds = needPeriods.Select(x => x.Id).ToList();

                _personalAccountsByRkcNumber = accountDomain.GetAll()
                    .Join(persAccCashCenterDomain.GetAll(),
                        x => x.Id,
                        y => y.PersonalAccount.Id,
                        (a, b) => new {Acc = a, RkcRo = b})
                    .Where(x => x.Acc.PersAccNumExternalSystems != null
                                && x.Acc.PersAccNumExternalSystems != string.Empty)
                    .Where(x => x.RkcRo.DateStart <= DateTime.Today)
                    .Where(x => !x.RkcRo.DateEnd.HasValue || x.RkcRo.DateEnd >= DateTime.Today)
                    .Select(x => new
                    {
                        x.Acc,
                        x.RkcRo.CashPaymentCenter.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}".FormatUsing(x.Acc.PersAccNumExternalSystems, x.Id))
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Acc).First());

                var cachPaymentCenterConnectionType = _container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
                IQueryable<BasePersonalAccount> persAccQuery;

                if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
                {
                    var cashPaymentQuery = persAccCashCenterDomain.GetAll()
                        .Where(x => x.CashPaymentCenter.Id == cashPaymentCenter.Id)
                        .Where(x => x.DateStart <= DateTime.Today)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd >= DateTime.Today);

                    persAccQuery = accountDomain.GetAll()
                        .Where(x => cashPaymentQuery.Any(y => y.PersonalAccount.Id == x.Id));

                    _rochargeSummaries = rochargeSummaryDomain.GetAll()
                    .Fetch(x => x.Account)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => cashPaymentQuery.Any(z => z.PersonalAccount.Room.RealityObject.Id == x.Account.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.Account.RealityObject.Id)
                    .ToDictionary(x => x.Key, z => z.ToDictionary(x => x.Period.Id));
                }
                else
                {
                     var cashPaymentQuery = roCashCenterDomain.GetAll()
                        .Where(x => x.CashPaymentCenter.Id == cashPaymentCenter.Id)
                        .Where(x => x.DateStart <= DateTime.Today)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd >= DateTime.Today);

                    persAccQuery = accountDomain.GetAll()
                        .Where(x => cashPaymentQuery.Any(y => y.RealityObject.Id == x.Room.RealityObject.Id));

                    _rochargeSummaries = rochargeSummaryDomain.GetAll()
                    .Fetch(x => x.Account)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => cashPaymentQuery.Any(z => z.RealityObject.Id == x.Account.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.Account.RealityObject.Id)
                    .ToDictionary(x => x.Key, z => z.ToDictionary(x => x.Period.Id));
                }

                

                _paSummaries = paSummaryDomain.GetAll()
                    .Where(x => needPeriodIds.Contains(x.Period.Id))
                    .Where(z => persAccQuery.Any(x => x.Id == z.PersonalAccount.Id))
                    .GroupBy(x => x.PersonalAccount.Id)
                    .ToDictionary(x => x.Key, z => z.ToDictionary(x => x.Period.Id));

                
            }
            finally
            {
                _container.Release(persAccCashCenterDomain);
                _container.Release(accountDomain);
                _container.Release(paSummaryDomain);
                _container.Release(rochargeSummaryDomain);
            }
        }

        private void WriteLogs()
        {
            foreach (var log in _logDict.OrderBy(x => x.Key))
            {
                var rowNumber = string.Format("Строка {0}. Л/с с файла: {1}",
                    log.Key,
                    log.Value.FirstOrDefault().Return(x => x.Value.FirstOrDefault()));

                foreach (var rowLog in log.Value)
                {
                    if (rowLog.Key)
                    {
                        _logImport.Info(rowNumber, rowLog.Value.Skip(1).AggregateWithSeparator("."));
                        _logImport.CountAddedRows++;
                    }
                    else
                    {
                        _logImport.Warn(rowNumber, rowLog.Value.Skip(1).AggregateWithSeparator("."));
                    }
                }
            }
        }

        private void AddLog(string accNum, string message, bool success = true)
        {
            if (!_logDict.ContainsKey(_rowNum))
            {
                _logDict[_rowNum] = new Dictionary<bool, List<string>>();
            }

            var log = _logDict[_rowNum];

            if (!log.ContainsKey(success))
            {
                log[success] = new List<string> { accNum };
            }

            log[success].Add(message);
        }

        private void ProcessLine(SyncChargePayment payment, BasePersonalAccount account, ChargePeriod period)
        {
            var summary = _paSummaries.Get(account.Id).Return(x => x.Get(period.Id));

            if (summary == null)
            {
                AddLog(account.PersAccNumExternalSystems, "Отсутствует информация о лс за период {0}".FormatUsing(period.Name), false);
                return;
            }

            var currentPeriod = _periodRepo.GetCurrentPeriod();

            var saldoIn = payment.BalanceIn.ToDecimal();
            var saldoOut = payment.BalanceOut.ToDecimal();
            var chargedByBaseTariff = payment.ChargedSum.ToDecimal();
            var chargedPenalty = payment.ChargedPenalty.ToDecimal();
            var paidByBaseTariff = payment.PaidSum.ToDecimal();
            var paidPenalty = payment.PaidPenalty.ToDecimal();
            var recalc = payment.RecalcSum.ToDecimal();

            var paymentDate = payment.PaidDate.ToDateTime();

            //начисления и сальды перетираются
            summary.SaldoIn = saldoIn;
            summary.SaldoOut = saldoOut;
            summary.ChargedByBaseTariff = chargedByBaseTariff;
            summary.ChargeTariff = chargedByBaseTariff;
            summary.Penalty = chargedPenalty;
            summary.RecalcByBaseTariff = recalc;

            _entitiesForSave.Add(summary);

            var rochargeAccount = _rochargeSummaries.Get(account.Room.RealityObject.Id).Return(x => x.Get(period.Id));

            //если есть какая нибудь инфа по начислениям, то создаем начисление
            if (chargedByBaseTariff != 0 || chargedPenalty != 0 || recalc != 0)
            {
                var charge = new PersonalAccountCharge
                {
                    BasePersonalAccount = account,
                    ChargeDate = period.GetEndDate(),
                    ChargeTariff = chargedByBaseTariff,
                    Penalty = chargedPenalty,
                    RecalcByBaseTariff = recalc,
                    Guid = Guid.NewGuid().ToString(),
                    IsFixed = true,
                    ChargePeriod = period
                };
                _entitiesForSave.Add(charge);

                var moneyOperation = charge.CreateOperation();
                _entitiesForSave.Add(moneyOperation);

                var transfers = account.ApplyCharge(charge, moneyOperation);
                transfers.ForEach(_entitiesForSave.Add);

                if (rochargeAccount == null)
                {
                    AddLog(account.PersAccNumExternalSystems, "Отсутствует счет начислений жилого дома");
                }
                else
                {
                    rochargeAccount.ApplyCharge(charge);
                    _entitiesForSave.Add(rochargeAccount);

                    _roForUpdate.Add(rochargeAccount.Account.RealityObject.Id);

                    AddLog(account.PersAccNumExternalSystems, "Добавлена информация о начислениях за период {0}".FormatUsing(period.Name));
                }
            }
            else
            {
                AddLog(account.PersAccNumExternalSystems, "Отсутствует информация о начислениях за период {0}".FormatUsing(period.Name), false);
            }

            //если есть какая нибудь инфа по оплатам, то создаем их
            if (paidByBaseTariff != 0 || paidPenalty != 0)
            {
                var packet = new UnacceptedPaymentPacket();
                packet.Accept();
                _entitiesForSave.Add(packet);

                var moneyOperation = packet.CreateOperation(currentPeriod);
                _entitiesForSave.Add(moneyOperation);

                if (paidByBaseTariff > 0)
                {
                    var tariffPayment = new UnacceptedPayment
                    {
                        Packet = packet,
                        Accepted = true,
                        PaymentType = PaymentType.Basic,
                        Sum = paidByBaseTariff,
                        PersonalAccount = account,
                        PaymentDate = paymentDate
                    };
                    _entitiesForSave.Add(tariffPayment);

                    var command = _paymentCommandFactory.GetCommand(PaymentType.Basic);

                    var transfers = account.ApplyPayment(command, new MoneyStream(packet, moneyOperation, paymentDate, paidByBaseTariff));

                    transfers.ForEach(_entitiesForSave.Add);
                }

                if (paidPenalty > 0)
                {
                    var penaltyPayment = new UnacceptedPayment
                    {
                        Packet = packet,
                        Accepted = true,
                        PaymentType = PaymentType.Penalty,
                        PenaltySum = paidPenalty,
                        PersonalAccount = account,
                        PaymentDate = paymentDate
                    };
                    _entitiesForSave.Add(penaltyPayment);

                    var command = _paymentCommandFactory.GetCommand(PaymentType.Penalty);

                    var transfers = account.ApplyPayment(command, new MoneyStream(packet, moneyOperation, paymentDate, paidPenalty));

                    transfers.ForEach(_entitiesForSave.Add);
                }

                AddLog(account.PersAccNumExternalSystems, "Добавлена информация об оплатах за период {0}".FormatUsing(period.Name));
            }
            else
            {
                AddLog(account.PersAccNumExternalSystems, "Отсутствует информация об оплатах за период {0}".FormatUsing(period.Name), false);
            }

            var startDate = period.StartDate;
            var endDate = period.GetEndDate();

            //удаление существующих начислений
            var charges = _chargeRepo.GetAll()
                .Where(x => x.BasePersonalAccount.Id == account.Id)
                .Where(x => x.ChargeDate >= startDate)
                .Where(x => x.ChargeDate <= endDate);

            var transfersToDelete = _transferRepo.GetAll()
                .Where(x => charges.Any(z => z.Guid == x.TargetGuid));

            foreach (var charge in charges)
            {
                _entitiesForDelete.Add(charge);
            }

            foreach (var transfer in transfersToDelete)
            {
                _entitiesForDelete.Add(transfer);
            }
        }

        private void Save()
        {
            var session = _container.Resolve<ISessionProvider>().OpenStatelessSession();

            foreach (var item in _entitiesForDelete)
                session.Delete(item);

            TransactionHelper.InsertInManyTransactions(_container, _entitiesForSave, 1000, true, true);

            _entitiesForSave.Clear();
        }

        private void UpdateRobjects()
        {
            var roUpdater = _container.Resolve<IRealityObjectAccountUpdater>();

            var roChargeAccountRepo = _container.ResolveRepository<RealityObjectChargeAccountOperation>();
            var paSummaryRepo = _container.ResolveRepository<PersonalAccountPeriodSummary>();

            var needPeriodIds = _needPeriods.Select(x => x.Id).ToArray();

            var roForUpdate = _roForUpdate.ToArray();

            var chargeAccounts = roChargeAccountRepo.GetAll()
                .Where(x => roForUpdate.Contains(x.Account.RealityObject.Id))
                .Where(x => needPeriodIds.Contains(x.Period.Id))
                .ToArray();

            var paSummaries = paSummaryRepo.GetAll()
                .Where(x => needPeriodIds.Contains(x.Period.Id))
                .Where(x => roForUpdate.Contains(x.PersonalAccount.Room.RealityObject.Id))
                .Select(x => new
                {
                    RoId = x.PersonalAccount.Room.RealityObject.Id,
                    PeriodId = x.Period.Id,
                    Summary = x
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, z => z
                    .GroupBy(x => x.PeriodId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Summary).ToArray()));

            var forUpdate = new List<PersistentObject>();

            foreach (var operation in chargeAccounts)
            {
                var operation1 = operation;
                var summaries =
                    paSummaries.Get(operation.Account.RealityObject.Id)
                        .Return(x => x.Get(operation1.Period.Id));

                if (summaries != null && summaries.Any())
                {
                    operation.ChargedPenalty = summaries.Sum(x => x.Penalty);
                    operation.ChargedTotal = summaries.Sum(x => x.GetChargedByBaseTariff() + x.GetChargedByDecisionTariff() + x.Penalty);

                    operation.SaldoIn = summaries.Sum(x => x.SaldoIn);
                    operation.SaldoOut = summaries.Sum(x => x.SaldoOut);

                    forUpdate.Add(operation);
                }
            }

            TransactionHelper.InsertInManyTransactions(_container, forUpdate, 1000, true, true);

            roUpdater.UpdateBalance(_container.ResolveRepository<RealityObject>().GetAll().Where(x => roForUpdate.Contains(x.Id)));
        }
    }
}