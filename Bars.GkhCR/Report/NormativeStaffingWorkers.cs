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

    public class NormativeStaffingWorkers : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programId = 0;
        private DateTime reportDate = DateTime.Now;
        private long[] finSourceIds;
        private long[] municipalityIds;

        private decimal workDaysCoef = decimal.Divide(6, 7);

        public NormativeStaffingWorkers()
            : base(new ReportTemplateBinary(Properties.Resources.NormativeWorkersCount))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.NormativeStaffingWorkers";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "05_Численность рабочих на заданную дату"; }
        }

        /// <summary>
        /// Группа
        /// </summary>
        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        /// <summary>
        /// Представление с пользователскими параметрами
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.NormativeStaffingWorkers"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "05_Численность рабочих на заданную дату"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programId = baseParams.Params["programCrId"].ToInt();

            reportDate = baseParams.Params["reportDate"].ToDateTime() != DateTime.MinValue ? baseParams.Params["reportDate"].ToDateTime() : DateTime.Now;

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
              ? baseParams.Params["municipalityIds"].ToString()
              : string.Empty;

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var finSourceIdsList = baseParams.Params.ContainsKey("finSources")
             ? baseParams.Params["finSources"].ToString()
             : string.Empty;

            finSourceIds = !string.IsNullOrEmpty(finSourceIdsList) ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var objectsCrCount = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .GroupBy(x => x.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToDictionary(x => x.Key, x => x.count);

            var works = Container.Resolve<IDomainService<Work>>()
                                     .GetAll()
                                     .Where(x => x.Consistent185Fz && x.TypeWork == TypeWork.Work)
                                     .ToArray();

            var vertColumn = reportParams.ComplexReportParams.ДобавитьСекцию("ВертСекция");

            foreach (var work in works)
            {
                vertColumn.ДобавитьСтроку();
                vertColumn["ВидРаботы"] = work.Name;
                vertColumn["Норматив"] = string.Format("$Норматив{0}$", work.Id);
                vertColumn["Факт"] = string.Format("$Факт{0}$", work.Id);
                vertColumn["Отклонение"] = string.Format("$Отклонение{0}$", work.Id);
                vertColumn["СуммаНорматив"] = string.Format("$СуммаНорматив{0}$", work.Id);
                vertColumn["СуммаФакт"] = string.Format("$СуммаФакт{0}$", work.Id);
                vertColumn["СуммаОтклонение"] = string.Format("$СуммаОтклонение{0}$", work.Id);
            }

            var typeWorks = reportDate.Date.Year >= 2013 ?
                 Container.Resolve<IDomainService<ArchiveSmr>>().GetAll()
                         .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programId)
                                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                                     .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                                     .Where(x => x.TypeWorkCr.Work.TypeWork == TypeWork.Work && x.TypeWorkCr.Work.Consistent185Fz)
                                     .Where(x => x.ObjectCreateDate < reportDate.AddDays(1))
                                     .Select(x => new
                                                       {
                                                           TypeWorkCrId = x.TypeWorkCr.Id,
                                                           x.ObjectCreateDate,
                                                           x.TypeWorkCr.Work.Id,
                                                           DateStartWork = x.TypeWorkCr.DateStartWork.ToDateTime(),
                                                           DateEndWork = x.TypeWorkCr.DateEndWork.ToDateTime(),
                                                           PercentOfCompletion = x.PercentOfCompletion.ToDecimal(),
                                                           Normative = x.TypeWorkCr.Work.Normative.ToDecimal(),
                                                           Volume = x.TypeWorkCr.Volume.ToDecimal(),
                                                           CountWorker = x.CountWorker.ToDecimal(), 
                                                           x.TypeWorkCr.ObjectCr.RealityObject.Municipality,
                                                           RealObjId = x.TypeWorkCr.ObjectCr.Id
                                                       })
                                      .AsEnumerable()
                                      .GroupBy(x => x.TypeWorkCrId)
                                      .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.ObjectCreateDate).First())
                                      .Select(x => x.Value)
                                      .Select(x => new
                                                       {
                                                           FactCount = GetFactCount(x.CountWorker, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                                           NormativeCount = GetNormativeCount(x.Normative * x.Volume, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                                           x.Municipality,
                                                           x.Id,
                                                           x.RealObjId
                                                       })
                                      .OrderBy(x => x.Municipality.Name)
                                      .GroupBy(x => x.Municipality)
                                      .ToDictionary(
                                          x => x.Key,
                                          y => new
                                                   {
                                                       RealObjCount = y.Select(x => x.RealObjId).Distinct().Count(),
                                                       Dict = y.GroupBy(x => x.Id).ToDictionary(
                                                           x => x.Key,
                                                           z => new
                                                                    {
                                                                                                       NormativeCount = z.Sum(x => x.NormativeCount),
                                                                                                       FactCount = z.Sum(x => x.FactCount)
                                                                                                   })
                                                   })

                : Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                                     .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                     .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.FinanceSource.Id))
                                     .Where(x => x.Work.TypeWork == TypeWork.Work && x.Work.Consistent185Fz)
                                     .Select(x => new
                                                       {
                                                           x.Work.Id,
                                                           DateStartWork = x.DateStartWork.ToDateTime(),
                                                           DateEndWork = x.DateEndWork.ToDateTime(),
                                                           PercentOfCompletion = x.PercentOfCompletion.ToDecimal(),
                                                           Normative = x.Work.Normative.ToDecimal(),
                                                           Volume = x.Volume.ToDecimal(),
                                                           CountWorker = x.CountWorker.ToDecimal(), x.ObjectCr.RealityObject.Municipality,
                                                           RealObjId = x.ObjectCr.RealityObject.Id
                                                       })
                                      .AsEnumerable()
                                      .Select(x => new
                                                       {
                                                           FactCount = GetFactCount(x.CountWorker, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                                           NormativeCount = GetNormativeCount(x.Normative * x.Volume, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                                           x.Municipality,
                                                           x.Id,
                                                           x.RealObjId
                                                       })
                                      .OrderBy(x => x.Municipality.Name)
                                      .GroupBy(x => x.Municipality)
                                      .ToDictionary(
                                          x => x.Key,
                                          y => new
                                                   {
                                                       RealObjCount = y.Select(x => x.RealObjId).Distinct().Count(),
                                                       Dict = y.GroupBy(x => x.Id).ToDictionary(
                                                           x => x.Key,
                                                           z => new
                                                                    {
                                                                                                       NormativeCount = z.Sum(x => x.NormativeCount),
                                                                                                       FactCount = z.Sum(x => x.FactCount)
                                                                                                   })
                                                   });

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияДанных");

            int number = 0;
            foreach (var typeWork in typeWorks)
            {
                number++;
                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["Номер"] = number;
                sectionMunicipality["МуниципальноеОбразование"] = typeWork.Key.Name;

                if (objectsCrCount.ContainsKey(typeWork.Key.Id))
                {
                    sectionMunicipality["КоличествоДомов"] = objectsCrCount[typeWork.Key.Id];
                }
                
                var typeWorkCr = typeWork.Value.Dict;

                foreach (var work in works)
                {
                    if (typeWorks[typeWork.Key].Dict.ContainsKey(work.Id))
                    {
                        sectionMunicipality["Норматив" + work.Id] = typeWorkCr[work.Id].NormativeCount;
                        sectionMunicipality["Факт" + work.Id] = typeWorkCr[work.Id].FactCount;
                        sectionMunicipality["Отклонение" + work.Id] = typeWorkCr[work.Id].NormativeCount
                                                                      - typeWorkCr[work.Id].FactCount;
                    }
                    else
                    {
                        sectionMunicipality["Норматив" + work.Id] = 0;
                        sectionMunicipality["Факт" + work.Id] = 0;
                        sectionMunicipality["Отклонение" + work.Id] = 0;
                    }
                }

                sectionMunicipality["ИтогоНорматив"] = typeWorkCr.Values.Sum(x => x.NormativeCount);
                sectionMunicipality["ИтогоФакт"] = typeWorkCr.Values.Sum(x => x.FactCount);
                sectionMunicipality["ИтогоОтклонение"] = typeWorkCr.Values.Sum(x => x.NormativeCount) - typeWorkCr.Values.Sum(x => x.FactCount);
            }

            reportParams.SimpleReportParams["СуммаКоличествоДомов"] = objectsCrCount.Values.Sum();

            var typeWorksCr = typeWorks.Values.Select(x => x.Dict);
            foreach (var work in works)
            {
                reportParams.SimpleReportParams["СуммаНорматив" + work.Id] = typeWorksCr.Sum(x => x.Where(y => y.Key == work.Id).Sum(y => y.Value.NormativeCount));
                reportParams.SimpleReportParams["СуммаФакт" + work.Id] = typeWorksCr.Sum(x => x.Where(y => y.Key == work.Id).Sum(y => y.Value.FactCount));
                reportParams.SimpleReportParams["СуммаОтклонение" + work.Id] = typeWorksCr.Sum(x => x.Where(y => y.Key == work.Id).Sum(y => y.Value.NormativeCount)) - typeWorks.Values.Select(x => x.Dict).Sum(x => x.Where(y => y.Key == work.Id).Sum(y => y.Value.FactCount));
            }

            reportParams.SimpleReportParams["ДатаОтчета"] = reportDate.ToShortDateString();
            reportParams.SimpleReportParams["ИтогоСуммаНорматив"] = typeWorks.Values.Select(x => x.Dict).Sum(x => x.Values.Sum(y => y.NormativeCount));
            reportParams.SimpleReportParams["ИтогоСуммаФакт"] = typeWorks.Values.Select(x => x.Dict).Sum(x => x.Values.Sum(y => y.FactCount));
            reportParams.SimpleReportParams["ИтогоСуммаОтклонение"] = typeWorks.Values.Select(x => x.Dict).Sum(x => x.Values.Sum(y => y.NormativeCount)) - typeWorks.Values.Select(x => x.Dict).Sum(x => x.Values.Sum(y => y.FactCount));
        }

        private decimal GetNormativeCount(decimal count, DateTime startDate, DateTime endDate, decimal percent)
        {
            if (reportDate < startDate || reportDate > endDate || percent >= 100)
            {
                return 0;
            }

            var days = (endDate - startDate).Days;

            if (days <= 0)
            {
                return 0;
            }

            return count / (8 * days * days * workDaysCoef);
        }

        private decimal GetFactCount(decimal count, DateTime startDate, DateTime endDate, decimal percent)
        {
            if (reportDate < startDate || reportDate > endDate || percent >= 100)
            {
                return 0;
            }

            return count;
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}