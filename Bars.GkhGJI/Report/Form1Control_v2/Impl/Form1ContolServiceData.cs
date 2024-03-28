namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Properties;

    using Castle.Windsor;

    // Данный класс служит сборкой для отчета Форма №1 - Контроль
    // Внимание Данная сборкаменяется в Томске 
    public class Form1ContolServiceData: IForm1ContolServiceData
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<ManagingOrganization> manOrgDomain { get; set; }
        public IDomainService<BaseJurPerson> baseJurPersonDomain { get; set; }
        public IDomainService<ActCheck> serviceActCheck { get; set; }
        public IDomainService<ActRemoval> serviceActRemoval { get; set; }
        public IDomainService<Prescription> servicePrescription { get; set; }
        public IDomainService<PrescriptionViol> servicePrescriptionViol { get; set; }
        public IDomainService<DocumentGjiChildren> serviceDocumentGjiChildren { get; set; }
        public IDomainService<ActRemovalViolation> serviceActRemovalViolation { get; set; }
        public IDomainService<ActCheckViolation> serviceActCheckViolation { get; set; }
        public IDomainService<ActCheckRealityObject> serviceActCheckRealityObject { get; set; }
        public IDomainService<Resolution> serviceResolution { get; set; }
        public IDomainService<ResolutionDispute> serviceResolutionDispute { get; set; }
        public IDomainService<ResolutionPayFine> serviceResolutionPayFine { get; set; }
        public IDomainService<DisposalExpert> serviceDisposalExpert { get; set; }
        public IDomainService<DocumentGji> serviceDocument { get; set; }

        protected List<TypeCheck> plannedList = new List<TypeCheck>
                                    {
                                        TypeCheck.PlannedExit,
                                        TypeCheck.PlannedDocumentation,
                                        TypeCheck.PlannedDocumentationExit
                                    };

        protected List<TypeCheck> unplannedList = new List<TypeCheck>
                                    {
                                        TypeCheck.NotPlannedExit,
                                        TypeCheck.NotPlannedDocumentation,
                                        TypeCheck.NotPlannedDocumentationExit
                                    };

        public virtual byte[] GetTemplate()
        {
            return Resources.Forma1_Kontrol;
        }

        public virtual Form1ContolData GetData(DateTime dateStart, DateTime dateEnd, List<long> municipalities)
        {
            var data = new Form1ContolData();

            /*
              Получаем по распоряжения в конечном статусе И попадающие по дате в интервал сборки и у которых есть не пустое основание
              PS: распоряжения с типом NullInspection могли быть в Б3 тоест ьраспоржения без оснвоания, в Б4 таког онет но их конвертировали в отчет их неберем 
            */
            var disposals = DisposalDomain.GetAll()
                         .Where(x => x.DocumentDate >= dateStart && x.DocumentDate <= dateEnd && x.TypeDisposal != TypeDisposalGji.NullInspection)
                         //.Where(x => x.State.FinalState)
                         .Select(x => new
                         {
                             disposalId = x.Id,
                             kindCheckCode = ((TypeCheck?)x.KindCheck.Code) ?? 0,
                             x.TypeDisposal,
                             typeBase = x.Inspection.TypeBase,
                             typeJurPerson = x.Inspection.TypeJurPerson,
                             x.Inspection.PersonInspection,
                             x.TypeAgreementProsecutor,
                             x.TypeAgreementResult,
                             contragentId = ((long?)x.Inspection.Contragent.Id) ?? -1,
                             typeDisposal = x.TypeDisposal,
                             typeAgreementProsecutor = x.TypeAgreementProsecutor,
                             typeAgreementResult = x.TypeAgreementResult,
                             stageId = x.Stage.Id
                         })
                         .ToArray();

            // получаем Id распоряжений которые являются Распоряжениями на првоерку предписания
            var prescriptionCheckDisposalIds = disposals.Where(x => x.typeDisposal == TypeDisposalGji.DocumentGji).Select(x => x.disposalId).ToArray();
            
            // простой словарь по ключу Id распоряжения
            var disposalsDict = disposals.ToDictionary(x => x.disposalId);

            // Получаем Id предписаний по которому созданы распоряжения
            var filteredDisposalsByPrescription = FilterDisposalsByPrescription(prescriptionCheckDisposalIds, municipalities);

            // поулчаем Id распоряжений которые не по предписанию (то есть основаные)
            var disposalsNotByPrescription = disposals.Where(x => x.typeDisposal != TypeDisposalGji.DocumentGji).Select(x => x.disposalId).ToList();

            var disposalActs = GetDisposalActData(data, municipalities, disposalsNotByPrescription, filteredDisposalsByPrescription);

            var disposalActsCount = disposalActs.ToDictionary(x => x.Key, x => x.Value.Count);

            var validDisposalIds = disposalActs.Keys.ToArray();

            var disposalsWithExperts = GetDisposalsWithExperts(validDisposalIds);

            var disposalResolutions = GetDisposalResolutions(validDisposalIds.ToDictionary(x => disposalsDict[x].stageId, x => x));

            var validDisposals = disposals
                .Where(x => validDisposalIds.Contains(x.disposalId))
                .Select(x => new
                {
                    x.disposalId,
                    x.contragentId,
                    x.kindCheckCode,
                    x.typeBase,
                    x.PersonInspection,
                    x.TypeAgreementProsecutor,
                    x.typeAgreementResult,
                    byPrescription = filteredDisposalsByPrescription.Contains(x.disposalId),
                    actCount = disposalActsCount.ContainsKey(x.disposalId) ? disposalActsCount[x.disposalId] : 0,
                    actList = disposalActs.ContainsKey(x.disposalId) ? disposalActs[x.disposalId] : new List<ActDataProxy>(),
                    hasActViolation = disposalActs.ContainsKey(x.disposalId) && disposalActs[x.disposalId].Any(y => y.hasViolation),
                    planned = plannedList.Contains(x.kindCheckCode),
                    unplanned = unplannedList.Contains(x.kindCheckCode),
                    hasExperts = disposalsWithExperts.Contains(x.disposalId),
                    resolutions = disposalResolutions.ContainsKey(x.disposalId) ? disposalResolutions[x.disposalId] : new List<ResolutionProxy>()
                }).ToArray();

            var disposalNotInspectionSurvey = validDisposals.Where(x => x.kindCheckCode != TypeCheck.InspectionSurvey).ToList();

            var row1_12CommonConditionList = disposalNotInspectionSurvey
                          .Where(
                              x =>
                              x.byPrescription
                              ||
                              x.typeBase == TypeBase.PlanJuridicalPerson
                              || ((x.typeBase == TypeBase.CitizenStatement || x.typeBase == TypeBase.ProsecutorsClaim
                                   || x.typeBase == TypeBase.DisposalHead)
                                  && x.PersonInspection == PersonInspection.Organization))
                          .ToList();

            data.cell1 = row1_12CommonConditionList
                          .Select(x => x.actCount)
                          .Sum();

            data.cell3 = disposalNotInspectionSurvey
               .Where(x => x.byPrescription)
               .Where(x => x.kindCheckCode != TypeCheck.InspectionSurvey)
               .Select(x => disposalActsCount.ContainsKey(x.disposalId) ? disposalActsCount[x.disposalId] : 0)
               .Sum();

            data.cell4 = disposalNotInspectionSurvey
                .Where(x => !x.byPrescription)
                .Where(x => x.typeBase == TypeBase.CitizenStatement && x.PersonInspection == PersonInspection.Organization)
                .Select(x => x.actCount)
                .Sum();

            data.cell5 = validDisposals
                .Where(x => x.unplanned)
                .Where(x => x.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
                .Select(x => x.actCount)
                .Sum();

            data.cell7 = data.cell4;

            data.cell9 = disposalNotInspectionSurvey
                .Where(x => !x.byPrescription)
                .Where(x => x.typeBase == TypeBase.DisposalHead && x.PersonInspection == PersonInspection.Organization)
                .Select(x => x.actCount)
                .Sum();

            data.cell10 = disposalNotInspectionSurvey
                .Where(x => !x.byPrescription)
                .Where(x => x.typeBase == TypeBase.ProsecutorsClaim && x.PersonInspection == PersonInspection.Organization)
                .Select(x => x.actCount)
                .Sum();

            var row12_13CommonList = row1_12CommonConditionList.Where(x => x.hasExperts).ToList();
            data.cell12 = row12_13CommonList.Select(x => x.actCount).Sum();

            data.cell13 = row12_13CommonList.Where(x => x.unplanned).Select(x => x.actCount).Sum();

            data.cell14 = row1_12CommonConditionList
                .Where(x => x.kindCheckCode == TypeCheck.PlannedDocumentation || x.kindCheckCode == TypeCheck.NotPlannedDocumentation)
                .Select(x => x.actCount)
                .Sum();

            data.cell15 = row1_12CommonConditionList
                .Where(x => x.kindCheckCode == TypeCheck.PlannedExit
                    || x.kindCheckCode == TypeCheck.NotPlannedExit
                    || x.kindCheckCode == TypeCheck.PlannedDocumentationExit
                    || x.kindCheckCode == TypeCheck.VisualSurvey
                    || x.kindCheckCode == TypeCheck.NotPlannedDocumentationExit)
                .Select(x => x.actCount)
                .Sum();

            var plannedInspDisposals = row1_12CommonConditionList.Where(x => x.planned).ToList();
            var unplannedInspDisposals = row1_12CommonConditionList.Where(x => x.unplanned).ToList();

            data.cell16_1 = plannedInspDisposals
                .Where(x => x.hasActViolation && x.contragentId > 0)
                .Select(x => x.contragentId)
                .Distinct()
                .Count();

            data.cell16_2 = unplannedInspDisposals
                .Where(x => x.hasActViolation && x.contragentId > 0)
                .Select(x => x.contragentId)
                .Distinct()
                .Count();

            data.cell19_1 = plannedInspDisposals
                .Select(x => x.actList.Count(y => y.hasViolation))
                .Sum();

            data.cell19_2 = unplannedInspDisposals
                .Select(x => x.actList.Count(y => y.hasViolation))
                .Sum();

            data.cell21_1 = plannedInspDisposals
                .Where(x => !x.byPrescription)
                .Select(x => x.actList.Sum(y => y.violationsCount))
                .Sum();

            data.cell21_2 = unplannedInspDisposals
                .Where(x => !x.byPrescription)
                .Select(x => x.actList.Sum(y => y.violationsCount))
                .Sum();

            // Данные по строке 23 высчитываются в ProcessRow23Data, так как логика иная

            data.cell24_1 = plannedInspDisposals
                .Select(x => x.actList.Count(y => y.hasDependentProtocol))
                .Sum();

            data.cell24_2 = unplannedInspDisposals
                .Select(x => x.actList.Count(y => y.hasDependentProtocol))
                .Sum();

            data.cell25_1 = plannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isAdmViolation))
                .Sum();

            data.cell25_2 = unplannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isAdmViolation))
                .Sum();

            data.cell29_1 = plannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isArest))
                .Sum();

            data.cell29_2 = unplannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isArest))
                .Sum();

            data.cell33_1 = plannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isWarning))
                .Sum();

            data.cell33_2 = unplannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isWarning))
                .Sum();

            data.cell35_1 = plannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isAdmPenalty && y.executantIsOfficialPerson))
                .Sum();

            data.cell35_2 = unplannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isAdmPenalty && y.executantIsOfficialPerson))
                .Sum();

            data.cell37_1 = plannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isAdmPenalty && y.executantIsJurPerson))
                .Sum();

            data.cell37_2 = unplannedInspDisposals
                .Select(x => x.resolutions.Count(y => y.isAdmPenalty && y.executantIsJurPerson))
                .Sum();

            data.cell39_1 = plannedInspDisposals
                .Select(x => x.resolutions.Where(y => y.isAdmPenalty && y.executantIsOfficialPerson).Sum(y => y.penaltyAmount.HasValue ? y.penaltyAmount.Value : 0))
                .Sum() / 1000;

            data.cell39_2 = unplannedInspDisposals
                .Select(x => x.resolutions.Where(y => y.isAdmPenalty && y.executantIsOfficialPerson).Sum(y => y.penaltyAmount.HasValue ? y.penaltyAmount.Value : 0))
                .Sum() / 1000;

            data.cell41_1 = plannedInspDisposals
                .Select(x => x.resolutions.Where(y => y.isAdmPenalty && y.executantIsJurPerson).Sum(y => y.penaltyAmount.HasValue ? y.penaltyAmount.Value : 0))
                .Sum() / 1000;

            data.cell41_2 = unplannedInspDisposals
                .Select(x => x.resolutions.Where(y => y.isAdmPenalty && y.executantIsJurPerson).Sum(y => y.penaltyAmount.HasValue ? y.penaltyAmount.Value : 0))
                .Sum() / 1000;

            var execCodesRow42 = new List<string> { "1", "10", "12", "13", "16", "19", "3", "5", "0", "11", "15", "18", "2", "4", "8", "9" };

            data.cell42_1 = plannedInspDisposals
                .SelectMany(x => x.resolutions)
                .Where(x => execCodesRow42.Contains(x.executantCode))
                .Sum(x => x.paidPenaltyAmount) / 1000;

            data.cell42_2 = unplannedInspDisposals
                .SelectMany(x => x.resolutions)
                .Where(x => execCodesRow42.Contains(x.executantCode))
                .Sum(x => x.paidPenaltyAmount) / 1000;

            var plannedDisposalResolutions = plannedInspDisposals
                .SelectMany(x => x.resolutions)
                .Where(x => x.executantIsOfficialPerson || x.executantIsJurPerson)
                .ToList();

            var unplannedDisposalResolutions = unplannedInspDisposals
                .SelectMany(x => x.resolutions)
                .Where(x => x.executantIsOfficialPerson || x.executantIsJurPerson)
                .ToList();

            data.cell46_1 = plannedDisposalResolutions.Count(x => x.disputeIsTypeRow46);

            data.cell46_2 = unplannedDisposalResolutions.Count(x => x.disputeIsTypeRow46);

            data.cell47_1 = plannedDisposalResolutions.Count(x => x.disputeIsTypeRow47);

            data.cell47_2 = unplannedDisposalResolutions.Count(x => x.disputeIsTypeRow47);

            data.cell48_1 = plannedDisposalResolutions.Count(x => x.disputeIsTypeRow48);

            data.cell48_2 = unplannedDisposalResolutions.Count(x => x.disputeIsTypeRow48);

            data.cell51 = row1_12CommonConditionList
                .Where(x => x.contragentId > 0)
                .Select(x => x.contragentId)
                .Distinct()
                .Count();

            var disposalsRequiresAgreement = validDisposals.Where(x => x.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement).ToList();
            data.cell54 = disposalsRequiresAgreement.Count;
            data.cell55 = disposalsRequiresAgreement.Count(x => x.typeAgreementResult == TypeAgreementResult.NotAgreed);
            data.cell56 = validDisposals.Where(x => x.hasExperts).Select(x => x.actCount).Sum();

            data.cell50 = manOrgDomain.GetAll().WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.Contragent.Municipality.Id))
                .Count(x => (x.TypeManagement == TypeManagementManOrg.UK
                               || x.TypeManagement == TypeManagementManOrg.TSJ
                               || x.TypeManagement == TypeManagementManOrg.JSK)
                            && x.ActivityGroundsTermination == GroundsTermination.NotSet);

            data.cell53 = manOrgDomain.GetAll().WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.Contragent.Municipality.Id))
                .Count(x => x.ActivityDateEnd >= dateStart
                    && x.ActivityDateEnd <= dateEnd
                    && (x.ActivityGroundsTermination == GroundsTermination.Liquidation || x.ActivityGroundsTermination == GroundsTermination.Bankruptcy));

            var inspectionYear = dateStart.Year;
            data.cell52 = baseJurPersonDomain.GetAll().WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.Contragent.Municipality.Id))
                         .Count(x => x.DateStart.HasValue && x.DateStart.Value.Year == inspectionYear);

            return data;
        }

        /// <summary>
        /// Распоряжения по предписаниям
        /// </summary>
        protected virtual IList<long> FilterDisposalsByPrescription(long[] disposalId, IList<long> municipalities)
        {
            var executantCodes = new[] { "0", "11", "15", "18", "2", "4", "8", "9" };

            // получаем по распоряжениям родителськие предписания
            var disposalsByPrescription = serviceDocumentGjiChildren.GetAll()
                                    .Where(x => disposalId.Contains(x.Children.Id))
                                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                                    .Select(x => new { parent = x.Parent.Id, child = x.Children.Id })
                                    .AsEnumerable()
                                    .Distinct()
                                    .Select(x => new ParentChildProxy { parent = x.parent, child = x.child })
                                    .ToList();


            var prescriptionIds = disposalsByPrescription.Select(x => x.parent).Distinct().ToArray();

            //В челябинске любят создавать дубликаты распоряжений по предписаниям
            //var prescriptionDisposalDict = disposalsByPrescription.ToDictionary(x => x.parent, x => x.child);
            Dictionary<long, long> prescriptionDisposalDict = new Dictionary<long, long>();
            disposalsByPrescription.ForEach(x =>
            {
                if (!prescriptionDisposalDict.ContainsKey(x.parent))
                {
                    prescriptionDisposalDict.Add(x.parent, x.child);
                }
                else
                {
                    var prescrId = x.parent;
                    //и как правило в ход идет распоряжение созданное последним
                    prescriptionDisposalDict[x.parent] = x.child;
                }
            }
            );

            // Фильтрация по исполнителю
            var exeCodeSatisfactoryPrescriptions = new List<long>();
            if (prescriptionIds.Any())
            {
               exeCodeSatisfactoryPrescriptions.AddRange(servicePrescription.GetAll()
                    .Where(x => prescriptionIds.Contains(x.Id))
                    .Where(x => executantCodes.Contains(x.Executant.Code))
                    .Select(x => x.Id)
                    .ToList());
            }

            // Фильтрация по МунОбр
            var municipalitySatisfactoryPrescriptions = new List<long>();

            if (exeCodeSatisfactoryPrescriptions.Any())
            {
                municipalitySatisfactoryPrescriptions.AddRange(servicePrescriptionViol.GetAll()
                                     .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                                     .Where(x => exeCodeSatisfactoryPrescriptions.Contains(x.Document.Id))
                                     .Select(x => x.Document.Id)
                                     .Distinct()
                                     .ToList());
            }

            return municipalitySatisfactoryPrescriptions.Select(x => prescriptionDisposalDict[x]).Distinct().ToList();
        }

        /// <summary>
        /// Заполнение строки 23
        /// </summary>
        protected virtual void ProcessRow23Data(Form1ContolData data, IList<long> actRemovalIds)
        {
            var actRemovalViolationList = new List<ActRemovalViolationProxy>();

            if (actRemovalIds.Any())
            {
                actRemovalViolationList.AddRange(serviceActRemovalViolation.GetAll()
                    .Where(x => actRemovalIds.Contains(x.Document.Id))
                    .Select(x => new
                    {
                        actId = x.Document.Id,
                        violationId = x.InspectionViolation.Violation.Id,
                        x.DateFactRemoval,
                        inspectiontId = x.InspectionViolation.Inspection.Id,
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.actId)
                    .Select(x => new ActRemovalViolationProxy
                    {
                        actRemovalId = x.Key,
                        violationsCount = x.Count(y => y.DateFactRemoval == null),
                        inspectionId = x.Select(y => y.inspectiontId).FirstOrDefault()
                    })
                    .ToList());
            }

            var voilationCountByInspection = actRemovalViolationList.GroupBy(x => x.inspectionId).ToDictionary(x => x.Key, x => x.Sum(y => y.violationsCount));

            var inspectionIds = actRemovalViolationList.Select(x => x.inspectionId).Distinct().ToList();

            var disposalsList = new List<DisposalProxy>();

            if (inspectionIds.Any())
            {
                disposalsList.AddRange(DisposalDomain.GetAll()

                    .Where(x => inspectionIds.Contains(x.Inspection.Id))
                    .Where(x => x.TypeDisposal == TypeDisposalGji.Base)
                    .Select(x => new DisposalProxy
                    {
                        id = x.Id,
                        kindCheckCode = ((TypeCheck?)x.KindCheck.Code) ?? 0,
                        inspectionId = x.Inspection.Id
                    })
                    .ToList());
            }

            var disposalDict = disposalsList.ToDictionary(x => x.id);

            var inspectionFirstDisposalDict = disposalsList
                .GroupBy(x => x.inspectionId)
                .ToDictionary(x => x.Key, x =>
                {
                    var disposalId = x.Select(y => y.id).First();
                    var disposalKindCheck = disposalDict[disposalId].kindCheckCode;
                    var violationsCount = voilationCountByInspection.ContainsKey(x.Key)
                                              ? voilationCountByInspection[x.Key]
                                              : 0;

                    return new { disposalKindCheck, violationsCount };
                });

            data.cell23_1 = inspectionFirstDisposalDict
                .Where(x => plannedList.Contains(x.Value.disposalKindCheck))
                .Select(x => x.Value.violationsCount)
                .Sum();

            data.cell23_2 = inspectionFirstDisposalDict
                .Where(x => unplannedList.Contains(x.Value.disposalKindCheck))
                .Select(x => x.Value.violationsCount)
                .Sum();
        }

        /// <summary>
        /// Акты проверки предписаний
        /// </summary>
        protected virtual Dictionary<long, List<ActDataProxy>> GetDisposalsActsRemoval(Form1ContolData data, IList<long> disposalIds)
        {
            var actDisposalList = new List<ParentChildProxy>();
            var dictDisposalStages = DisposalDomain.GetAll()
                              .Where(x => disposalIds.Contains(x.Id))
                              .Select(x => new { StageId = x.Stage.Id, x.Id })
                              .AsEnumerable()
                              .GroupBy(x => x.StageId)
                              .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());
            
            if (disposalIds.Any())
            {
                var dispStages = dictDisposalStages.Keys.ToList();

                // Тут получаю акты именно те которые находятся в группе распоряжения по Stage потому что 
                // последовательность создания Акта и Предписания в регионах может быть разная
                actDisposalList.AddRange(serviceActCheck.GetAll()
                    .Where(x => dispStages.Contains(x.Stage.Parent.Id))
                    //.Where(x => x.State.FinalState)
                    .Select(x => new { parentStageId = x.Stage.Parent.Id, actId = x.Id })
                    .AsEnumerable()
                    .Select(x => new ParentChildProxy { parent = dictDisposalStages[x.parentStageId], child = x.actId })
                    .Distinct()
                    .ToList());
            }

            var actIds = actDisposalList.Select(x => x.child).ToList();

            var actDisposalDict = actDisposalList.ToDictionary(x => x.child, x => x.parent);

            var actCheckActRemovalList = new List<ParentChildProxy>();
            if (actIds.Any())
            {
                // тут получаем по найденным актам его дочерние Акты устранения нарушений
                actCheckActRemovalList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => actIds.Contains(x.Parent.Id))
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                    //.Where(x => x.Children.State.FinalState)
                    .Select(x => new { parent = x.Parent.Id, child = x.Children.Id })
                    .AsEnumerable()
                    .Select(x => new ParentChildProxy { parent = x.parent, child = x.child })
                    .Distinct()
                    .ToList());
            }

            var actCheckByActremovalDict = actCheckActRemovalList.ToDictionary(x => x.child, x => x.parent);

            var actRemovalIds = actCheckByActremovalDict.Keys.ToList();

            this.ProcessRow23Data(data, actRemovalIds);

            var actRemovalList = new List<ActDataProxy>();
            if (actRemovalIds.Any())
            {
                actRemovalList.AddRange(serviceActRemoval.GetAll()
                    .Where(x => actRemovalIds.Contains(x.Id))
                    .Select(x => new ActDataProxy { actid = x.Id, hasViolation = x.TypeRemoval == YesNoNotSet.No })
                    .ToList());
            }

            var disposalsActsRemoval = actRemovalList
                .GroupBy(x => actDisposalDict[actCheckByActremovalDict[x.actid]])
                .ToDictionary(x => x.Key, x => x.ToList());

            return disposalsActsRemoval;
        }

        protected virtual Dictionary<long, long> GetActViolations(IList<long> actIds)
        {
            var actViolationList = new List<ActDataProxy>();
            if (actIds.Any())
            {
                actViolationList.AddRange(serviceActCheckViolation.GetAll()
                    .Where(x => actIds.Contains(x.ActObject.ActCheck.Id))
                    .Select(x => new { actId = x.ActObject.ActCheck.Id, violationId = x.InspectionViolation.Violation.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.actId)
                    .Select(x => new ActDataProxy { actid = x.Key, violationsCount = x.Count() })
                    .ToList());
            }

            var actViolationDict = actViolationList.ToDictionary(x => x.actid, x => x.violationsCount);

            return actViolationDict;
        }

        /// <summary>
        /// Акты распоряжений не по проверке предписания  
        /// </summary>
        protected virtual Dictionary<long, List<ActDataProxy>> GetDisposalActs(IList<long> municipalities, IList<long> disposalIds)
        {
            var actDisposalList = new List<ParentChildProxy>();

            var dictDisposalStages = DisposalDomain.GetAll()
                              .Where(x => disposalIds.Contains(x.Id))
                              .Select(x => new { StageId = x.Stage.Id, x.Id })
                              .AsEnumerable()
                              .GroupBy(x => x.StageId)
                              .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());

            if (disposalIds.Any())
            {
                var dispStages = dictDisposalStages.Keys.ToList();

                // Тут получаю акты именно те которые находятся в группе распоряжения по Stage потому что 
                // последовательность создания Акта и Предписания в регионах может быть разная

                actDisposalList.AddRange(serviceActCheck.GetAll()
                    .Where(x => dispStages.Contains(x.Stage.Parent.Id))
                    //.Where(x => x.State.FinalState)
                    .Select(x => new { parentStageId = x.Stage.Parent.Id, actId = x.Id })
                    .AsEnumerable()
                    .Select(x => new ParentChildProxy { parent = dictDisposalStages[x.parentStageId], child = x.actId })
                    .Distinct()
                    .ToList());
            }

            actDisposalList = actDisposalList.Distinct().ToList();

            var actIdList = actDisposalList.Select(x => x.child).Distinct().ToArray();
            var actHasViolationIds = new List<ActDataProxy>();

            if (actIdList.Any())
            {
                actHasViolationIds.AddRange(serviceActCheckRealityObject.GetAll()
                    .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => actIdList.Contains(x.ActCheck.Id))
                    .Select(x => new ActDataProxy
                    {
                        actid = x.ActCheck.Id,
                        hasViolation = x.HaveViolation == YesNoNotSet.Yes,
                        inspectionId = x.ActCheck.Inspection.Id
                    }));
            }

            var actDisposalDict = actDisposalList.ToDictionary(x => x.child, x => x.parent);

            var actsHasViolationDict = actHasViolationIds.GroupBy(x => x.actid).ToDictionary(x => x.Key, x => x.Any(y => y.hasViolation));

            var actsInspectionDict = actHasViolationIds.GroupBy(x => x.actid).ToDictionary(x => x.Key, x => x.Select(y => y.inspectionId).FirstOrDefault());

            var actViolations = this.GetActViolations(actIdList);

            var disposalActs = actsHasViolationDict.Keys
                .GroupBy(x => actDisposalDict[x])
                .ToDictionary(x => x.Key,
                    x => x.Select(y => new ActDataProxy
                    {
                        actid = y,
                        hasViolation = actsHasViolationDict[y],
                        violationsCount = actViolations.ContainsKey(y) ? actViolations[y] : 0,
                        inspectionId = actsInspectionDict.ContainsKey(y) ? actsInspectionDict[y] : 0L
                    })
                .ToList());

            return disposalActs;
        }

        /// <summary>
        /// Получение данных об актах распоряжений
        /// </summary>
        protected virtual Dictionary<long, List<ActDataProxy>> GetDisposalActData(Form1ContolData data, IList<long> municipalities, IList<long> disposalIdsNotByPrescription, IList<long> filteredDisposalsByPrescription)
        {
            // Акты для распоряжений не по предписаниям
            var res1 = GetDisposalActs(municipalities, disposalIdsNotByPrescription);

            // Акты проверки предписаний
            var res2 = GetDisposalsActsRemoval(data, filteredDisposalsByPrescription);

            var disposalActsDict = res1
                .Union(res2)
                .GroupBy(d => d.Key)
                .ToDictionary(d => d.Key, d => d.First().Value);

            var actIdList = disposalActsDict.SelectMany(x => x.Value.Select(y => y.actid).ToList()).ToArray();

            var disposalsByActs = disposalActsDict.Keys.ToList();

            var dictDisposalStages = DisposalDomain.GetAll()
                              .Where(x => disposalsByActs.Contains(x.Id))
                              .Select(x => new { StageId = x.Stage.Id, x.Id })
                              .AsEnumerable()
                              .GroupBy(x => x.StageId)
                              .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());

            var dispStages = dictDisposalStages.Keys.ToList();

            var actPrescriptionList = new List<ParentChildProxy>();

            if (dispStages.Any())
            {

                actPrescriptionList.AddRange(serviceDocument.GetAll()
                    .Where(x => dispStages.Contains(x.Stage.Parent.Id))
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Prescription)
                    //.Where(x => x.State.FinalState)
                    .Select(x => new { parentStageId = x.Stage.Parent.Id, x.Id })
                    .AsEnumerable()
                    .Select(x => new ParentChildProxy { parent = dictDisposalStages[x.parentStageId], child = x.Id })
                    .ToList());
            }

            var actsOfPrescriptionDict = actPrescriptionList.ToDictionary(x => x.child, x => x.parent);

            var prescriptionsWithProtocolList = new List<long>();
            var actPrescriptionIds = actsOfPrescriptionDict.Keys.ToArray();
            if (actPrescriptionIds.Any())
            {
                // тут нужно получить созданные из предписаний протокола 
                prescriptionsWithProtocolList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => actPrescriptionIds.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    //.Where(x => x.Children.State.FinalState)
                    .Select(x => x.Parent.Id)
                    .Distinct());
            }

            var actsWithProtocolList = prescriptionsWithProtocolList.Select(x => actsOfPrescriptionDict[x]).ToList();

            if (actIdList.Any())
            {
                // тут получем созданные из актов протокола
                actsWithProtocolList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => actIdList.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    //.Where(x => x.Children.State.FinalState)
                    .Select(x => x.Parent.Id)
                    .Distinct());
            }

            actsWithProtocolList = actsWithProtocolList.Distinct().ToList();

            disposalActsDict.ForEach(x => x.Value.ForEach(y => y.hasDependentProtocol = actsWithProtocolList.Contains(y.actid)));

            return disposalActsDict;
        }

        /// <summary>
        /// Постановления распоряжений
        /// </summary>
        protected virtual Dictionary<long, List<ResolutionProxy>> GetDisposalResolutions(Dictionary<long, long> disposalIdByStageDict)
        {
            var disposalsStages = disposalIdByStageDict.Keys.ToArray();

            var resolutions = new List<ResolutionProxy>();

            if (disposalsStages.Any())
            {
                resolutions.AddRange(serviceResolution.GetAll()
                    .Where(x => disposalsStages.Contains(x.Stage.Parent.Id))
                    //.Where(x => x.State.FinalState)
                    .Select(x => new ResolutionProxy
                    {
                        parentStageId = x.Stage.Parent.Id,
                        id = x.Id,
                        executantCode = x.Executant.Code ?? string.Empty,
                        penaltyAmount = x.PenaltyAmount,
                        sanctionCode = x.Sanction.Code ?? string.Empty,
                        disputeIsTypeRow46 = serviceResolutionDispute.GetAll().Any(y => y.Resolution.Id == x.Id && y.CourtVerdict.Code == "2" && y.Appeal == ResolutionAppealed.Law),
                        disputeIsTypeRow47 = serviceResolutionDispute.GetAll().Any(y => y.Resolution.Id == x.Id && y.CourtVerdict.Code == "2" && y.ProsecutionProtest),
                        disputeIsTypeRow48 = serviceResolutionDispute.GetAll().Any(y => y.Resolution.Id == x.Id && y.CourtVerdict.Code == "2" &&
                            (y.Appeal == ResolutionAppealed.RealityObjInspection
                            || y.Appeal == ResolutionAppealed.HeadInspection
                             || y.Appeal == ResolutionAppealed.SubHeadInspection))
                    }));
            }

            var resolutionDisposalStageDict = resolutions.ToDictionary(x => x.id, x => x.parentStageId);
            var resolutionIds = resolutionDisposalStageDict.Keys.ToArray();

            var resolutionPayFineList = new List<ResolutionPayFineProxy>();

            if (resolutionIds.Any())
            {
                resolutionPayFineList.AddRange(serviceResolutionPayFine.GetAll()
                    .Where(x => resolutionIds.Contains(x.Resolution.Id))
                    .Select(x => new ResolutionPayFineProxy
                    {
                        resolutionId = x.Resolution.Id,
                        paid = x.Amount.HasValue ? x.Amount.Value : 0
                    }));
            }

            var resolutionPayFineDict = resolutionPayFineList.GroupBy(x => x.resolutionId).ToDictionary(x => x.Key, x => x.Sum(y => y.paid));

            var OfficialPersonCodes = new[] { "1", "10", "12", "13", "16", "19", "3", "5" };
            var JuridicalPersonCodes = new[] { "0", "11", "15", "18", "2", "4", "8", "9" };

            var resolutionPreceed = resolutions.Select(
                    x =>
                    {
                        var res = new ResolutionProxy
                        {
                            parentStageId = x.parentStageId,
                            penaltyAmount = x.penaltyAmount,
                            paidPenaltyAmount = resolutionPayFineDict.ContainsKey(x.id) ? resolutionPayFineDict[x.id] : 0,
                            disputeIsTypeRow46 = x.disputeIsTypeRow46,
                            disputeIsTypeRow47 = x.disputeIsTypeRow47,
                            disputeIsTypeRow48 = x.disputeIsTypeRow48,
                            executantCode = x.executantCode
                        };

                        if (x.sanctionCode == "5")
                        {
                            res.isArest = true;
                            res.isAdmViolation = true;
                        }
                        else if (x.sanctionCode == "1")
                        {
                            res.isAdmPenalty = true;
                            res.isAdmViolation = true;
                        }
                        else if (x.sanctionCode == "4")
                        {
                            res.isWarning = true;
                            res.isAdmViolation = true;
                        }

                        if (OfficialPersonCodes.Contains(x.executantCode))
                        {
                            res.executantIsOfficialPerson = true;
                        }
                        else if (JuridicalPersonCodes.Contains(x.executantCode))
                        {
                            res.executantIsJurPerson = true;
                        }

                        return res;
                    })
                        .ToList();

            var disposalResolutions = resolutionPreceed
                .GroupBy(x => disposalIdByStageDict[x.parentStageId])
                .ToDictionary(x => x.Key, x => x.ToList());

            return disposalResolutions;
        }

        /// <summary>
        /// Фильтр распоряжний по наличию экспертов
        /// </summary>
        protected virtual IList<long> GetDisposalsWithExperts(IList<long> disposalIds)
        {
            var disposalsWithExperts = new List<long>();
  
            if (disposalIds.Any())
            {
                disposalsWithExperts.AddRange(serviceDisposalExpert.GetAll()
                    .Where(x => disposalIds.Contains(x.Disposal.Id))
                    .Where(x => x.Expert.Code != "23")
                    .Select(x => x.Disposal.Id)
                    .Distinct());
            }

            return disposalsWithExperts;
        }

        protected class DisposalProxy
        {
            public long id;
            public long inspectionId;
            public TypeCheck kindCheckCode;
        }

        protected class ActRemovalViolationProxy
        {
            public long actRemovalId;
            public long violationsCount;

            public long inspectionId;
        }

        protected class ActDataProxy
        {
            public long actid;
            public bool hasViolation;
            public bool hasDependentProtocol;
            public long violationsCount;
            public long inspectionId;
        }

        protected class PrescriptionProxy
        {
            public long id;
            public string executantCode;
        }

        protected class ResolutionPayFineProxy
        {
            public long resolutionId;
            public decimal paid;
        }

        protected class ResolutionProxy
        {
            public long id;
            public string executantCode;
            public string sanctionCode;
            public decimal? penaltyAmount;
            public long parentStageId;

            public decimal paidPenaltyAmount;

            public bool disputeIsTypeRow46;
            public bool disputeIsTypeRow47;
            public bool disputeIsTypeRow48;

            public bool isAdmPenalty;
            public bool isWarning;
            public bool isAdmViolation;
            public bool isArest;

            public bool executantIsJurPerson;
            public bool executantIsOfficialPerson;
        }

        protected struct ParentChildProxy
        {
            public long parent;
            public long child;
        }
    }
}
