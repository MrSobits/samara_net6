namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using GkhCr.Entities;
    using Overhaul.Domain.RealityObjectServices;

    public class CalendarCostPlaningReport : BasePrintForm
    {
        #region .ctor

        public CalendarCostPlaningReport()
            : base(new ReportTemplateBinary(Properties.Resources.CalendarCostPlaningReport))
        {
        }

        #endregion .ctor

        #region BasePrintForm properties

        public override string Name
        {
            get { return "Календарь планирования расходов"; }
        }

        public override string Desciption
        {
            get { return "Календарь планирования расходов"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CalendarCostPlaning"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.CalendarCostPlaning"; }
        }

        #endregion BasePrintForm properties

        #region Fields

        private long[] _moIds;

        private long[] _mrIds;

        private long _programId;

        private ProgramCr _program;

        #endregion Fields

        private IEnumerable<Data> _data;

        public IWindsorContainer Container { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            _moIds = baseParams.Params.GetAs<string>("moIds").ToLongArray();
            _mrIds = baseParams.Params.GetAs<string>("mrIds").ToLongArray();
            _programId = baseParams.Params.GetAsId("programId");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programDomain = Container.ResolveDomain<ProgramCr>();
            _program = programDomain.Get(_programId);

            if (_program == null)
            {
                throw new ValidationException("Не удалось получить программу капитального ремонта");
            }

            CacheData();

            Fill(reportParams);
        }

        protected virtual void CacheData()
        {
            var typeWorkDomain = Container.ResolveDomain<TypeWorkCr>();
            var buildTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();
            var objectDpkrService = Container.Resolve<IObjectCrDpkrDataService>();
            var chargesDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var paymentsDomain = Container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            var subsidyDomain = Container.ResolveDomain<RealityObjectSubsidyAccountOperation>();

            var data = typeWorkDomain.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == _programId)
                .WhereIf(!EnumerableExtension.IsEmpty(_moIds),
                    x => _moIds.Contains(x.ObjectCr.RealityObject.Municipality.Id)
                         || _moIds.Contains(x.ObjectCr.RealityObject.MoSettlement.Id))
                .WhereIf(!EnumerableExtension.IsEmpty(_mrIds),
                    x => _mrIds.Contains(x.ObjectCr.RealityObject.Municipality.Id)
                         || _mrIds.Contains(x.ObjectCr.RealityObject.MoSettlement.Id))
                .Select(x => new
                {
                    x.Id,
                    x.ObjectCr.RealityObject,
                    Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                    Settlement = x.ObjectCr.RealityObject.MoSettlement.Name,
                    x.ObjectCr.RealityObject.Address,
                    Work = x.Work.Name,
                    x.DateStartWork,
                    x.DateEndWork
                });

            //ключ TypeWorkId
            var dictBuilder = buildTypeWorkDomain.GetAll()
                .Where(y => data.Any(x => x.Id == y.TypeWork.Id))
                .Select(x => new
                {
                    x.TypeWork.Id,
                    x.BuildContract.Builder.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Name).First());

            var dictNeed = objectDpkrService.GetShortProgramRecordsProgramAndMunicipality(_program)
                .Where(y => data.Any(x => x.RealityObject == y.RealityObject))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.PlanYear,
                    x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.Sum));

            var dictCollection = chargesDomain.GetAll()
                .Where(y => data.Any(x => x.RealityObject == y.Account.RealityObject))
                .Select(x => new
                {
                    RoId = x.Account.RealityObject.Id,
                    x.ChargedTotal,
                    x.ChargedPenalty,
                    x.PaidPenalty,
                    x.PaidTotal
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y =>
                {
                    var charged = y.Sum(x => x.ChargedTotal + x.ChargedPenalty);

                    if (charged == 0)
                    {
                        return 0m;
                    }
                    var payment = y.Sum(x => x.PaidTotal + x.PaidPenalty);

                    return payment / charged;
                });

            var dictSubsidy = subsidyDomain.GetAll()
                .Where(y => data.Any(x => x.RealityObject == y.Account.RealityObject))
                .Select(x => new
                {
                    RoId = x.Account.RealityObject.Id,
                    x.OperationSum
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.OperationSum));

            var paymentsQuery = paymentsDomain.GetAll()
                .Where(y => data.Any(x => x.RealityObject == y.Account.RealityObject));

            var dictOwnerPayments = paymentsQuery
                .Where(x => x.OperationType == PaymentOperationType.IncomePenalty
                            || x.OperationType == PaymentOperationType.IncomeByDecisionTariff
                            || x.OperationType == PaymentOperationType.IncomeByMinTariff)
                .Select(x => new
                {
                    RoId = x.Account.RealityObject.Id,
                    x.OperationSum
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.OperationSum));

            var dictOtherPayments = paymentsQuery
                .Where(x => x.OperationType != PaymentOperationType.IncomePenalty
                            && x.OperationType != PaymentOperationType.IncomeByDecisionTariff
                            && x.OperationType != PaymentOperationType.IncomeByMinTariff)
                .Select(x => new
                {
                    RoId = x.Account.RealityObject.Id,
                    x.OperationSum
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.OperationSum));

            _data = data
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Settlement)
                .ThenBy(x => x.Address)
                .ThenBy(x => x.Work)
                .Select(x => new Data
                {
                    TypeWorkId = x.Id,
                    RoId = x.RealityObject.Id,
                    Address = x.Address,
                    Work = x.Work,
                    Mo = x.Settlement,
                    Mr = x.Municipality,
                    DateEnd = x.DateEndWork.HasValue ? x.DateEndWork.Value.ToShortDateString() : null,
                    DateStart = x.DateStartWork.HasValue ? x.DateStartWork.Value.ToShortDateString() : null,
                })
                .AsEnumerable()
                .Select(x => new Data
                {
                    RoId = x.RoId,
                    Address = x.Address,
                    Work = x.Work,
                    Mo = x.Mo,
                    Mr = x.Mr,
                    DateEnd = x.DateEnd,
                    DateStart = x.DateStart,
                    Builder = dictBuilder.ContainsKey(x.TypeWorkId) ? dictBuilder[x.TypeWorkId] : null,
                    Need = dictNeed.ContainsKey(x.RoId) ? dictNeed[x.RoId] : 0m,
                    OwnerPayment = dictOwnerPayments.Get(x.RoId),
                    OtherPayment = dictOtherPayments.Get(x.RoId),
                    Collection = dictCollection.Get(x.RoId),
                    Subsidy = dictSubsidy.Get(x.RoId)
                })
                .ToList();
        }

        protected virtual void Fill(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["ReportDate"] = DateTime.Today.ToShortDateString();
            reportParams.SimpleReportParams["Program"] = _program.Name;

            int number = 0;
            int prevRoId = 0;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var item in _data)
            {
                section.ДобавитьСтроку();

                if (prevRoId != item.RoId)
                {
                    number++;
                }

                section["Number1"] = number;
                section["Mr"] = item.Mr;
                section["Mo"] = item.Mo;
                section["Address"] = item.Address;
                section["Need"] = item.Need;
                section["Collection"] = item.Collection;
                section["Subsidy"] = item.Subsidy;
                section["OwnerPayment"] = item.OwnerPayment;
                section["OtherPayment"] = item.OtherPayment;
                section["Work"] = item.Work;
                section["Builder"] = item.Builder;
                section["DateStart"] = item.DateStart;
                section["DateEnd"] = item.DateEnd;
            }
        }

        protected class Data
        {
            public long RoId { get; set; }

            public long TypeWorkId { get; set; }

            /// <summary>
            /// Муниципальный район
            /// </summary>
            public string Mr { get; set; }

            /// <summary>
            /// Муниципальное образование
            /// </summary>
            public string Mo { get; set; }

            /// <summary>
            /// Адрес
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// Потребность
            /// </summary>
            public decimal Need { get; set; }

            /// <summary>
            /// Собираемость
            /// </summary>
            public decimal Collection { get; set; }

            /// <summary>
            /// Субсидии
            /// </summary>
            public decimal Subsidy { get; set; }

            /// <summary>
            /// Платежи собственников
            /// </summary>
            public decimal OwnerPayment { get; set; }

            /// <summary>
            /// Иные поступления
            /// </summary>
            public decimal OtherPayment { get; set; }

            /// <summary>
            /// Подрядчик
            /// </summary>
            public string Builder { get; set; }

            /// <summary>
            /// Работа
            /// </summary>
            public string Work { get; set; }

            /// <summary>
            /// Дата начала работ
            /// </summary>
            public string DateStart { get; set; }

            /// <summary>
            /// Дата окончания работ
            /// </summary>
            public string DateEnd { get; set; }
        }
    }
}