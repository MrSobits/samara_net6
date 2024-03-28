using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl;
using Bars.Gkh.RegOperator.Entities.Loan;

namespace Bars.Gkh.RegOperator.Report.PaymentDocument
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.GkhCr.Entities;
    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using Gkh.Domain;
    using Gkh.Domain.Cache;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities.RealEstateType;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;
    using Gkh.Utils;
    using NHibernate.Linq;
    using Overhaul.Entities;
    using Domain.Interfaces;
    using Domain.ParametersVersioning;
    using Entities;
    using Entities.Dict;
    using Entities.PersonalAccount;
    using StimulReport;

    using Castle.Windsor;
    using Enums;

    [System.Obsolete("use class SnapshotCreator")]
    public class Receipt : StimulReport, IBaseReport
    {
        internal IWindsorContainer Container;
        internal IDomainService<RegoperatorParam> RegoperatorParamDomain;
        internal IParameterTracker ParamTracker;
        internal IDomainService<PeriodSummaryBalanceChange> SaldoChangeDomain;
        internal IDomainService<BasePersonalAccount> AccountDomain;
        internal IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain;
        internal IDomainService<PaymentDocInfo> PaymentDocInfoDomain;
        internal IRealityObjectDecisionsService DecisionService;
        internal IDomainService<RegOperator> RegOperatorDomain;
        internal IDomainService<DecisionNotification> DecisionNotificationDomain;
        internal IDomainService<RealityObjectDecisionProtocol> RealObjDecisionProtocolDomain;
        internal IDomainService<AccountOwnerDecision> AccountOwnerDecisionDomain;
        internal IDomainService<CalcAccountRealityObject> RegopCalcAccountRealityObjectDomain;
        internal IDomainService<CreditOrgDecision> CreditOrgDecisionDomain;
        internal IDomainService<CreditOrg> CreditOrgDomain;
        internal IDomainService<ManOrgContractRealityObject> MoRoDomain;
        internal IDomainService<PersonalAccountPayment> PersAccountPaymentDomain;
        internal IDomainService<PaymentDocument> PaymentDocumentDomain;
        internal GkhCache Cache;
        internal IDomainService<RealityObjectPaymentAccountOperation> RoPaymentAccOperationService;
        internal IDomainService<RealityObjectLoanPayment> RealityObjectLoanPaymentService;
        internal IDomainService<RealObjLoanPaymentAccOper> RealObjLoanPaymentAccOperService;
        internal IDomainService<RealObjPaymentAccOperPerfAct> RealObjPaymentAccOperPerfActService;
        internal IDomainService<PaymentSrcFinanceDetails> PaymentSrcFinanceDetailsService;
        internal IDomainService<RegOperatorMunicipality> RegOperatorMunicipality;

        public Receipt(IWindsorContainer container)
        {
            Container = container;
            RegoperatorParamDomain = container.ResolveDomain<RegoperatorParam>();
            ParamTracker = container.Resolve<IParameterTracker>();
            SaldoChangeDomain = container.ResolveDomain<PeriodSummaryBalanceChange>();
            AccountDomain = container.ResolveDomain<BasePersonalAccount>();
            PeriodSummaryDomain = container.ResolveDomain<PersonalAccountPeriodSummary>();
            PaymentDocInfoDomain = container.ResolveDomain<PaymentDocInfo>();
            DecisionService = container.Resolve<IRealityObjectDecisionsService>();
            RegOperatorDomain = container.ResolveDomain<RegOperator>();
            DecisionNotificationDomain = container.ResolveDomain<DecisionNotification>();
            RealObjDecisionProtocolDomain = container.ResolveDomain<RealityObjectDecisionProtocol>();
            AccountOwnerDecisionDomain = container.ResolveDomain<AccountOwnerDecision>();
            RegopCalcAccountRealityObjectDomain = container.ResolveDomain<CalcAccountRealityObject>();
            CreditOrgDecisionDomain = container.ResolveDomain<CreditOrgDecision>();
            CreditOrgDomain = container.ResolveDomain<CreditOrg>();
            MoRoDomain = container.ResolveDomain<ManOrgContractRealityObject>();
            PersAccountPaymentDomain = container.ResolveDomain<PersonalAccountPayment>();
            PaymentDocumentDomain = Container.ResolveDomain<PaymentDocument>();
            Cache = Container.Resolve<GkhCache>();
            RoPaymentAccOperationService = container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            RealityObjectLoanPaymentService = container.ResolveDomain<RealityObjectLoanPayment>();
            RealObjLoanPaymentAccOperService = container.ResolveDomain<RealObjLoanPaymentAccOper>();
            PaymentSrcFinanceDetailsService = container.ResolveDomain<PaymentSrcFinanceDetails>();
            RegOperatorMunicipality = container.ResolveDomain<RegOperatorMunicipality>();
        }

        public long PeriodId { get; set; }

        public long[] AccountIds { get; set; }

        public long[] OwnerIds { get; set; }

        // флажок нулевых печатных форм.Если IsZeroPaymentDoc = true, то не выводим сведения о расчете
        public bool IsZeroPaymentDoc = false;

        protected Dictionary<long, BankProxy> Banks { get; set; }

        protected RegOperator RegOperator { get; set; }

        protected HashSet<long> RealObjsWithCustomOwnDecType = new HashSet<long>();

        protected HashSet<long> RealObjsWithRegopCrFoundType = new HashSet<long>();

        protected Dictionary<long, CrFundFormationDecisionType> CrFoundType = new Dictionary<long, CrFundFormationDecisionType>();

        protected HashSet<long> RealObjsWithRegopOwnType = new HashSet<long>();
        
        private IDomainService<CalculationParameterTrace> _calcParamTraceDomain;
        protected IDomainService<CalculationParameterTrace> CalcParamTraceDomain
        {
            get
            {
                return _calcParamTraceDomain
                    ?? (_calcParamTraceDomain = Container.ResolveDomain<CalculationParameterTrace>());
            }
        }

        private IDomainService<PersonalAccountCharge> _accountChargeDomain;
        protected IDomainService<PersonalAccountCharge> AccountChargeDomain
        {
            get
            {
                return _accountChargeDomain
                    ?? (_accountChargeDomain = Container.ResolveDomain<PersonalAccountCharge>());
            }
        }

        public string Name
        {
            get { return "Документ на оплату"; }
        }
        
        public virtual Stream GetTemplate()
        {
            return new ReportTemplateBinary(Properties.Resources.PaymentDocumentReceipt).GetTemplate();
        }

        public void PrepareReport(ReportParams reportParams)
        {
            var period = Container.ResolveDomain<ChargePeriod>().Get(PeriodId);

            if (period == null)
            {
                throw new ReportProviderException("Не удалось получить период");
            }

            if (!IsZeroPaymentDoc && !period.IsClosed)
            {
                throw new ReportProviderException("Выгрузить оплаты можно только для закрытого периода!");
            }

            this.DataSources.Add(new MetaData
            {
                SourceName = "Записи",
                MetaType = nameof(Object),
                Data = GetData(period)
            });
        }

        protected virtual object GetData(ChargePeriod period)
        {
            var data = GetCommonData<ExtendedReportRecord>(period);

            var paypenaltyDomain = Cache.GetCache<PaymentPenalties>();
            var indivaccownerDomain = Cache.GetCache<IndividualAccountOwner>();
            var currentPercentPenalty = paypenaltyDomain.GetEntities()
                .OrderByDescending(x => x.DateStart)
                .AsQueryable()
                .WhereIf(period.EndDate.HasValue, x => x.DateStart <= period.EndDate)
                .FirstOrDefault(x => !x.DateEnd.HasValue || x.DateEnd >= period.StartDate);

            var payBeforeDate = IsZeroPaymentDoc
                ? string.Empty
                : period.StartDate.AddMonths(1)
                    .AddDays(currentPercentPenalty.Return(x => x.Days))
                    .ToShortDateString();

            var ownerIds = data.Select(x => x.OwnerId).Distinct().ToList();

            var accountOwnersDict = indivaccownerDomain.GetEntities()
                .Where(x => ownerIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Surname,
                    x.FirstName,
                    x.SecondName
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            var doc = this.RegoperatorParamDomain.GetAll().FirstOrDefault(item => item.Key == "PaymentDocFormat");
            var depersonalized = false;
            if(doc != null)
            {
                int value;
                if(int.TryParse(doc.Value, out value))
                {
                    var enumValue = (PaymentDocumentFormat)value;
                    if(enumValue == PaymentDocumentFormat.Depersonalized)
                    {
                        depersonalized = true;
                    }
                }
            }

            foreach (var account in data)
            {
                if (accountOwnersDict.ContainsKey(account.OwnerId) && !depersonalized)
                {
                    var accountOwner = accountOwnersDict[account.OwnerId];

                    account.ФИОСобственника = string.Format(
                        "{0} {1} {2}", accountOwner.Surname, accountOwner.FirstName, accountOwner.SecondName);
                }

                account.ОплатитьДо = !IsZeroPaymentDoc ? payBeforeDate : string.Empty;
            }
            //}

            return data;
        }

        /// <summary>
        /// Подготовка словарей для дальнейшей работы 
        /// Внимание все кеши формиуются в PaymentDocumentService.InitCache
        /// </summary>
        protected virtual void WarmCache(ChargePeriod period)
        {           

           _accountSaldoChanges = Cache.GetCache<PeriodSummaryBalanceChange>().GetEntities()//SaldoChangeDomain.GetAll()
                .Where(x => AccountIds.Contains(x.PeriodSummary.PersonalAccount.Id))
                .Where(x => x.ObjectCreateDate >= period.StartDate && x.ObjectCreateDate <= period.EndDate)
                .Select(x => new
                {
                    AccId = x.PeriodSummary.PersonalAccount.Id,
                    x.NewValue,
                    x.CurrentValue
                })
                .AsEnumerable()
                .GroupBy(x => x.AccId)
                .ToDictionary(x => x.Key, x => x.SafeSum(e => e.CurrentValue - e.NewValue));

            //var rentPaymentDomain = Container.ResolveDomain<RentPaymentIn>();
            //var accumFundsDomain = Container.ResolveDomain<AccumulatedFunds>();

            //using (Container.Using(rentPaymentDomain, accumFundsDomain))
            //{
            var rentPayments = Cache.GetCache<RentPaymentIn>().GetEntities()//rentPaymentDomain.GetAll()
                   .Where(x => AccountIds.Contains(x.Account.Id))
                   .Where(x => x.OperationDate >= period.StartDate)
                   .Where(x => x.OperationDate <= period.EndDate)
                   .Select(x => new { AccId = x.Account.Id, x.Sum })
                   .ToArray();

            var accumulatedFunds = Cache.GetCache<AccumulatedFunds>().GetEntities()//accumFundsDomain.GetAll()
                   .Where(x => AccountIds.Contains(x.Account.Id))
                   .Where(x => x.OperationDate >= period.StartDate)
                   .Where(x => x.OperationDate <= period.EndDate)
                   .Select(x => new { AccId = x.Account.Id, x.Sum })
                   .ToArray();

               _accountAdditionalPayments = rentPayments
                   .Union(accumulatedFunds)
                   .GroupBy(x => x.AccId)
                   .ToDictionary(x => x.Key, x => x.Sum(y => y.Sum));
            //}

            _periodSummariesAll = Cache.GetCache<PersonalAccountPeriodSummary>().GetEntities()//PeriodSummaryDomain.GetAll()
                .Where(x => AccountIds.Contains(x.PersonalAccount.Id))
                .OrderByDescending(x => x.Period.StartDate)
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Period.StartDate).ToList());

            _paymentDocInfoList = Cache.GetCache<PaymentDocInfo>().GetEntities()//PaymentDocInfoDomain.GetAll()
                .Where(x => (!period.EndDate.HasValue || x.DateStart <= period.EndDate) && (!x.DateEnd.HasValue || x.DateEnd >= period.StartDate))
                .ToList();

            var realObjIds = Cache.GetCache<BasePersonalAccount>().GetEntities()//AccountDomain.GetAll()
              .Where(x => AccountIds.Contains(x.Id))
              .Select(x => x.Room.RealityObject.Id);

            RegOperator = Cache.GetCache<RegOperator>().GetByKey(ContragentState.Active.ToString());
                //RegOperatorDomain.GetAll().FirstOrDefault(x => x.Contragent.ContragentState == ContragentState.Active);

            _manOrgByRealObjDict = Cache.GetCache<ManOrgContractRealityObject>().GetEntities()//MoRoDomain.GetAll()
                .Where(x => realObjIds.Contains(x.RealityObject.Id))
                .Where(x =>
                        x.ManOrgContract.StartDate <= period.StartDate
                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= period.StartDate))
                .Select(x => new
                {
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.ManagingOrganization,
                    x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    y => y.OrderByDescending(x => x.StartDate).Select(x => x.ManagingOrganization).First());

            //var protocolIds = Cache.GetCache<RealityObjectDecisionProtocol>().GetEntities()//RealObjDecisionProtocolDomain.GetAll()
            //        .Where(x => x.State.FinalState)
            //        .Where(x => realObjIds.Contains(x.RealityObject.Id))
            //        .Select(x => new
            //        {
            //            x.ProtocolDate, 
            //            RoId = x.RealityObject.Id, 
            //            x.Id
            //        })
            //        .AsEnumerable()
            //        .GroupBy(x => x.RoId)
            //        .Select(x => x.OrderByDescending(d => d.ProtocolDate).Select(d => d.Id).FirstOrDefault())
            //        .ToList();




            // получаем только те дома у которых тип Решения = Специальный
            RealObjsWithRegopCrFoundType = Cache.GetCache<CrFundFormationDecision>().GetEntities()
                .Where(x => x.Decision == CrFundFormationDecisionType.SpecialAccount)
                .Select(x => x.Protocol.RealityObject.Id)
                .AsEnumerable()
                .ToHashSet();

            // словарь в котором идет по дому к какому типу фонда он относится
            CrFoundType =
                Cache.GetCache<CrFundFormationDecision>()
                     .GetEntities()
                     .Where(
                         x =>
                         x.Decision == CrFundFormationDecisionType.SpecialAccount
                         || x.Decision == CrFundFormationDecisionType.RegOpAccount)
                     .Select(x => new { x.Protocol.RealityObject.Id, x.Decision })
                     .AsEnumerable()
                     .GroupBy(x => x.Id)
                     .ToDictionary(x => x.Key, y => y.Select(z => z.Decision).First());


            // получаем только те дома у которых тип Владелеца счета =  Рег. оператор
            RealObjsWithRegopOwnType = Cache.GetCache<AccountOwnerDecision>().GetEntities()
                .Where(x => x.DecisionType == AccountOwnerDecisionType.RegOp)
                .Select(x => x.Protocol.RealityObject.Id)
                .AsEnumerable()
                .ToHashSet();

            monthlyFeeDecisions = Cache.GetCache<MonthlyFeeAmountDecision>().GetEntities()
                .GroupBy(x => x.Protocol.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.Protocol.ProtocolDate).First());

            paysizeRecCache = Cache.GetCache<PaysizeRecord>().GetEntities()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

            paysizeRetCache = Cache.GetCache<PaysizeRealEstateType>().GetEntities()
                    .GroupBy(x => x.Record.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y
                        .GroupBy(x => x.RealEstateType.Id)
                        .ToDictionary(x => x.Key, z => z.ToList()));

            realEstTypesByRo = Cache.GetCache<RealEstateTypeRealityObject>().GetEntities()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.RealEstateType.Id).ToList());

            var activeNotifAccNum = Cache.GetCache<DecisionNotification>().GetEntities()//DecisionNotificationDomain.GetAll()
                    //.Where(x => protocolIds.Contains(x.Protocol.Id))
                    .Select(x => new
                    {
                        RoId = x.Protocol.RealityObject.Id,
                        x.AccountNum
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.AccountNum).FirstOrDefault());

            RealObjsWithCustomOwnDecType = Cache.GetCache<AccountOwnerDecision>().GetEntities()//AccountOwnerDecisionDomain.GetAll()
                .Where(x => /*protocolIds.Contains(x.Protocol.Id) && */x.DecisionType == AccountOwnerDecisionType.Custom)
                .Select(x => x.Protocol.RealityObject.Id)
                .AsEnumerable()
                .ToHashSet();

            var contragentBanks = Cache.GetCache<CalcAccountRealityObject>().GetEntities()//RegopCalcAccountRealityObjectDomain.GetAll()
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
                .Where(x => x.Account.AccountOwner.Id == RegOperator.Contragent.Id)
                .Where(x => realObjIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    ((RegopCalcAccount)x.Account).ContragentCreditOrg.SettlementAccount,
                    Name = ((RegopCalcAccount)x.Account).ContragentCreditOrg.CreditOrg.Name ?? x.Account.CreditOrg.Name,
                    Bik = ((RegopCalcAccount)x.Account).ContragentCreditOrg.CreditOrg.Bik ?? x.Account.CreditOrg.Bik,
                    CorrAccount = ((RegopCalcAccount)x.Account).ContragentCreditOrg.CreditOrg.CorrAccount ?? x.Account.CreditOrg.CorrAccount,
                    Address = ((RegopCalcAccount)x.Account).ContragentCreditOrg.CreditOrg.Address ?? x.Account.CreditOrg.Address
                })
                .AsEnumerable()
                .Select(x => new BankProxy
                {
                    RoId = x.RoId,
                    SettlementAccount = x.SettlementAccount,
                    Name = x.Name,
                    Bik = x.Bik,
                    CorrAccount = x.CorrAccount,
                    Address = x.Address
                });

            Banks = Cache.GetCache<CreditOrgDecision>().GetEntities()//CreditOrgDecisionDomain.GetAll()
                //.Where(x => protocolIds.Contains(x.Protocol.Id))
                .Select(x => new 
                {
                    RoId = x.Protocol.RealityObject.Id,
                    x.Decision.Name,
                    x.Decision.Bik,
                    x.Decision.CorrAccount,
                    x.Decision.Address,
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => new BankProxy
                {
                    RoId = x.RoId,
                    SettlementAccount = activeNotifAccNum.Get(x.RoId),
                    Name = x.Name,
                    Bik = x.Bik,
                    CorrAccount = x.CorrAccount,
                    Address = x.Address,
                }).FirstOrDefault());

            foreach (var bank in contragentBanks)
            {
                Banks[bank.RoId] = bank;
            }

            _socialSupportDict = Cache.GetCache<PersonalAccountPayment>().GetEntities()//PersAccountPaymentDomain.GetAll()
                .Where(x => AccountIds.Contains(x.BasePersonalAccount.Id))
                .Where(x => x.PaymentDate >= period.StartDate)
                .Where(x => x.PaymentDate <= period.EndDate)
                .Where(x => x.Type == PaymentType.SocialSupport)
                .Select(x => new
                {
                    x.BasePersonalAccount.Id,
                    x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Sum));
        }

        protected List<T> GetCommonData<T>(ChargePeriod period) where T : ReportRecord, new()
        {
            WarmCache(period);

            var accCache = Cache.GetCache<BasePersonalAccount>();

            var accounts = accCache.GetEntities() //AccountDomain.GetAll()
                .Where(x => AccountIds.Contains(x.Id))
                .Select(x => new PersonalAccountProxy
                {
                    Id = x.Id,
                    RoomNum = x.Room.RoomNum,
                    IsRoomHasNoNumber = x.Room.IsRoomHasNoNumber,
                    PostCode = x.Room.RealityObject.FiasAddress.PostCode,
                    AddressName = x.Room.RealityObject.FiasAddress.AddressName,
                    Area = x.Room.Area,
                    AreaShare = x.AreaShare,
                    MunicipalityId = x.Room.RealityObject.Municipality.Id,
                    OwnerId = x.AccountOwner.Id,
                    PersonalAccountNum = x.PersonalAccountNum,
                    OwnerType = x.AccountOwner.OwnerType,
                    Account = x,
                    PlaceGuidId = x.Room.RealityObject.FiasAddress.PlaceGuidId,
                    MuId = x.Room.RealityObject.Municipality.Id,
                    SettlementId = x.Room.RealityObject.MoSettlement.Return(y => y.Id, (long?)null),
                    RoId = x.Room.RealityObject.Id,
                    Notation = x.Room.Notation,
                    AreaLivingNotLivingMkd = x.Room.RealityObject.AreaLivingNotLivingMkd
                })
                .OrderBy(x => x.AddressName)
                .ThenBy(x => x.RoomNum.ToInt())
                .ToList();

            var periodSummaryCache = Cache.GetCache<PersonalAccountPeriodSummary>();

            var allSummaries = periodSummaryCache.GetEntities()
                .Where(x => AccountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Period.StartDate <= period.StartDate)
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y));

            var periodSummaryDict = periodSummaryCache.GetEntities()//Container.ResolveDomain<PersonalAccountPeriodSummary>().GetAll()
                .Where(x => AccountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Period.Id == PeriodId)
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var realObjIds = accounts.Select(x => x.RoId).ToArray();
       
            var roPaymentAccOperCreditList = RoPaymentAccOperationService.GetAll()
                .Where(x => (realObjIds.Contains(x.Account.RealityObject.Id)))
                .Where(x => (x.Date >= period.StartDate && x.Date < period.EndDate))
                .Where(x => (x.OperationType == PaymentOperationType.OutcomeAccountPayment))
                .AsEnumerable()
                .GroupBy(x => x.Account.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.OperationSum));

            var innRegOper = RegOperatorMunicipality.GetAll().ToList()
                .AsEnumerable()
                .Select(x => new {idMumicipal = x.Municipality.Id, innRegOper = x.RegOperator.Contragent.Inn})               
                .ToDictionary(x => x.idMumicipal, x => x.innRegOper);

            var accountChargeData =
                AccountChargeDomain.GetAll()
                    .Where(x => x.ChargeDate >= period.StartDate)
                    .Where(x => x.ChargeDate <= period.EndDate)
                    .Select(x => new
                    {
                        accountId = x.BasePersonalAccount.Id,
                        x.Guid
                    })
                    .ToList();

            var calcParamTraceData = CalcParamTraceDomain.GetAll()
                    .Where(x => x.CalculationType == CalculationTraceType.Charge)
                    .Select(x => new { x.ParameterValues, x.CalculationGuid });

            var data = new List<T>();

            var documentDate = DateTime.Today.ToShortDateString();
            foreach (var account in accounts)
            {
                var rec = GetRecord<T>(account, period, documentDate);

                //вычисление площади
                //доля собственности
                var areaShare = 0m;
                //площадь
                decimal area = 0m;
                //площадь*долю собственности
                decimal areaRoom = 0m;

                decimal tarif = 0m;

                var accountChargeGuid = accountChargeData.Where(y => y.accountId == account.Id).Select(y => y.Guid).FirstOrDefault();

                if (accountChargeGuid != null)
                {
                    var acountCalcParamTraceData = calcParamTraceData.FirstOrDefault(y => accountChargeGuid == y.CalculationGuid);

                    //но такого не должно быть!
                    if (acountCalcParamTraceData != null)
                    {
                        areaShare = acountCalcParamTraceData.ParameterValues.Get(VersionedParameters.AreaShare).ToDecimal();
                        tarif = acountCalcParamTraceData.ParameterValues.Get(VersionedParameters.BaseTariff).ToDecimal();
                        area = acountCalcParamTraceData.ParameterValues.Get(VersionedParameters.RoomArea).ToDecimal();
                        areaRoom = areaShare * area;
                    }
                }
                else
                {
                    var tarifParam = ParamTracker.GetParameter(VersionedParameters.BaseTariff, account.Account, period);

                    tarif = tarifParam.GetActualByDate<decimal>(
                        account.Account,
                        period.EndDate ?? period.StartDate.AddMonths(1).AddDays(-1), false).Value;

                    var areaParam = ParamTracker.GetParameter(VersionedParameters.RoomArea, account.Account, period);
                    area = areaParam.GetActualByDate<decimal?>(
                        account.Account,
                        period.EndDate ?? period.StartDate, true, true).Value ?? account.Area;

                    var areaShareParam = ParamTracker.GetParameter(VersionedParameters.AreaShare, account.Account, period);

                    areaShare = areaShareParam.GetActualByDate<decimal?>(
                        account.Account,
                        period.EndDate ?? period.StartDate,
                        true).Value ?? account.AreaShare;

                    areaRoom = area * areaShare;
                }

                rec.ОбщаяПлощадь = areaRoom;
                rec.Тариф = tarif;

                if (periodSummaryDict.ContainsKey(account.Id) && !IsZeroPaymentDoc)
                {
                    var periodSummary = periodSummaryDict[account.Id];

                    var startPenaltyDebt = GetStartPenaltyDebt(period, account.Account);

                    // "Переплата/Задолженность на начало периода" = "Сальдо на начало периода" - "Пени на начало пероида"
                    // т.к. в periodSummary уже все агрегировано

                    decimal startDebt = periodSummary.SaldoIn - startPenaltyDebt;
                    rec.ПереплатаНаНачало = startDebt < 0 ? -startDebt : 0;
                    rec.ДолгНаНачало = startDebt > 0 ? startDebt : 0;

                    rec.ПереплатаПениНаНачало = startPenaltyDebt < 0 ? -startPenaltyDebt : 0;
                    rec.ДолгПениНаНачало = startPenaltyDebt > 0 ? startPenaltyDebt : 0;

                    rec.Начислено = periodSummary.ChargeTariff + GetSaldoChange(account.Account);
                    rec.Перерасчет = periodSummary.RecalcByBaseTariff;
                    rec.Оплачено = periodSummary.TariffPayment + GetAdditionalPayments(account.Account);

                    var endOverPay = startDebt + rec.Начислено + rec.Перерасчет - _socialSupportDict.Get(account.Id) - rec.Оплачено;
                    rec.ПереплатаНаКонец = endOverPay < 0 ? -endOverPay : 0;
                    rec.ДолгНаКонец = endOverPay > 0 ? endOverPay : 0;

                    rec.ОплаченоПени = periodSummary.PenaltyPayment;
                    rec.НачисленоПени = periodSummary.Penalty;

                    var penaltyEndDebt = startPenaltyDebt + rec.НачисленоПени - rec.ОплаченоПени;
                    rec.ПереплатаПениНаКонец = penaltyEndDebt < 0 ? -penaltyEndDebt : 0;
                    rec.ДолгПениНаКонец = penaltyEndDebt > 0 ? penaltyEndDebt : 0;
                    
                    
#warning Добавлено только для смоленска после переноса удалить
                    // TODO fix recalc
                    rec.ПотраченоНаКР = roPaymentAccOperCreditList.ContainsKey(account.RoId) 
                        ?  Math.Round((decimal)((roPaymentAccOperCreditList[account.RoId] / account.AreaLivingNotLivingMkd) * account.Area), 2, MidpointRounding.AwayFromZero) : 0;
                    rec.УплаченоФКР = Math.Round(Math.Abs(allSummaries.Get(account.Id)
                        .ReturnSafe(x => x.Sum(y => y.TariffPayment + y.TariffDecisionPayment))), 2, MidpointRounding.AwayFromZero);
                    rec.ШтрихКод = innRegOper.ContainsKey(account.MunicipalityId) ? innRegOper[account.MunicipalityId].Substring(5, 5) + rec.ЛицевойСчет : string.Empty;
                }

                rec.Итого = rec.ДолгНаКонец;
                rec.Пени = rec.ДолгПениНаКонец;
                rec.СоцПоддержка = _socialSupportDict.Get(account.Id);
                rec.СуммаВсего = rec.ДолгНаКонец + rec.ДолгПениНаКонец;
                rec.ИтогоКОплате =  rec.Итого + rec.Пени - rec.СоцПоддержка;
                rec.ПредоставленнаяМСП = _socialSupportDict.Get(account.Id);                

                data.Add(rec);
            }

            return data;
        }

        protected virtual T GetRecord<T>(PersonalAccountProxy account, ChargePeriod period, string documentDate) where T : ReportRecord, new()
        {
            var contragent = RealObjsWithCustomOwnDecType.Contains(account.RoId) ? _manOrgByRealObjDict.Get(account.RoId).Return(x => x.Contragent) : RegOperator.Return(x => x.Contragent);
            var bank = Banks.Get(account.RoId);

            var crFoundMessage = (RealObjsWithRegopCrFoundType.Contains(account.RoId) && RealObjsWithRegopOwnType.Contains(account.RoId) 
                ? "Дом формирует фонд капитального ремонта на специальном счёте" : string.Empty);

            var crFoundtypeStr = string.Empty;
            
            if (CrFoundType.ContainsKey(account.RoId))
            {
                var type = CrFoundType[account.RoId];
                if ( type == CrFundFormationDecisionType.SpecialAccount)
                {
                    crFoundtypeStr = "Специальный счет";
                }
                else if ( type == CrFundFormationDecisionType.RegOpAccount)
                {
                    crFoundtypeStr = "Счет регионального оператора";
                }
            }

            // Если у квартиры выставлена галочка, что дом у квартиры отсутсвует ЛС и при этом указано Примечание
            // То вместо лицевого счета ставим Примечание из карточки квартиры

            var personalaccount = !IsZeroPaymentDoc
                ? account.PersonalAccountNum
                : string.Empty;
            
            var info = _paymentDocInfoList
                .Where(x => x.RealityObject == null || x.RealityObject.Id == account.RoId)
                .Where(x => x.LocalityAoGuid == null || x.LocalityAoGuid == account.PlaceGuidId)
                .Where(x => x.MoSettlement == null || x.MoSettlement.Id == account.SettlementId)
                .Where(x => x.Municipality == null || x.Municipality.Id == account.MuId)
                .Select(x => new
                    {
                        x.Information, 
                        // Частная информация приоритетнее общей
                        Priority = x.RealityObject != null ? 1 :
                                        x.LocalityAoGuid != null ? 2 :
                                    x.MoSettlement != null ? 3 : 4
                    })
                .GroupBy(x => x.Priority)
                .Select(x => new { Priopity = x.Key, info = x.Select(y => y.Information).ToList() })
                .OrderBy(x => x.Priopity)
                .Select(x => x.info.AggregateWithSeparator("\n"))
                .FirstOrDefault();
           
            return new T
            {
                Id = account.Id,                
                ФондСпецСчет = crFoundMessage,
                СпособФормированияФонда = crFoundtypeStr,
                OwnerId = account.OwnerId,
                ЛицевойСчет = personalaccount,
                НаименованиеПериода = period.Name,
                ДатаНачалаПериода = period.StartDate.ToString("dd.MM.yyyy"),
                ДатаОкончанияПериода = period.EndDate.HasValue ? period.EndDate.Value.ToString("dd.MM.yyyy") : string.Empty,
                МесяцГодНачисления = period.StartDate.ToString("MMMM yyyy"),
                Индекс = account.PostCode,
                АдресКвартиры = string.Format("{0} {1}", account.AddressName, account.IsRoomHasNoNumber ? account.Notation : ", кв. " +  account.RoomNum),
                ДатаДокумента = documentDate,
                НаименованиеПолучателя = contragent != null ? contragent.Name : string.Empty,
                ИннПолучателя = contragent != null ? contragent.Inn : string.Empty,
                КппПолучателя = contragent != null ? contragent.Kpp : string.Empty,
                ОргнПолучателя = contragent != null ? contragent.Ogrn : string.Empty,
                РсчетПолучателя = bank.Return(x => x.SettlementAccount, string.Empty),
                АдресБанка = bank.Return(x => x.Address, string.Empty),
                НаименованиеБанка = bank.Return(x => x.Name, string.Empty),
                БикБанка = bank.Return(x => x.Bik, string.Empty),
                КсБанка = bank.Return(x => x.CorrAccount, string.Empty),
                ТелефоныПолучателя = contragent != null ? contragent.Phone : string.Empty,
                АдресПолучателя = contragent != null ? contragent.MailingAddress : string.Empty,
                EmailПолучателя = contragent != null ? contragent.Email : string.Empty,
                WebSiteПолучателя = contragent != null ? contragent.OfficialWebsite : string.Empty,
                Информация = info,
                ЗначениеQRКода = !IsZeroPaymentDoc ? account.PersonalAccountNum : string.Empty,
                ШтрихКод = ""
                        
            };
        }

        protected int GetDocNumber(long accountId, long periodId)
        {
            var cache = Cache.GetCache<PaymentDocument>();

            // Если в рамках периода по ЛС уже печатали счета, то номер выводим уже существующий

            var existingNum = cache.GetByKey("{0}|{1}|{2}".FormatUsing(accountId, periodId, DateTime.Today.Year));

            //paymentDocumentDomain.GetAll()
            //.Where(x => x.AccountId == accountId)
            //.Where(x => x.PeriodId == periodId)
            //.Where(x => x.Year == DateTime.Today.Year)
            //.Select(x => (int?)x.Number)
            //.FirstOrDefault();

            int documentNumber;

            if (existingNum.IsNull())
            {
                var maxExistingNumber = cache.GetEntities()
                    .Where(x => x.Year == DateTime.Today.Year)
                    .Max(x => (int?)x.Number) ?? 0;

                documentNumber = maxExistingNumber + 1;

                var newRecord = new PaymentDocument
                {
                    AccountId = accountId,
                    PeriodId = periodId,
                    Year = DateTime.Today.Year,
                    Number = documentNumber
                };

                PaymentDocumentDomain.Save(newRecord);
                cache.AddEntity(newRecord);
            }
            else
            {
                documentNumber = existingNum.Number;
            }

            return documentNumber;
        }

        protected decimal GetRobjectTariff(long roId, long muId, long? settlId)
        {
            var settlementId = settlId ?? 0L;

            var roDecision = monthlyFeeDecisions.Get(roId);

            if (roDecision != null && roDecision.Decision != null)
            {
                var current = roDecision.Decision
                    .Where(x => !x.To.HasValue || x.To >= DateTime.Today)
                    .FirstOrDefault(x => x.From <= DateTime.Today);

                if (current != null)
                {
                    return current.Value;
                }
            }

            var roTypes = realEstTypesByRo.Get(roId);

            return (GetPaysizeByType(roTypes, paysizeRetCache.Get(settlementId), DateTime.Today)
                    ?? GetPaysizeByType(roTypes, paysizeRetCache.Get(muId), DateTime.Today)
                    ?? GetPaysizeByMu(paysizeRecCache.Get(settlementId), DateTime.Today)
                    ?? GetPaysizeByMu(paysizeRecCache.Get(muId), DateTime.Today)
                    ?? 0).ToDecimal();
        }

        private decimal? GetPaysizeByType(IEnumerable<long> roTypes, IDictionary<long, List<PaysizeRealEstateType>> dict, DateTime date)
        {
            if (dict == null)
            {
                return null;
            }

            decimal? value = null;

            //получаем максимальный тариф по типу дома
            foreach (var roType in roTypes)
            {
                if (dict.ContainsKey(roType))
                {
                    if (dict[roType]
                        .Where(x => x.Record.Paysize.DateStart <= date)
                        .Any(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date))
                    {
                        value = Math.Max(value ?? 0,
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

        private decimal GetStartPenaltyDebt(ChargePeriod period, BasePersonalAccount account)
        {
            if (_periodSummariesAll == null || !_periodSummariesAll.ContainsKey(account.Id))
            {
                return 0;
            }

            var summaries = _periodSummariesAll[account.Id].Where(x => x.Period.StartDate < period.StartDate).ToList();

            if (!summaries.Any())
            {
                return 0m;
            }

            return summaries.Sum(x => x.Penalty - x.PenaltyPayment);
        }

        private decimal GetSaldoChange(BasePersonalAccount account)
        {
            return _accountSaldoChanges.Get(account.Id);
        }

        // оплата аренды и ранее накопленные средства
        private decimal GetAdditionalPayments(BasePersonalAccount account)
        {
            return _accountAdditionalPayments.Get(account.Id);
        }

        private decimal GetLastTariff(BasePersonalAccount account, ChargePeriod period)
        {
            var end = period.EndDate.HasValue
                ? period.EndDate.Value
                : period.StartDate.AddMonths(1).AddDays(-1);

            var tariffParam = ParamTracker.GetParameter(VersionedParameters.BaseTariff, account, period);

            return tariffParam.GetActualByDate<decimal>(account, end, false).Value;
        }


        private decimal GetKRSum(long roId, ChargePeriod period)
        {
           
            var roPaymentAccOperCreditList = RoPaymentAccOperationService.GetAll()
                .Where(x => (x.Account.RealityObject.Id == roId))
                .Where(x => (x.Date >= period.StartDate && x.Date < period.EndDate))
                .Where(x => (x.OperationType == PaymentOperationType.OutcomeAccountPayment))
                .AsEnumerable().Sum(x => x.OperationSum);


            return roPaymentAccOperCreditList;
        }


        #region private fields
        private List<PaymentDocInfo> _paymentDocInfoList = new List<PaymentDocInfo>();
        private Dictionary<long, decimal> _accountSaldoChanges = new Dictionary<long, decimal>();
        private Dictionary<long, decimal> _accountAdditionalPayments = new Dictionary<long, decimal>();
        private Dictionary<long, List<PersonalAccountPeriodSummary>> _periodSummariesAll;
        private Dictionary<long, ManagingOrganization> _manOrgByRealObjDict;
        private Dictionary<long, decimal> _socialSupportDict = new Dictionary<long, decimal>();
        private Dictionary<long, CrFundFormationDecisionType> _crFormDict = new Dictionary<long, CrFundFormationDecisionType>();
        private Dictionary<long, MonthlyFeeAmountDecision> monthlyFeeDecisions = new Dictionary<long, MonthlyFeeAmountDecision>();
        private Dictionary<long, List<PaysizeRecord>> paysizeRecCache = new Dictionary<long, List<PaysizeRecord>>();
        private Dictionary<long, Dictionary<long, List<PaysizeRealEstateType>>> paysizeRetCache = new Dictionary<long, Dictionary<long, List<PaysizeRealEstateType>>>();
        private Dictionary<long, List<long>> realEstTypesByRo = new Dictionary<long, List<long>>();
        #endregion

        #region Proxy class
        private class ExtendedReportRecord : ReportRecord
        {
            public string ФИОСобственника { get; set; }

            public string ОплатитьДо { get; set; }

        }

        protected class BankProxy
        {
            public long RoId { get; set; }

            public string SettlementAccount { get; set; }

            public string Name { get; set; }

            public string Bik { get; set; }

            public string CorrAccount { get; set; }

            public string Address { get; set; }

        }

        public class ReportRecord
        {
            public long Id { get; set; }
            
            public long OwnerId { get; set; }

            public string СпособФормированияФонда { get; set; }

            public string ФондСпецСчет { get; set; }

            public string МесяцГодНачисления { get; set; }

            public string ЛицевойСчет { get; set; }

            public string Индекс { get; set; }

            public string АдресКвартиры { get; set; }

            public decimal Тариф { get; set; }

            public decimal ОбщаяПлощадь { get; set; }

            public decimal Итого { get; set; }

            public decimal Пени { get; set; }

            public string НаименованиеПериода { get; set; }

            public string ДатаДокумента { get; set; }

            public string ДатаНачалаПериода { get; set; }

            public string ДатаОкончанияПериода { get; set; }

            public decimal ПереплатаНаНачало { get; set; }

            public decimal ПереплатаПениНаНачало { get; set; }

            public decimal ДолгПениНаНачало { get; set; }

            public decimal ПереплатаНаКонец { get; set; }

            public decimal ДолгНаКонец { get; set; }

            public decimal ДолгНаНачало { get; set; }

            public decimal Начислено { get; set; }

            public decimal НачисленоПени { get; set; }

            public decimal Перерасчет { get; set; }

            public decimal Оплачено { get; set; }

            public decimal ОплаченоПени { get; set; }

            public decimal ПереплатаПениНаКонец { get; set; }

            public decimal ДолгПениНаКонец { get; set; }

            public decimal СуммаВсего { get; set; }

            public string НаименованиеПолучателя { get; set; }

            public string ИннПолучателя { get; set; }

            public string КппПолучателя { get; set; }

            public string ОргнПолучателя { get; set; }

            public string РсчетПолучателя { get; set; }

            public string АдресБанка { get; set; }

            public string НаименованиеБанка { get; set; }

            public string БикБанка { get; set; }

            public string КсБанка { get; set; }

            public string ТелефоныПолучателя { get; set; }

            public string АдресПолучателя { get; set; }

            public string EmailПолучателя { get; set; }

            public string WebSiteПолучателя { get; set; }

            public decimal ИтогоКОплате { get; set; }

            public string Информация { get; set; }

            public string ЗначениеQRКода { get; set; }

            public decimal СоцПоддержка { get; set; }

            public decimal ПредоставленнаяМСП { get; set; }

            public decimal УплаченоФКР { get; set; }
            public decimal ПотраченоНаКР { get; set; }

            public string ШтрихКод { get; set; }

        }

        public  class PersonalAccountProxy
        {
            public long Id { get; set; }
            public string RoomNum { get; set; }
            public bool IsRoomHasNoNumber { get; set; }
            public string PostCode { get; set; }
            public string AddressName { get; set; }
            public decimal Area { get; set; }
            public decimal AreaShare { get;set; }
            public long MunicipalityId { get; set; }
            public long OwnerId { get; set; }
            public string PersonalAccountNum { get; set; }
            public PersonalAccountOwnerType OwnerType { get; set; }
            public BasePersonalAccount Account { get; set; }
            public string PlaceGuidId { get; set; }
            public long MuId { get; set; }
            public long? SettlementId { get; set; }
            public long RoId { get; set; }
            public string Notation { get; set; }
            public decimal? AreaLivingNotLivingMkd { get; set; }
        }
        #endregion
    }
}