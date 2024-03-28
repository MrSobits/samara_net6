namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Properties;
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Entities;
    using Castle.Windsor;

    public class CitizenFundsIncomeExpenseReport : BasePrintForm
    {
        /// <summary>
        /// Идентификатор программы кап. ремонта
        /// </summary>
        private long programCrId;

        /// <summary>
        /// Идентификаторы МО
        /// </summary>
        private List<long> municipalityIds;

        /// <summary>
        /// Дата начала периода
        /// </summary>
        private DateTime reportDateStart;

        /// <summary>
        /// Дата окончания периода
        /// </summary>
        private DateTime reportDateFinish;

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет приход и расход по средствам граждан";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет приход и расход по средствам граждан";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CitizenFundsIncomeExpense";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.CitizenFundsIncomeExpense";
            }
        }

        public CitizenFundsIncomeExpenseReport()
            : base(new ReportTemplateBinary(Resources.CitizenFundsIncomeExpense))
        {
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params.GetAs<long>("reportCrProgram");
            reportDateStart = baseParams.Params.GetAs("reportDateStart", DateTime.MinValue);
            reportDateFinish = baseParams.Params.GetAs("reportDateFinish", DateTime.MinValue);
            
            var munIds = baseParams.Params.ContainsKey("municipalityIds") ? baseParams.Params["municipalityIds"].ToStr() : String.Empty;

            if (!string.IsNullOrEmpty(munIds))
            {
                municipalityIds = munIds.Split(',').Select(x => x.ToLong()).ToList();
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (this.reportDateStart >= this.reportDateFinish)
            {
                return;
            }

            reportParams.SimpleReportParams["ДатаОтчета"] = DateTime.Now.ToString("dd MMMM yyyy");

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияМО");

            // столбец 1,2,3 Объекты кап. ремонта
            var objectsCr =
                Container.Resolve<IDomainService<ObjectCr>>()
                         .GetAll()
                         .Where(
                             x =>
                             municipalityIds.Contains(x.RealityObject.Municipality.Id) &&
                             x.ProgramCr.Id == programCrId).Select(x => new
                                 {
                                     x.Id,
                                     x.RealityObject
                                 }).ToDictionary(
                                 x => x.Id,
                                 arg => new
                                     {
                                         arg.RealityObject.Id,
                                         arg.RealityObject.FiasAddress.StreetName,
                                         arg.RealityObject.FiasAddress.House,
                                         arg.RealityObject.FiasAddress.PlaceName,
                                         munId = arg.RealityObject.Municipality.Id
                                     });

            var objectsCrIds = objectsCr.Keys.ToList();

            var realityObjIds = objectsCr.Select(x => x.Value.Id).ToList();

            // столбец 4,5 платежные поручения
            var paymentOrders = Container.Resolve<IDomainService<BasePaymentOrder>>()
                                        .GetAll()
                                        .Where(
                                            x =>
                                            x.TypeFinanceSource == TypeFinanceSource.OccupantFunds &&
                                            x.TypePaymentOrder == TypePaymentOrder.In &&
                                            x.DocumentDate >= reportDateStart &&
                                            x.DocumentDate <= reportDateFinish &&
                                            objectsCrIds.Contains(x.BankStatement.ObjectCr.Id))
                                           .GroupBy(x => x.BankStatement.ObjectCr.Id)
                                                       .ToDictionary(
                                                           x => x.Key,
                                                           r =>
                                                           new
                                                           {
                                                               munId = r.Min(p => p.BankStatement.ObjectCr.RealityObject.Municipality.Id),

                                                               payerContragent = r.Where(p => p.PayerContragent != null)
                                                                    .Select(p => p.PayerContragent)
                                                                    .Distinct(p => p.Id)
                                                                    .Aggregate(
                                                                        string.Empty,
                                                                        (x, y) => x += x != string.Empty ? ";" + y.Name : y.Name),

                                                               receiverContragent =
                                                                   r.Where(p => p.ReceiverContragent != null)
                                                                    .Select(p => p.ReceiverContragent)
                                                                    .Distinct(p => p.Id).Aggregate(
                                                                       string.Empty,
                                                                       (x, y) => x += x != string.Empty ? ";" + y.Name : y.Name)
                                                           });

            // столбец 6,7 Лимит финансирования по средствам граждан по основной и доп. программе
            var finSourceResources =
                Container.Resolve<IDomainService<FinanceSourceResource>>()
                         .GetAll()
                         .Where(x => objectsCrIds.Contains(x.ObjectCr.Id))
                         .GroupBy(x => x.ObjectCr.Id)
                             .ToDictionary(
                             x => x.Key,
                             r => new
                             {
                                 munId = r.Min(y => y.ObjectCr.RealityObject.Municipality.Id),
                                 mainLimit =
                                                            r.Where(
                                                                y =>
                                                                (y.FinanceSource.Code == "1" || y.FinanceSource.Code == "3")) // Финансирования по 185-ФЗ и финансированию по 185-ФЗ (по доп программам)
                                                               .Sum(y => y.OwnerResource),
                                 additionalLimit =
                                                            r.Where(
                                                                y =>
                                                                y.FinanceSource.Code != "1" && y.FinanceSource.Code != "3") // остальные
                                                                .Sum(y => y.OwnerResource)
                             });

            // столбец 8 Перечисления в Рег. Фонд
            var tranfersRfSum =
                Container.Resolve<IDomainService<TransferRfRecObj>>().GetAll()
                    .Where(x => realityObjIds.Contains(x.RealityObject.Id) 
                        && x.TransferRfRecord.TransferDate >= reportDateStart 
                        && x.TransferRfRecord.TransferDate <= reportDateFinish)
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(r => r.Key,
                    z => new
                        {
                            munId = z.Min(arg => arg.RealityObject.Municipality.Id),
                            sum = z.Sum(arg => arg.Sum)
                        });

            // столбец 9,10 Перечисления ден. средств
            var requestTrasferRf =
                Container.Resolve<IDomainService<TransferFundsRf>>().GetAll()
                    .Where(x => x.RequestTransferRf.ProgramCr.Id == programCrId
                        && x.RequestTransferRf.DateFrom >= reportDateStart
                        && x.RequestTransferRf.DateFrom <= reportDateFinish
                        && realityObjIds.Contains(x.RealityObject.Id))
                     .GroupBy(x => x.RealityObject.Id)
                     .ToDictionary(
                     x => x.Key, 
                     r => new
                         {
                             munId = r.Min(y => y.RealityObject.Municipality.Id),
                             mainSum = r.Where(y => y.RequestTransferRf.TypeProgramRequest == TypeProgramRequest.Primary).Sum(z => z.Sum),
                             additionalSum = r.Where(y => y.RequestTransferRf.TypeProgramRequest != TypeProgramRequest.Primary).Sum(z => z.Sum)
                         });

            decimal mainLimit = 0;
            decimal additionalLimit = 0;
            decimal expenseMain = 0;
            decimal expenseAdditional = 0;

            foreach (var municipalityId in municipalityIds)
            {
                var municipality = Container.Resolve<IDomainService<Municipality>>().Get(municipalityId);

                if (municipality == null)
                {
                    continue;
                }

                section.ДобавитьСтроку();
                section["НазваниеМО"] = municipality.Name;

                // Объекты кап. ремонта конкретного МО
                var objectsCrMo = objectsCr.Where(x => x.Value.munId == municipalityId).ToDictionary(
                                 x => x.Key,
                                 arg => new
                                 {
                                     arg.Value.Id,
                                     arg.Value.StreetName,
                                     arg.Value.House,
                                     arg.Value.PlaceName
                                 });


                var dataSection = section.ДобавитьСекцию("СекцияДанных");

                int i = 0;

                foreach (var objCr in objectsCrMo)
                {
                    mainLimit = 0;
                    additionalLimit = 0;
                    expenseMain = 0;
                    expenseAdditional = 0;

                    dataSection.ДобавитьСтроку();

                    dataSection["НаселенныйПункт"] = objCr.Value.PlaceName;
                    dataSection["АдресДома"] = String.Format(
                        "{0}, д.{1}",
                        objCr.Value.StreetName,
                        objCr.Value.House);

                    if (paymentOrders.ContainsKey(objCr.Key))
                    {
                        dataSection["Плательщик"] = paymentOrders[objCr.Key].payerContragent;
                        dataSection["ПолучательCредств"] = paymentOrders[objCr.Key].receiverContragent;
                    }

                    if (finSourceResources.ContainsKey(objCr.Key))
                    {
                        mainLimit = finSourceResources[objCr.Key].mainLimit != null
                                         ? finSourceResources[objCr.Key].mainLimit.ToDecimal()
                                         : 0;

                        additionalLimit = finSourceResources[objCr.Key].additionalLimit != null
                                         ? finSourceResources[objCr.Key].additionalLimit.ToDecimal()
                                         : 0;

                        dataSection["ЛимитОсновной"] = mainLimit;
                        dataSection["ЛимитДополнительный"] = additionalLimit;
                    }

                    if (tranfersRfSum.ContainsKey(objCr.Value.Id))
                    {
                        dataSection["ПриходУО"] = tranfersRfSum[objCr.Value.Id].sum;
                    }

                    if (requestTrasferRf.ContainsKey(objCr.Value.Id))
                    {
                        expenseAdditional = requestTrasferRf[objCr.Value.Id].additionalSum != null
                                                ? requestTrasferRf[objCr.Value.Id].additionalSum.ToDecimal()
                                                : 0;

                        expenseMain = requestTrasferRf[objCr.Value.Id].mainSum != null
                                          ? requestTrasferRf[objCr.Value.Id].mainSum.ToDecimal()
                                          : 0;

                        dataSection["РасходДополнительный"] = expenseAdditional;
                        dataSection["РасходОсновной"] = expenseMain;
                    }

                    dataSection["ОстатокОсновной"] = mainLimit - expenseMain;
                    dataSection["ОстатокДополнительный"] = additionalLimit - expenseAdditional;

                    i++;
                }

                // Раздел Итого по Муниципальному образованию
                mainLimit = finSourceResources.Where(x => x.Value.munId == municipalityId).Sum(x => x.Value.mainLimit).ToDecimal();
                additionalLimit = finSourceResources.Where(x => x.Value.munId == municipalityId).Sum(x => x.Value.additionalLimit).ToDecimal();
                expenseMain = requestTrasferRf.Where(x => x.Value.munId == municipalityId).Sum(x => x.Value.mainSum).ToDecimal();
                expenseAdditional = requestTrasferRf.Where(x => x.Value.munId == municipalityId).Sum(x => x.Value.additionalSum).ToDecimal();

                section["МоИтогоЛимитОсновной"] = mainLimit;
                section["МоИтогоЛимитДополнительный"] = additionalLimit;
                section["МоИтогоПриходУО"] = tranfersRfSum.Where(x => x.Value.munId == municipalityId).Sum(x => x.Value.sum);
                section["МоИтогоРасходОсновной"] = expenseMain;
                section["МоИтогоРасходДополнительный"] = expenseAdditional;

                section["МоИтогоОстатокОсновной"] = mainLimit - expenseMain;
                section["МоИтогоОстатокДополнительный"] = additionalLimit - expenseAdditional;
            }


            // Раздел Итого отчета
            mainLimit = finSourceResources.Sum(x => x.Value.mainLimit).ToDecimal();
            additionalLimit = finSourceResources.Sum(x => x.Value.additionalLimit).ToDecimal();
            expenseMain = requestTrasferRf.Sum(x => x.Value.mainSum).ToDecimal();
            expenseAdditional = requestTrasferRf.Sum(x => x.Value.additionalSum).ToDecimal();

            reportParams.SimpleReportParams["ИтогоЛимитОсновной"] = mainLimit;
            reportParams.SimpleReportParams["ИтогоЛимитДополнительный"] = additionalLimit;
            reportParams.SimpleReportParams["ИтогоПриходУО"] = tranfersRfSum.Sum(x => x.Value.sum);
            reportParams.SimpleReportParams["ИтогоРасходОсновной"] = expenseMain;
            reportParams.SimpleReportParams["ИтогоРасходДополнительный"] = expenseAdditional;

            reportParams.SimpleReportParams["ИтогоОстатокОсновной"] = mainLimit - expenseMain;
            reportParams.SimpleReportParams["ИтогоОстатокДополнительный"] = additionalLimit - expenseAdditional;
        }
    }
}
