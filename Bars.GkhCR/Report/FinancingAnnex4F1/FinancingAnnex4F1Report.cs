namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Report.FinancingAnnex4F1;

    using Castle.Windsor;

    public class FinancingAnnex4F1Report : BasePrintForm
    {
        private long programCrId;
        private DateTime reportDate;
        private long[] municipalityIds;
        private TypeFinanceGroup typeFinanceGroup;

        public FinancingAnnex4F1Report()
            : base(new ReportTemplateBinary(Properties.Resources.FinancingAnnex4F1))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Приложение 4 к отчету о расходовании средств Фонда (форма 1)"; }
        }

        public override string Desciption
        {
            get { return "Приложение 4 к отчету о расходовании средств Фонда (форма 1)"; }
        }

        public override string GroupName
        {
            get { return "Финансирование"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.FinancingAnnex4F1"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.FinancingAnnex4F1"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();
            var date = baseParams.Params["reportDate"].ToDateTime();
            this.reportDate = date == DateTime.MinValue ? DateTime.Now : date;
            this.municipalityIds = !string.IsNullOrEmpty(baseParams.Params["municipalityIds"].ToStr()) ? baseParams.Params["municipalityIds"].ToStr().Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            typeFinanceGroup = (TypeFinanceGroup)baseParams.Params["typeFinanceGroup"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["месяц"] = this.reportDate.ToString("MMMM");
            reportParams.SimpleReportParams["год"] = this.reportDate.ToString("yyyy");
            var data = this.GetPaymentOrders();

            // сумма возвратов итого
            var номерСтрки = 1;
            var sum = 0m;

            // сумма повторно
            var sumRepeate = 0M;
            foreach (var municipality in data.GroupBy(x => x.Municipality).ToDictionary(x => x.Key, x => x.ToList()))
            {
                var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияРайоны");
                sectionMunicipality.ДобавитьСтроку();

                sectionMunicipality["МунРайон"] = municipality.Key;

                var sumMunicipality = 0M;
                var sumRepeateMunicipality = 0M;

                foreach (var rec in municipality.Value)
                {
                    var секцияДок = sectionMunicipality.ДобавитьСекцию("СекцияДок");
                    секцияДок.ДобавитьСтроку();

                    секцияДок["Адрес"] = rec.Address;
                    секцияДок["Наименование"] = municipality.Key;
                    секцияДок["НомЗаписи"] = номерСтрки++;

                    if (rec.IncomingSum.HasValue)
                    {
                        секцияДок["ДатаНомер"] = rec.DocumentDateNum;
                        секцияДок["Сумма"] = rec.IncomingSum == 0 ? string.Empty : rec.IncomingSum.ToStr();
                        секцияДок["СуммаПовторно"] = rec.RedirectFunds == 0
                                                         ? string.Empty
                                                         : rec.RedirectFunds.ToStr();

                        sumMunicipality += rec.IncomingSum.Value;
                        sumRepeateMunicipality += rec.RedirectFunds.HasValue ? rec.RedirectFunds.Value : 0M;
                    }
                }

                sectionMunicipality["СуммаВсего"] = sumMunicipality;
                sectionMunicipality["СуммаПовторноВсего"] = sumRepeateMunicipality;

                sum += sumMunicipality;
                sumRepeate += sumRepeateMunicipality;
            }

            reportParams.SimpleReportParams["СуммаВсегоВсего"] = sum;
            reportParams.SimpleReportParams["СуммаПовторноВсегоВсего"] = sumRepeate;
        }

        private long[] GetRealtyObjectIds()
        {
           var servPersonalAccount = this.Container.Resolve<IDomainService<PersonalAccount>>();
           return servPersonalAccount.GetAll()
                .Where(x => x.FinanceGroup == this.typeFinanceGroup && x.ObjectCr.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Length > 0,x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Select(x => x.ObjectCr.RealityObject.Id)
                .ToArray();
        }

        private IEnumerable<PaymentOrderProxy> GetPaymentOrders()
        {
            var realtyObjectIds = this.GetRealtyObjectIds();
            var startMonth = this.reportDate.AddDays(-(this.reportDate.Day - 1));
            var endMonth = startMonth.AddMonths(1).AddDays(-1);
             var periodId = this.Container.Resolve<IDomainService<ProgramCr>>().GetAll()
            .Where(x => x.Id == this.programCrId)
            .Select(x => x.Period.Id)
            .First();

            var servPaymentOrderIn = this.Container.Resolve<IDomainService<PaymentOrderIn>>();

            var result = new List<PaymentOrderProxy>();

            for (var i = 0; i < realtyObjectIds.Length; i += 1000)
            {
                var takeCount = realtyObjectIds.Length - i < 1000 ? realtyObjectIds.Length - i : 1000;
                var tempList = realtyObjectIds.Skip(i).Take(takeCount).ToArray();

                result.AddRange(servPaymentOrderIn.GetAll()
                             .Where(x => tempList.Contains(x.BankStatement.ObjectCr.RealityObject.Id))
                             .Where(x => (startMonth <= x.DocumentDate && endMonth >= x.DocumentDate) && x.BankStatement.TypeFinanceGroup == this.typeFinanceGroup && x.BankStatement.Period.Id == periodId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.BankStatement.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.TypeFinanceSource != TypeFinanceSource.OccupantFunds)
                .Select(x => new PaymentOrderProxy
                                {
                                    Municipality = x.BankStatement.ObjectCr.RealityObject.Municipality.Name,
                                    Address = x.BankStatement.ObjectCr.RealityObject.Address,
                                    BidDate = x.BidDate,
                                    BidNum = x.BidNum,
                                    DocumentDate = x.DocumentDate,
                                    DocumentNum = x.DocumentNum,
                                    IncomingSum = x.Sum,
                                    RedirectFunds = x.RedirectFunds,
                                    RepeatSend = x.RepeatSend,
                                    PayPurpose = x.PayPurpose,
                                    TypeFinanceSource = x.TypeFinanceSource,
                                    PayerContragent = x.PayerContragent.Name,
                                    ReceiverContragent = x.ReceiverContragent.Name,
                                    BankStatementNum = x.BankStatement.DocumentNum 
                                })
                                .ToList());
            }

            return result.OrderBy(x => x.Municipality).ThenBy(x => x.DocumentDate).ThenBy(x => x.DocumentNum);
        }
    }
}
