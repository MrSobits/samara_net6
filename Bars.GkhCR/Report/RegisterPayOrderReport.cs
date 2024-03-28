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

    using Castle.Windsor;

    /// <summary>
    /// Отчет "Реестр платежных документов (за период)"
    /// </summary>
    class RegisterPayOrderReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        // идентификатор программы КР
        private int programCrId = 0;
        private readonly List<long> municipalityIds = new List<long>();
        private DateTime startDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MaxValue;
        private TypeFinanceGroup typeFinGroup;

        public RegisterPayOrderReport()
            : base(new ReportTemplateBinary(Properties.Resources.Financing_CheckRegister))
        {
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.Financing_CheckRegister"; }
        }

        public override string Desciption
        {
            get { return "Реестр платежных документов (за период)"; }
        }

        public override string GroupName
        {
            get { return "Финансирование"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RegisterPayOrder"; }
        }

        public override string Name
        {
            get { return "Реестр платежных документов (за период)"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();
            startDate = baseParams.Params["dateStart"].ToDateTime();

            var dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            endDate = dateEnd != DateTime.MinValue ? dateEnd : DateTime.Now;

            typeFinGroup = baseParams.Params["typeFinGroup"].As<TypeFinanceGroup>();
            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
                                  ? baseParams.Params["municipalityIds"].ToString()
                                  : string.Empty;

            if (!string.IsNullOrEmpty(municipalityIdsList))
            {
                if (municipalityIdsList.Contains(','))
                {
                    municipalityIds.AddRange(municipalityIdsList.Split(',').Select(id => id.ToLong()));
                }
                else
                {
                    municipalityIds.Add(municipalityIdsList.ToInt());
                }
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["НачалоПериода"] = startDate.ToShortDateString();
            reportParams.SimpleReportParams["КонецПериода"] = endDate.ToShortDateString();

            var paymentOrders = Container.Resolve<IDomainService<BasePaymentOrder>>()
                .GetAll()
                .WhereIf(typeFinGroup != 0, x => x.BankStatement.TypeFinanceGroup == typeFinGroup)
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.BankStatement.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.DocumentDate > startDate && x.DocumentDate < endDate && x.BankStatement.ObjectCr.ProgramCr.Id == programCrId)
                .Select(x => new
                {
                    Municipality = x.BankStatement.ObjectCr.RealityObject.Municipality.Name,
                    x.BankStatement.ObjectCr.RealityObject.Address,
                    DateNum = string.Format("{0} от {1}", x.DocumentNum, x.DocumentDate.HasValue ? x.DocumentDate.ToDateTime().ToShortDateString() : string.Empty),
                    BidDateNum = string.Format("{0} от {1}", x.BidNum, x.BidDate.HasValue ? x.BidDate.ToDateTime().ToShortDateString() : string.Empty),
                    FinSource = x.TypeFinanceSource.GetEnumMeta().Display,
                    In = x.TypePaymentOrder == TypePaymentOrder.In ? x.Sum : 0,
                    Out = x.TypePaymentOrder == TypePaymentOrder.Out ? x.Sum : 0,
                    PayerName = x.PayerContragent.Name,
                    ReceiverName = x.ReceiverContragent.Name,
                    x.PayPurpose
                })
                .ToList();

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияРайоны");

            foreach (var municipalityData in paymentOrders.GroupBy(x => x.Municipality).OrderBy(x => x.Key))
            {
                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["МунРайон"] = municipalityData.Key;
                sectionMunicipality["СуммаПриходРайон"] = municipalityData.Sum(y => y.In);
                sectionMunicipality["СуммаРасходРайон"] = municipalityData.Sum(y => y.Out);

                var sectionRealObj = sectionMunicipality.ДобавитьСекцию("СекцияДома");

                foreach (var realtyObjectData in municipalityData.GroupBy(x => x.Address).OrderBy(x => x.Key))
                {
                    sectionRealObj.ДобавитьСтроку();
                    sectionRealObj["АдресДома"] = realtyObjectData.Key;

                    var sectionPayOrder = sectionRealObj.ДобавитьСекцию("СекцияВыписки");

                    var i = 0;
                    foreach (var paymentOrder in realtyObjectData)
                    {
                        sectionPayOrder.ДобавитьСтроку();
                        sectionPayOrder["НомВыписки"] = ++i;
                        sectionPayOrder["ДатаНомер"] = paymentOrder.DateNum;
                        sectionPayOrder["ДатаНомерЗаявки"] = paymentOrder.BidDateNum;
                        sectionPayOrder["Источник"] = paymentOrder.FinSource;
                        sectionPayOrder["СуммаПриход"] = paymentOrder.In;
                        sectionPayOrder["СуммаРасход"] = paymentOrder.Out;
                        sectionPayOrder["Плательщик"] = paymentOrder.PayerName;
                        sectionPayOrder["Получатель"] = paymentOrder.ReceiverName;
                        sectionPayOrder["Назначение"] = paymentOrder.PayPurpose;
                    }

                    sectionRealObj["СуммаПриходДом"] = realtyObjectData.Sum(x => x.In);
                    sectionRealObj["СуммаРасходДом"] = realtyObjectData.Sum(x => x.Out);
                }
            }

            reportParams.SimpleReportParams["СуммаПриходВсего"] = paymentOrders.Sum(x => x.In);
            reportParams.SimpleReportParams["СуммаРасходВсего"] = paymentOrders.Sum(x => x.Out);
        }
    }
}