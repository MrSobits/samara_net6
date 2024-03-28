namespace Bars.GkhGji.Report
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Properties;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;

    public class SubjectRequestsUkTsj : BasePrintForm
    {
        public IAppealCitsService<ViewAppealCitizens> AppealCitsService { get; set; }

        public IWindsorContainer Container { get; set; }

        private List<long> municipalities;

        private DateTime dateStart = DateTime.MinValue;

        private DateTime dateEnd = DateTime.MaxValue;

        public SubjectRequestsUkTsj()
            : base(new ReportTemplateBinary(Resources.SubjectRequestsUkTsj))
        {
        }

        #region свойства
        public override string Name
        {
            get
            {
                return "Отчет по тематике обращений граждан в разрезе УК, ТСЖ";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по тематике обращений граждан в разрезе УК, ТСЖ";
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
                return "B4.controller.report.SubjectRequestsUkTsj";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.SubjectRequestsUkTsj";
            }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalitiesParam = baseParams.Params["municipalityIds"].ToString();
            municipalities = !string.IsNullOrEmpty(municipalitiesParam)
                                 ? municipalitiesParam.Split(',').Select(x => x.ToLong()).ToList()
                                 : new List<long>();
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceAppealCitsRealityObject = Container.Resolve<IDomainService<AppealCitsRealityObject>>();

            var serviceManOrgContractRealityObject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var serviceAppealCitsStatSubject = Container.Resolve<IDomainService<AppealCitsStatSubject>>();

            // все дома, имеющие УО
            var realObjManOrgQuery = serviceManOrgContractRealityObject.GetAll()
                         .Where(x => municipalities.Contains(x.RealityObject.Municipality.Id)
                         && x.ManOrgContract.ManagingOrganization.Contragent != null)
                         .Select(x => new RealByManOrgProxy
                         {
                             realityId = x.RealityObject.Id,
                             manOrgId = x.ManOrgContract.ManagingOrganization.Id,
                             manOrg = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                             typeManOrg = x.ManOrgContract.TypeContractManOrgRealObj,
                             manOrgStartDate = x.ManOrgContract.StartDate ?? DateTime.MinValue,
                             manOrgEndDate = x.ManOrgContract.EndDate ?? DateTime.MinValue,
                             municipality = x.RealityObject.Municipality.Name,
                             addressManOrg = x.ManOrgContract.ManagingOrganization.Contragent.JuridicalAddress
                         });

            var manOrgIdsQuery = realObjManOrgQuery.Select(x => x.manOrgId).Distinct();

            // Словарь: managOrgId - managOrgName
            var manOrgDict = serviceManOrgContractRealityObject.GetAll()
                .Where(x => manOrgIdsQuery.Contains(x.ManOrgContract.ManagingOrganization.Id))
                .Select(x => new
                 {
                     x.ManOrgContract.ManagingOrganization.Id,
                     x.ManOrgContract.ManagingOrganization.Contragent.Name
                 })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Name).FirstOrDefault());

            var allRealityIdQuery = realObjManOrgQuery.Select(x => x.realityId);

            // Дом - УО
            var realObjManOrgDict = realObjManOrgQuery
                                    .Where(y => (y.manOrgEndDate >= dateStart || y.manOrgEndDate <= DateTime.MinValue) && y.manOrgStartDate <= dateEnd)
                                    .AsEnumerable()
                                    .GroupBy(x => x.realityId)
                                    .ToDictionary(x => x.Key, x =>
                                    {
                                        var item = x.FirstOrDefault();

                                        var tempManOrg = new RealByManOrgProxy();
                                        if (x.Count() > 1)
                                        {
                                            var temp = x.ToList();

                                            if (temp.Any(y => y.typeManOrg == TypeContractManOrg.ManagingOrgJskTsj))
                                            {
                                                tempManOrg = temp.Where(y => y.typeManOrg == TypeContractManOrg.ManagingOrgJskTsj)
                                                 .OrderByDescending(y => y.manOrgStartDate)
                                                 .First();
                                            }
                                            else
                                            {
                                                tempManOrg = temp.OrderByDescending(y => y.manOrgStartDate).FirstOrDefault();
                                            }
                                        }
                                        else if (x.Count() == 1)
                                        {
                                            tempManOrg = x.First();
                                        }

                                        if (tempManOrg != null)
                                        {
                                            item.manOrg = tempManOrg.manOrg;
                                            item.manOrgId = tempManOrg.manOrgId;
                                            item.manOrgStartDate = tempManOrg.manOrgStartDate;
                                            item.manOrgEndDate = tempManOrg.manOrgEndDate;
                                            item.addressManOrg = tempManOrg.addressManOrg;
                                        }

                                        return item;
                                    });

            var appealCitByMo = this.AppealCitsService.FilterByActiveAppealCits(serviceAppealCitsRealityObject.GetAll(), x => x.AppealCits.State)
                .Where(
                    x => allRealityIdQuery.Contains(x.RealityObject.Id)
                        && x.AppealCits.DateFrom >= dateStart
                        && x.AppealCits.DateFrom <= dateEnd)
                .Select(
                    x => new
                    {
                        realObjId = x.RealityObject.Id,
                        x.AppealCits.Id,
                        x.AppealCits.DateFrom,
                        x.AppealCits.QuestionsCount
                    })
                .AsEnumerable()
                .Where(x => realObjManOrgDict.ContainsKey(x.realObjId))
                .Select(
                    x => new AppealCitByMoProxy
                    {
                        requestId = x.Id,
                        appealDate = x.DateFrom ?? DateTime.MinValue,
                        countQuestions = x.QuestionsCount,
                        manOrgId = realObjManOrgDict[x.realObjId].manOrgId,
                        municipality = realObjManOrgDict[x.realObjId].municipality,
                        addressManOrg = realObjManOrgDict[x.realObjId].addressManOrg
                    })
                .ToList();

            // AppealId - NameManagOrg
            var appealCitByManOrgDict = appealCitByMo
                             .Select(x => new { appId = x.requestId, x.manOrgId })
                             .AsEnumerable()
                             .GroupBy(x => x.appId)
                             .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            // Список обращений
            var appealCitIdList = appealCitByMo.Select(x => x.requestId).Distinct().ToList();

            // всего обращений, кол-во вопросов
            var appealCitByMoDict = appealCitByMo.GroupBy(x => x.manOrgId)
                .ToDictionary(x => x.Key, x => new
                {
                    requestCount = x.Select(z => z.requestId).Count(),
                    countQuestions = x.Sum(z => z.countQuestions),
                    nameMunicipality = x.Select(y => y.municipality).FirstOrDefault(),
                    address = x.Select(y => y.addressManOrg).FirstOrDefault()
                });

            // Все тематики по обращениям
            var start = 999;
            var tmpStatSubjCount = appealCitIdList.Count > start ? appealCitIdList.Take(999).ToArray() : appealCitIdList.ToArray();

            var statSubjCountList = this.AppealCitsService.FilterByActiveAppealCits(serviceAppealCitsStatSubject.GetAll(), x => x.AppealCits.State) 
                    .Where(x => tmpStatSubjCount.Contains(x.AppealCits.Id))
                         .Select(x => new
                         {
                             appId = x.AppealCits.Id,
                             code = x.Subject.Code
                         })
                         .ToList();

            while (start < appealCitIdList.Count)
            {
                var tmpStatSubj = appealCitIdList.Skip(start).Take(999).ToArray();

                statSubjCountList.AddRange(this.AppealCitsService.FilterByActiveAppealCits(serviceAppealCitsStatSubject.GetAll(), x => x.AppealCits.State)
                     .Where(x => tmpStatSubj.Contains(x.AppealCits.Id))
                         .Select(x => new
                         {
                             appId = x.AppealCits.Id,
                             code = x.Subject.Code
                         })
                         .ToList());

                start += 999;
            }

            var statSubjCount = statSubjCountList
                                     .Where(x => appealCitByManOrgDict.ContainsKey(x.appId))
                                     .Select(x => new SubjectByManOrgProxy
                                     {
                                         subjectCode = x.code,
                                         manOrgId = appealCitByManOrgDict[x.appId].manOrgId
                                     })
                                     .ToList();

            // NameManagOrg - Кол-во тематик по коду
            var statSubjCountDict = statSubjCount
                                        .AsEnumerable()
                                        .GroupBy(x => x.manOrgId)
                                        .ToDictionary(x => x.Key, x => new
                                        {
                                            countType1 = x.Count(y => y.subjectCode == "1"),
                                            countType2 = x.Count(y => y.subjectCode == "2"),
                                            countType3 = x.Count(y => y.subjectCode == "3"),
                                            countType4 = x.Count(y => y.subjectCode == "4"),
                                            countType5 = x.Count(y => y.subjectCode == "5"),
                                            countType6 = x.Count(y => y.subjectCode == "6"),
                                            countType7 = x.Count(y => y.subjectCode == "7"),
                                            countType8 = x.Count(y => y.subjectCode == "8"),
                                        });

            // Слоаврь ManagOrgId - CountViolation
            var manOrgIdByViolCount = GetManOrgIdByViolation(appealCitIdList, appealCitByMo);

            reportParams.SimpleReportParams["Начало периода"] = dateStart.ToShortDateString();
            reportParams.SimpleReportParams["Конец периода"] = dateEnd.ToShortDateString();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("секция");
            var total = new long[11];
            foreach (var item in appealCitByMoDict.OrderBy(x => x.Value.nameMunicipality))
            {
                section.ДобавитьСтроку();
                if (item.Value.nameMunicipality != null)
                {
                    if (item.Value.nameMunicipality.StartsWith("г."))
                    {
                        section["Municipality"] = item.Value.nameMunicipality;
                    }
                    else
                    {
                        if (item.Value.address != null)
                        {
                            var tmpInd = item.Value.address.IndexOf(",");
                            var cityName = tmpInd > 0 ? item.Value.address.Substring(0, tmpInd) : string.Empty;
                            section["Municipality"] = item.Value.nameMunicipality + (!string.IsNullOrEmpty(cityName) ? ", " + cityName : string.Empty);
                        }
                        else
                        {
                            section["Municipality"] = item.Value.nameMunicipality;
                        }
                    }
                }

                section["nameManOrg"] = manOrgDict[item.Key];
                section["countAppeals"] = item.Value.requestCount;
                total[0] += item.Value.requestCount;

                section["countQuestions"] = item.Value.countQuestions;
                total[1] += item.Value.countQuestions;

                section["countViolations"] = manOrgIdByViolCount[item.Key];
                total[2] += manOrgIdByViolCount[item.Key];

                if (statSubjCountDict.ContainsKey(item.Key))
                {
                    section["homeCondition"] = statSubjCountDict[item.Key].countType1;
                    total[3] += statSubjCountDict[item.Key].countType1;

                    section["badServices"] = statSubjCountDict[item.Key].countType2;
                    total[4] += statSubjCountDict[item.Key].countType2;

                    section["badProperty"] = statSubjCountDict[item.Key].countType3;
                    total[5] += statSubjCountDict[item.Key].countType3;

                    section["badCr"] = statSubjCountDict[item.Key].countType4;
                    total[6] += statSubjCountDict[item.Key].countType4;

                    section["reDevelop"] = statSubjCountDict[item.Key].countType5;
                    total[7] += statSubjCountDict[item.Key].countType5;

                    section["constrDetails"] = statSubjCountDict[item.Key].countType6;
                    total[8] += statSubjCountDict[item.Key].countType6;

                    section["badManOrgWork"] = statSubjCountDict[item.Key].countType7;
                    total[9] += statSubjCountDict[item.Key].countType7;

                    section["reasonsForPayment"] = statSubjCountDict[item.Key].countType8;
                    total[10] += statSubjCountDict[item.Key].countType8;
                }
                else
                {
                    section["homeCondition"] = 0;
                    section["badServices"] = 0;
                    section["badProperty"] = 0;
                    section["badCr"] = 0;
                    section["reDevelop"] = 0;
                    section["constrDetails"] = 0;
                    section["badManOrgWork"] = 0;
                    section["reasonsForPayment"] = 0;
                }
            }

            reportParams.SimpleReportParams["countAppeals"] = total[0];
            reportParams.SimpleReportParams["countQuestions"] = total[1];
            reportParams.SimpleReportParams["countViolations"] = total[2];
            reportParams.SimpleReportParams["homeCondition"] = total[3];
            reportParams.SimpleReportParams["badServices"] = total[4];
            reportParams.SimpleReportParams["badProperty"] = total[5];
            reportParams.SimpleReportParams["badCr"] = total[6];
            reportParams.SimpleReportParams["reDevelop"] = total[7];
            reportParams.SimpleReportParams["constrDetails"] = total[8];
            reportParams.SimpleReportParams["badManOrgWork"] = total[9];
            reportParams.SimpleReportParams["reasonsForPayment"] = total[10];
        }

        #region Слоаврь ManagOrgId - CountViolation (кол-во нарушений, если у обращений есть зависимые предписания и протоколы)
        private Dictionary<long, long> GetManOrgIdByViolation(List<long> appealCitIdList, List<AppealCitByMoProxy> appealCitByMo)
        {
            // связываем обращения и инспекции
            var start = 999;
            var repInspByAppList = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var tmpInspByAppList = appealCitIdList.Count > start ? appealCitIdList.Take(999).ToArray() : appealCitIdList.ToArray();

            var inspByAppList = this.AppealCitsService.FilterByActiveAppealCits(repInspByAppList.GetAll(), x => x.AppealCits.State) 
                .Where(x => tmpInspByAppList.Contains(x.AppealCits.Id))
                         .Select(x => new
                         {
                             inspId = x.Inspection.Id,
                             appId = x.AppealCits.Id
                         })
                         .ToList();

            while (start < appealCitIdList.Count)
            {
                var tmpInspByApp = appealCitIdList.Skip(start).Take(999).ToArray();

                inspByAppList.AddRange(this.AppealCitsService.FilterByActiveAppealCits(repInspByAppList.GetAll(), x => x.AppealCits.State)
                    .Where(x => tmpInspByApp.Contains(x.AppealCits.Id))
                         .Select(x => new
                         {
                             inspId = x.Inspection.Id,
                             appId = x.AppealCits.Id
                         })
                         .ToList());

                start += 999;
            }

            var inspectionIdList = inspByAppList.Select(x => x.inspId).ToList();

            // распоряжения 
            start = 999;
            var repDisposalList = Container.Resolve<IDomainService<Disposal>>();
            var tmpDisposalList = inspectionIdList.Count > start ? inspectionIdList.Take(999).ToArray() : inspectionIdList.ToArray();

            var disposalList = repDisposalList.GetAll()
                   .Where(x => tmpDisposalList.Contains(x.Inspection.Id)
                             && x.TypeDisposal != TypeDisposalGji.DocumentGji)
                         .Select(x => x.Stage.Id)
                         .ToList();

            while (start < inspectionIdList.Count)
            {
                var tmpDisposal = inspectionIdList.Skip(start).Take(999).ToArray();

                disposalList.AddRange(repDisposalList.GetAll()
                    .Where(x => tmpDisposal.Contains(x.Inspection.Id)
                             && x.TypeDisposal != TypeDisposalGji.DocumentGji)
                         .Select(x => x.Stage.Id)
                         .ToList());

                start += 999;
            }

            // нарушения по инспекциям
            start = 999;
            var repInspViolList = Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var tmpInspViolList = disposalList.Count > start ? disposalList.Take(999).ToArray() : disposalList.ToArray();

            var inspViolList = repInspViolList.GetAll()
                   .Where(x => tmpInspViolList.Contains(x.Document.Stage.Parent.Id)
                             && (x.Document.TypeDocumentGji == TypeDocumentGji.Prescription
                                 || x.Document.TypeDocumentGji == TypeDocumentGji.Protocol))
                         .Select(x => new
                         {
                             inspId = x.InspectionViolation.Inspection.Id,
                             violId = x.InspectionViolation.Violation.Id
                         })
                                 .ToList();

            while (start < disposalList.Count)
            {
                var tmpInspViol = disposalList.Skip(start).Take(999).ToArray();

                inspViolList.AddRange(repInspViolList.GetAll()
                    .Where(x => tmpInspViol.Contains(x.Document.Stage.Parent.Id)
                             && (x.Document.TypeDocumentGji == TypeDocumentGji.Prescription
                                 || x.Document.TypeDocumentGji == TypeDocumentGji.Protocol))
                         .Select(x => new
                         {
                             inspId = x.InspectionViolation.Inspection.Id,
                             violId = x.InspectionViolation.Violation.Id
                         })
                         .ToList());

                start += 999;
            }

            // нарушения по обращениям
            var appByCountViol =
                inspViolList.Join(inspByAppList, x => x.inspId, y => y.inspId, (x, y) => new { x.violId, y.appId }).ToList();

            // словарь: УО - кол-во нарушений
            var manOrgIdByViolCount = appealCitByMo
                                       .GroupBy(x => x.manOrgId)
                                       .ToDictionary(x => x.Key, x =>
                                       {
                                           var requesrIds = x.Select(y => y.requestId).ToList();
                                           return
                                               (long)appByCountViol.Count(z => requesrIds.Contains(z.appId));
                                       });

            return manOrgIdByViolCount;
        }
        #endregion

        private class SubjectByManOrgProxy
        {
            public long manOrgId;

            public string subjectCode;
        }

        private class RealByManOrgProxy
        {
            public long realityId;

            public long manOrgId;

            public string manOrg;

            public TypeContractManOrg typeManOrg;

            public DateTime manOrgStartDate;

            public DateTime manOrgEndDate;

            public string municipality;

            public string addressManOrg;
        }

        private class AppealCitByMoProxy
        {
            public long requestId;

            public DateTime appealDate;

            public int countQuestions;

            public long manOrgId;

            public string municipality;

            public string addressManOrg;
        }
    }
}
