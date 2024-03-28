namespace Bars.GkhGji.Regions.Yanao.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет Форма 1 - госжилинспекция
    /// </summary>
    /// 
    public class Form1StateHousingInspection : BasePrintForm
    {
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private DateTime reportDate = DateTime.Now;

        private long[] municipalityIds;
        private long programCrId;

        public Form1StateHousingInspection()
            : base(new ReportTemplateBinary(Properties.Resources.Form1StateHousingInspection))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Отчет \"Форма 1 - госжилинспекция\""; }
        }

        public override string GroupName
        {
            get { return "Инспекторская деятельность"; }
        }

        public override string Desciption
        {
            get { return "Отчет \"Форма 1 - госжилинспекция\""; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Form1StateHousingInspection"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.Form1StateHousingInspection"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            reportDate = baseParams.Params["reportDate"].ToDateTime();

            programCrId = baseParams.Params.GetAs<long>("programCrId");

            var strMunIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunIds) ? strMunIds.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["период"] = string.Format("{0} - {1}", dateStart.ToString("MMMM"), dateEnd.ToString("MMMM"));
            reportParams.SimpleReportParams["год"] = dateEnd.Year;

            foreach (var rec in GetData())
            {
                var number = rec.Key;
                var value = rec.Value;
                if (number == 1 || number == 2 || number == 24 || (number > 11 && number < 18))
                {
                    value = decimal.Divide(value, 1000);
                }

                reportParams.SimpleReportParams[string.Format("{0}", number)] = value;
            }
        }

        private Dictionary<int, decimal> GetData()
        {
            var result = new Dictionary<int, decimal>();

            var servActCheck = this.Container.Resolve<IDomainService<ActCheck>>();
            var serviceActCheckRealityObject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var servInspectionGjiRealityObject = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var servPrescriptionViol = this.Container.Resolve<IDomainService<PrescriptionViol>>();
            var servProtocolViolation = this.Container.Resolve<IDomainService<ProtocolViolation>>();
            var servViolationFeatureGji = this.Container.Resolve<IDomainService<ViolationFeatureGji>>();
            var servPrescription = this.Container.Resolve<IDomainService<Prescription>>();
            var servProtocol = this.Container.Resolve<IDomainService<Protocol>>();
            var servResolution = this.Container.Resolve<IDomainService<Resolution>>();
            var servObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var servInspector = this.Container.Resolve<IDomainService<Inspector>>();

            try
            {
                var area = serviceActCheckRealityObject.GetAll()
                          .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.ActCheck.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                          .Where(y => this.Container.Resolve<IDomainService<Disposal>>().GetAll().Any(x => x.TypeDisposal == TypeDisposalGji.Base && x.Inspection.Id == y.ActCheck.Inspection.Id && x.KindCheck != null))
                          .Where(x => x.ActCheck.DocumentDate != null && x.ActCheck.DocumentDate >= this.dateStart && x.ActCheck.DocumentDate <= this.dateEnd)
                          .Select(x => new { x.ActCheck.Id, x.ActCheck.Area })
                          .AsEnumerable()
                          .Distinct(x => x.Id)
                          .Sum(x => x.Area);
                result.Add(1, area.Value);

                var areaBaseInsCheck = serviceActCheckRealityObject.GetAll()
                        .WhereIf(dateStart != DateTime.MinValue, x => x.ActCheck.DocumentDate >= dateStart)
                        .WhereIf(dateEnd != DateTime.MinValue, x => x.ActCheck.DocumentDate <= dateEnd)
                        .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.ActCheck.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                        .Where(x => x.ActCheck.Inspection.TypeBase == TypeBase.Inspection)
                        .Where(x => x.ActCheck.TypeDocumentGji == TypeDocumentGji.ActCheck)
                        .Where(y => this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll().Any(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal && x.Children.Id == y.ActCheck.Id))
                         .Select(x => new { x.ActCheck.Id, x.ActCheck.Area })
                         .ToList();

                var areaPlaned1And3 = serviceActCheckRealityObject.GetAll()
                      .WhereIf(dateStart != DateTime.MinValue, x => x.ActCheck.DocumentDate >= dateStart)
                      .WhereIf(dateEnd != DateTime.MinValue, x => x.ActCheck.DocumentDate <= dateEnd)
                      .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.ActCheck.Inspection.Id && municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                      .Where(x => x.ActCheck.TypeDocumentGji == TypeDocumentGji.ActCheck)
                      .Where(y => Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                                             .Any(
                                                 x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal
                                                 && x.Children.Id == y.ActCheck.Id
                                                 && Container.Resolve<IDomainService<Disposal>>().GetAll()
                                                 .Any(z =>
                                                     z.Id == x.Parent.Id
                                                     && z.KindCheck != null &&
                                                     (z.KindCheck.Code == TypeCheck.PlannedExit || z.KindCheck.Code == TypeCheck.PlannedDocumentation))))
                                .Where(x => x.ActCheck.DocumentDate != null && x.ActCheck.DocumentDate >= this.dateStart && x.ActCheck.DocumentDate <= this.dateEnd)
                                .Select(x => new { x.ActCheck.Id, x.ActCheck.Area })
                                .ToList();

                areaBaseInsCheck.AddRange(areaPlaned1And3);

                var areaPlaned = areaBaseInsCheck.Distinct(x => x.Id).Sum(x => x.Area);
                result.Add(2, areaPlaned.Value);

                var codes4 = new[] { "1", "2", "3", "4", "5", "13", "14", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36" };

                var rec4 = servPrescriptionViol.GetAll()
                             .WhereIf(this.municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.InspectionViolation.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                             .Where(y => servViolationFeatureGji.GetAll().Any(x => x.ViolationGji.Id == y.InspectionViolation.Violation.Id && codes4.Contains(x.FeatureViolGji.Code)))
                             .Where(x => x.Document.DocumentDate != null && x.Document.DocumentDate >= this.dateStart && x.Document.DocumentDate <= this.dateEnd)
                             .Select(x => new { x.InspectionViolation.Inspection.Id, InspectionViolationId = x.InspectionViolation.Id })
                             .AsEnumerable()
                             .GroupBy(x => x.Id)
                             .ToDictionary(x => x.Key, x => x.Select(y => y.InspectionViolationId).Distinct().Count())
                             .Sum(x => x.Value);
                result.Add(4, rec4);

                var codes5 = new[] { "15", "16", "17" };
                var rec5 = servPrescriptionViol.GetAll()
                          .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.InspectionViolation.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                          .Where(y => servViolationFeatureGji.GetAll().Any(x => x.ViolationGji.Id == y.InspectionViolation.Violation.Id && codes5.Contains(x.FeatureViolGji.Code)))
                          .Where(x => x.Document.DocumentDate != null && x.Document.DocumentDate >= this.dateStart && x.Document.DocumentDate <= this.dateEnd)
                          .Select(x => new { x.InspectionViolation.Inspection.Id, InspectionViolationId = x.InspectionViolation.Id })
                          .AsEnumerable()
                          .GroupBy(x => x.Id)
                          .ToDictionary(x => x.Key, x => x.Select(y => y.InspectionViolationId).Distinct().Count())
                          .Sum(x => x.Value);
                result.Add(5, rec5);

                var codes6 = new[] { "6", "7", "8", "9", "10", "11", "12" };
                var rec6 = servPrescriptionViol.GetAll()
                            .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.InspectionViolation.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                            .Where(y => servViolationFeatureGji.GetAll().Any(x => x.ViolationGji.Id == y.InspectionViolation.Violation.Id && codes6.Contains(x.FeatureViolGji.Code)))
                            .Where(x => x.Document.DocumentDate != null && x.Document.DocumentDate >= this.dateStart && x.Document.DocumentDate <= this.dateEnd)
                            .Select(x => new { x.InspectionViolation.Inspection.Id, InspectionViolationId = x.InspectionViolation.Id })
                            .AsEnumerable()
                            .GroupBy(x => x.Id)
                            .ToDictionary(x => x.Key, x => x.Select(y => y.InspectionViolationId).Distinct().Count())
                            .Sum(x => x.Value);
                result.Add(6, rec6);

                result.Add(3, rec4 + rec5 + rec6);

                var countActCheck = 0;
                var countPrescription = 0;
                var countProtocol = 0;

                if (municipalityIds.Length > 0)
                {
                    /*имитация работы вьюшки*/

                    countPrescription = servPrescriptionViol.GetAll()
                        .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                        .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd)
                        .Where(x => x.InspectionViolation.RealityObject != null)
                        .GroupBy(x => x.Document.Id)
                        .Select(x => x.Min(y => y.InspectionViolation.RealityObject.Municipality.Id))
                        .AsEnumerable()
                        .Count(this.municipalityIds.Contains);

                    countProtocol = servProtocolViolation.GetAll()
                        .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                        .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd)
                        .Where(x => x.InspectionViolation.RealityObject != null)
                        .GroupBy(x => x.Document.Id)
                        .Select(x => x.Min(y => y.InspectionViolation.RealityObject.Municipality.Id))
                        .AsEnumerable()
                        .Count(this.municipalityIds.Contains);

                    countActCheck = serviceActCheckRealityObject.GetAll()
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.ActCheck.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.ActCheck.DocumentDate <= this.dateEnd)
                        .Where(x => x.RealityObject != null)
                        .GroupBy(x => x.ActCheck.Id)
                        .Select(x => x.Min(y => y.RealityObject.Municipality.Id))
                        .AsEnumerable()
                        .Count(this.municipalityIds.Contains);
                }
                else
                {
                    countPrescription = servPrescription.GetAll()
                        .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                        .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                        .Count();

                    countProtocol = servProtocol.GetAll()
                        .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                        .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                        .Count();

                    countActCheck = servActCheck.GetAll()
                        .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                        .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                        .Count();
                }

                result.Add(8, countPrescription);
                result.Add(9, countProtocol);
                result.Add(10, countActCheck);

                result.Add(7, countPrescription + countProtocol + countActCheck);

                var rec11 = this.GetFoundViolsOfPrescribedPeriods(servInspectionGjiRealityObject);
                result.Add(11, rec11);

                var executantCodes13 = new[] { "7", "14", "17" };
                var countResolution13 = servResolution.GetAll()
                   .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                   .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                   .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                   .Where(x => executantCodes13.Contains(x.Executant.Code)
                       && x.Sanction.Code == "1")
                   .Sum(x => x.PenaltyAmount);
                result.Add(13, countResolution13.HasValue ? countResolution13.Value : 0M);

                var executantCodes14 = new[] { "0", "1", "2", "3", "9", "10", "11", "12" };
                var countResolution14 = servResolution.GetAll()
                       .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                       .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                       .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                       .Where(x => executantCodes14.Contains(x.Executant.Code) && x.Sanction.Code == "1")
                       .Sum(x => x.PenaltyAmount);
                result.Add(14, countResolution14.HasValue ? countResolution14.Value : 0M);

                var executantCodes15 = new[] { "4", "5", "6" };
                var countResolution15 = servResolution.GetAll()
                              .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                              .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                              .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                              .Where(x => executantCodes15.Contains(x.Executant.Code) && x.Sanction.Code == "1")
                              .Sum(x => x.PenaltyAmount);
                result.Add(15, countResolution15.HasValue ? countResolution15.Value : 0M);

                var executantCodes16 = new[] { "8", "13", "15", "16", "18", "19" };
                var countResolution16 = servResolution.GetAll()
                                  .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                                  .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                                  .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                                  .Where(x => executantCodes16.Contains(x.Executant.Code) && x.Sanction.Code == "1")
                                  .Sum(x => x.PenaltyAmount);
                result.Add(16, countResolution16.HasValue ? countResolution16.Value : 0M);

                result.Add(12, (countResolution13 ?? 0) + (countResolution14 ?? 0) + (countResolution15 ?? 0) + (countResolution16 ?? 0));

                var countResolution17 = servResolution.GetAll()
                                  .WhereIf(municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.Inspection.Id && municipalityIds.Contains(x.RealityObject.Municipality.Id)))
                                  .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                                  .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                                  .Where(x => x.Paided == YesNoNotSet.Yes)
                                  .Sum(x => x.PenaltyAmount);
                result.Add(17, countResolution17.HasValue ? countResolution17.Value : 0M);

                var rec26 = servObjectCr.GetAll()
                                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                                .Count(x => x.ProgramCr.Id == this.programCrId && x.DateAcceptCrGji != null && x.DateAcceptCrGji >= this.dateStart && x.DateAcceptCrGji <= this.dateEnd);
                result.Add(26, rec26);

                var workCodes = new[] { "7", "8", "9", "10", "11" };

                // Все работы, запланированные по ОКР
                var works = this.Container.Resolve<IDomainService<TypeWorkCr>>()
                            .GetAll()
                            .WhereIf(municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                            .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                            .Where(x => x.ObjectCr.DateAcceptCrGji != null && x.ObjectCr.DateAcceptCrGji >= this.dateStart && x.ObjectCr.DateAcceptCrGji <= this.dateEnd)
                            .Where(x => workCodes.Contains(x.Work.Code))
                            .Select(x => new
                            {
                                typeWorkId = x.Id,
                                ObjectCrId = x.ObjectCr.Id,
                                x.PercentOfCompletion
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.ObjectCrId)
                            .ToDictionary(
                                 x => x.Key,
                                 x => x.ToDictionary(y => y.typeWorkId, y => y.PercentOfCompletion));

                var recs = this.reportDate.Date.Year >= 2013
                    ? this.Container.Resolve<IDomainService<ArchiveSmr>>()
                            .GetAll()
                            .WhereIf(municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                            .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programCrId)
                            .Where(x => x.TypeWorkCr.ObjectCr.DateAcceptCrGji != null && x.TypeWorkCr.ObjectCr.DateAcceptCrGji >= this.dateStart && x.TypeWorkCr.ObjectCr.DateAcceptCrGji <= this.dateEnd)
                            .Where(x => workCodes.Contains(x.TypeWorkCr.Work.Code))
                            .Where(x => x.DateChangeRec <= this.reportDate)
                            .Select(x => new
                            {
                                typeWorkCrId = x.TypeWorkCr.Id,
                                ObjectCrId = x.TypeWorkCr.ObjectCr.Id,
                                x.PercentOfCompletion,
                                x.ObjectCreateDate
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.typeWorkCrId)
                            .Select(x =>
                            {
                                var archRec = x.OrderByDescending(y => y.ObjectCreateDate).First();
                                return new
                                {
                                    typeWorkId = x.Key,
                                    archRec.ObjectCrId,
                                    archRec.PercentOfCompletion
                                };
                            })
                            .GroupBy(x => x.ObjectCrId)
                            .ToDictionary(
                                 x => x.Key,
                                 x => x.ToDictionary(y => y.typeWorkId, y => y.PercentOfCompletion))
                    : works;

                var rec27 = works.Select(
                    x =>
                    {
                        // Проверяем все ли работы есть в архиве, и все ли они = 100%

                        if (!recs.ContainsKey(x.Key))
                        {
                            return false;
                        }

                        var factObjectCrWorks = recs[x.Key];

                        return x.Value.All(y => factObjectCrWorks.ContainsKey(y.Key) && factObjectCrWorks[y.Key] == 100);
                    })
                    .Count(x => x);

                result.Add(27, rec27);
                result.Add(35, servInspector.GetAll().Count());
                result.Add(36, servInspector.GetAll().Count(x => !x.IsHead));

                return result;
            }
            finally
            {
                Container.Release(servActCheck);
                Container.Release(serviceActCheckRealityObject);
                Container.Release(servInspectionGjiRealityObject);
                Container.Release(servPrescriptionViol);
                Container.Release(servViolationFeatureGji);
                Container.Release(servPrescription);
                Container.Release(servProtocol);
                Container.Release(servResolution);
                Container.Release(servObjectCr);
                Container.Release(servInspector);
            }
        }

        /// <summary>
        /// Выявлено нарушений предписанных сроков
        /// </summary>
        /// <param name="servInspectionGjiRealityObject"></param>
        /// <returns></returns>
        private int GetFoundViolsOfPrescribedPeriods(IDomainService<InspectionGjiRealityObject> servInspectionGjiRealityObject)
        {
            return Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
               .WhereIf(this.municipalityIds.Length > 0, y => servInspectionGjiRealityObject.GetAll().Any(x => x.Inspection.Id == y.Document.Inspection.Id && this.municipalityIds.Contains(x.RealityObject.Municipality.Id)))
               .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
               .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd)
               .Select(x => new { x.Id, DocumentId = x.Document.Id })
               .Count();
        }
    }
}
