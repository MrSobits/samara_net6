namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class StaffingWorkers : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programId = 0;
        private DateTime reportDate = DateTime.MinValue;
        private long[] finSourceIds;
        private long[] municipalityIds;

        private decimal workDaysCoef = decimal.Divide(6, 7);

        public StaffingWorkers() : base(new ReportTemplateBinary(Properties.Resources.StaffingWorkers))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.StaffingWorkers";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "05_Нормативная численность рабочих"; }
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
            get { return "B4.controller.report.StaffingWorkers"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "05_Нормативная численность рабочих"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programId = baseParams.Params["programCrId"].ToInt();

            this.reportDate = baseParams.Params["reportDate"].ToDateTime();

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
              ? baseParams.Params["municipalityIds"].ToString()
              : string.Empty;

            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var finSourceIdsList = baseParams.Params.ContainsKey("finSources")
             ? baseParams.Params["finSources"].ToString()
             : string.Empty;

            this.finSourceIds = !string.IsNullOrEmpty(finSourceIdsList) ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

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

            var works = this.Container.Resolve<IDomainService<Work>>()
                                     .GetAll()
                                     .Where(x => x.Consistent185Fz && x.TypeWork == TypeWork.Work)  
                                     .ToArray();

            var vertColumn = reportParams.ComplexReportParams.ДобавитьСекцию("ВертСекция");

            int column = 4;
            foreach (var work in works)
            {
                vertColumn.ДобавитьСтроку();
                vertColumn["ВидРаботы"] = work.Name;
                vertColumn["Расчет"] = string.Format("$Расчет{0}$", work.Id);
                vertColumn["СуммаРасчет"] = string.Format("$СуммаРасчет{0}$", work.Id);
                vertColumn["Колонка"] = column;
                column++;
            }

            reportParams.SimpleReportParams["ИтогоКолонка"] = column;

            var typeWorks = this.reportDate.Date.Year >= 2013 ?
                this.Container.Resolve<IDomainService<ArchiveSmr>>()
                                    .GetAll()
                                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programId)
                                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                                    .WhereIf(this.finSourceIds.Length > 0, x => this.finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                                    .Where(x => x.TypeWorkCr.Work.TypeWork == TypeWork.Work && x.TypeWorkCr.Work.Consistent185Fz)
                                    .Where(x => x.ObjectCreateDate < this.reportDate.AddDays(1))
                                    .Select(x => new
                                    {
                                        x.TypeWorkCr.Work.Id,
                                        DateStartWork = x.TypeWorkCr.DateStartWork.ToDateTime(),
                                        DateEndWork = x.TypeWorkCr.DateEndWork.ToDateTime(),
                                        TypeWorkCrId = x.TypeWorkCr.Id,
                                        x.ObjectCreateDate,
                                        PercentOfCompletion = x.PercentOfCompletion.ToDecimal(),
                                        Normative = x.TypeWorkCr.Work.Normative.ToDecimal(),
                                        Volume = x.TypeWorkCr.Volume.ToDecimal(),
                                        x.TypeWorkCr.ObjectCr.RealityObject.Municipality
                                    })
                                     .AsEnumerable()
                                     .GroupBy(x => x.TypeWorkCrId)
                                     .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.ObjectCreateDate).First())
                                     .Select(x => x.Value)
                                     .Select(x => new
                                     {
                                         NormativeCount = this.GetNormativeCount(x.Normative * x.Volume, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                         x.Municipality,
                                         x.Id
                                     })
                                     .OrderBy(x => x.Municipality.Name)
                                     .GroupBy(x => x.Municipality)
                                     .ToDictionary(
                                         x => x.Key,
                                         y => new
                                         {
                                             Dict = y.GroupBy(x => x.Id).ToDictionary(
                                                 x => x.Key,
                                                 z => z.Sum(x => x.NormativeCount))
                                         })

               : this.Container.Resolve<IDomainService<TypeWorkCr>>()
                                    .GetAll()
                                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programId)
                                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                    .WhereIf(this.finSourceIds.Length > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                    .Where(x => x.Work.TypeWork == TypeWork.Work && x.Work.Consistent185Fz)
                                    .Select(x => new
                                    {
                                        x.Work.Id,
                                        DateStartWork = x.DateStartWork.ToDateTime(),
                                        DateEndWork = x.DateEndWork.ToDateTime(),
                                        TypeWorkCrId = x.Id,
                                        x.ObjectCreateDate,
                                        PercentOfCompletion = x.PercentOfCompletion.ToDecimal(),
                                        Normative = x.Work.Normative.ToDecimal(),
                                        Volume = x.Volume.ToDecimal(),
                                        x.ObjectCr.RealityObject.Municipality
                                    })
                                     .AsEnumerable()
                                     .Select(x => new
                                     {
                                         NormativeCount = this.GetNormativeCount(x.Normative * x.Volume, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                         x.Municipality,
                                         x.Id
                                     })
                                     .OrderBy(x => x.Municipality.Name)
                                     .GroupBy(x => x.Municipality)
                                     .ToDictionary(
                                         x => x.Key,
                                         y => new
                                         {
                                             Dict = y.GroupBy(x => x.Id)
                                                .ToDictionary(
                                                     x => x.Key,
                                                     z => z.Sum(x => x.NormativeCount))
                                         });

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("ГоризСекция");

            var number = 0;
            foreach (var typeWork in typeWorks)
            {
                number++;
                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["Номер"] = number;
                sectionMunicipality["МунОбр"] = typeWork.Key.Name;

                if (objectsCrCount.ContainsKey(typeWork.Key.Id))
                {
                    sectionMunicipality["КоличествоДомов"] = objectsCrCount[typeWork.Key.Id];
                }
                
                var typeWorkCr = typeWork.Value.Dict;

                foreach (var work in works)
                {
                    if (typeWorks[typeWork.Key].Dict.ContainsKey(work.Id))
                    {
                        sectionMunicipality["Расчет" + work.Id] = typeWorkCr[work.Id];
                    }
                    else
                    {
                        sectionMunicipality["Расчет" + work.Id] = 0;
                    }
                }

                sectionMunicipality["Сумма"] = typeWorkCr.Values.Sum();
            }

            reportParams.SimpleReportParams["СуммаКолДомов"] = objectsCrCount.Values.Sum();

            var typeWorksCr = typeWorks.Values.Select(x => x.Dict);
            foreach (var work in works)
            {
                reportParams.SimpleReportParams["СуммаРасчет" + work.Id] = typeWorksCr.Sum(x => x.Where(y => y.Key == work.Id).Sum(z => z.Value));
            }

            reportParams.SimpleReportParams["ДатаОтчета"] = this.reportDate.ToShortDateString();
            reportParams.SimpleReportParams["СуммаСумм"] = typeWorks.Values.Select(x => x.Dict).Sum(x => x.Values.Sum());
        }

        private decimal GetNormativeCount(decimal count, DateTime startDate, DateTime endDate, decimal percent)
        {
            if (this.reportDate < startDate || this.reportDate > endDate || percent >= 100)
            {
                return 0;
            }

            var days = (endDate - startDate).Days;

            if (days <= 0)
            {
                return 0;
            }

            return count / (8 * days * days * this.workDaysCoef);
        }
    }
}