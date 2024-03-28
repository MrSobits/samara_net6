namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Castle.Windsor;
    using Decisions.Nso.Entities;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums.Decisions;
    using Gkh.Utils;
    using Overhaul.Entities;

    public class RepairContributionInfoReport : BasePrintForm
    {
        #region .ctor

        public RepairContributionInfoReport() : base(new ReportTemplateBinary(Properties.Resources.RepairContributionInfoReport))
        {
        }

        #endregion .ctor

        #region Properties

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Сведения о поступлениях и остатках взносов на капитальный ремонт для гос. инспекции"; }
        }

        public override string Desciption
        {
            get { return "Сведения о поступлениях и остатках взносов на капитальный ремонт для гос. инспекции"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RepairContributionInfo"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.RepairContributionInfoReport"; }
        }

        #endregion Properties

        #region Fields

        private DateTime _reportDate;
        private long[] _roIds = new long[0];
        private long _regopId;
        private CrFundFormationDecisionType _crFormFund;
        private AccountOwnerDecisionType _accOwner;

        #endregion Fields

        private readonly Dictionary<long, RobjectProxy> _dictRoProxy = new Dictionary<long, RobjectProxy>();

        public override void SetUserParams(BaseParams baseParams)
        {
            _roIds = baseParams.Params.GetAs<string>("roIds").ToLongArray();
            _regopId = baseParams.Params.GetAsId("regopId");
            _reportDate = baseParams.Params.GetAs<DateTime>("reportDate");
            _crFormFund = baseParams.Params.GetAs<CrFundFormationDecisionType>("crFormFund");
            _accOwner = baseParams.Params.GetAs<AccountOwnerDecisionType>("accOwner");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            LoadData();

            reportParams.SimpleReportParams["ReportDate"] = _reportDate.ToShortDateString();
            reportParams.SimpleReportParams["AccountOwner"] = _accOwner.GetEnumMeta().Display;
            reportParams.SimpleReportParams["KindAccount"] = _crFormFund.GetEnumMeta().Display;

            var regopDomain = Container.ResolveDomain<RegOperator>();

            using (Container.Using(regopDomain))
            {
                reportParams.SimpleReportParams["Regoperator"] =
                    regopDomain.Get(_regopId)
                        .Return(x => x.Contragent)
                        .Return(x => x.Name);
            }

            var orderedData = GetOrderedRecords();

            FillPage1(reportParams, orderedData);
            FillPage2(reportParams, orderedData);
            FillPage3(reportParams, orderedData);
        }

        private void LoadData()
        {
            var roprotDomain = Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var crFundDecDomain = Container.ResolveDomain<CrFundFormationDecision>();
            var accOwnerDecDomain = Container.ResolveDomain<AccountOwnerDecision>();
            var creditorgDecDomain = Container.ResolveDomain<CreditOrgDecision>();
            var monthlyFeeDecDomain = Container.ResolveDomain<MonthlyFeeAmountDecision>();
            var roRep = Container.ResolveRepository<RealityObject>();
            var roaccDomain = Container.ResolveDomain<RealityObjectPaymentAccount>();
            var roaccopDomain = Container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            var paysizemuDomain = Container.ResolveDomain<PaymentSizeMuRecord>();

            using (Container.Using(
                roprotDomain,
                crFundDecDomain,
                accOwnerDecDomain,
                creditorgDecDomain,
                monthlyFeeDecDomain,
                roRep,
                roaccDomain,
                roaccopDomain,
                paysizemuDomain))
            {
                var crFundQuery = FilterDecisions(crFundDecDomain);
                var accOwnerQuery = FilterDecisions(accOwnerDecDomain);
                var creditOrgQuery = FilterDecisions(creditorgDecDomain);
                var monthlyFeeQuery = FilterDecisions(monthlyFeeDecDomain);

                var roaccQuery = roaccDomain.GetAll()
                    .Where(x => x.DateOpen <= _reportDate)
                    .Where(x => !x.DateClose.HasValue || x.DateClose >= _reportDate);

                var roaccopAllQuery =
                    roaccopDomain.GetAll()
                        .Where(x => x.Date <= _reportDate);

                var roaccopIncomeQuery = roaccopAllQuery
                    .Where(y =>
                        y.OperationType != PaymentOperationType.OutcomeAccountPayment
                        && y.OperationType != PaymentOperationType.ExpenseLoan
                        && y.OperationType != PaymentOperationType.OutcomeLoan
                        && y.OperationType != PaymentOperationType.CashService
                        && y.OperationType != PaymentOperationType.OpeningAcc);

                var muMonthlyFee = paysizemuDomain.GetAll()
                    .Where(x => x.PaymentSizeCr.DateStartPeriod <= _reportDate)
                    .Where(x => !x.PaymentSizeCr.DateEndPeriod.HasValue || x.PaymentSizeCr.DateEndPeriod >= _reportDate)
                    .Select(x => new
                    {
                        MuId = x.Municipality.Id,
                        x.PaymentSizeCr.PaymentSize,
                        x.PaymentSizeCr.DateStartPeriod
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key,
                        y => y
                            .OrderByDescending(x => x.DateStartPeriod)
                            .Select(x => x.PaymentSize)
                            .First());

                foreach (var tmpIds in _roIds.SplitArray())
                {
                    if (tmpIds.Length == 0)
                    {
                        continue;
                    }

                    var ids = tmpIds;

                    var tmpAccOwnerDecisions = accOwnerQuery
                        .Where(x => ids.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            RoId = x.Protocol.RealityObject.Id,
                            x.DecisionType,
                            x.Protocol.ProtocolDate,
                            x.Protocol.DocumentNum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key,
                            y => y
                                .OrderByDescending(x => x.ProtocolDate)
                                .First());

                    var tmpCrFundDecisions = crFundQuery
                        .Where(x => ids.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            RoId = x.Protocol.RealityObject.Id,
                            x.Decision,
                            x.Protocol.ProtocolDate,
                            x.Protocol.DocumentNum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key,
                            y => y
                                .OrderByDescending(x => x.ProtocolDate)
                                .First());

                    var tmpCreditOrgDecisions = creditOrgQuery
                        .Where(x => ids.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            RoId = x.Protocol.RealityObject.Id,
                            CreditOrg = x.Decision.Name,
                            x.Protocol.ProtocolDate,
                            x.Protocol.DocumentNum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key,
                            y => y
                                .OrderByDescending(x => x.ProtocolDate)
                                .First());

                    var tmpMonthlyFeeDecisions = monthlyFeeQuery
                        .Where(x => ids.Contains(x.Protocol.RealityObject.Id))
                        .Select(x => new
                        {
                            RoId = x.Protocol.RealityObject.Id,
                            x.Decision
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key,
                            y => y
                                .SelectMany(x => x.Decision)
                                .Where(x => x.From <= _reportDate)
                                .Where(x => !x.To.HasValue || x.To >= _reportDate)
                                .OrderByDescending(x => x.From)
                                .FirstOrDefault()
                                .Return(x => (decimal?)x.Value));

                    var tmpAccounts = roaccQuery
                        .Where(x => ids.Contains(x.RealityObject.Id))
                        .Select(x => new
                        {
                            RoId = x.RealityObject.Id,
                            x.AccountNumber,
                            x.AccountType
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.First());

                    roRep.GetAll()
                        .Where(x => ids.Contains(x.Id))
                        .Select(x => new
                        {
                            x.Id,
                            x.FiasAddress.House,
                            MunicipalityId = x.Municipality.Id,
                            Municipality = x.Municipality.Name,
                            x.FiasAddress.PlaceName,
                            x.FiasAddress.StreetName,
                            x.AreaMkd,
                            Income =
                                roaccopIncomeQuery
                                    .Where(y => y.Account.RealityObject.Id == x.Id)
                                    .Sum(y => (decimal?) y.OperationSum) ?? 0,
                            Balance =
                                roaccopAllQuery
                                    .Where(y => y.Account.RealityObject.Id == x.Id)
                                    .Sum(y => (decimal?) y.OperationSum) ?? 0
                        })
                        .AsEnumerable()
                        .Select(x => new RobjectProxy
                        {
                            Id = x.Id,
                            MunicipalityId = x.MunicipalityId,
                            Municipality = x.Municipality,
                            Place = x.PlaceName,
                            Street = x.StreetName,
                            House = x.House,
                            AreaMkd = x.AreaMkd,
                            Income = x.Income,
                            Balance = x.Balance,
                            Bank = tmpCreditOrgDecisions.Get(x.Id).Return(y => y.CreditOrg),
                            AccountNumber = tmpAccounts.Get(x.Id).Return(y => y.AccountNumber),

                            ProtocolDate =
                                tmpAccOwnerDecisions.ContainsKey(x.Id)
                                    ? tmpAccOwnerDecisions[x.Id].ProtocolDate
                                    : tmpCrFundDecisions.ContainsKey(x.Id)
                                        ? tmpCrFundDecisions[x.Id].ProtocolDate
                                        : DateTime.MinValue,

                            ProtocolNumber = tmpAccOwnerDecisions.ContainsKey(x.Id)
                                ? tmpAccOwnerDecisions[x.Id].DocumentNum
                                : tmpCrFundDecisions.ContainsKey(x.Id)
                                    ? tmpCrFundDecisions[x.Id].DocumentNum
                                    : null,

                            MonthlyFee =
                                tmpMonthlyFeeDecisions.ContainsKey(x.Id) && tmpMonthlyFeeDecisions[x.Id].HasValue
                                    ? tmpMonthlyFeeDecisions[x.Id].Value
                                    : muMonthlyFee.Get(x.MunicipalityId)
                        })
                        .ForEach(x => _dictRoProxy[x.Id] = x);
                }
            }
        }

        protected void FillPage1(ReportParams reportParams, IEnumerable<RobjectProxy> data)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section1");

            var i = 0;

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["Number1"] = ++i;
                section["Municipality"] = item.Municipality;
                section["Place"] = item.Place;
                section["Street"] = item.Street;
                section["House"] = item.House;
                section["Income"] = item.Income;
                section["AccountNumber"] = item.AccountNumber;
                section["Bank"] = item.Bank;
            }
        }

        protected void FillPage2(ReportParams reportParams, IEnumerable<RobjectProxy> data)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section2");

            var i = 0;

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["Number1"] = ++i;
                section["Municipality"] = item.Municipality;
                section["Place"] = item.Place;
                section["Street"] = item.Street;
                section["House"] = item.House;
                section["Balance"] = item.Income;
                section["AccountNumber"] = item.AccountNumber;
                section["Bank"] = item.Bank;
            }
        }

        protected void FillPage3(ReportParams reportParams, IEnumerable<RobjectProxy> data)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section3");

            var i = 0;

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["Number1"] = ++i;
                section["Municipality"] = item.Municipality;
                section["Place"] = item.Place;
                section["Street"] = item.Street;
                section["House"] = item.House;
                section["Area"] = (item.AreaMkd ?? 0).RegopRoundDecimal(2);
                section["ProtocolNumber"] = item.ProtocolNumber;
                section["ProtocolDate"] = item.ProtocolDate != DateTime.MinValue
                    ? item.ProtocolDate.ToShortDateString()
                    : null;
                section["MonthlyFee"] = item.MonthlyFee.RegopRoundDecimal(2);
            }
        }

        private List<RobjectProxy> GetOrderedRecords()
        {
            return _dictRoProxy
                .Select(x => x.Value)
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Place)
                .ThenBy(x => x.Street)
                .ThenBy(x => x.House)
                .ToList();
        }

        protected IQueryable<T> FilterDecisions<T>(IDomainService<T> domainService) where T : UltimateDecision
        {
            return domainService.GetAll()
                .Where(x => x.Protocol.ProtocolDate <= _reportDate)
                .Where(x => x.Protocol.State.FinalState);
        }

        protected class RobjectProxy
        {
            public long Id { get; set; }

            public long MunicipalityId { get; set; }

            public string Municipality { get; set; }

            public string Place { get; set; }

            public string Street { get; set; }

            public string House { get; set; }

            public decimal? AreaMkd { get; set; }

            public string ProtocolNumber { get; set; }

            public DateTime ProtocolDate { get; set; }

            public decimal MonthlyFee { get; set; }

            public decimal Income { get; set; }

            public decimal Balance { get; set; }

            public string AccountNumber { get; set; }

            public string Bank { get; set; }
        }
    }
}