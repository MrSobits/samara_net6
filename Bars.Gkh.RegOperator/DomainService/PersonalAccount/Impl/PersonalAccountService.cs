namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Export.ExportToEbir;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using NHibernate.Linq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Сервис лс
    /// </summary>
    public class PersonalAccountService : IPersonalAccountService
    {
        public IDomainService<PeriodSummaryBalanceChange> SaldoChangeDomain { get; set; }
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }
        public IDomainService<PersonalAccountPayment> PersonalAccountPaymentDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IWindsorContainer Container { get; set; }
        public IParameterTracker ParameterTracker { get; set; }
        public IChargePeriodRepository ChargePeriodRepo { get; set; }
        public IDomainService<PaysizeRecord> PaysizeRecordDomain { get; set; }
        public IDomainService<PaysizeRealEstateType> PaysizeRealEstTypeDomain { get; set; }
        public IDomainService<RealEstateTypeRealityObject> RealEstateTypeRoDomain { get; set; }
        public IRealityObjectDecisionsService UltimateDecisionService { get; set; }
        public IDomainService<PersonalAccountPaymentTransfer> TransferDomain { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }

        public IDataResult ListAccountsForComparsion(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var parentId = baseParams.Params.ContainsKey("parentId")
            ? baseParams.Params.GetValue("parentId").ToInt()
            : 0;
            if (parentId > 0)
            {

                //return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                return null;
            }
            else
            {
                return null;
            }

           
        }


        /// <summary>
        /// Получить тариф
        /// </summary>
        /// <param name="roId">Идентификатор дома</param>
        /// <param name="muId">Идентификатор муниципального образования</param>
        /// <param name="settlementId">Идентификатор поселения</param>
        /// <param name="date">Дата</param>
        /// <param name="roomId">Идентификатор помещения. Если передан, то тариф будет браться из подъезда</param>
        /// <returns>Тариф</returns>
        public decimal GetTariff(long roId, long muId, long? settlementId, DateTime date, long roomId = 0)
        {
            var settId = settlementId ?? 0L;

            var paysizeRecCache = this.PaysizeRecordDomain.GetAll()
                .Where(x => x.Municipality.Id == muId || x.Municipality.Id == settId)
                .Where(x => x.Value.HasValue)
                .Fetch(x => x.Paysize)
                .ToList()
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.AsEnumerable());

            var paysizeRetCache = this.PaysizeRealEstTypeDomain.GetAll()
                .Where(x => x.Record.Municipality.Id == muId || x.Record.Municipality.Id == settId)
                .Where(x => x.Value.HasValue)
                .Fetch(x => x.Record)
                .ThenFetch(x => x.Paysize)
                .ToList()
                .GroupBy(x => x.Record.Municipality.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => x.RealEstateType.Id)
                        .ToDictionary(x => x.Key, z => z.AsEnumerable()));

            //Если передан идентификатор помещения, то при расчете тарифа будет учитываться тариф подъезда (если он установлен)
            var roTypes = new List<long>();
            var room = roomId > 0 ? this.RoomDomain.Get(roomId) : null;

            if (room != null && room.Entrance != null && room.Entrance.RealEstateType != null)
            {
                roTypes.Add(room.Entrance.RealEstateType.Id);
            }
            else
            {
                roTypes = this.RealEstateTypeRoDomain.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Select(x => x.RealEstateType.Id)
                    .ToList();
            }

            var commonTariff = this.GetPaysizeByType(roTypes, paysizeRetCache.Get(settId), date)
                ?? this.GetPaysizeByType(roTypes, paysizeRetCache.Get(muId), date)
                    ?? this.GetPaysizeByMu(paysizeRecCache.Get(settId), date)
                        ?? this.GetPaysizeByMu(paysizeRecCache.Get(muId), date)
                            ?? 0;

            var roDecision = this.UltimateDecisionService
                .GetActualDecision<MonthlyFeeAmountDecision>(
                    new RealityObject { Id = roId },
                    true);

            if (roDecision != null && roDecision.Decision != null)
            {
                var current = roDecision.Decision
                    .Where(x => !x.To.HasValue || x.To >= date)
                    .FirstOrDefault(x => x.From <= date);

                if (current != null)
                {
                    var customTariff = current.Value;

                    return Math.Max(commonTariff, customTariff);
                }
            }

            return commonTariff;
        }

        /// <summary>
        /// Получить тариф
        /// </summary>
        /// <param name="account">Счёт</param>
        /// <param name="date">Дата</param>
        /// <returns>Тариф</returns>
        public decimal GetTariff(BasePersonalAccount account, DateTime date)
        {
            var roId = account
                .Return(x => x.Room)
                .Return(x => x.RealityObject)
                .Return(x => x.Id);

            var muId = account
                .Return(x => x.Room)
                .Return(x => x.RealityObject)
                .Return(x => x.Municipality)
                .Return(x => x.Id);

            var stlId = account
                .Return(x => x.Room)
                .Return(x => x.RealityObject)
                .Return(x => x.MoSettlement)
                .Return(x => x.Id);

            return this.GetTariff(roId, muId, stlId, date);
        }

        /// <summary>
        /// Получить список операций по периоду
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список операций</returns>
        public IDataResult ListOperations(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var openDate = loadParams.Filter.Get("OpenDate", DateTime.MinValue);
            var closeDate = loadParams.Filter.Get("CloseDate", DateTime.MaxValue);
            var accountId = baseParams.Params.GetAsId("accountId");

            var charges = this.PersonalAccountChargeDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == accountId && x.IsFixed)
                .Where(x => x.ChargeDate >= openDate && x.ChargeDate <= closeDate)
                .Select(
                    x => new
                    {
                        Date = x.ChargeDate,
                        Charged = x.ChargeTariff,
                        Recalc = x.RecalcByBaseTariff,
                        ChargedPenalty = x.Penalty,
                        Paid = 0m,
                        PaidPenalty = 0m
                    })
                .ToList();

            var payments = this.PersonalAccountPaymentDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == accountId)
                .Where(x => x.PaymentDate >= openDate && x.PaymentDate <= closeDate)
                .Select(
                    x => new
                    {
                        Date = x.PaymentDate,
                        Charged = 0m,
                        Recalc = 0m,
                        ChargedPenalty = 0m,
                        Paid = x.Type == PaymentType.Basic ? x.Sum : 0m,
                        PaidPenalty = x.Type == PaymentType.Penalty ? x.Sum : 0m
                    })
                .ToList();

            var saldoChanges = this.SaldoChangeDomain.GetAll()
                .Where(x => x.PeriodSummary.PersonalAccount.Id == accountId)
                .ToList()
                .Select(
                    x => new
                    {
                        Date = x.ObjectCreateDate,
                        Charged = x.NewValue - x.CurrentValue,
                        Recalc = 0m,
                        ChargedPenalty = 0m,
                        Paid = 0m,
                        PaidPenalty = 0m
                    })
                .ToList();

            charges.AddRange(payments);
            charges.AddRange(saldoChanges);

            var periods = this.ChargePeriodDomain.GetAll().ToList();

            var byPeriod = periods
                .Select(
                    x =>
                    {
                        var filtered = charges
                            .Where(c => c.Date >= x.StartDate)
                            .Where(c => !x.EndDate.HasValue || x.EndDate.Value >= c.Date)
                            .ToList();

                        return new
                        {
                            x.Id,
                            AccountId = accountId,
                            Date = x.Name,
                            Charged = filtered.SafeSum(y => y.Charged),
                            Recalc = filtered.SafeSum(y => y.Recalc),
                            ChargedPenalty = filtered.SafeSum(y => y.ChargedPenalty),
                            Paid = filtered.SafeSum(y => y.Paid),
                            PaidPenalty = filtered.SafeSum(y => y.PaidPenalty)
                        };
                    })
                .ToList();

            var operations = byPeriod
                .AsQueryable()
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .ToList();

            return new ListDataResult(operations, operations.Count);
        }

        /// <summary>
        /// Получить номера счетов, по идентификатору
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список номеров</returns>
        public IDataResult GetPersonalNumByAccount(BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAsId("accId");

            var loadParams = baseParams.GetLoadParam();
            var result = this.PersonalAccountDomain.GetAll()
                .Where(x => x.Id == accId)
                .Select(x => new { x.Id, x.PersonalAccountNum })
                .Filter(loadParams, this.Container);

            return new ListDataResult(result.Order(loadParams).ToArray(), result.Count());
        }

        /// <summary>
        /// Получить ЛС по дому
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список ЛС</returns>
        public IDataResult PersonalAccountsByRo(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var roId = baseParams.Params.GetAs("roId", 0L);

            var data = this.PersonalAccountDomain.GetAll()
                .Where(x => x.Room.RealityObject.Id == roId)
                .Select(
                    x =>
                        new
                        {
                            x.Id,
                            AccountNum = x.PersonalAccountNum,
                            x.Room.RoomNum,
                            AccountOwner = x.AccountOwner.Name
                        })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        /// <summary>
        /// Вернуть список статусов для закрытия лс
        /// </summary>
        /// <returns></returns>
        public List<State> GetCloseStates()
        {
            var result = this.StateDomain.GetAll()
                .Where(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "2")
                .ToList();

            result.Insert(
                0,
                new State
                {
                    Id = 0,
                    Name = "Автоматическое присвоение статуса"
                });

            return result;
        }

        public IDataResult GetTarifForRealtyObject(BaseParams baseParams)
        {
            var realtyObjectId = baseParams.Params.GetAs<long>("realtyObjectId");

            var value = this.PersonalAccountDomain.GetAll().FirstOrDefault(x => x.Room.RealityObject.Id == realtyObjectId);

            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var paymentSizeMuDomain = this.Container.Resolve<IDomainService<PaymentSizeMuRecord>>();
            var tariffCache = this.Container.Resolve<ITariffCache>();

            using (this.Container.Using(roDomain, paymentSizeMuDomain, tariffCache))
            {
                var query = roDomain.GetAll().Where(x => x.Id == realtyObjectId);

                tariffCache.Init(query);

                var realtyObject = query.FirstOrDefault();

                var tariff = value != null
                    ? this.ParameterTracker.GetParameter(
                        VersionedParameters.BaseTariff,
                        value,
                        this.ChargePeriodRepo.GetCurrentPeriod())
                        .GetActualByDate<decimal>(value, DateTime.Now.Date, false).Value
                    : paymentSizeMuDomain.GetAll()
                        .WhereIf(
                            realtyObject != null && realtyObject.Municipality != null,
                            x => x.Municipality.Id == realtyObject.Municipality.Id)
                        .OrderByDescending(x => x.PaymentSizeCr.DateEndPeriod)
                        .Select(x => (decimal?) x.PaymentSizeCr.PaymentSize)
                        .FirstOrDefault();

                return new BaseDataResult(tariff);
            }
        }

        /// <summary>
        /// Получение всех приходов за период
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetAccountChargeInfoInPeriod(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var periodId = baseParams.Params.GetAsId("periodId");
            var persAccIds = baseParams.Params.GetAs<string>("persAccIds").ToLongArray();

            var chargePeriod = this.ChargePeriodDomain.Get(periodId);
            var periodStart = chargePeriod.StartDate;
            var periodEnd = chargePeriod.GetEndDate();

            var persAccCharges = this.PersonalAccountChargeDomain.GetAll()
                .Where(x => persAccIds.Contains(x.BasePersonalAccount.Id))
                .Where(x => x.IsFixed)
                .Where(x => x.ChargeDate >= periodStart && x.ChargeDate <= periodEnd)
                .ToList()
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.BasePersonalAccount.Room.RealityObject.Municipality.Name,
                        x.BasePersonalAccount.Room.RealityObject.Address,
                        x.BasePersonalAccount.PersonalAccountNum,
                        ChargeSum = x.ChargeTariff,
                        CancellationSum = x.ChargeTariff
                    })
                .ToList();

            var result = persAccCharges
                .AsQueryable()
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                .Order(loadParam)
                .ToList();

            return new ListDataResult(result, persAccCharges.Count);
        }

        /// <summary>
        /// Получение всех юр. лиц
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListJurialContragents(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var acIds = baseParams.Params.GetAs("acIds", string.Empty).ToLongArray();
            var data = this.Container.Resolve<IDomainService<PersonalAccountOwner>>().GetAll()
                .Where(
                    x => x.OwnerType == PersonalAccountOwnerType.Legal
                        && this.PersonalAccountDomain.GetAll().Where(y => !acIds.Any() || acIds.Contains(y.Id)).Any(y => y.AccountOwner == x))
                .Select(x => new {x.Id, x.Name})
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Получить информацию по оплатам за все периоды
        /// </summary>
        /// <param name="baseParams"> baseParams </param>
        /// <returns> информацию по оплатам за все периоды </returns>
        public IDataResult ListPaymentsInfo(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var accountId = loadParam.Filter.GetAsId("accId");

            if (accountId == 0)
            {
                accountId = baseParams.Params.GetAsId("accId");
            }

            var result = this.ListPayments(accountId).AsQueryable().Filter(loadParam, this.Container);

            var totalCount = result.Count();

            return new ListDataResult(result.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        /// <summary>
        /// Получить информацию по оплатам за все периоды
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IList<PaymentProxy> ListPayments(long accountId)
        {
            var paymentsToClosedPeriodsImportDomain = this.Container.ResolveDomain<RecordPaymentsToClosedPeriodsImport>();
            var moneyOperationDomain = this.Container.ResolveDomain<MoneyOperation>();
            var bankDocImportDomain = this.Container.ResolveDomain<BankDocumentImport>();
            var bankAccStDomain = this.Container.ResolveDomain<BankAccountStatement>();
            var importedDomain = this.Container.ResolveDomain<ImportedPayment>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var paymentsDomain = this.Container.ResolveDomain<PaymentOperationBase>();
            var paymentCorrectionSourceDomain = this.Container.ResolveDomain<PaymentCorrectionSource>();

            try
            {
                var persAcc = this.PersonalAccountDomain.Get(accountId);

                var transferList = this.TransferDomain.GetAll()
                    .Fetch(x => x.Operation)
                    .Where(x => x.Owner == persAcc)
                    .Where(x => !x.IsInDirect)
                    .Where(x => !moneyOperationDomain.GetAll().Any(y => y.CanceledOperation.Id == x.Operation.Id) && x.Operation.CanceledOperation == null)
                    .ToList();

                var transferGuids = transferList.Select(x => x.Operation.OriginatorGuid).AsEnumerable().Distinct().ToList();

                var bankDocumentImportDict = importedDomain.GetAll()
                    .Where(x => x.PersonalAccount.Id == accountId)
                    .Where(x => transferGuids.Contains(x.BankDocumentImport.TransferGuid))
                    .Select(
                        x => new
                        {
                            x.BankDocumentImport.TransferGuid,
                            x.BankDocumentImport.DocumentNumber,
                            x.BankDocumentImport.DocumentDate,
                            x.BankDocumentImport.PaymentAgentCode,
                            x.BankDocumentImport.PaymentAgentName,
                            x.BankDocumentImport.ImportDate,
                            x.AcceptDate,
                            x.BankDocumentImport.Id,
                            x.PaymentType,
                            x.PaymentNumberUs,
                            x.Sum,
                            x.PaymentDate
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.PaymentDate)
                    .ToDictionary(
                        x =>
                            x.Key,
                        x => x.GroupBy(w => w.TransferGuid)
                            .ToDictionary(
                                s => s.Key,
                                s =>
                                {
                                    var sum = s.Where(
                                        z => z.PaymentType != ImportedPaymentType.Refund
                                            && z.PaymentType != ImportedPaymentType.PenaltyRefund).Sum(z => z.Sum);

                                    var refundSum = s.Where(
                                        z => z.PaymentType == ImportedPaymentType.Refund
                                            || z.PaymentType == ImportedPaymentType.PenaltyRefund).Sum(z => z.Sum);

                                    return s.Select(
                                        z => new
                                        {
                                            z.DocumentDate,
                                            z.PaymentType,
                                            RealSum = z.Sum,
                                            Sum =
                                                z.PaymentType != ImportedPaymentType.Refund && z.PaymentType != ImportedPaymentType.PenaltyRefund
                                                    ? sum
                                                    : (decimal?) null,
                                            RefundSum =
                                                z.PaymentType == ImportedPaymentType.Refund || z.PaymentType == ImportedPaymentType.PenaltyRefund
                                                    ? refundSum
                                                    : (decimal?) null,
                                            z.TransferGuid,
                                            z.DocumentNumber,
                                            z.PaymentAgentCode,
                                            z.PaymentAgentName,
                                            z.Id,
                                            z.ImportDate,
                                            z.PaymentNumberUs,
                                            z.PaymentDate,
                                            z.AcceptDate
                                        }).ToList();
                                }));

                var paymentsToClosedPeriodsImportDict = paymentsToClosedPeriodsImportDomain.GetAll()
                    .Where(x => transferGuids.Contains(x.PaymentOperation.OriginatorGuid))
                    .GroupBy(x => x.TransferGuid)
                    .AsEnumerable()

                    // Должна быть одна запись RecordPaymentsToClosedPeriodsImport на один или несколько трансферов
                    .ToDictionary(x => x.Key, y => y.Count() == 1 ? y.First() : null);

                var bankAccStInfoDict = bankAccStDomain
                    .GetAll()
                    .Where(x => transferGuids.Contains(x.TransferGuid))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.TransferGuid,
                            x.DocumentNum,
                            DocumentDate = (DateTime?) x.DocumentDate,
                            x.OperationDate,
                            x.DateReceipt,
                            x.DistributionDate,
                            x.DistributionCode
                        })
                    .AsEnumerable()
                    .ToDictionary(x => x.TransferGuid);

                var paymentCorrections = paymentCorrectionSourceDomain.GetAll()
                    .Where(x => x.PersonalAccount.Id == persAcc.Id)
                    .AsEnumerable()
                    .ToDictionary(x => x.OriginatorGuid);

                var transferSumAmount = transferList
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.SourceGuid,
                            x.TargetGuid,
                            x.OperationDate,
                            x.PaymentDate,
                            Reason = x.Reason ?? x.Operation.Reason,
                            x.Amount
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.PaymentDate)
                    .ToDictionary(
                        x => x.Key,
                        y => new[]
                        {
                            y.GroupBy(z => z.SourceGuid)
                                .ToDictionary(
                                    u => u.Key,
                                    v => v.Sum(w => w.Amount)),
                            y.GroupBy(z => z.TargetGuid)
                                .ToDictionary(
                                    u => u.Key,
                                    v => v.Sum(w => w.Amount))
                        });

                var transferSourceGuids = transferSumAmount.Values.SelectMany(x => x[0].Keys).Distinct().ToArray();

                var result = transferList
                    .Where(x => transferSourceGuids.Contains(x.SourceGuid))
                    .AsEnumerable()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.SourceGuid,
                            x.TargetGuid,
                            x.Operation.OriginatorGuid,
                            x.OperationDate,
                            x.PaymentDate,
                            Reason = x.Reason ?? x.Operation.Reason,
                            x.Amount,
                            x.ChargePeriod,
                            x.Operation.UserLogin
                        })
                    .ToList()
                    .Select(
                        x =>
                        {
                            var bankAccStInfo = bankAccStInfoDict.Get(x.SourceGuid) ?? bankAccStInfoDict.Get(x.TargetGuid);

                            var bankDocumentImportInfos = bankDocumentImportDict.Get(x.PaymentDate)?.Get(x.SourceGuid);

                            var refundbBankDocumentImportInfos = bankDocumentImportDict.Get(x.PaymentDate)?.Get(x.TargetGuid);

                            var bankDocumentImportInfo = bankDocumentImportInfos?.FirstOrDefault(
                                y =>
                                    y.PaymentDate == x.PaymentDate && y.Sum == transferSumAmount.Get(x.PaymentDate)?[0].Get(x.SourceGuid))??
                                refundbBankDocumentImportInfos?.FirstOrDefault(
                                    y =>
                                        y.PaymentDate == x.PaymentDate && y.RefundSum == transferSumAmount.Get(x.PaymentDate)?[1].Get(x.TargetGuid));

                            var importToClosePeriodRecord = paymentsToClosedPeriodsImportDict.Get(x.SourceGuid) ??
                                paymentsToClosedPeriodsImportDict.Get(x.TargetGuid);

                            var paymentCorrectionSource = x.OriginatorGuid.IsNotEmpty()
                                ? paymentCorrections.Get(x.OriginatorGuid)
                                : null;

                            var period = x.ChargePeriod;

                            var source = paymentCorrectionSource?.PaymentSource ??
                                importToClosePeriodRecord?.Source ??
                                    (bankAccStInfo != null
                                        ? TypeTransferSource.BankAccountStatement
                                        : bankDocumentImportInfo != null
                                            ? TypeTransferSource.BankDocumentImport
                                            : TypeTransferSource.NoDefined);

                            return new PaymentProxy
                            {
                                Id = x.Id,
                                Period = period.Return(y => y.Name),
                                PaymentDate = x.PaymentDate,
                                Reason = x.Reason,
                                Amount = x.Amount,
                                Source = source,
                                DocumentNum = source == TypeTransferSource.BankDocumentImport
                                    ? bankDocumentImportInfo.Return(y => y.DocumentNumber)
                                    : source == TypeTransferSource.BankAccountStatement
                                        ? bankAccStInfo.Return(y => y.DocumentNum)
                                        : source == TypeTransferSource.PaymentCorrection
                                            ? paymentCorrectionSource.Return(y => y.DocumentNumber)
                                            : importToClosePeriodRecord.Return(y => y.DocumentNum),
                                DocumentDate = source == TypeTransferSource.BankDocumentImport
                                    ? bankDocumentImportInfo.Return(y => y.PaymentDate)
                                    : source == TypeTransferSource.BankAccountStatement
                                        ? bankAccStInfo.Return(y => y.DocumentDate)
                                        : source == TypeTransferSource.PaymentCorrection
                                            ? paymentCorrectionSource.ReturnSafe(y => y.DocumentDate)
                                            : importToClosePeriodRecord.Return(y => y.DocumentDate),
                                PaymentAgentCode = bankDocumentImportInfo.Return(y => y.PaymentAgentCode),
                                PaymentAgentName = source == TypeTransferSource.BankDocumentImport
                                    ? bankDocumentImportInfo.Return(y => y.PaymentAgentName)
                                    : importToClosePeriodRecord.Return(y => y.PaymentAgentName),
                                ImportDate = bankDocumentImportInfo.Return(y => y.ImportDate),
                                PaymentType = bankDocumentImportInfo.Return(y => y.PaymentType.GetEnumMeta().Display),
                                PaymentNumberUs = source == TypeTransferSource.BankDocumentImport
                                    ? bankDocumentImportInfo.Return(y => y.PaymentNumberUs)
                                    : importToClosePeriodRecord.Return(y => y.PaymentNumberUs),
                                OperationDate = source == TypeTransferSource.BankAccountStatement
                                    ? bankAccStInfo.Return(y => y.OperationDate)
                                    : source == TypeTransferSource.PaymentCorrection
                                        ? paymentCorrectionSource.Return(y => y.FactOperationDate)
                                        : source == TypeTransferSource.BankDocumentImport
                                            ? bankDocumentImportInfo.Return(y => y.ImportDate)
                                            : importToClosePeriodRecord.Return(y => y.OperationDate),
                                DateReceipt = bankAccStInfo.Return(y => y.DateReceipt),
                                DistributionDate = bankAccStInfo.Return(y => y.DistributionDate),
                                DistributionCode = bankAccStInfo.Return(y => y.DistributionCode),
                                DocumentId = source == TypeTransferSource.BankDocumentImport
                                    ? bankDocumentImportInfo.Return(y => y.Id)
                                    : source == TypeTransferSource.BankAccountStatement
                                        ? bankAccStInfo.Return(y => y.Id)
                                        : source == TypeTransferSource.PaymentCorrection
                                            ? paymentCorrectionSource.ReturnSafe(y => y.Id)
                                            : importToClosePeriodRecord.Return(y => y.Id),
                                AcceptDate = source == TypeTransferSource.BankDocumentImport ? bankDocumentImportInfo.Return(y => y.AcceptDate) : null,
                                UserLogin = x.UserLogin
                            };
                        });

                return result.ToList();
            }
            finally
            {
                this.Container.Release(moneyOperationDomain);
                this.Container.Release(periodDomain);
                this.Container.Release(bankAccStDomain);
                this.Container.Release(bankDocImportDomain);
                this.Container.Release(periodDomain);
                this.Container.Release(importedDomain);
                this.Container.Release(paymentsDomain);
                this.Container.Release(paymentCorrectionSourceDomain);
            }
        }

        /// <summary>
        /// Получить идентификаторы счетов по адресу (полнотекстовый поиск)
        /// </summary>
        /// <param name="loadParams">Входные параметры</param>
        /// <returns>Идентификаторы счетов </returns>
        public long[] GetAccountIdsByAddress(LoadParam loadParams)
        {
            long[] result = null;

            if (loadParams != null && loadParams.ComplexFilter != null)
        {
                var rule = loadParams.GetFilterByName("RoomAddress");
                if (rule != null)
            {
                    var pattern = rule.Value.ToStr();
                    if (!string.IsNullOrWhiteSpace(pattern)
                        && pattern.StartsWith("%")
                        && ApplicationContext.Current.Configuration.DbDialect == DbDialect.PostgreSql)
            {
                        var sessionProvider = this.Container.Resolve<ISessionProvider>();
                        using (this.Container.Using(sessionProvider))
        {
                            var queryString = "SELECT rpc.ID"
                                + " FROM REGOP_PERS_ACC rpc"
                                + " LEFT OUTER JOIN GKH_ROOM room ON rpc.ROOM_ID = room.ID"
                                + " LEFT OUTER JOIN GKH_REALITY_OBJECT ro ON room.RO_ID = ro.ID"

                                // полнотекстовый поиск для Postgres
                                + " WHERE (ro.ADDRESS || ', кв. ' || room.CROOM_NUM || (case when coalesce(room.CHAMBER_NUM,'')  !='' then ', ком. ' || room.CHAMBER_NUM else '' end)) @@ '{0}'";
                            result = sessionProvider.CurrentSession.CreateSQLQuery(string.Format(queryString, pattern))
                                .List<object>()
                                .Select(x => x.To<long>())
                                .ToArray();
        }

                        if (result.IsNotEmpty())
        {
                            loadParams.DeleteRule("RoomAddress");
            }
            }
        }
                }

            return result;
        }

        public IDataResult SetBalance(BaseParams baseParams)
        {
            return this.Container.Resolve<IPersonalAccountChangeService>().ChangePeriodBalance(baseParams);
        }

        public IDataResult ExportToEbir(BaseParams baseParams)
        {
            var type = baseParams.Params.GetAs("type", string.Empty);
            var periodId = baseParams.Params.GetAs<long>("periodId");

            var ebirExports = this.Container.ResolveAll<IEbirExport>();

            using (this.Container.Using(ebirExports))
            {
                var typedExport = ebirExports.FirstOrDefault(x => x.Format == type);

                if (typedExport == null)
                {
                    return new BaseDataResult(false, "Неизвестный формат");
                }

                try
                {
                    var fileId = typedExport.GetExportFileId(periodId);

                    return new BaseDataResult(new {Id = fileId});
                }
                catch (Exception)
                {
                    return new BaseDataResult(false, "Произошла ошибка во время экспорта");
                }
            }
        }

        private DateTime GetPeriodStartDate(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId("periodId");

            var periodStartDate = DateTime.MaxValue;
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();

            var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);

            if (period != null)
            {
                periodStartDate = period.StartDate;
            }
            return periodStartDate;
        }

        private decimal? GetPaysizeByType(IEnumerable<long> roTypes, IDictionary<long, IEnumerable<PaysizeRealEstateType>> dict, DateTime date)
        {
            if (dict == null)
            {
                return null;
            }

            decimal? value = null;

            // получаем максимальный тариф по типу дома
            foreach (var roType in roTypes)
            {
                if (dict.ContainsKey(roType))
                {
                    if (dict[roType]
                        .Where(x => x.Record.Paysize.DateStart <= date)
                        .Any(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date))
                    {
                        value = Math.Max(
                            value ?? 0,
                            dict[roType]
                                .Where(x => x.Record.Paysize.DateStart <= date)
                                .Where(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date)
                                .Select(x => x.Value)
                                .Max() ?? 0);
                    }
                }
            }

            return value;
        }

        private decimal? GetPaysizeByMu(IEnumerable<PaysizeRecord> list, DateTime date)
        {
            if (list == null)
            {
                return null;
            }

            return list
                .OrderByDescending(x => x.Paysize.DateStart)
                .Where(x => !x.Paysize.DateEnd.HasValue || x.Paysize.DateEnd.Value >= date)
                .FirstOrDefault(x => x.Paysize.DateStart <= date)
                .Return(x => x.Value);
        }

        private Expression<Func<BasePersonalAccount, bool>> CreateExpressionForFilterValue(List<CrOwnerFilterType> crOwnerFilterTypes)
        {
            if (crOwnerFilterTypes == null)
            {
                return dto => true;
            }
            var parameterDto = Expression.Parameter(typeof(BasePersonalAccount), "dto");
            Expression propertyAccountOwner = Expression.Property(parameterDto, "AccountOwner");
            Expression propertyOwnerType = Expression.Property(propertyAccountOwner, "OwnerType");
            Expression constantLegal = Expression.Constant(PersonalAccountOwnerType.Legal);

            Expression totalExpression = null;
            var first = true;

            foreach (var currentOwnerType in crOwnerFilterTypes)
            {
                switch (currentOwnerType)
                {
                    case CrOwnerFilterType.LegalPerson:
                    {
                        Expression expression = Expression.Equal(propertyOwnerType, constantLegal);
                        if (first)
                        {
                            totalExpression = expression;
                            first = false;
                        }
                        else
                        {
                            totalExpression = Expression.OrElse(totalExpression, expression);
                        }
                    }
                        break;
                    case CrOwnerFilterType.PrysicalPerson:
                    {
                        Expression constantIndividual = Expression.Constant(PersonalAccountOwnerType.Individual);
                        Expression expression = Expression.Equal(propertyOwnerType, constantIndividual);
                        if (first)
                        {
                            totalExpression = expression;
                            first = false;
                        }
                        else
                        {
                            totalExpression = Expression.OrElse(totalExpression, expression);
                        }
                    }
                        break;
                    case CrOwnerFilterType.LegalPersonWithOneRoom:
                    {
                        Expression ownerEqualConstant = Expression.Equal(propertyOwnerType, constantLegal);

                        Expression properState = Expression.Property(parameterDto, "State");
                        Expression properStartState = Expression.Property(properState, "StartState");
                        Expression properActiveAccountsCount = Expression.Property(propertyAccountOwner, "ActiveAccountsCount");

                        Expression constantTrue = Expression.Constant(true);
                        Expression constantOne = Expression.Constant(1);

                        //dto => dto.State.StartState == true
                        Expression ownerHasOnlyRoom = Expression.Equal(properStartState, constantTrue);

                        //dto => dto.AccountOwner.ActiveAccountsCount == 1
                        Expression onlyOneActiveAccountsCount = Expression.Equal(properActiveAccountsCount, constantOne);

                        //dto => (dto.State.StartState == true && dto.AccountOwner.ActiveAccountsCount == 1)
                        Expression expressionAnd = Expression.AndAlso(ownerHasOnlyRoom, onlyOneActiveAccountsCount);

                        Expression ownerEqualConstantAndownerHasOnlyRoom = Expression.AndAlso(
                            ownerEqualConstant,
                            expressionAnd);
                        if (first)
                        {
                            totalExpression = ownerEqualConstantAndownerHasOnlyRoom;
                            first = false;
                        }
                        else
                        {
                            totalExpression = Expression.OrElse(totalExpression, ownerEqualConstantAndownerHasOnlyRoom);
                        }
                    }
                        break;
                }
            }
            if (totalExpression == null)
            {
                return dto => true;
            }
            var result = Expression.Lambda<Func<BasePersonalAccount, bool>>(totalExpression, parameterDto);
            return result;
        }
    }
}