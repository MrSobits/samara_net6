namespace Bars.GkhGji.Regions.Samara.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ControlAppealsExecution : BasePrintForm
    {
        #region Свойства
        public IWindsorContainer Container { get; set; }

        private DateTime DateStart = DateTime.MinValue;

        private DateTime DateEnd = DateTime.MaxValue;

        // идентификаторы муниципальных образований
        private List<long> municipalityIds = new List<long>();

        // идентификаторы инспекторов
        private List<long> inspectorIds = new List<long>();

        public ControlAppealsExecution()
            : base(new ReportTemplateBinary(Properties.Resources.ControlAppealsExecution))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ControlAppealsExecution";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Контроль исполнения обращений";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ControlAppealsExecution";
            }
        }

        public override string Name
        {
            get
            {
                return "Контроль исполнения обращений";
            }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            this.DateStart = baseParams.Params["dateStart"].ToDateTime();
            this.DateEnd = baseParams.Params["dateEnd"].ToDateTime();

            var municipalityIdsStr = baseParams.Params.GetAs("municipalityIds", string.Empty);
            var inspectorIdsStr = baseParams.Params.GetAs("inspectorIds", string.Empty);

            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsStr)
                                       ? municipalityIdsStr.Split(',').Select(id => id.ToLong()).ToList()
                                       : new List<long>();
            this.inspectorIds = !string.IsNullOrEmpty(inspectorIdsStr)
                                    ? inspectorIdsStr.Split(',').Select(id => id.ToLong()).ToList()
                                    : new List<long>();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceZonalInspectionMunicipality = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();

            reportParams.SimpleReportParams["НачалоПериода"] = this.DateStart.ToShortDateString();
            reportParams.SimpleReportParams["ОкончаниеПериода"] = this.DateEnd.ToShortDateString();

            var municipalityByZonalInspectionDict = serviceZonalInspectionMunicipality.GetAll()
                .Where(x => this.municipalityIds.Contains(x.Municipality.Id))
                .Select(x => new
                {
                    ZonalInspectionName = x.ZonalInspection.ZoneName ?? x.ZonalInspection.Name,
                    x.Municipality.Id,
                    x.Municipality.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.ZonalInspectionName)
                .ToDictionary(x => x.Key, x => x.ToList());

            var appealsByMunicipalityDict = this.GetCitizenAppeals();
            var sectionZonalInspection = reportParams.ComplexReportParams.ДобавитьСекцию("sectionZonalInspection");
            var sectionDocument = sectionZonalInspection.ДобавитьСекцию("sectionDocument");

            var municipalitiesWithDocuments = appealsByMunicipalityDict.Keys
                .Distinct()
                .ToList();

            foreach (var zonalInspection in municipalityByZonalInspectionDict.OrderBy(x => x.Key))
            {
                var zonalInspectionHasAnyDocument = zonalInspection.Value.Select(x => x.Id).Any(municipalitiesWithDocuments.Contains);

                if (!zonalInspectionHasAnyDocument)
                {
                    continue;
                }

                sectionZonalInspection.ДобавитьСтроку();
                sectionZonalInspection["ЗЖИ"] = zonalInspection.Key;

                Action<Dictionary<long, List<ReportRow>>> fillZonalInspectionDocument =
                    x => zonalInspection.Value.ForEach(municipality => this.FillReport(sectionDocument, municipality.Id, municipality.Name, x));

                fillZonalInspectionDocument(appealsByMunicipalityDict);
            }
        }

        // Обращения
        private Dictionary<long, List<ReportRow>> GetCitizenAppeals()
        {
            var serviceAppealCitsRealityObject = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var serviceAppealCitsAnswer = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();

            var hasAnswerWithNumberQuery = serviceAppealCitsAnswer.GetAll()
                .Where(x => x.DocumentNumber != string.Empty);

            var appeals = serviceAppealCitsRealityObject.GetAll()
                .WhereIf(this.DateStart != DateTime.MinValue, x => x.AppealCits.DateFrom >= this.DateStart)
                .WhereIf(this.DateEnd != DateTime.MinValue, x => x.AppealCits.CheckTime <= this.DateEnd)
                .Where(x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    appealCitId = x.AppealCits.Id,
                    MunicipalityId = x.RealityObject.Municipality.Id,
                    x.RealityObject.Address,
                    numberGJI = x.AppealCits.NumberGji,
                    startDate = x.AppealCits.DateFrom,
                    endDate = x.AppealCits.CheckTime,
                    Text = x.AppealCits.Description,
                    Inspector = x.AppealCits.Tester.Fio ?? x.AppealCits.Executant.Fio,
                    InspectorId = ((long?)x.AppealCits.Tester.Id) ?? ((long?)x.AppealCits.Executant.Id) ?? -1,
                    SuretyResolve = x.AppealCits.SuretyResolve.Code,
                    Resolve = x.AppealCits.SuretyResolve.Name,
                    hasAnswerWithNumber = hasAnswerWithNumberQuery.Any(y => y.AppealCits.Id == x.AppealCits.Id)
                })
                .AsEnumerable()
                .ToList();

            if (this.inspectorIds.Any())
            {
                appeals = appeals
                    .Where(x => this.inspectorIds.Contains(x.InspectorId))
                    .ToList();
            }

            var res = appeals
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Distinct().Select(y =>
                    {
                        var row = new ReportRow
                        {
                            DocumentName = "Обращение",
                            DocumentNumber = y.numberGJI,
                            DateStart = y.startDate,
                            DateEnd = y.endDate,
                            Address = y.Address,
                            Content = y.Text,
                            Resolution = y.Resolve,
                            Executant = y.Inspector,
                            Answer = y.hasAnswerWithNumber ? "Нет" : "Да"
                        };

                        if (y.startDate.HasValue && y.endDate.HasValue)
                        {
                            var time = y.endDate.ToDateTime() - y.startDate.ToDateTime();
                            row.ExecutionPeriod = time.Days;
                        }

                        return row;
                    })
                    .ToList());

            return res;
        }

        private void FillReport(Section section, long municipalityId, string municipalityName, Dictionary<long, List<ReportRow>> dataByMunicipalityIdDict)
        {
            if (!dataByMunicipalityIdDict.ContainsKey(municipalityId))
            {
                return;
            }

            foreach (var data in dataByMunicipalityIdDict[municipalityId].OrderBy(x => x.DocumentNumber))
            {
                section.ДобавитьСтроку();
                section["Документ"] = data.DocumentName;
                section["Номер"] = data.DocumentNumber;
                section["ДатаНачала"] = data.DateStart;
                section["ДатаОкончания"] = data.DateEnd;
                section["СрокИсполнения"] = data.ExecutionPeriod;
                section["НаКонтроле"] = data.Answer;
                section["НаименованиеМО"] = municipalityName;
                section["Адрес"] = data.Address;
                section["Содержание"] = data.Content;
                section["Резолюция"] = data.Resolution;
                section["Инспектор"] = data.Executant;
            }
        }
    }

    internal sealed class ReportRow
    {
        public string DocumentName { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? ExecutionPeriod { get; set; }
        public string Address { get; set; }
        public string Content { get; set; }
        public string Resolution { get; set; }
        public string Executant { get; set; }
        public string Answer { get; set; }
    }

    internal sealed class InspectionRealtyObjectProxy
    {
        public string Address { get; set; }
        public long InspectionId { get; set; }
        public long MunicipalityId { get; set; }
    }

    internal struct InspectorProxy
    {
        public int Id;
        public string Fio;
    }
}