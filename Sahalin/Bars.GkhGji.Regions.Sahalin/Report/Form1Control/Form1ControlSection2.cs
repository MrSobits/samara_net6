namespace Bars.GkhGji.Regions.Sahalin.Report.Form1Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Enums;

    class Form1ControlSection2 : BaseReportSection
    {
        public IWindsorContainer Container { get; set; }

        private readonly TypeCheck [] kindCheckCodes_1_3_7_8 = { TypeCheck.PlannedExit, TypeCheck.PlannedDocumentation, TypeCheck.PlannedDocumentationExit, TypeCheck.VisualSurvey };
        private readonly TypeCheck[] kindCheckCodes_2_4_9 = { TypeCheck.NotPlannedExit, TypeCheck.NotPlannedDocumentation, TypeCheck.NotPlannedDocumentationExit };

        private IDomainService<Disposal> serviceDisposal;
        private IDomainService<Prescription> servicePrescription;
        private IDomainService<DocumentGjiChildren> serviceDocGjiChildren;
        private IDomainService<ActCheckRealityObject> serviceActCheckRO;
        private IDomainService<Resolution> serviceResolution;
        private IDomainService<ResolutionDispute> serviceResolutionDispute;
        private IDomainService<ResolutionPayFine> serviceResolutionPayFine;

        List<long> kindCheckCodes_2_4_9DisposalIds;
        List<long> kindCheckCodes_2_4_9DisposalStageIds;
        List<long> plannedJurkindCheckCodes_1_3_7_8DisposalIds;
        List<long> plannedJurkindCheckCodes_1_3_7_8DisposalStageIds;

        List<long> jurPersonsPrescriptions;
        List<long> jurPersonsProtocols;
        private long exeDocWithViolationsCell18_7Count;
        private long exeDocWithViolationsCell18_6Count;
        private long violationsCountCell20_6;
        private long violationsCountCell20_7;
        private long actsViolationCount22_6;
        private long actsViolationCount22_7;
        private long disposalsCount23_6;
        private long disposalsCount23_7;
        private long resolutionCount28_6;
        private long resolutionCount28_7;
        private long resolutionCount32_6;
        private long resolutionCount32_7;
        private long resolutionCount33_6;
        private long resolutionCount33_7;
        private long resolutionCount34_6;
        private long resolutionCount34_7;
        private long resolutionCount35_6;
        private long resolutionCount35_7;
        private long resolutionCount37_6;
        private long resolutionCount37_7;

        public Form1ControlSection2(long[] inspections, DateTime dateStart, DateTime dateEnd, IWindsorContainer container)
            : base(inspections, dateStart, dateEnd)
        {
            Container = container;

            serviceDisposal = Container.Resolve<IDomainService<Disposal>>();
            serviceDocGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            serviceActCheckRO = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            serviceResolution = Container.Resolve<IDomainService<Resolution>>();
            serviceResolutionDispute = Container.Resolve<IDomainService<ResolutionDispute>>();
            servicePrescription = Container.Resolve<IDomainService<Prescription>>();
            serviceResolutionPayFine = Container.Resolve<IDomainService<ResolutionPayFine>>();

            this.getDisposals();
            this.getRows17_37Values();
        }

        private int GetCell17_5()
        {
            return this.jurPersonsPrescriptions.Union(this.jurPersonsProtocols).Distinct().Count();
        }

        private long GetCell18_6()
        {
            return this.exeDocWithViolationsCell18_6Count;
        }

        private long GetCell18_7()
        {
            return this.exeDocWithViolationsCell18_7Count;
        }

        private long GetCell19_6()
        {
            return this.violationsCountCell20_6 + this.actsViolationCount22_6;
        }

        private long GetCell19_7()
        {
            return this.violationsCountCell20_7 + this.actsViolationCount22_7;
        }

        private long GetCell20_6()
        {
            return this.violationsCountCell20_6;
        }

        private long GetCell20_7()
        {
            return this.violationsCountCell20_7;
        }

        private long GetCell22_6()
        {
            return this.actsViolationCount22_6;
        }

        private long GetCell22_7()
        {
            return this.actsViolationCount22_7;
        }

        private long GetCell23_6()
        {
            return this.disposalsCount23_6;
        }

        private long GetCell23_7()
        {
            return this.disposalsCount23_7;
        }

        private long GetCell24_6()
        {
            return this.resolutionCount28_6;
        }

        private long GetCell24_7()
        {
            return this.resolutionCount28_7;
        }

        private long GetCell28_6()
        {
            return this.resolutionCount28_6;
        }

        private long GetCell28_7()
        {
            return this.resolutionCount28_7;
        }

        private long GetCell32_6()
        {
            return this.resolutionCount32_6;
        }

        private long GetCell32_7()
        {
            return this.resolutionCount32_7;
        }

        private long GetCell33_6()
        {
            return this.resolutionCount33_6;
        }

        private long GetCell33_7()
        {
            return this.resolutionCount33_7;
        }

        private long GetCell34_6()
        {
            return this.resolutionCount34_6;
        }

        private long GetCell34_7()
        {
            return this.resolutionCount34_7;
        }

        private long GetCell35_6()
        {
            return this.resolutionCount35_6;
        }

        private long GetCell35_7()
        {
            return this.resolutionCount35_7;
        }

        private long GetCell37_6()
        {
            return this.resolutionCount37_6;
        }

        private long GetCell37_7()
        {
            return this.resolutionCount37_7;
        }

        private int GetCell38_6()
        {
            return 0;
        }

        private int GetCell38_7()
        {
            return 0;
        }

        private decimal GetCell39_6()
        {
            return getLines39(plannedJurkindCheckCodes_1_3_7_8DisposalStageIds);
        }

        private decimal GetCell39_7()
        {
            return getLines39(kindCheckCodes_2_4_9DisposalStageIds);
        }

        private decimal GetCell40_6()
        {
            return getLines40(plannedJurkindCheckCodes_1_3_7_8DisposalStageIds);
        }

        private decimal GetCell40_7()
        {
            return getLines40(kindCheckCodes_2_4_9DisposalStageIds);
        }

        private decimal GetCell42_6()
        {
            return getLines42(plannedJurkindCheckCodes_1_3_7_8DisposalStageIds);
        }

        private decimal GetCell42_7()
        {
            return getLines42(kindCheckCodes_2_4_9DisposalStageIds);
        }

        private int GetCell46_6()
        {
            return this.getLine46(plannedJurkindCheckCodes_1_3_7_8DisposalStageIds);
        }

        private int GetCell46_7()
        {
            return this.getLine46(kindCheckCodes_2_4_9DisposalStageIds);
        }

        private int GetCell47_6()
        {
            var plannedJurActCheckIds = new List<long>();
            for (int i = 0; i < InspectionsIds.Length; i += 1000)
            {
                var tmpInspectionIds = InspectionsIds.Skip(i).Take(1000).ToArray();

                plannedJurActCheckIds.AddRange(serviceActCheckRO.GetAll()
                    .Where(x => tmpInspectionIds.Contains(x.ActCheck.Inspection.Id))
                    .Where(x => x.HaveViolation == YesNoNotSet.No)
                    .Where(x => x.ActCheck.Inspection.TypeBase == TypeBase.PlanJuridicalPerson)
                    .Select(x => x.ActCheck.Id)
                    .Distinct());
            }

            return this.getLine47(plannedJurActCheckIds, kindCheckCodes_1_3_7_8); 
        }

        private int GetCell47_7()
        {
            var actCheckIds = new List<long>();
            for (int i = 0; i < InspectionsIds.Length; i += 1000)
            {
                var tmpInspectionIds = InspectionsIds.Skip(i).Take(1000).ToArray();

                actCheckIds.AddRange(serviceActCheckRO.GetAll()
                    .Where(x => tmpInspectionIds.Contains(x.ActCheck.Inspection.Id))
                    .Where(x => x.HaveViolation == YesNoNotSet.No)
                    .Select(x => x.ActCheck.Id)
                    .Distinct());
            }

            return this.getLine47(actCheckIds, kindCheckCodes_2_4_9);
        }

        private int GetCell48_7()
        {
            var actCheckIds = new List<long>();
            for (int i = 0; i < InspectionsIds.Length; i += 1000)
            {
                var tmpInspectionIds = InspectionsIds.Skip(i).Take(1000).ToArray();

                actCheckIds.AddRange(serviceActCheckRO.GetAll()
                    .Where(x => x.ActCheck.Inspection.TypeBase == TypeBase.CitizenStatement)
                    .Where(x => tmpInspectionIds.Contains(x.ActCheck.Inspection.Id))
                    .Where(x => x.HaveViolation == YesNoNotSet.No)
                    .Select(x => x.ActCheck.Id).Distinct());
            }

            return this.getLine47(actCheckIds, kindCheckCodes_2_4_9);
        }

        private int GetCell49_6()
        {
            return getLine49(plannedJurkindCheckCodes_1_3_7_8DisposalStageIds);
        }

        private int GetCell49_7()
        {
            return getLine49(kindCheckCodes_2_4_9DisposalStageIds);
        }

        private int GetCell50_6()
        {
            return getLine50(plannedJurkindCheckCodes_1_3_7_8DisposalStageIds);
        }

        private int GetCell50_7()
        {
            return getLine50(kindCheckCodes_2_4_9DisposalStageIds);
        }

        private int GetCell51_6()
        {
            return getLine51(plannedJurkindCheckCodes_1_3_7_8DisposalStageIds);
        }

        private int GetCell51_7()
        {
            return getLine51(kindCheckCodes_2_4_9DisposalStageIds);
        }

        int getLines39_42(IList<long> disposalStageIds, List<long> contractorType)
        {
            decimal count = 0;
            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();

                count += GetDocuments<Resolution>()
                    .Where(x => tmpDisposalIds.Contains(x.Stage.Parent.Id)
                                && contractorType.Contains(x.Executant.Code.ToLong())
                                && x.Sanction.Code == "1"
                                && x.DocumentDate >= this.DateStart
                                && x.DocumentDate <= this.DateEnd)
                    .Select(x => x.PenaltyAmount)
                    .AsEnumerable()
                    .Sum(x => x.HasValue ? x.Value : 0);
            }

            return count.ToInt();
        }

        private decimal GetResolutionPays(IList<long> resolutionIds)
        {
            decimal sum = 0;

            for (int i = 0; i < resolutionIds.Count(); i += 1000)
            {
                var tmpResolutionIds = resolutionIds.Skip(i).Take(1000).ToArray();

                var c = serviceResolutionPayFine.GetAll()
                    .Where(x => tmpResolutionIds.Contains(x.Resolution.Id))
                    .Select(x => x.Amount)
                    .ToList();

                sum += c.Sum(x => x.HasValue ? x.Value : 0);
            }

            return sum / 1000;
        }

        decimal getLines39(IList<long> disposalStageIds)
        {
            var resolutionIds = new List<long>();

            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();

                resolutionIds.AddRange(GetDocuments<Resolution>()
                    .Where(x => tmpDisposalIds.Contains(x.Stage.Parent.Id))
                    .Where(x => x.Sanction.Code == "1")
                    .Where(x => x.DocumentDate >= DateStart)
                    .Where(x => x.DocumentDate <= DateEnd)
                    .Where(x => x.Executant.Code == "6"
                                || x.Executant.Code == "7"
                                || x.Executant.Code == "14")
                    .Select(x => x.Id));
            }

            return GetResolutionPays(resolutionIds);
        }

        decimal getLines40(IList<long> disposalStageIds)
        {
            var resolutionIds = new List<long>();

            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();
                resolutionIds.AddRange(GetDocuments<Resolution>()
                    .Where(x => tmpDisposalIds.Contains(x.Stage.Parent.Id))
                    .Where(x => x.Sanction.Code == "1")
                    .Where(x => x.DocumentDate >= DateStart)
                    .Where(x => x.DocumentDate <= DateEnd)
                    .Where(x => x.Executant.Code == "1"
                                || x.Executant.Code == "3"
                                || x.Executant.Code == "5"
                                || x.Executant.Code == "10"
                                || x.Executant.Code == "12"
                                || x.Executant.Code == "12"
                                || x.Executant.Code == "16"
                                || x.Executant.Code == "19")
                    .Select(x => x.Id));
            }

            return GetResolutionPays(resolutionIds);
        }

        decimal getLines42(IList<long> disposalStageIds)
        {
            var resolutionIds = new List<long>();
            
            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();

                resolutionIds.AddRange(GetDocuments<Resolution>()
                    .Where(x => tmpDisposalIds.Contains(x.Stage.Parent.Id))
                    .Where(x => x.Sanction.Code == "1")
                    .Where(x => x.DocumentDate >= DateStart)
                    .Where(x => x.DocumentDate <= DateEnd)
                    .Where(x => x.Executant.Code == "0"
                                || x.Executant.Code == "2"
                                || x.Executant.Code == "4"
                                || x.Executant.Code == "8"
                                || x.Executant.Code == "9"
                                || x.Executant.Code == "11"
                                || x.Executant.Code == "15")
                    .Select(x => x.Id));
            }

            return GetResolutionPays(resolutionIds);
        }

        int getLine46(IList<long> disposalStageIds)
        {
            int count = 0;
            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();

                count += GetDocuments<Prescription>()
                    .Count(x => tmpDisposalIds.Contains(x.Stage.Parent.Id)
                        && x.DocumentDate >= DateStart
                        && x.DocumentDate <= DateEnd);
            }

            return count;
        }

        int getLine47(ICollection<long> actSurveyIds, TypeCheck[] kindCheckCodes)
        {
            var disposalIds = new List<long>();

            for (int i = 0; i < actSurveyIds.Count; i += 1000)
            {
                var tmpActSurveyIds = actSurveyIds.Skip(i).Take(1000).ToArray();

                disposalIds.AddRange(serviceDocGjiChildren.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => tmpActSurveyIds.Contains(x.Children.Id))
                    .Where(x => x.Parent.DocumentDate >= DateStart)
                    .Where(x => x.Parent.DocumentDate <= DateEnd)
                    .Select(x => x.Parent.Id));
            }

            int count = 0;

            for (int i = 0; i < disposalIds.Count; i += 1000)
            {
                var tmpDisposalIds = disposalIds.Skip(i).Take(1000).ToArray();

                count += serviceDisposal.GetAll()
                    .Count(x => tmpDisposalIds.Contains(x.Id)
                                && kindCheckCodes.Contains(x.KindCheck.Code));
            }

            return count;
        }

        int getLine49(IList<long> disposalStageIds)
        {
            int count = 0;
            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();

                count += GetDocuments<Resolution>()
                    .Count(x => serviceResolutionDispute.GetAll().Any(y => y.Resolution.Id == x.Id && y.CourtVerdict.Code == "2")
                        && tmpDisposalIds.Contains(x.Stage.Parent.Id)
                        && x.DocumentDate >= DateStart
                        && x.DocumentDate <= DateEnd);
            }

            return count;
        }

        int getLine50(IList<long> disposalStageIds)
        {
            int count = 0;
            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();

                count += GetDocuments<Resolution>()
                    .Count(x => serviceResolutionDispute.GetAll().Any(y => y.Resolution.Id == x.Id && y.CourtVerdict.Code == "2" && y.Appeal == ResolutionAppealed.Law)
                        && tmpDisposalIds.Contains(x.Stage.Parent.Id)
                        && x.DocumentDate >= DateStart
                        && x.DocumentDate <= DateEnd);
            }

            return count;
        }

        int getLine51(IList<long> disposalStageIds)
        {
            int count = 0;
            for (int i = 0; i < disposalStageIds.Count(); i += 1000)
            {
                var tmpDisposalIds = disposalStageIds.Skip(i).Take(1000).ToArray();

                count += GetDocuments<Resolution>()
                    .Count(x => serviceResolutionDispute.GetAll().Any(y => y.Resolution.Id == x.Id && y.CourtVerdict.Code == "2" && y.Appeal == ResolutionAppealed.RealityObjInspection)
                        && tmpDisposalIds.Contains(x.Stage.Parent.Id)
                        && x.DocumentDate >= this.DateStart
                        && x.DocumentDate <= this.DateEnd);
            }

            return count;
        }

        void getDisposals()
        {
            plannedJurkindCheckCodes_1_3_7_8DisposalIds = new List<long>();
            plannedJurkindCheckCodes_1_3_7_8DisposalStageIds = new List<long>();
            kindCheckCodes_2_4_9DisposalIds = new List<long>();
            kindCheckCodes_2_4_9DisposalStageIds = new List<long>();

            for (int i = 0; i < InspectionsIds.Length; i += 1000)
            {
                var tmpInspectionIds = InspectionsIds.Skip(i).Take(1000).ToArray();

                var tempData =
                    GetDocuments<Disposal>()
                        .Where(x => x.DocumentDate >= DateStart)
                        .Where(x => x.DocumentDate <= DateEnd)
                        .Where(x => kindCheckCodes_1_3_7_8.Contains(x.KindCheck.Code)
                                    && x.Inspection.TypeBase == TypeBase.PlanJuridicalPerson
                                    && tmpInspectionIds.Contains(x.Inspection.Id))
                        .Select(x => new {x.Id, stageID = x.Stage.Id})
                        .ToArray();

                plannedJurkindCheckCodes_1_3_7_8DisposalIds.AddRange(tempData.Select(x => x.Id).Distinct());
                plannedJurkindCheckCodes_1_3_7_8DisposalStageIds.AddRange(tempData.Select(x => x.stageID).Distinct());

                var tempData2 =
                    GetDocuments<Disposal>()
                        .Where(x => x.DocumentDate >= DateStart)
                        .Where(x => x.DocumentDate <= DateEnd)
                        .Where(x => tmpInspectionIds.Contains(x.Inspection.Id))
                        .Where(x => kindCheckCodes_2_4_9.Contains(x.KindCheck.Code))
                        .Select(x => new {x.Id, stageID = x.Stage.Id})
                        .ToArray();

                kindCheckCodes_2_4_9DisposalIds.AddRange(tempData2.Select(x => x.Id).Distinct());
                kindCheckCodes_2_4_9DisposalStageIds.AddRange(tempData2.Select(x => x.stageID).Distinct());
            }
        }

        private void getRows17_37Values()
        {
            var childrenDomain = Container.ResolveDomain<DocumentGjiChildren>();

            var start = 1000;

            var servProtocols = GetDocuments<Protocol>()
                .WhereIf(DateStart != DateTime.MinValue, x => x.DocumentDate >= DateStart)
                .WhereIf(DateEnd != DateTime.MinValue, x => x.DocumentDate <= DateEnd);

            var servPrescriptions =
                GetDocuments<Prescription>()
                    .WhereIf(DateStart != DateTime.MinValue, x => x.DocumentDate >= DateStart)
                    .WhereIf(DateEnd != DateTime.MinValue, x => x.DocumentDate <= DateEnd);

            //var servDisposals = this.Container.Resolve<IDomainService<Disposal>>().GetAll();

            var servExeDocActs =
                childrenDomain.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription
                                || x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .WhereIf(DateStart != DateTime.MinValue, x => x.Children.DocumentDate >= DateStart)
                    .WhereIf(DateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= DateEnd);

            var servActsDisposals =
                childrenDomain.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck);

            var tmpFirst = InspectionsIds.Length > start ? InspectionsIds.Take(1000).ToArray() : InspectionsIds;

            var inspectionsProtocols =
                servProtocols
                    .Where(x => tmpFirst.Contains(x.Inspection.Id))
                    .Select(x => new
                    {
                        x.Id,
                        inspectionId = x.Inspection.Id,
                        contragentId = (long?) x.Contragent.Id,
                        x.Inspection.TypeBase
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var contragentId = x.contragentId ?? 0;
                        return new {x.Id, x.inspectionId, contragentId, x.TypeBase};
                    })
                    .ToList();

            var inspectionsPrescriptions =
                servPrescriptions.Where(x => tmpFirst.Contains(x.Inspection.Id))
                    .Select(x => new
                    {
                        x.Id,
                        inspectionId = x.Inspection.Id,
                        contragentId = (long?) x.Contragent.Id,
                        x.Inspection.TypeBase
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var contragentId = x.contragentId ?? 0;
                        return new {x.Id, x.inspectionId, contragentId, x.TypeBase};
                    })
                    .ToList();

            var exeDocActIds =
                servExeDocActs
                    .Where(x => tmpFirst.Contains(x.Children.Inspection.Id))
                    .Select(x => new {exeDocId = x.Children.Id, actId = x.Parent.Id})
                    .ToList();

            while (start < InspectionsIds.Length)
            {
                var tmp = InspectionsIds.Skip(start).Take(1000).ToArray();

                inspectionsProtocols.AddRange(
                    servProtocols
                        .Where(x => tmp.Contains(x.Inspection.Id))
                        .Select(x => new
                        {
                            x.Id,
                            inspectionId = x.Inspection.Id,
                            contragentId = (long?) x.Contragent.Id,
                            x.Inspection.TypeBase
                        })
                        .AsEnumerable()
                        .Select(x =>
                        {
                            var contragentId = x.contragentId ?? 0;
                            return new {x.Id, x.inspectionId, contragentId, x.TypeBase};
                        })
                        .ToList());

                inspectionsPrescriptions.AddRange(
                    servPrescriptions
                        .Where(x => tmp.Contains(x.Inspection.Id))
                        .Select(x => new
                        {
                            x.Id,
                            inspectionId = x.Inspection.Id,
                            //contragentId = x.Contragent != null ? x.Contragent.Id : 0,
                            contragentId = (long?) x.Contragent.Id,
                            x.Inspection.TypeBase
                        })
                        .AsEnumerable()
                        .Select(x =>
                        {
                            var contragentId = x.contragentId ?? 0;
                            return new {x.Id, x.inspectionId, contragentId, x.TypeBase};
                        })
                        .ToList());

                exeDocActIds.AddRange(
                    servExeDocActs
                        .Where(x => tmp.Contains(x.Children.Inspection.Id))
                        .Select(x => new {exeDocId = x.Children.Id, actId = x.Parent.Id})
                        .ToList());

                start += 1000;
            }

            var tempActsIds = exeDocActIds.Select(x => x.actId).ToArray();

            start = 1000;

            tmpFirst = tempActsIds.Length > start ? tempActsIds.Take(1000).ToArray() : tempActsIds;

            var actDisposalIds =
                servActsDisposals
                    .Where(x => tmpFirst.Contains(x.Children.Id))
                    .Select(x => new {actId = x.Children.Id, disposalId = x.Parent.Id})
                    .ToList();

            while (start < tempActsIds.Length)
            {
                var tmp = tempActsIds.Skip(start).Take(1000).ToArray();

                actDisposalIds.AddRange(
                    servActsDisposals.Where(x => tmp.Contains(x.Children.Id))
                        .Select(x => new {actId = x.Children.Id, disposalId = x.Parent.Id})
                        .ToList());

                start += 1000;
            }

            var tempDisposalsIds = actDisposalIds.Select(x => x.disposalId).ToArray();

            start = 1000;

            tmpFirst = tempDisposalsIds.Length > start ? tempDisposalsIds.Take(1000).ToArray() : tempDisposalsIds;

            var disposalsWithExeDocs =
                GetDocuments<Disposal>()
                    .Where(x => tmpFirst.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        Code = x.KindCheck != null ? x.KindCheck.Code : 0,
                        x.TypeDisposal,
                        x.Inspection.TypeBase,
                        x.Stage
                    })
                    .ToList();

            while (start < tempDisposalsIds.Length)
            {
                var tmp = tempDisposalsIds.Skip(start).Take(1000).ToArray();

                disposalsWithExeDocs.AddRange(
                    GetDocuments<Disposal>()
                        .Where(x => tmp.Contains(x.Id))
                        .Select(x => new
                        {
                            x.Id,
                            Code = x.KindCheck != null ? x.KindCheck.Code : 0,
                            x.TypeDisposal,
                            x.Inspection.TypeBase,
                            x.Stage
                        })
                        .ToList());

                start += 1000;
            }

            var exeDocsDisposalsIds =
                actDisposalIds
                    .Join(exeDocActIds,
                        x => x.actId,
                        y => y.actId,
                        (x, y) => new {x.disposalId, y.exeDocId})
                    .ToList();

            var disposalsIdsRow17 =
                disposalsWithExeDocs
                    .Where(x => x.Code == TypeCheck.PlannedExit
                                || x.Code == TypeCheck.PlannedDocumentation
                                || x.Code == TypeCheck.PlannedDocumentationExit
                                || x.Code == TypeCheck.VisualSurvey)
                    .Select(x => x.Id)
                    .ToList();

            // Ид требуемых исп.документов
            var tempExeDocsIds =
                exeDocsDisposalsIds.Select(x => x.exeDocId)
                    .ToArray();

            // Ид требуемых исп.документов (строка 17)
            var tempExeDocsIdsRow17 =
                exeDocsDisposalsIds.Where(x => disposalsIdsRow17.Contains(x.disposalId))
                    .Select(x => x.exeDocId)
                    .ToArray();

            this.jurPersonsPrescriptions =
                inspectionsPrescriptions.Where(x => tempExeDocsIdsRow17.Contains(x.Id))
                    .Where(x => x.TypeBase == TypeBase.PlanJuridicalPerson)
                    .Select(x => x.contragentId)
                    .ToList();

            this.jurPersonsProtocols =
                inspectionsProtocols.Where(x => tempExeDocsIdsRow17.Contains(x.Id))
                    .Where(x => x.TypeBase == TypeBase.PlanJuridicalPerson)
                    .Select(x => x.contragentId)
                    .ToList();

            start = 1000;

            var servExeDocsWithViol = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll();

            tmpFirst = tempExeDocsIds.Length > start ? tempExeDocsIds.Take(1000).ToArray() : tempExeDocsIds;

            // Исп. документы + нарушения
            var exeDocsWithViolations =
                servExeDocsWithViol
                    .Where(x => tmpFirst.Contains(x.Document.Id))
                    .Select(x => new
                    {
                        exeDocId = x.Document.Id,
                        x.Document.Inspection.TypeBase,
                        DocViol = new
                        {
                            exeDocNum = x.Document.DocumentNumber,
                            violId = x.InspectionViolation.Id
                        }
                    })
                    .ToList();

            while (start < tempExeDocsIds.Length)
            {
                var tmp = tempExeDocsIds.Skip(start).Take(1000).ToArray();

                exeDocsWithViolations.AddRange(
                    servExeDocsWithViol
                        .Where(x => tmp.Contains(x.Document.Id))
                        .Select(x => new
                        {
                            exeDocId = x.Document.Id,
                            x.Document.Inspection.TypeBase,
                            DocViol = new
                            {
                                exeDocNum = x.Document.DocumentNumber,
                                violId = x.InspectionViolation.Id
                            }
                        })
                        .ToList());

                start += 1000;
            }

            var exeDocsWithViolationsIds = exeDocsWithViolations.Select(x => x.exeDocId).Distinct().ToList();

            // Распоряжения с кодами Видов проверок: 2,4,9 
            var disposalsIdsCell18_7 =
                disposalsWithExeDocs
                    .Where(x => x.Code == TypeCheck.NotPlannedExit
                                || x.Code == TypeCheck.NotPlannedDocumentation
                                || x.Code == TypeCheck.NotPlannedDocumentationExit);

            var disposalsIdsCell18_7Ids = disposalsIdsCell18_7.Select(x => x.Id).ToList();

            var exeDocWithViolationsCell18_6 =
                exeDocsDisposalsIds
                    .Where(x => disposalsIdsRow17.Contains(x.disposalId))
                    .Where(x => exeDocsWithViolationsIds.Contains(x.exeDocId))
                    .Select(x => x.exeDocId)
                    .Distinct()
                    .ToList();

            // Количество Распоряжений, у которых есть зависимые Предписания и\или Протоколы с значениями во вкладке "Нарушения". Учитывать Распоряжения с кодами Видов проверок: 1,3,7,8
            this.exeDocWithViolationsCell18_6Count = exeDocWithViolationsCell18_6.Count;

            var exeDocWithViolationsCell18_7 =
                exeDocsDisposalsIds
                    .Where(x => disposalsIdsCell18_7Ids.Contains(x.disposalId))
                    .Where(x => exeDocsWithViolationsIds.Contains(x.exeDocId))
                    .Select(x => x.exeDocId)
                    .Distinct()
                    .ToList();

            // Количество Распоряжений, у которых есть зависимые Предписания и\или Протоколы с значениями во вкладке "Нарушения". Учитывать Распоряжения с кодами Видов проверок: 2,4,9 
            this.exeDocWithViolationsCell18_7Count = exeDocWithViolationsCell18_7.Count;

            // Количество нарушений из предписаний и Протоколов--вкладка "Нарушения". Учитывать Предписания, Протоколы, которые созданы из Распоряжения с кодами  Видов проверок = 1,3,7,8, 
            // а в поле "Основание обследования" значение = Плановая проверка юр.лица. Если у Предписания и Протокола номера одинаковые, то проверять нарушения на уникальность.
            this.violationsCountCell20_6 =
                exeDocsWithViolations
                    .Where(x => x.TypeBase == TypeBase.PlanJuridicalPerson)
                    .Where(x => exeDocWithViolationsCell18_6.Contains(x.exeDocId))
                    .Select(x => x.DocViol)
                    .Distinct()
                    .Count();

            var disposalsIdsCell20_7Ids =
                disposalsIdsCell18_7
                    .Where(x => x.TypeDisposal != TypeDisposalGji.DocumentGji)
                    .Select(x => x.Id)
                    .ToList();

            var exeDocWithViolationsCell20_7 =
                exeDocsDisposalsIds
                    .Where(x => disposalsIdsCell20_7Ids.Contains(x.disposalId))
                    .Where(x => exeDocsWithViolationsIds.Contains(x.exeDocId))
                    .Select(x => x.exeDocId)
                    .Distinct()
                    .ToList();

            // Количество нарушений из предписаний и Протоколов--вкладка "Нарушения". Учитывать Предписания, Протоколы, которые созданы из Распоряжения с кодами Видов проверок = 2,4,9 
            // в поле "Основание обследования" значение НЕ = Предписание. Если у Предписания и Протокола номера одинаковые, то проверять нарушения на уникальность.
            this.violationsCountCell20_7 =
                exeDocsWithViolations
                    .Where(x => exeDocWithViolationsCell20_7.Contains(x.exeDocId))
                    .Select(x => x.DocViol)
                    .Distinct()
                    .Count();

            var disposalsIdsCell22_6 =
                disposalsWithExeDocs
                    .Where(x => x.Code == TypeCheck.PlannedExit || x.Code == TypeCheck.PlannedDocumentation)
                    .Where(x => x.TypeBase == TypeBase.PlanJuridicalPerson)
                    .Select(x => x.Id)
                    .ToList();

            var disposalsIdsCell22_7 =
                disposalsWithExeDocs
                    .Where(x => x.Code == TypeCheck.NotPlannedExit || x.Code == TypeCheck.NotPlannedDocumentation)
                    .Select(x => x.Id)
                    .ToList();

            var actsIds22_6 =
                actDisposalIds.Where(x => disposalsIdsCell22_6.Contains(x.disposalId)).Select(x => x.actId).ToList();

            var actsIds22_7 =
                actDisposalIds.Where(x => disposalsIdsCell22_7.Contains(x.disposalId)).Select(x => x.actId).ToList();

            var allActsIds = actsIds22_6.Union(actsIds22_7).ToArray();

            start = 1000;

            var servActCheckViolation =
                Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
                    .Where(x => x.InspectionViolation.DateFactRemoval == null);

            tmpFirst = allActsIds.Length > start ? allActsIds.Take(1000).ToArray() : allActsIds;

            var actCheckViolations =
                servActCheckViolation
                    .Where(x => tmpFirst.Contains(x.ActObject.ActCheck.Id))
                    .Select(x => new {violationId = x.InspectionViolation.Id, actId = x.ActObject.ActCheck.Id})
                    .ToList();

            while (start < allActsIds.Length)
            {
                var tmp = allActsIds.Skip(start).Take(1000).ToArray();

                actCheckViolations.AddRange(
                    servActCheckViolation
                        .Where(x => tmp.Contains(x.ActObject.ActCheck.Id))
                        .Select(x => new {violationId = x.InspectionViolation.Id, actId = x.ActObject.ActCheck.Id})
                        .ToList());

                start += 1000;
            }

            // Количество нарушений из Акта проверки-вкладка "Результаты проверки ", у которых в поле "Дата факт. исполнения" пусто. 
            // Учитывать те акты, которое создано из Распоряжения с Видом  проверки = Плановое документарное, Плановое выездное 
            // и в поле "Основание обследования" значение = Плановая проверка юр. лица
            this.actsViolationCount22_6 = actCheckViolations.Count(x => actsIds22_6.Contains(x.actId));

            // Количество нарушений из Акта проверки-вкладка "Результаты проверки ", у которых в поле "Дата факт. исполнения" пусто. 
            // Учитывать те акты, которое создано из Распоряжения с Видом  проверки = ВнеПлановое документарное, ВнеПлановое выездное 
            this.actsViolationCount22_7 = actCheckViolations.Count(x => actsIds22_7.Contains(x.actId));

            var protocolsIds = inspectionsProtocols.Select(x => x.Id).ToList();

            // Количество Распоряжений, у которых есть зависимые Протоколы. Учитывать Распоряжения с Видом проверки = Плановое документарное, Плановое выездное и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.disposalsCount23_6 =
                exeDocsDisposalsIds.Where(
                    x => disposalsIdsCell22_6.Contains(x.disposalId) && protocolsIds.Contains(x.exeDocId))
                    .Select(x => x.disposalId)
                    .Distinct()
                    .Count();

            // Количество Распоряжений, у которых есть зависимые Протоколы. Учитывать Распоряжения с Видом проверки = ВнеПлановое документарное, ВнеПлановое выездное
            this.disposalsCount23_7 =
                exeDocsDisposalsIds
                    .Where(x => disposalsIdsCell22_7.Contains(x.disposalId) && protocolsIds.Contains(x.exeDocId))
                    .Select(x => x.disposalId)
                    .Distinct()
                    .Count();

            var dispStagesIds28_6 = disposalsWithExeDocs
                .Where(x => x.Code == TypeCheck.PlannedExit
                            || x.Code == TypeCheck.PlannedDocumentation
                            || x.Code == TypeCheck.PlannedDocumentationExit
                            || x.Code == TypeCheck.VisualSurvey)
                .Where(x => x.TypeBase == TypeBase.PlanJuridicalPerson)
                .Select(x => x.Stage.Id)
                .Distinct()
                .ToList();

            var dispStagesIds28_7 = disposalsWithExeDocs
                .Where(x => x.Code == TypeCheck.NotPlannedExit
                            || x.Code == TypeCheck.NotPlannedDocumentation
                            || x.Code == TypeCheck.NotPlannedDocumentationExit)
                .Where(x => x.TypeBase == TypeBase.PlanJuridicalPerson)
                .Select(x => x.Stage.Id)
                .ToList();

            var allDispStagesIds = dispStagesIds28_6.Union(dispStagesIds28_7).ToArray();

            start = 1000;

            var servResolutions =
                this.Container.Resolve<IDomainService<Resolution>>().GetAll();

            tmpFirst = allDispStagesIds.Length > start ? allDispStagesIds.Take(1000).ToArray() : allDispStagesIds;

            var resolutions =
                GetDocuments<Resolution>()
                    .Where(x => tmpFirst.Contains(x.Stage.Parent.Id))
                    .Select(x => new
                    {
                        resolutionId = x.Id,
                        parentStageId = x.Stage.Parent.Id,
                        sanctionCode = x.Sanction.Code,
                        executantCode = x.Executant.Code
                    })
                    .ToList();

            while (start < allDispStagesIds.Length)
            {
                var tmp = allDispStagesIds.Skip(start).Take(1000).ToArray();

                resolutions.AddRange(GetDocuments<Resolution>()
                    .Where(x => tmp.Contains(x.Stage.Parent.Id))
                    .Select(x => new
                    {
                        resolutionId = x.Id,
                        parentStageId = x.Stage.Parent.Id,
                        sanctionCode = x.Sanction.Code,
                        executantCode = x.Executant.Code
                    })
                    .ToList());

                start += 1000;
            }

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный арест. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 1, 3,7,8  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount28_6 =
                resolutions
                    .Where(x => dispStagesIds28_6.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "5")
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение =  Административный арест.
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 2, 4,9  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount28_7 =
                resolutions
                    .Where(x => dispStagesIds28_7.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "5")
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Предупреждение.
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 1, 3,7,8  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount32_6 =
                resolutions
                    .Where(x => dispStagesIds28_6.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "4")
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Предупреждение. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 2, 4,9  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount32_7 =
                resolutions
                    .Where(x => dispStagesIds28_7.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "4")
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 1, 3,7,8  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount33_6 =
                resolutions
                    .Where(x => dispStagesIds28_6.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 2, 4,9  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount33_7 =
                resolutions
                    .Where(x => dispStagesIds28_7.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            var executantCodesRow34 = new [] {"6", "7", "14"};
            var executantCodesRow35 = new [] {"1", "3", "5", "10", "12", "13", "16", "19"};
            var executantCodesRow37 = new [] {"0", "2", "4", "8", "9", "11", "15"};

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 1, 3,7,8  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount34_6 =
                resolutions
                    .Where(x => dispStagesIds28_6.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Where(x => executantCodesRow34.Contains(x.executantCode))
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 2, 4,9  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount34_7 =
                resolutions
                    .Where(x => dispStagesIds28_7.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Where(x => executantCodesRow34.Contains(x.executantCode))
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 1, 3,7,8  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount35_6 =
                resolutions
                    .Where(x => dispStagesIds28_6.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Where(x => executantCodesRow35.Contains(x.executantCode))
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 2, 4,9  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount35_7 =
                resolutions.Where(x => dispStagesIds28_7.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Where(x => executantCodesRow35.Contains(x.executantCode))
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 1, 3,7,8  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount37_6 =
                resolutions
                    .Where(x => dispStagesIds28_6.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Where(x => executantCodesRow37.Contains(x.executantCode))
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();

            // Количество Постановлений, у которых в  "Вид санкции" значение = Административный штраф. 
            // Учитывать Постановления, созданные из Распоряжений  с кодами  Видов проверок = 2, 4,9  и в поле "Основание обследования" значение = Плановая проверка юр.лица
            this.resolutionCount37_7 =
                resolutions
                    .Where(x => dispStagesIds28_7.Contains(x.parentStageId))
                    .Where(x => x.sanctionCode == "1")
                    .Where(x => executantCodesRow37.Contains(x.executantCode))
                    .Select(x => x.resolutionId)
                    .Distinct()
                    .Count();
        }
    }
}
