namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Domain.CollectionExtensions;
    using Entities;
    using Enums;

    using Castle.Windsor;

    public class RepairPaymentByOwnerReport : BasePrintForm
    {
        public RepairPaymentByOwnerReport()
            : base(new ReportTemplateBinary(Properties.Resources.RepairPaymentByOwnerReport))
        {
        }

        #region Properties

        public IWindsorContainer Container { get; set; }

        private long personalId;
        
        private DateTime startDate;

        private DateTime endDate;

        public override string Name
        {
            get { return "Размер начисленных и уплаченных взносов на кр каждым собственником"; }
        }

        public override string Desciption
        {
            get { return "Размер начисленных и уплаченных взносов на кр каждым собственником"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RepairPaymentByOwnerReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.RepairPaymentByOwnerReport"; }
        }

        #endregion Properties

        public override void SetUserParams(BaseParams baseParams)
        {
            personalId = baseParams.Params.GetAs<long>("personalId");
            
            startDate = baseParams.Params.GetAs<DateTime>("startDate");
            endDate = baseParams.Params.GetAs<DateTime>("endDate");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var account = Container.Resolve<IDomainService<BasePersonalAccount>>().Get(personalId);

            var ro = account.Room.RealityObject;

            if (ro == null)
            {
                return;
            }

            reportParams.SimpleReportParams["DateStart"] = startDate.ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = endDate.ToShortDateString();
            reportParams.SimpleReportParams["Mo"] = ro.Municipality.Name;
            reportParams.SimpleReportParams["Ms"] = ro.MoSettlement.Name;
            reportParams.SimpleReportParams["Address"] = ro.Address;

            reportParams.SimpleReportParams["Fio"] = account.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal
                                                         ? (account.AccountOwner as LegalAccountOwner).Contragent.Name
                                                         : account.AccountOwner.Name;

            reportParams.SimpleReportParams["Number"] = account.PersonalAccountNum;

            var periodSummaryDomain = Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();
            var accountChargeDomain = Container.Resolve<IDomainService<PersonalAccountCharge>>();

            // Находим максимальную дату окончания среди уже закрытых периодов по этому ЛС
            var maxClosedPeriodDate =
                periodSummaryDomain.GetAll()
                    .Where(x => x.Period.IsClosed)
                    .Where(x => x.PersonalAccount.Id == personalId)
                    .SafeMax(x => x.Period.EndDate);

            if (maxClosedPeriodDate < startDate)
            {
                return;
            }

            var realEndDate = endDate.AddDays(1).AddMilliseconds(-1);
            
            var charges =
                accountChargeDomain.GetAll()
                    .Where(
                        x =>
                        x.BasePersonalAccount.Id == personalId && x.ChargeDate >= startDate
                        && x.ChargeDate <= realEndDate && x.IsFixed)
                    .Select(x => new
                                     {
                                         x.ChargeDate, 
                                         x.ChargeTariff, 
                                         Recalc = x.RecalcByBaseTariff,
                                         x.Penalty,
                                         x.OverPlus
                                     })
                    .ToArray();

            var chargedByBaseTariffSum = charges.SafeSum(x => x.ChargeTariff - x.OverPlus);
            var chargedTariffSum = charges.SafeSum(x => x.ChargeTariff);
            var recalcSum = charges.SafeSum(x => x.Recalc);


            var chargedTariff = chargedByBaseTariffSum + recalcSum;
            var chargedOwnerDecision = chargedTariffSum - chargedByBaseTariffSum;
            
            var chargedPenaltyTotal = charges.SafeSum(x => x.Penalty);

            reportParams.SimpleReportParams["TotalCharge"] = chargedTariff + chargedPenaltyTotal + chargedOwnerDecision;

            var accountPaymentInfo =
                Container.ResolveDomain<PersonalAccountPayment>()
                    .GetAll()
                    .Where(
                        x =>
                        x.BasePersonalAccount.Id == personalId && x.PaymentDate <= realEndDate
                        && x.PaymentDate >= startDate)
                    .Select(x => new { x.Sum, x.Type })
                    .AsEnumerable()
                    .GroupBy(x => x.Type)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Sum));

            var paymentTariff = accountPaymentInfo.Get(PaymentType.Basic);
            var paymentPenalty = accountPaymentInfo.Get(PaymentType.Penalty);

            reportParams.SimpleReportParams["TotalDebt"] = (chargedTariff + chargedPenaltyTotal + chargedOwnerDecision)
                                                           - (paymentTariff + paymentPenalty);
            reportParams.SimpleReportParams["PaymentTotal"] = paymentTariff + paymentPenalty;
            reportParams.SimpleReportParams["ChargedPenalty"] = paymentPenalty;
        }
    }
}