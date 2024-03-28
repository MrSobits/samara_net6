namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет МО о ходе работ (по домам)
    /// </summary>
    public class ObjectCrInfoService : BasePrintForm
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] finSourceIds;
        private DateTime reportDate = DateTime.MinValue;

        public ObjectCrInfoService()
            : base(new ReportTemplateBinary(Properties.Resources.ObjectCrInfoService))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ObjectCrInfoService"; }
        }

        public override string Desciption
        {
            get { return "Отчет информация по объектам о ходе КР по услугам (ГЖИ)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ObjectCrInfoService"; }
        }

        public override string Name
        {
            get { return "Отчет информация по объектам о ходе КР по услугам (ГЖИ)"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            reportDate = baseParams.Params["reportDate"].ToDateTime();

            var finSourceIdsList = baseParams.Params.ContainsKey("finSourceIds")
                      ? baseParams.Params["finSourceIds"].ToString()
                      : string.Empty;

            this.finSourceIds = !string.IsNullOrEmpty(finSourceIdsList) ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Get(this.programCrId);
            var programCrName = programCr.Name;
            var programCrYear = programCr.Period.DateEnd.HasValue
                        ? programCr.Period.DateEnd.Value.Year
                        : this.reportDate.Date.Year;

            var builderContractService = this.Container.Resolve<IDomainService<BuildContract>>();
            var actService = this.Container.Resolve<IDomainService<PerformedWorkAct>>();

            var workCodes = new List<string> { "1018", "1019", "1020", "30" };

            var works = Container.Resolve<IDomainService<Work>>().GetAll()
                .Where(x => x.TypeWork == TypeWork.Service)
                .Where(x => workCodes.Contains(x.Code))
                .Select(x => new { x.Id, x.Name, UnitMeasureName = x.UnitMeasure.Name, x.Code })
                .ToList();

            var builderByRealtyObjectDict = builderContractService.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Select(x => new { x.ObjectCr.RealityObject.Id, x.Builder.Contragent.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Name).First());

            // Если собирают отчет с дато1 >= 2013 года то тянем через Архив СМР
            // Иначе берем просто последние записи
            var typeWorks = programCrYear >= 2013
                ? Container.Resolve<IDomainService<ArchiveSmr>>()
                         .GetAll()
                         .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId && x.TypeWorkCr.Work.TypeWork == TypeWork.Service)
                         .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                         .Where(x => workCodes.Contains(x.TypeWorkCr.Work.Code))
                         .Where(x => x.ObjectCreateDate <= reportDate)
                         .Select(x => new
                         {
                             Municipality = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Name,
                             x.TypeWorkCr.ObjectCr.GjiNum,
                             x.TypeWorkCr.ObjectCr.DateAcceptCrGji,
                             x.TypeWorkCr.ObjectCr.DateEndBuilder,
                             x.TypeWorkCr.ObjectCr.DateStopWorkGji,
                             x.TypeWorkCr.ObjectCr.RealityObject.Address,
                             RealObjId = x.TypeWorkCr.ObjectCr.RealityObject.Id,
                             WorkId = x.TypeWorkCr.Work.Id,
                             FinanceSourceId = x.TypeWorkCr.FinanceSource.Id,
                             FinanceSourceName = x.TypeWorkCr.FinanceSource.Name,
                             x.TypeWorkCr.Volume,
                             x.TypeWorkCr.Sum,
                             x.TypeWorkCr.DateStartWork,
                             x.TypeWorkCr.DateEndWork,
                             x.VolumeOfCompletion,
                             x.CostSum,
                             x.PercentOfCompletion,
                             CountAct = actService.GetAll().Count(y => y.TypeWorkCr.Id == x.TypeWorkCr.Id),
                             SumAct = actService.GetAll().Where(y => y.TypeWorkCr.Id == x.TypeWorkCr.Id).Sum(y => y.Sum),
                             TypeWorkCrId = x.TypeWorkCr.Id,
                             archId = x.Id
                         })
                         .AsEnumerable()
                         .GroupBy(x => x.TypeWorkCrId)
                         .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.archId).FirstOrDefault())
                         .Select(x => x.Value)
                         .GroupBy(x => new
                         {
                             x.RealObjId,
                             x.FinanceSourceId,
                             x.GjiNum,
                             x.Address,
                             x.FinanceSourceName,
                             x.Municipality,
                             x.DateAcceptCrGji,
                             x.DateEndBuilder,
                             x.DateStopWorkGji,
                         })
                         .OrderBy(x => x.Key.Municipality)
                         .ThenBy(x => x.Key.Address)
                         .ThenBy(x => x.Key.FinanceSourceName)
                         .ToDictionary(
                             x => x.Key,
                             y =>
                             y.ToDictionary(
                                 x => x.WorkId,
                                 z =>
                                 new
                                 {
                                     z.Volume,
                                     z.Sum,
                                     z.DateStartWork,
                                     z.DateEndWork,
                                     z.VolumeOfCompletion,
                                     z.CostSum,
                                     z.PercentOfCompletion,
                                     z.CountAct,
                                     z.SumAct
                                 }))
                : Container.Resolve<IDomainService<TypeWorkCr>>()
                         .GetAll()
                         .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work.TypeWork == TypeWork.Service)
                         .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.FinanceSource.Id))
                         .Where(x => workCodes.Contains(x.Work.Code))
                         .Select(
                             x =>
                             new
                                 {
                                     Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                                     x.ObjectCr.GjiNum,
                                     x.ObjectCr.DateAcceptCrGji,
                                     x.ObjectCr.DateEndBuilder,
                                     x.ObjectCr.DateStopWorkGji,
                                     x.ObjectCr.RealityObject.Address,
                                     RealObjId = x.ObjectCr.RealityObject.Id,
                                     WorkId = x.Work.Id,
                                     FinanceSourceId = x.FinanceSource.Id,
                                     FinanceSourceName = x.FinanceSource.Name,
                                     x.Volume,
                                     x.Sum,
                                     x.DateStartWork,
                                     x.DateEndWork,
                                     x.VolumeOfCompletion,
                                     x.CostSum,
                                     x.PercentOfCompletion,
                                     CountAct = actService.GetAll().Count(y => y.TypeWorkCr.Id == x.Id),
                                     SumAct = actService.GetAll().Where(y => y.TypeWorkCr.Id == x.Id).Sum(y => y.Sum)
                                 })
                         .AsEnumerable()
                         .GroupBy(x => new
                         {
                             x.RealObjId,
                             x.FinanceSourceId,
                             x.GjiNum,
                             x.Address,
                             x.FinanceSourceName,
                             x.Municipality,
                             x.DateAcceptCrGji,
                             x.DateEndBuilder,
                             x.DateStopWorkGji,
                         })
                         .OrderBy(x => x.Key.Municipality)
                         .ThenBy(x => x.Key.Address)
                         .ThenBy(x => x.Key.FinanceSourceName)
                         .ToDictionary(
                             x => x.Key,
                             y =>
                             y.ToDictionary(
                                 x => x.WorkId,
                                 z =>
                                 new
                                     {
                                         z.Volume,
                                         z.Sum,
                                         z.DateStartWork,
                                         z.DateEndWork,
                                         z.VolumeOfCompletion,
                                         z.CostSum,
                                         z.PercentOfCompletion,
                                         z.CountAct,
                                         z.SumAct
                                     }));

            var vertColumn = reportParams.ComplexReportParams.ДобавитьСекцию("ВертСекция");

            var columnNumber = 9;
            foreach (var work in works)
            {
                vertColumn.ДобавитьСтроку();

                vertColumn["Работа"] = work.Name;
                vertColumn["Колонка1"] = columnNumber + 1;
                vertColumn["Колонка2"] = columnNumber + 2;
                vertColumn["Колонка3"] = columnNumber + 3;
                vertColumn["Колонка4"] = columnNumber + 4;
                vertColumn["Колонка5"] = columnNumber + 5;
                vertColumn["Колонка6"] = columnNumber + 6;
                vertColumn["Колонка7"] = columnNumber + 7;
                vertColumn["Колонка8"] = columnNumber + 8;
                vertColumn["Колонка9"] = columnNumber + 9;
                vertColumn["ОбъемСмета"] = string.Format("$ОбъемСмета{0}$", work.Id);
                vertColumn["СуммаСмета"] = string.Format("$СуммаСмета{0}$", work.Id);
                vertColumn["НачДата"] = string.Format("$НачДата{0}$", work.Id);
                vertColumn["КонДата"] = string.Format("$КонДата{0}$", work.Id);
                vertColumn["ОбъемФакт"] = string.Format("$ОбъемФакт{0}$", work.Id);
                vertColumn["СуммаФакт"] = string.Format("$СуммаФакт{0}$", work.Id);
                vertColumn["Процент"] = string.Format("$Процент{0}$", work.Id);
                vertColumn["Количество"] = string.Format("$Количество{0}$", work.Id);
                vertColumn["Сумма"] = string.Format("$Сумма{0}$", work.Id);
                columnNumber += 8;
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("ГоризСекция");

            foreach (var typeWork in typeWorks)
            {
                section.ДобавитьСтроку();
                section["Район"] = typeWork.Key.Municipality;
                section["Подрядчик"] = builderByRealtyObjectDict.ContainsKey(typeWork.Key.RealObjId) ? builderByRealtyObjectDict[typeWork.Key.RealObjId] : string.Empty;
                section["НомерГЖИ"] = typeWork.Key.GjiNum;
                section["Адрес"] = typeWork.Key.Address;
                section["КапРемонтПринят"] = typeWork.Key.DateAcceptCrGji.HasValue && typeWork.Key.DateAcceptCrGji.Value != DateTime.MinValue ? typeWork.Key.DateAcceptCrGji.Value.ToShortDateString() : string.Empty;
                section["КапРемонтЗавершены"] = typeWork.Key.DateEndBuilder.HasValue && typeWork.Key.DateEndBuilder.Value != DateTime.MinValue ? typeWork.Key.DateEndBuilder.Value.ToShortDateString() : string.Empty;
                section["РаботыОстановлены"] = typeWork.Key.DateStopWorkGji.HasValue && typeWork.Key.DateStopWorkGji.Value != DateTime.MinValue ? typeWork.Key.DateStopWorkGji.Value.ToShortDateString() : string.Empty;
                section["ИсточникФинансирования"] = typeWork.Key.FinanceSourceName;

                foreach (var work in typeWork.Value)
                {
                    section["ОбъемСмета" + work.Key] = work.Value.Volume.ToDecimal().RoundDecimal(2);
                    section["СуммаСмета" + work.Key] = work.Value.Sum.ToDecimal().RoundDecimal(2);
                    section["НачДата" + work.Key] = work.Value.DateStartWork.HasValue && work.Value.DateStartWork.Value != DateTime.MinValue ? work.Value.DateStartWork.Value.ToShortDateString() : string.Empty;
                    section["КонДата" + work.Key] = work.Value.DateEndWork.HasValue && work.Value.DateEndWork.Value != DateTime.MinValue ? work.Value.DateEndWork.Value.ToShortDateString() : string.Empty;
                    section["ОбъемФакт" + work.Key] = work.Value.VolumeOfCompletion.ToDecimal().RoundDecimal(2);
                    section["СуммаФакт" + work.Key] = work.Value.CostSum.ToDecimal().RoundDecimal(2);
                    section["Процент" + work.Key] = work.Value.PercentOfCompletion.ToDecimal().RoundDecimal(2);
                    section["Количество" + work.Key] = work.Value.CountAct.ToInt();
                    section["Сумма" + work.Key] = work.Value.SumAct.ToDecimal().RoundDecimal(2);
                }

                section["ОбъемСмета"] = typeWork.Value.Sum(x => x.Value.Volume).ToDecimal().RoundDecimal(2);
                section["СуммаСмета"] = typeWork.Value.Sum(x => x.Value.Sum).ToDecimal().RoundDecimal(2);
                section["ОбъемФакт"] = typeWork.Value.Sum(x => x.Value.VolumeOfCompletion).ToDecimal().RoundDecimal(2);
                section["СуммаФакт"] = typeWork.Value.Sum(x => x.Value.CostSum).ToDecimal().RoundDecimal(2);
                var count = typeWork.Value.Count(x => x.Value.PercentOfCompletion.HasValue).ToDecimal();
                section["Процент"] = count == 0 ? 0m : decimal.Divide(typeWork.Value.Sum(x => x.Value.PercentOfCompletion).ToDecimal(), count).RoundDecimal(2);
                section["Количество"] = typeWork.Value.Sum(x => x.Value.CountAct).ToInt();
                section["Сумма"] = typeWork.Value.Sum(x => x.Value.SumAct).ToDecimal().RoundDecimal(2);
                var minStartDate = typeWork.Value.Min(x => x.Value.DateStartWork);
                section["НачДатаМин"] = minStartDate.HasValue && minStartDate.Value != DateTime.MinValue ? minStartDate.Value.ToShortDateString() : string.Empty;
                var maxStartDate = typeWork.Value.Max(x => x.Value.DateEndWork);
                section["КонДатаМакс"] = maxStartDate.HasValue && maxStartDate.Value != DateTime.MinValue ? maxStartDate.Value.ToShortDateString() : string.Empty;

            }

            reportParams.SimpleReportParams["ДатаОтчета"] = reportDate.ToShortDateString();
            reportParams.SimpleReportParams["Программа"] = programCrName;
            reportParams.SimpleReportParams["Колонка1"] = columnNumber + 1;
            reportParams.SimpleReportParams["Колонка2"] = columnNumber + 2;
            reportParams.SimpleReportParams["Колонка3"] = columnNumber + 3;
            reportParams.SimpleReportParams["Колонка4"] = columnNumber + 4;
            reportParams.SimpleReportParams["Колонка5"] = columnNumber + 5;
            reportParams.SimpleReportParams["Колонка6"] = columnNumber + 6;
            reportParams.SimpleReportParams["Колонка7"] = columnNumber + 7;
            reportParams.SimpleReportParams["Колонка8"] = columnNumber + 8;
            reportParams.SimpleReportParams["Колонка9"] = columnNumber + 9;
        }
    }
}