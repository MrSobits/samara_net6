using Bars.B4.DataAccess;
using Bars.Gkh.Config;
using Bars.Gkh.Domain;
using Bars.Gkh.Utils;

namespace Bars.Gkh.Decisions.Nso.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Windsor;

    using NHibernate.Linq;

    public class RepairAbovePaymentReport : BasePrintForm
    {
        public RepairAbovePaymentReport()
            : base(new ReportTemplateBinary(Properties.Resources.RepairAbovePaymentReport))
        {
        }

        #region Properties

        public IWindsorContainer Container { get; set; }

        private long[] settlementIds;
        private long[] municipalityIds;
        private CrFundFormationDecisionType methodCr;
        private bool equalsMin;

        private DateTime date;

        public override string Name
        {
            get { return "Размер взноса на капремонт"; }
        }

        public override string Desciption
        {
            get { return "Размер взноса на капремонт"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RepairAbovePaymentReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.DecisionsNso.RepairAbovePayment"; }
        }

        #endregion Properties

        public override void SetUserParams(BaseParams baseParams)
        {
            this.date = baseParams.Params.GetAs<DateTime>("date");
            this.municipalityIds = baseParams.Params.GetAs<string>("msIds").ToLongArray();
            this.settlementIds = baseParams.Params.GetAs<string>("settlIds").ToLongArray();

            this.methodCr = baseParams.Params.GetAs<CrFundFormationDecisionType>("methodCr");
            this.equalsMin = baseParams.Params.GetAs<bool>("equalsMin");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var municipalityDomainService = Container.Resolve<IRepository<Municipality>>();
            var crFundFormationDecisionDomainService = Container.Resolve<IDomainService<CrFundFormationDecision>>();
            var monthlyFeeAmountDecisionDomainService = Container.Resolve<IDomainService<MonthlyFeeAmountDecision>>();
            var paymentSizeMuRecordDomainService = Container.Resolve<IDomainService<PaymentSizeMuRecord>>();
            var gkhParamsService = Container.Resolve<IGkhParams>();

            try
            {
                var gkhParams = gkhParamsService.GetParams();
                var moLevel = (MoLevel)gkhParams.GetAs<int>("MoLevel");

                List<Municipality> municipalities;

                if (municipalityIds.Length > 0)
                {
                    if (settlementIds.Length > 0)
                    {
                        municipalities = municipalityDomainService.GetAll()
                            .Where(x => settlementIds.Contains(x.Id))
                            .OrderBy(x => x.ParentMo.Name ?? x.Name)
                            .ToList();
                    }
                    else
                    {
                        municipalities = municipalityDomainService.GetAll()
                            .Where(x => municipalityIds.Contains(x.Id)
                                || municipalityIds.Contains(x.ParentMo.Id))
                            .OrderBy(x => x.ParentMo.Name ?? x.Name)
                            .ToList();
                    }
                }
                else
                {
                    municipalities = municipalityDomainService.GetAll()
                        .OrderBy(x => x.ParentMo.Name ?? x.Name)
                        .ToList();
                }

                var query =
                    crFundFormationDecisionDomainService.GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Protocol.RealityObject.Municipality.Id))
                    .WhereIf(settlementIds.Length > 0, x => settlementIds.Contains(x.Protocol.RealityObject.MoSettlement.Id))
                    .WhereIf(methodCr != CrFundFormationDecisionType.Unknown, x => x.Decision == methodCr)
                    .Where(x => x.Protocol.ProtocolDate <= date && x.Protocol.State.FinalState);

                var protByCrFundIds = query.Select(x => x.Protocol.Id);

                var protByMonthlyFeeIds =
                    monthlyFeeAmountDecisionDomainService.GetAll()
                        .Where(x => protByCrFundIds.Contains(x.Protocol.Id))
                        .Where(x => x.IsChecked)
                        .ToList()
                        .Where(x => x.Decision != null && x.Decision.Any(y => !y.To.HasValue || y.To.Value >= date))
                        .Select(x => x.Protocol.Id)
                        .ToArray();

                var protocolArr = query.Fetch(x => x.Protocol).ToArray();

                Dictionary<long, CrFundFormationDecision[]> protocolDictByMu;

                if (moLevel == MoLevel.MunicipalUnion)
                {
                    protocolDictByMu = protocolArr.Where(x => protByMonthlyFeeIds.Contains(x.Protocol.Id) != equalsMin)
                        .GroupBy(x => x.Protocol.RealityObject.Id)
                        .Select(x => x.OrderBy(y => y.Protocol.ProtocolDate).ThenBy(y => y.Protocol.Id).Last())
                         .OrderBy(x => x.Protocol.RealityObject.MoSettlement.Name)
                        .GroupBy(x => x.Protocol.RealityObject.Municipality.Id)
                        .ToDictionary(x => x.Key, y => y.ToArray());
                }
                else
                {
                    protocolDictByMu = protocolArr.Where(x => protByMonthlyFeeIds.Contains(x.Protocol.Id) != equalsMin)
                        .GroupBy(x => x.Protocol.RealityObject.Id)
                        .Select(x => x.OrderBy(y => y.Protocol.ProtocolDate).ThenBy(y => y.Protocol.Id).Last())
                        .OrderBy(x => x.Protocol.RealityObject.MoSettlement.Name)
                        .GroupBy(x => x.Protocol.RealityObject.MoSettlement.Id)
                        .ToDictionary(x => x.Key, y => y.ToArray());
                }

                var protocolIds = protocolDictByMu.Values.SelectMany(x => x.Select(y => y.Protocol.Id)).ToArray();

                reportParams.SimpleReportParams["Date"] = date.ToShortDateString();

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("row");

                var paymentSizeDomain = paymentSizeMuRecordDomainService.GetAll();


                var paymentSizeByMunicipality =
                    paymentSizeDomain.Where(x => x.PaymentSizeCr.DateStartPeriod <= date)
                        .Where(x => x.PaymentSizeCr.DateEndPeriod == null || x.PaymentSizeCr.DateEndPeriod >= date)
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                        .Select(x => new
                        {
                            x.PaymentSizeCr.PaymentSize,
                            Id = x.Municipality.ParentMo != null ? x.Municipality.ParentMo.Id : x.Municipality.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.PaymentSize).FirstOrDefault());

                var monthlyFeeDecision =
                    monthlyFeeAmountDecisionDomainService.GetAll()
                        .Where(x => protocolIds.Contains(x.Protocol.Id))
                        .ToDictionary(x => x.Protocol.Id, y => y.Decision);

                int numb = 0;

                foreach (var mu in municipalities)
                {
                    var protocols = protocolDictByMu.Get(mu.Id) ?? new CrFundFormationDecision[0];
                    var parentMuId = mu.ParentMo != null ? mu.ParentMo.Id : mu.Id;
                    var paymentSize = paymentSizeByMunicipality.Get(parentMuId);

                    foreach (var row in protocols)
                    {
                        numb++;

                        FillRow(section, numb, row, mu, paymentSize, row.Decision);

                        if (monthlyFeeDecision.ContainsKey(row.Protocol.Id))
                        {
                            var list = monthlyFeeDecision[row.Protocol.Id];
                            var count = list.Count;

                            for (int i = 0; i < count; i++)
                            {
                                var dec = list[i];
                                section["TakenSize"] = dec.Value;
                                section["DateStart"] = dec.From.HasValue ? dec.From.Value.ToShortDateString() : string.Empty;
                                section["DateEnd"] = dec.To.HasValue ? dec.To.Value.ToShortDateString() : string.Empty;

                                if (count - 1 != i)
                                {
                                    FillRow(section, numb, row, mu, paymentSize, row.Decision);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                Container.Release(municipalityDomainService);
                Container.Release(crFundFormationDecisionDomainService);
                Container.Release(monthlyFeeAmountDecisionDomainService);
                Container.Release(paymentSizeMuRecordDomainService);
            }
        }

        private void FillRow(Section section, int numb, dynamic row, Municipality mu, decimal paymentSize, CrFundFormationDecisionType crMethod)
        {
            section.ДобавитьСтроку();
            section["Number"] = numb;
            section["DateAndNumb"] = string.Format(
                "№{0} от {1}",
                row.Protocol.DocumentNum,
                row.Protocol.ProtocolDate.ToShortDateString());

            section["Mo"] = row.Protocol.RealityObject.Municipality.Name;
            section["Ms"] = row.Protocol.RealityObject.MoSettlement.Name;
            section["Address"] = row.Protocol.RealityObject.Address;
            section["MethodCr"] = crMethod.GetEnumMeta().Display;
            section["MinCr"] = paymentSize;
        }
    }
}