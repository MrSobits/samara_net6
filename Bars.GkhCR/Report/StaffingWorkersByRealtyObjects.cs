namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class StaffingWorkersByRealtyObjects : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programId = 0;
        private DateTime reportDate = DateTime.MinValue;
        private long[] finSourceIds;
        private long[] municipalityIds;

        private decimal workDaysCoef = decimal.Divide(6, 7);

        public StaffingWorkersByRealtyObjects()
            : base(new ReportTemplateBinary(Properties.Resources.StaffingWorkersByRealtyObjects))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.StaffingWorkersByRealtyObjects";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "Нормативная численность рабочих (по домам)"; }
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
            get { return "B4.controller.report.StaffingWorkersByRealtyObjects"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Нормативная численность рабочих (по домам)"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programId = baseParams.Params["programCrId"].ToInt();

            reportDate = baseParams.Params["reportDate"].ToDateTime();

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
              ? baseParams.Params["municipalityIds"].ToString()
              : string.Empty;

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var finSourceIdsList = baseParams.Params.ContainsKey("finSources")
             ? baseParams.Params["finSources"].ToString()
             : string.Empty;

            finSourceIds = !string.IsNullOrEmpty(finSourceIdsList) ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                     .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                     .Select(x => new { x.Id, x.Name })
                     .OrderBy(x => x.Name)
                     .ToDictionary(x => x.Id, x => x.Name);

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
            
            var codeList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "29" };

            var typeWorks = this.reportDate.Date.Year >= 2013 ?
                    this.Container.Resolve<IDomainService<ArchiveSmr>>()
                                    .GetAll()
                                    .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programId)
                                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                                    .WhereIf(this.finSourceIds.Length > 0, x => this.finSourceIds.Contains(x.TypeWorkCr.FinanceSource.Id))
                                    .Where(x => codeList.Contains(x.TypeWorkCr.Work.Code))
                                    .Where(x => x.ObjectCreateDate < this.reportDate.AddDays(1))
                                    .Select(x => new
                                    {
                                        TypeWorkCrId = x.TypeWorkCr.Id,
                                        x.ObjectCreateDate,
                                        x.TypeWorkCr.Work.Code,
                                        DateStartWork = x.TypeWorkCr.DateStartWork.ToDateTime(),
                                        DateEndWork = x.TypeWorkCr.DateEndWork.ToDateTime(),
                                        PercentOfCompletion = x.PercentOfCompletion.ToDecimal(),
                                        Normative = x.TypeWorkCr.Work.Normative.ToDecimal(),
                                        Volume = x.TypeWorkCr.Volume.ToDecimal(),
                                        MunicipalityId = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id,
                                        RealObjId = x.TypeWorkCr.ObjectCr.RealityObject.Id,
                                        x.TypeWorkCr.ObjectCr.RealityObject.Address
                                    })
                                     .AsEnumerable()
                                     .GroupBy(x => x.TypeWorkCrId)
                                     .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.ObjectCreateDate).First())
                                     .Select(x => x.Value)
                                     .Select(x => new
                                     {
                                         NormativeCount = this.GetNormativeCount(x.Normative * x.Volume, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                         x.MunicipalityId,
                                         x.Code,
                                         x.RealObjId,
                                         x.Address
                                     })
                                     .OrderBy(x => x.Address)
                                     .GroupBy(x => x.MunicipalityId)
                                     .ToDictionary(
                                        x => x.Key,
                                        x => x.GroupBy(y => y.RealObjId)
                                              .ToDictionary(
                                                y => y.Key,
                                                y => new
                                                {
                                                    Address = y.Select(z => z.Address).FirstOrDefault(),
                                                    Dict = y.GroupBy(z => z.Code)
                                                            .ToDictionary(
                                                                z => z.Key,
                                                                z => z.Sum(k => k.NormativeCount))
                                                }))

                    : this.Container.Resolve<IDomainService<TypeWorkCr>>()
                                    .GetAll()
                                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programId)
                                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                                    .WhereIf(this.finSourceIds.Length > 0, x => this.finSourceIds.Contains(x.FinanceSource.Id))
                                    .Where(x => codeList.Contains(x.Work.Code))
                                    .Select(x => new
                                    {
                                        x.Work.Code,
                                        DateStartWork = x.DateStartWork.ToDateTime(),
                                        DateEndWork = x.DateEndWork.ToDateTime(),
                                        PercentOfCompletion = x.PercentOfCompletion.ToDecimal(),
                                        Normative = x.Work.Normative.ToDecimal(),
                                        Volume = x.Volume.ToDecimal(),
                                        MunicipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                                        RealObjId = x.ObjectCr.RealityObject.Id,
                                        x.ObjectCr.RealityObject.Address
                                    })
                                     .AsEnumerable()
                                     .Select(x => new
                                     {
                                         NormativeCount = this.GetNormativeCount(x.Normative * x.Volume, x.DateStartWork, x.DateEndWork, x.PercentOfCompletion),
                                         x.MunicipalityId,
                                         x.Code,
                                         x.RealObjId,
                                         x.Address
                                     })
                                     .OrderBy(x => x.Address)
                                     .GroupBy(x => x.MunicipalityId)
                                     .ToDictionary(
                                        x => x.Key,
                                        x => x.GroupBy(y => y.RealObjId)
                                              .ToDictionary(
                                                y => y.Key,
                                                y => new
                                                {
                                                    Address = y.Select(z => z.Address).FirstOrDefault(),
                                                    Dict = y.GroupBy(z => z.Code)
                                                            .ToDictionary(
                                                                z => z.Key,
                                                                z => z.Sum(k => k.NormativeCount))
                                                }));

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");

            reportParams.SimpleReportParams["ReportDate"] = this.reportDate.ToShortDateString();
            var number = 0;

            foreach (var municipality in municipalityDict)
            {
                if (!typeWorks.ContainsKey(municipality.Key))
                {
                    continue;
                }

                sectionMu.ДобавитьСтроку();
                sectionMu["Municipality"] = municipality.Value;

                foreach (var realtyObject in typeWorks[municipality.Key])
                {
                    number++;
                    sectionRo.ДобавитьСтроку();
                    sectionRo["Number"] = number;
                    sectionRo["Address"] = realtyObject.Value.Address;

                    var paramDict = realtyObject.Value.Dict;

                    foreach (var code in codeList)
                    {
                        if (!paramDict.ContainsKey(code))
                        {
                            continue;
                        }

                        sectionRo[string.Format("Param{0}", code)] = paramDict[code];
                    }

                    sectionRo["RoTotal"] = realtyObject.Value.Dict.Values.Sum(x => x);
                }

                var valuesByMu = typeWorks[municipality.Key].Values.SelectMany(x => x.Dict).ToList();

                if (objectsCrCount.ContainsKey(municipality.Key))
                {
                    sectionMu["RoCount"] = objectsCrCount[municipality.Key];
                }

                sectionMu["MuTotal"] = valuesByMu.Sum(x => x.Value);

                foreach (var code in codeList)
                {
                    sectionMu[string.Format("MuTotalParam{0}", code)] = valuesByMu.Where(x => x.Key == code).Sum(x => x.Value);
                }
            }

            var totalValues = typeWorks.Values.SelectMany(x => x.Values.SelectMany(y => y.Dict)).ToList();

            reportParams.SimpleReportParams["TotalRoCount"] = objectsCrCount.Values.Sum();

            reportParams.SimpleReportParams["Total"] = totalValues.Sum(x => x.Value);

            foreach (var code in codeList)
            {
                reportParams.SimpleReportParams[string.Format("TotalParam{0}", code)] = totalValues.Where(x => x.Key == code).Sum(x => x.Value);
            }
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