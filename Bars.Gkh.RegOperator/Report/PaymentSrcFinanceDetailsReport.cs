namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;
    using B4.Modules.Reports;
    using Castle.Windsor;

    using B4;
    
    
    using B4.Utils;
    using DomainService;
    using Gkh.Domain;
    using GkhCr.Entities;
    using Entities;
    using ViewModels;

    public class PaymentSrcFinanceDetailsReport : BasePrintForm
    {
        public IWindsorContainer Container{ get; set; }
        public IDomainService<PerformedWorkActPayment> ActPaymentDomain { get; set; }
        public IDomainService<PerformedWorkAct> ActDomain { get; set; }
        public IDomainService<PaymentSrcFinanceDetails> DetailsDomain { get; set; }
        public IDomainService<PaymentOrderDetail> DetailDomain { get; set; }

        private long _paymentId;
        private PaymentOrderDetailSource[] _details;

        public PaymentSrcFinanceDetailsReport()
            : base(new ReportTemplateBinary(Properties.Resources.PaymentSrcFinanceDetailsReport))
        {
        }

        public override string Name
        {
            get { return "PaymentSrcFinanceDetailsReport"; }
        }

        public override string Desciption
        {
            get { return "Печатная форма оплаты акта выполненных работ"; }
        }

        public override string GroupName
        {
            get { return string.Empty; }
        }

        public override string ParamsController
        {
            get { return string.Empty; }
        }

        public override string RequiredPermission
        {
            get { return string.Empty; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            _paymentId = baseParams.Params.GetAsId("paymentId");

            if (_paymentId == 0)
            {
                throw new Exception("Необходимо указать Идентификатор ");
            }

            _details = baseParams.Params.GetAs<PaymentOrderDetailSource[]>("details");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var payment = ActPaymentDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ActNum = x.PerformedWorkAct.DocumentNum,
                    ActDate = x.PerformedWorkAct.DateFrom,
                    x.DatePayment,
                    x.TypeActPayment
                })
                .FirstOrDefault(x => x.Id == _paymentId);

            if (payment == null)
            {
                throw new Exception("Не удалось получить оплату");
            }

            DateTime date = DateTime.Now;

            reportParams.SimpleReportParams["НомерРаспоряжения"] = "_________";
            reportParams.SimpleReportParams["ТекущаяДата"] = date.ToString("dd.MM.yyyy");

            reportParams.SimpleReportParams["НомерАкта"] = payment.ActNum;
            reportParams.SimpleReportParams["ДатаАкта"] = payment.ActDate.ToDateTime().ToString("dd.MM.yyyy");
            reportParams.SimpleReportParams["ДатаОплаты"] = payment.DatePayment.ToDateTime().ToString("dd.MM.yyyy");
            reportParams.SimpleReportParams["ВидОплаты"] = payment.TypeActPayment.GetEnumMeta().Display;


            if (_details == null || _details.Length == 0)
            {
                return;
            }

            //var data = DetailDomain.GetAll().Where(x => x.PaymentOrder.Id == _paymentId).ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция_источники");

            foreach (var finSrc in _details)
            {
                section.ДобавитьСтроку();
                section["Источник"] = finSrc.WalletName;
                section["Сальдо"] = finSrc.Detail.Wallet.Balance;
                section["Оплата"] = finSrc.Detail.Amount;
            }

            reportParams.SimpleReportParams["СальдоИтого"] = _details.Sum(x => x.Detail.Wallet.Balance);
            reportParams.SimpleReportParams["ОплатаИтого"] = _details.Sum(x => x.Detail.Amount);
        }
    }
}