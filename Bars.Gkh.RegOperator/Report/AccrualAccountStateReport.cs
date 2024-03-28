using System;

namespace Bars.Gkh.RegOperator.Report
{
    using Bars.B4;
    using B4.Modules.Reports;
    
    using Bars.B4.Utils;

    using System.Linq;

    using Bars.Gkh.RegOperator.Entities;

    public class AccrualAccountStateReport : BasePrintForm
    {
        public IDomainService<PersonalAccountPeriodSummary> Service { get; set; }

        private long[] municipalityIds;

        public AccrualAccountStateReport()
            : base(new ReportTemplateBinary(Properties.Resources.AccrualAccountStateReport))
        {
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }


        public override void PrepareReport(ReportParams reportParams)
        {

            var allEntities =
                Service.GetAll()
                    .WhereIf(
                        municipalityIds.Any(),
                        x =>
                        x.PersonalAccount.Room.RealityObject.Municipality != null
                        && municipalityIds.Contains(x.PersonalAccount.Room.RealityObject.Municipality.Id)
                        || x.PersonalAccount.Room.RealityObject.MoSettlement != null
                        && municipalityIds.Contains(x.PersonalAccount.Room.RealityObject.MoSettlement.Id))
                    .GroupBy(x => x.PersonalAccount.Room.RealityObject.Municipality.Id)
                    .ToDictionary(
                        x => x.Key,
                        arg =>
                        arg.OrderBy(
                            z =>
                            z.PersonalAccount.Room.RealityObject.MoSettlement != null
                                ? z.PersonalAccount.Room.RealityObject.MoSettlement.Name
                                : z.PersonalAccount.Room.RealityObject.Municipality != null
                                      ? z.PersonalAccount.Room.RealityObject.Municipality.Name
                                      : string.Empty)
                            .ThenBy(z => z.PersonalAccount.Room.RealityObject.Address)
                            .ThenBy(z => z.Period.StartDate)
                            .ToList());

            reportParams.SimpleReportParams["ДатаОтчета"] = DateTime.Now.ToString("dd.MM.yyyy"); 

            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMo");

            foreach (var mo in allEntities.Keys)
            {
                sectionMo.ДобавитьСтроку();

                var sectionPeriod = sectionMo.ДобавитьСекцию("sectionPeriod");

                var groupByRealityOBj = allEntities[mo].GroupBy(x =>
                    new
                    {
                        x.PersonalAccount.Room.RealityObject,
                        x.Period
                    }
                    ).ToDictionary(x => x.Key, arg => new
                    {
                        SaldoIn = arg.Sum(x => x.SaldoIn),
                        ChargeTariff = arg.Sum(x => x.ChargeTariff),
                        TariffPayment = arg.Sum(x => x.TariffPayment),
                        Penalty = arg.Sum(x => x.Penalty),
                        PenaltyPayment = arg.Sum(x => x.PenaltyPayment),
                        SaldoOut = arg.Sum(x => x.SaldoOut)
                    }).OrderBy(x => x.Key.RealityObject.Address).ThenBy(x => x.Key.Period.StartDate);


                foreach (var obj in groupByRealityOBj)
                {
                    sectionPeriod.ДобавитьСтроку();

                    sectionPeriod["МуниципальныйРайон"] = obj.Key.RealityObject.Municipality.Name;
                    sectionPeriod["МуниципальноеОбразование"] = obj.Key.RealityObject.MoSettlement.ReturnSafe(x => x.Name);
                    sectionPeriod["Адрес"] = obj.Key.RealityObject.Address;
                    sectionPeriod["Период"] = obj.Key.Period.Name;
                    sectionPeriod["ВходящееCальдо"] = obj.Value.SaldoIn;
                    sectionPeriod["ИтогоНачислено"] = obj.Value.ChargeTariff;
                    sectionPeriod["ИтогоОплачено"] = obj.Value.TariffPayment;
                    sectionPeriod["НачисленоПени"] = obj.Value.Penalty;
                    sectionPeriod["ОплаченоПени"] = obj.Value.PenaltyPayment;
                    sectionPeriod["ИсходящееСальдо"] = obj.Value.SaldoOut;
                }

                sectionMo["суммаНачислено"] = allEntities[mo].Sum(x => x.ChargeTariff);
                sectionMo["суммаОплачено"] = allEntities[mo].Sum(x => x.TariffPayment);
                sectionMo["суммаНачисленоПени"] = allEntities[mo].Sum(x => x.Penalty);
                sectionMo["ОплаченоПени"] = allEntities[mo].Sum(x => x.PenaltyPayment);

            }
        }

        public override string Name
        {
            get { return "Отчет по состоянию счета начислений"; }
        }

        public override string Desciption
        {
            get { return "Отчет по состоянию счета начислений"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.AccrualAccountStateReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.AccrualAccountStateReport"; }
        }
    }
}
