namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Properties;

    using Castle.Windsor;

    public class SubjectRequestsReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        public IAppealCitsService<ViewAppealCitizens> AppealCitsService { get; set; }

        private List<long> municipalities;

        private DateTime dateStart = DateTime.MinValue;

        private DateTime dateEnd = DateTime.MaxValue;

        public SubjectRequestsReport()
            : base(new ReportTemplateBinary(Resources.SubjectRequestsReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Тематика обращений";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Тематика обращений";
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
                return "B4.controller.report.SubjectRequests";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.SubjectRequests";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalitiesParam = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalities = !string.IsNullOrEmpty(municipalitiesParam)
                                 ? municipalitiesParam.Split(',').Select(x => x.ToLong()).ToList()
                                 : new List<long>();
            dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
        }

        public override string ReportGenerator { get; set; }

        // Тут предполагаем, что кол-во подтематик и характеристик нарушений не превышает 1000
        private const int SubjectMultiplier = 1000000;
        private const int SubSubjectMultiplier = 1000;

        private int PrepareVerticalSection(
            Section section,
            string subjectName, 
            int subjectNum, 
            string subsubjectName = "", 
            int subsubjectNum = 0, 
            string featureName = "",
            int featureNum = 0)
        {
            // Составной индекс = индексТематики*SubjectMultiplier + индексПодтематики*SubSubjectMultiplier + индексХарактеристики
            var index = (subjectNum * SubjectMultiplier) + (subsubjectNum * SubSubjectMultiplier) + featureNum;

            section.ДобавитьСтроку();
            section["MU_SubType"] = string.Format("$Res_{0}$", index);
            section["Insp_SubType"] = string.Format("$Res_{0}_Sum$", index);
            section["Total"] = string.Format("$Res_{0}_Total$", index);
            section["Percent"] = string.Format("$Res_{0}_Percent$", index);
            
            section["Subject"] = subjectName;
            section["SubjectNum"] = subjectNum;
            
            if (subsubjectNum > 0)
            {
                section["Subsubject"] = subsubjectName;
                section["SubsbjNum"] = string.Format("{0}.{1}", subjectNum, subsubjectNum);

                if (featureNum > 0)
                {
                    section["Feature"] = featureName;
                    section["FeatureNum"] = string.Format("{0}.{1}.{2}", subjectNum, subsubjectNum, featureNum);
                }
            }

            return index;
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            #region Определяем поля отчета
            //--------Словарь: характеристики обращений по подтематикам--------
            var featuresBysubSubject = Container.Resolve<IDomainService<StatSubsubjectFeatureGji>>().GetAll()
                .Select(x => new
                {
                    subsubjectId = x.Subsubject.Id,
                    featureId = x.FeatureViol.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.subsubjectId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.featureId).ToList());

            //--------Словарь: характеристики обращений по подтематикам и по тематикам--------
            var subsubjectsBySubject = Container.Resolve<IDomainService<StatSubjectSubsubjectGji>>().GetAll()
                .Select(x => new
                {
                    subjectId = x.Subject.Id,
                    subsubjectId = x.Subsubject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.subjectId)
                .ToDictionary(
                    x => x.Key, 
                    x => x.ToDictionary(
                        y => y.subsubjectId,
                        y => featuresBysubSubject.ContainsKey(y.subsubjectId) ? featuresBysubSubject[y.subsubjectId] : null));

            var subjects = Container.Resolve<IDomainService<StatSubjectGji>>().GetAll()
                .OrderBy(x => x.Code)
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var subsubjects = Container.Resolve<IDomainService<StatSubsubjectGji>>().GetAll()
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var features = Container.Resolve<IDomainService<FeatureViolGji>>().GetAll()
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var subjectsTree = subjects.Keys.ToDictionary(x => x, x => subsubjectsBySubject.ContainsKey(x) ? subsubjectsBySubject[x] : null);

            /*-------Вывод столбцов---------*/
            var sectionVertical = reportParams.ComplexReportParams.ДобавитьСекцию("sectionVertical");
            var fieldList = new List<long>();

            var subjectNum = 0;
            var subjectTotal = new Dictionary<long, long>();

            var subjectIndex = new Dictionary<long, long>();
            var subsubjectIndex = new Dictionary<long, long>();
            var featureIndex = new Dictionary<long, long>();

            foreach (var subject in subjectsTree)
            {
                subjectIndex[subject.Key] = ++subjectNum;

                var subsubjectNum = 0;
                subjectTotal[subject.Key] = 0;
                var totalDefined = false;

                Action<long> defineTotal = key =>
                    {
                        if (totalDefined == false)
                        {
                            sectionVertical["SubjectTotal"] = string.Format("$SubjectTotal_{0}$", key);
                            sectionVertical["SubPercent"] = string.Format("$SubPercent_{0}$", key);
                            totalDefined = true;
                        }
                    };
                
                if (subject.Value != null)
                {
                    foreach (var subsubject in subject.Value)
                    {
                        subsubjectIndex[subsubject.Key] = ++subsubjectNum;

                        var featureNum = 0;
                        if (subsubject.Value != null)
                        {
                            foreach (var feature in subsubject.Value)
                            {
                                featureIndex[feature] = ++featureNum;

                                var index = PrepareVerticalSection(
                                    sectionVertical,
                                    subjects[subject.Key],
                                    subjectNum,
                                    subsubjects[subsubject.Key],
                                    subsubjectNum,    
                                    features[feature],
                                    featureNum);

                                fieldList.Add(index);
                                
                                defineTotal(subject.Key);
                            }
                        }
                        else
                        {
                            var index = PrepareVerticalSection(
                                    sectionVertical,
                                    subjects[subject.Key], 
                                    subjectNum,
                                    subsubjects[subsubject.Key],
                                    subsubjectNum);

                            fieldList.Add(index);
                            
                            defineTotal(subject.Key);
                        }
                    }
                }
                else
                {
                    var index = PrepareVerticalSection(sectionVertical, subjects[subject.Key], subjectNum);

                    fieldList.Add(index);
                    
                    defineTotal(subject.Key);
                }
            }

            fieldList = fieldList.Distinct().ToList();
            #endregion

            /*-------Все зональные инспекции по заданным МО-----------*/
            var zonalInspectionByMunicipality = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.Municipality.Id))
                .Select(x => new
                {
                    x.ZonalInspection.Id,
                    zoneName = x.ZonalInspection.ZoneName ?? string.Empty,
                    MunicipalityId = x.Municipality.Id,
                    Municipality = x.Municipality.Name
                })
                .ToList()
                .GroupBy(x => x.zoneName)
                .ToDictionary(x => x.Key, y => y.ToList());

            var appealsQuery = this.AppealCitsService.FilterByActiveAppealCits(this.Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll(), x => x.AppealCits.State)
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.AppealCits.DateFrom >= dateStart)
                .Where(x => x.AppealCits.DateFrom <= dateEnd);

            var appealCits = appealsQuery
                .Select(x => new
                {
                    MunicipalityId = x.RealityObject.Municipality.Id,
                    RequestId = x.AppealCits.Id,
                    CountQuestions = x.AppealCits.QuestionsCount
                })
                .AsEnumerable()
                .GroupBy(x => x.RequestId)
                .Select(x => new
                {
                    RequestId = x.Key,
                    MunicipalityId = x.Select(y => y.MunicipalityId).FirstOrDefault(),
                    CountQuestions = x.Select(y => y.CountQuestions).FirstOrDefault()
                })
                .ToList();
            
            //*--------Сумма значений из поля "Кол-во вопросов" и Кол-во обращений по МО-------
            var appealCitByMoAggregate = appealCits
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(
                    x => x.Key,
                    x => new { requestCount = x.Count(), CountQuestions = x.Sum(z => z.CountQuestions) });

            var municipalityByAppealCits = appealCits
                .GroupBy(x => x.RequestId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.MunicipalityId).FirstOrDefault());

            var appealIdsQuery = appealsQuery.Select(x => x.AppealCits.Id);
            
            var appealCitStats = Container.Resolve<IDomainService<AppealCitsStatSubject>>().GetAll()
                .Where(x => appealIdsQuery.Contains(x.AppealCits.Id))
                .Select(x => new
                {
                    AppealCitsId = x.AppealCits.Id,
                    subjectId = (long?)x.Subject.Id,
                    subsubjectId = (long?)x.Subsubject.Id,
                    featureId = (long?)x.Feature.Id
                })
                .ToList();

            var appealCitStatsByMo = new Dictionary<long, Dictionary<long, long>>();

            foreach (var appealCitStat in appealCitStats)
            {
                var municipalityId = municipalityByAppealCits[appealCitStat.AppealCitsId];

                if (!appealCitStatsByMo.ContainsKey(municipalityId))
                {
                    appealCitStatsByMo[municipalityId] = new Dictionary<long, long>();
                }

                // Составной индекс = индексТематики*SubjectMultiplier + индексПодтематики*SubSubjectMultiplier + индексХарактеристики
                var index = (appealCitStat.subjectId.HasValue && subjectIndex.ContainsKey(appealCitStat.subjectId.Value) ? subjectIndex[appealCitStat.subjectId.Value] * SubjectMultiplier : 0)
                            + (appealCitStat.subsubjectId.HasValue && subsubjectIndex.ContainsKey(appealCitStat.subsubjectId.Value) ? subsubjectIndex[appealCitStat.subsubjectId.Value] * SubSubjectMultiplier : 0)
                            + (appealCitStat.featureId.HasValue && featureIndex.ContainsKey(appealCitStat.featureId.Value) ? featureIndex[appealCitStat.featureId.Value] : 0);

                if (appealCitStatsByMo[municipalityId].ContainsKey(index))
                {
                    appealCitStatsByMo[municipalityId][index]++;
                }
                else
                {
                    appealCitStatsByMo[municipalityId][index] = 1;
                }

                if(appealCitStat.subjectId.HasValue)
                {
                    subjectTotal[appealCitStat.subjectId.Value]++;
                }
            }

            /*-------Вывод строк---------*/
            var sectionInspection = reportParams.ComplexReportParams.ДобавитьСекцию("sectionInspection");
            var sectionMUnion = sectionInspection.ДобавитьСекцию("sectionMUnion");
            var inspectionTotalRequests = 0;
            var inspectionTotalQuestions = 0;
            var reportTotal = fieldList.ToDictionary(x => x, x => (long)0);

            foreach (var zonalUnoin in zonalInspectionByMunicipality.OrderBy(x => x.Key))
            {
                var inspectionCountRequests = 0;
                var inspectionCountQuestions = 0;
                sectionInspection.ДобавитьСтроку();
                sectionInspection["InspectionName"] = zonalUnoin.Key;

                var inspectionSum = fieldList.ToDictionary(x => x, x => (long)0);

                foreach (var municipality in zonalUnoin.Value)
                {
                    var municipalityId = municipality.MunicipalityId;

                    sectionMUnion.ДобавитьСтроку();
                    sectionMUnion["MUName"] = municipality.Municipality;

                    if (appealCitByMoAggregate.ContainsKey(municipalityId))
                    {
                        var appealCit = appealCitByMoAggregate[municipalityId];
                        sectionMUnion["MURequests"] = appealCit.requestCount;
                        inspectionCountRequests += appealCit.requestCount;
                        inspectionCountQuestions += appealCit.CountQuestions;
                    }
                    else
                    {
                        sectionMUnion["MURequests"] = 0;
                    }

                    foreach (var field in fieldList)
                    {
                        long value = 0;

                        if (appealCitStatsByMo.ContainsKey(municipalityId) && appealCitStatsByMo[municipalityId].ContainsKey(field))
                        {
                            value = appealCitStatsByMo[municipalityId][field];
                        }

                        sectionMUnion["Res_" + field] = value;
                        inspectionSum[field] += value;
                    }
                }

                foreach (var field in fieldList)
                {
                    sectionInspection[string.Format("Res_{0}_Sum", field)] = inspectionSum[field];
                    reportTotal[field] += inspectionSum[field];
                }

                sectionInspection["InspectionRequests"] = inspectionCountRequests;
                inspectionTotalRequests += inspectionCountRequests;
                inspectionTotalQuestions += inspectionCountQuestions;
            }

            foreach (var field in fieldList)
            {
                reportParams.SimpleReportParams[string.Format("Res_{0}_Total", field)] = reportTotal[field];
                reportParams.SimpleReportParams[string.Format("Res_{0}_Percent", field)] = inspectionTotalQuestions != 0 ? (reportTotal[field] * 100.00) / inspectionTotalQuestions : 0;
            }

            long totalSub = 0;
            foreach (var total in subjectTotal)
            {
                reportParams.SimpleReportParams[string.Format("SubjectTotal_{0}", total.Key)] = total.Value;
                reportParams.SimpleReportParams[string.Format("SubPercent_{0}", total.Key)] = inspectionTotalQuestions != 0 ? total.Value * 100.00 / inspectionTotalQuestions : 0;
                totalSub += total.Value;
            }

            reportParams.SimpleReportParams["Total"] = totalSub;
            reportParams.SimpleReportParams["dateStart"] = dateStart.ToString("d MMMM yyyy г.");
            reportParams.SimpleReportParams["dateFinish"] = dateEnd.ToString("d MMMM yyyy г.");
            reportParams.SimpleReportParams["InspectionTotalRequests"] = inspectionTotalRequests;
        }
    }
}
