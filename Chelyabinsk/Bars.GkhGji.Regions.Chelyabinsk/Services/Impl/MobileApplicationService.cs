namespace Bars.GkhGji.Regions.Chelyabinsk.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Bars.B4.Modules.Reports;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts;
    using Castle.Windsor;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncOperators;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts;
    using System.Security.Cryptography;
    using System.Text;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncDictionaries;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncInspections;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;
    using Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.InspectionRules;
    using DomainS = Bars.GkhGji.InspectionRules;

    using ActCheckToPrescriptionRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.ActCheckToPrescriptionRule;
    using DisposalToActCheckRule = Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules.DisposalToActCheckRule;
    using Bars.GkhGji.Regions.Chelyabinsk.Report;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.ActCheck;
    using Bars.Gkh.Report;
    using System.IO;
    using Bars.Gkh.StimulReport;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Report;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Сервис сведений об обращениях граждан
    /// </summary>
    public class MobileApplicationService : IMobileApplicationService
    {
        public MobileApplicationService()
        {
            this.Container = ApplicationContext.Current.Container;
        }
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container { get; set; }
        public IDomainService<Operator> OperatorDomain { get; set; }
        public IDomainService<UserPassword> UserPasswordDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }
        public IDomainService<ViolationGji> ViolationGjiDomain { get; set; }
        public IDomainService<ActionsRemovViol> ActionViolDomain { get; set; }
        public IDomainService<ArticleLawGji> ArticleLawGjiDomain { get; set; }
        public IDomainService<KindCheckGji> KindCheckGjiDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }
        public IDomainService<DocumentGjiChildren> ParentChildrenDomain { get; set; }
        public IDomainService<ActCheckRealityObject> ActCheckRealityObjectDomain { get; set; }
        public IDomainService<ChelyabinskDisposal> DisposalDomain { get; set; }
        public IDomainService<Decision> DecisionDomain { get; set; }
        public IDomainService<Prescription> PrescriptionDomain { get; set; }
        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<ActCheck> ActCheckDomain { get; set; }
        public IDomainService<ActCheckControlListAnswer> ActCheckCLAnswerDomain { get; set; }
        public IDomainService<ChelyabinskActRemoval> ActRemovalDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionRODomain { get; set; }
        public IDomainService<DocumentGji> DocumentGjiService { get; set; }
        public IDomainService<ViolationFeatureGji> ViolationFeatureGjiService { get; set; }
        public IDomainService<RealityObject> RealityObjectService { get; set; }
        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }
        public IDomainService<InspectionGjiViol> InspectionGjiViolDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<InspectionGji> InspectionService { get; set; }
        public IDomainService<ActCheckViolation> ActCheckViolService { get; set; }
        public IDomainService<ActRemovalViolation> ActRemovalViolService { get; set; }
        public IDomainService<PrescriptionViol> PrescriptionViolService { get; set; }
        public IDomainService<ProtocolViolation> ProtocolViolService { get; set; }
        public IDomainService<ChelyabinskDocumentLongText> LongTextService { get; set; }
        public IDomainService<ViolationActionsRemovGji> ViolActRemoLinks { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspInspService { get; set; }
        public IDomainService<ViolationFeatureGji> ViolGroupService { get; set; }
        public IDomainService<ExecutantDocGji> ExecutantDocGjiService { get; set; }
        public IDomainService<PhysicalPersonDocType> PhysicalPersonDocTypService { get; set; }
        public IDomainService<ControlList> ControlListDomain { get; set; }
        public IDomainService<ControlListQuestion> ControlListQuestionDomain { get; set; }
        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public OperatorGJIResponse GetOperatorsList(string token)
        {
            if (!ValidateToken(token))
            {
                return new OperatorGJIResponse
                {
                    RequestResult = RequestResult.IncorrectToken
                };
            }

            var operators = new List<OperatorGJI>();

            var operatorsList = OperatorDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    UserId = x.User.Id,
                    x.User.Login,
                    Inspector = x.Inspector != null ? new Inspector { Id = x.Inspector.Id, Fio = x.Inspector.Fio, Position = x.Inspector.Position, Active = x.Inspector.Active } : null,
                    x.User.Name,
                    x.ObjectEditDate
                }).ToList();


            operatorsList.ForEach(x =>
            {
                if (x.Inspector != null)
                    x.Inspector.Position = x.Inspector.Position + " " + ZonalInspInspService.GetAll().Where(y => y.Inspector.Id == x.Inspector.Id)
                        .Select(t => t.ZonalInspection.NameGenetive)
                        .FirstOrDefault();
            });


            var userInfoDict = UserPasswordDomain.GetAll()
                .Select(x => new
                {
                    x.User.Id,
                    x.Password
                })
                .AsEnumerable()
                .GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Select(v => v.Password).First());
            operatorsList.ForEach(obj =>
            {
                var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(obj.Login));
                var hashlogin = Convert.ToBase64String(hash);
                bool IsGJI = obj.Inspector != null;
                if (IsGJI)
                {
                    if(obj.Inspector.Active)
                    {
                        operators.Add(
                        new OperatorGJI
                        {
                            Id = obj.Id,
                            Login = hashlogin,
                            Password = userInfoDict.ContainsKey(obj.UserId) ? userInfoDict[obj.UserId] : "4565454545hghgjgjgjh",
                            Name = obj.Inspector.Fio,
                            IsGJI = IsGJI,
                            ObjectEditDate = obj.ObjectEditDate,
                            Position = obj.Inspector.Position
                        });
                    }
                }
            });


            OperatorGJIResponse result = new OperatorGJIResponse
            {
                RequestResult = RequestResult.NoErrors,
                OperatorsGJI = operators.ToArray()
            };


            return result;
        }

        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public DictionaryGJIResponse GetDictionaries(string token)
        {
            if (!ValidateToken(token))
            {
                return new DictionaryGJIResponse
                {
                    RequestResult = RequestResult.IncorrectToken
                };
            }

            List<DictionaryGJI> dictionariesList = new List<DictionaryGJI>();
            dictionariesList.Add(GetInspectors());
            dictionariesList.Add(GetViolations());
            dictionariesList.Add(GetArtLaws());
            dictionariesList.Add(GetKindCheck());
            dictionariesList.Add(GetActionsRemovViols());
            dictionariesList.Add(GetExecutantDocGji());
            dictionariesList.Add(GetPhysicalPersonDocType());
            dictionariesList.Add(GetControlLists());
            dictionariesList.Add(GetControlListQuestions());
            dictionariesList.Add(GetFeatureViolGji());
            dictionariesList.Add(GetViolGroups());

            return new DictionaryGJIResponse
            {
                RequestResult = RequestResult.NoErrors,
                DictionarysGJI = dictionariesList.ToArray()
            };
        }

        public RequestResult ImportInspectionResult(string token, long inspectorId, IncpectionGJIRequest request)
        {
            //lists of received docs 
            var inspectionGjis = new List<InspectionGji>();
            var actChecks = new Dictionary<long, ChelyabinskActCheck>();
            var actRemovals = new Dictionary<long, ChelyabinskActRemoval>();
            var prescriptions = new Dictionary<long, ChelyabinskPrescription>();
            var actCheckViolations = new List<ActCheckViolation>();
            var actCheckRealityObjects = new List<ActCheckRealityObject>();
            var actRemovalViolations = new List<ActRemovalViolation>();
            var prescriptionViolations = new List<PrescriptionViol>();
            var parentChildrenList = new List<DocumentGjiChildren>();
            var inspectionGjiViols = new List<InspectionGjiViol>();
            var longTexts = new List<ChelyabinskDocumentLongText>();
            var documentGjiInspectors = new List<DocumentGjiInspector>();
            var protocols = new Dictionary<long, ChelyabinskProtocol>();

            var inspector = this.OperatorDomain.GetAll().Where(x => x.Id == inspectorId).FirstOrDefault();

            if (!this.ValidateToken(token))
                return RequestResult.AuthorizationFailed;

            if (inspector == null || !inspector.Inspector.Active)
                return RequestResult.InspectorUnknown;

            try
            {
                //run on all inspections in request
                foreach (var inspection in request.IncpectionsGJI)
                {
                    var documentList = DocumentGjiService.GetAll()
                        .Where(y => y.Inspection.Id == inspection.Id).ToList();

                    var documentProxyList = inspection.DocumentsGJI.ToList();

                    // С августа 2023 решение меняется в мобилке, поэтому теперь тянем его тоже.
                    // В первую очередь ищем все акты. И только после сохранения актов ищем зависимые документы.
                    foreach (var doc in inspection.DocumentsGJI)
                    {
                        if (doc.TypeDocumentGji == TypeDocumentGji.Decision)
                        {
                            var decisionProxy = request.DocumentsGJIData.Decisions.ToList().FirstOrDefault(x => x.Id == doc.Id);
                            var existingDec = documentList.FirstOrDefault(x => x.Id == doc.Id);
                            var decision = this.DecisionDomain.GetAll()
                                .FirstOrDefault(x => x.Id == existingDec.Id);

                            decision.ObjectEditDate = DateTime.Now;
                            decision.DateStart = decisionProxy.DateStart;
                            decision.ObjectVisitStart = decisionProxy.ObjectVisitStart;
                            decision.ObjectVisitEnd = decisionProxy.ObjectVisitEnd;
                            decision.DocumentDate = decisionProxy.DocumentDate;
                            decision.KindKNDGJI = decisionProxy.KindKNDGJI;
                            decision.TypeDisposal = decisionProxy.TypeDisposalGji;
                            decision.TypeAgreementProsecutor = decisionProxy.TypeAgreementProsecutor;
                            decision.TypeAgreementResult = decisionProxy.TypeAgreementResult;
                            decision.KindCheck = KindCheckGjiDomain.Get(decisionProxy.KindCheckGjiId);

                            DecisionDomain.Update(decision);
                        }

                        if (doc.TypeDocumentGji == TypeDocumentGji.ActCheck)
                        {
                            var actProxy = request.DocumentsGJIData.ActChecks.ToList().FirstOrDefault(x => x.Id == doc.Id);
                            var existingact = documentList.FirstOrDefault(x => x.Id == doc.Id);
                            var actCheckIsExists = existingact != null;

                            if (actProxy == null)
                                continue;

                            var actCheckRule = new DecisionlToActCheckRule();
                            actCheckRule.Container = ApplicationContext.Current.Container;
                            DocumentGji parentAct = this.DocumentGjiService.GetAll().FirstOrDefault(x => x.Id == doc.ParentId);

                            //если такого акта не было в системе - то создали новый. 
                            var actCheck = this.ActCheckDomain.GetAll()
                                .FirstOrDefault(x => x.Id == (actCheckIsExists ? existingact.Id : this.GetDocId(actCheckRule.CreateDocument(parentAct))));

                            // И соответвтвенно нужно поменять Id родителя для зависимых документов
                            //протоколов и предписаний. Что бы иерархия не нарушилась.
                            //if (!actCheckIsExists)
                            //{
                            //    foreach (var inspDoc in inspection.DocumentsGJI)
                            //    {
                            //        if (inspDoc.ParentId == doc.Id)
                            //            inspDoc.ParentId = actCheck.Id;
                            //    }
                            //}

                            actCheck.ObjectEditDate = DateTime.Now;
                            actCheck.AcquaintedPerson = actProxy.AcquaintedPerson;
                            actCheck.AcquaintState = actProxy.AcquaintState;
                            actCheck.Area = actProxy.Area;
                            actCheck.DocumentPlace = actProxy.DocumentPlace;
                            actCheck.DocumentTime = actProxy.DocumentDateTime;
                            actCheck.DocumentDate = doc.DocumentDate;
                            actCheck.DocumentNumber = doc.DocumentNumber;
                            actCheck.DocumentYear = ((DateTime)doc.DocumentDate).Year;

                            //actCheck.SignatoryInspector = inspector.Inspector; (?)

                            //Создаем нарушения
                            var existingROs = this.ActCheckRealityObjectDomain.GetAll()
                                           .Where(x => x.ActCheck == actCheck).ToList();
                            var roProxyList = actProxy.RealityObjectViolations.ToList();
                            if (actProxy.HasViolations == YesNoNotSet.No)
                            {
                                foreach (var roProxy in roProxyList)
                                {
                                    var roAct = existingROs.FirstOrDefault(x => x.RealityObject.Id == roProxy.IncpectionGJIRealityObject.RoId);
                                    if (roAct != null)
                                    {
                                        roAct.HaveViolation = YesNoNotSet.No;
                                        this.ActCheckRealityObjectDomain.Update(roAct);
                                    }
                                    else
                                    {
                                        this.ActCheckRealityObjectDomain.Save(new ActCheckRealityObject
                                        {
                                            HaveViolation = YesNoNotSet.No,
                                            ActCheck = actCheck,
                                            Description = "Создано из мобильного приложения",
                                            RealityObject = new RealityObject { Id = roProxy.IncpectionGJIRealityObject.RoId }
                                        });
                                    }
                                }
                            }
                            else if (actProxy.HasViolations == YesNoNotSet.Yes)
                            {
                                foreach (var roProxy in roProxyList)
                                {
                                    var roAct = existingROs.FirstOrDefault(x => x.RealityObject.Id == roProxy.IncpectionGJIRealityObject.RoId);
                                    if (roAct != null)
                                    {
                                        roAct.HaveViolation = roProxy.ViolationId > 0 ? roAct.HaveViolation = YesNoNotSet.Yes : YesNoNotSet.No;
                                        InspectionGjiViol newInspViolation = new InspectionGjiViol
                                        {
                                            Inspection = new InspectionGji { Id = inspection.Id },
                                            RealityObject = new RealityObject { Id = roProxy.IncpectionGJIRealityObject.RoId },
                                            Violation = new ViolationGji { Id = roProxy.ViolationId },
                                            Description = roProxy.ViolationDescription
                                        };
                                        ActCheckViolation newViolation = new ActCheckViolation
                                        {
                                            ActObject = roAct,
                                            Document = new DocumentGji { Id = actCheck.Id },
                                            InspectionViolation = newInspViolation,
                                            TypeViolationStage = TypeViolationStage.Detection,
                                            DatePlanRemoval = roProxy.DatePlanRemoval,
                                            DateFactRemoval = roProxy.DateFactRemoval,
                                            DatePlanExtension = roProxy.DatePlanExtension
                                        };
                                        this.InspectionGjiViolDomain.Save(newInspViolation);
                                        this.ActCheckViolService.Save(newViolation);
                                        this.ActCheckRealityObjectDomain.Update(roAct);
                                    }
                                    else
                                    {
                                        this.ActCheckRealityObjectDomain.Save(new ActCheckRealityObject
                                        {
                                            HaveViolation = YesNoNotSet.No,
                                            ActCheck = actCheck,
                                            Description = "Создано из мобильного приложения",
                                            RealityObject = new RealityObject { Id = roProxy.IncpectionGJIRealityObject.RoId }
                                        });
                                    }
                                }
                            }

                            if (actCheck.Id > 0)
                                this.ActCheckDomain.Update(actCheck);
                            else
                                this.ActCheckDomain.Save(actCheck);

                            if (actProxy.ActCheckWitnessesproxy.Length > 0)
                            {
                                foreach (var witnessProxy in actProxy.ActCheckWitnessesproxy)
                                {
                                    this.ActCheckWitnessDomain.Save(new ActCheckWitness
                                    {
                                        ActCheck = actCheck,
                                        Fio = witnessProxy.Fio,
                                        Position = witnessProxy.Position,
                                        IsFamiliar = witnessProxy.IsFamiliar
                                    }
                                    );
                                }
                            }

                            try
                            {
                                if (actProxy.ActCheckAnswerProxy.Length > 0)
                                {
                                    var answersForDeleteIds = ActCheckCLAnswerDomain.GetAll().Where(x => x.ActCheck.Id == actCheck.Id).Select(x => x.Id).ToList();
                                    foreach (var answerId in answersForDeleteIds)
                                    {
                                        ActCheckCLAnswerDomain.Delete(answerId);
                                    }

                                    foreach (var answerProxy in actProxy.ActCheckAnswerProxy)
                                    {
                                        var thisQuestion = ControlListQuestionDomain.Get(answerProxy.ControlListQuestionId);
                                        if (thisQuestion != null)
                                        {
                                            this.ActCheckCLAnswerDomain.Save(new ActCheckControlListAnswer
                                            {
                                                ActCheck = actCheck,
                                                ControlListQuestion = thisQuestion,
                                                Question = thisQuestion.Name,
                                                NpdName = thisQuestion.NPDName,
                                                Description = thisQuestion.Description,
                                                YesNoNotApplicable = GetYesNoNotApplicable(answerProxy.YesNoNotApplicable)
                                            });
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                return new RequestResult
                                {
                                    Code = "badACAnswer",
                                    Message = "Упало в Answer-ах",
                                    Name = "BadACAnswer"
                                };
                            }

                            if (actCheckIsExists)
                            {
                                var existingLongText = this.LongTextService.GetAll().Where(x => x.DocumentGji == actCheck).FirstOrDefault();
                                if (existingLongText == null)
                                    this.LongTextService.Save(new ChelyabinskDocumentLongText
                                    {
                                        DocumentGji = actCheck,
                                        PersonViolationInfo = actProxy.PersonViolationInfo,
                                        PersonViolationActionInfo = actProxy.PersonViolationActionInfo,
                                        ViolationDescription = actProxy.ViolationDescription
                                    });
                                else
                                {
                                    existingLongText.PersonViolationInfo = actProxy.PersonViolationInfo;
                                    existingLongText.PersonViolationActionInfo = actProxy.PersonViolationActionInfo;
                                    existingLongText.ViolationDescription = actProxy.ViolationDescription;
                                    this.LongTextService.Update(existingLongText);
                                }
                            }
                        }
                    }

                    foreach (var doc in inspection.DocumentsGJI)
                    {
                        if (doc.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                        {
                            var actRemProxy = request.DocumentsGJIData.ActRemovals.ToList().FirstOrDefault(x => x.Id == doc.Id);
                            var existingActRem = documentList.FirstOrDefault(x => x.Id == doc.Id);
                            var actRemIsExists = existingActRem != null;
                            if (actRemProxy == null)
                                continue;
                            var proxyViolationsIds = actRemProxy.RealityObjectViolations.Select(x => x.Id).ToList();

                            DocumentGji parent = this.DocumentGjiService.GetAll().FirstOrDefault(x => x.Id == doc.ParentId);
                            var rule = new DecisionToActRemovalPrescriptionRule();
                            rule.Container = ApplicationContext.Current.Container;

                            //если такого акта не было в системе - то создали новый.
                            var actRem = this.ActRemovalDomain.GetAll()
                                .FirstOrDefault(x => x.Id == (actRemIsExists ? existingActRem.Id : this.GetDocId(rule.CreateDocument(parent))));

                            // И соответвтвенно нужно поменять Id родителя для зависимых документов
                            //протоколов и предписаний. Что бы иерархия не нарушилась.
                            if (!actRemIsExists)
                            {
                                foreach (var inspDoc in inspection.DocumentsGJI)
                                {
                                    if (inspDoc.ParentId == doc.Id)
                                        inspDoc.ParentId = actRem.Id;
                                }
                            }

                            actRem.ObjectEditDate = DateTime.Now;
                            actRem.Area = actRemProxy.Area;
                            actRem.DocumentDate = doc.DocumentDate;
                            actRem.DocumentNumber = doc.DocumentNumber;
                            actRem.DocumentYear = ((DateTime)doc.DocumentDate).Year;
                            actRem.TypeRemoval = actRemProxy.IsRemooved;
                            actRem.AcquaintedWithDisposalCopy = actRemProxy.AcquaintState == AcquaintState.Acquainted ? "Ознакомлен" : "Не ознакомлен";
                            actRem.DocumentPlace = actRemProxy.DocumentPlace;
                            actRem.DocumentTime = actRemProxy.DocumentDateTime;

                            if (actRem.Id > 0)
                                this.ActRemovalDomain.Update(actRem);
                            else
                                this.ActRemovalDomain.Save(actRem);

                            //Создаем нарушения
                            var roRemProxyList = actRemProxy.RealityObjectViolations.ToList();

                            var existingViolations = new Dictionary<long, InspectionGjiViol>();

                            //ищем все нарушения, которые пришли из мобилки и забиваем их в словарь
                            var inspViols = this.ActRemovalViolService.GetAll()
                                .Where(x => proxyViolationsIds.Contains(x.InspectionViolation.Id))
                                .Select(x => x.InspectionViolation).ToList();
                            foreach (var ispViol in inspViols)
                            {
                                if (existingViolations.ContainsKey(ispViol.Id))
                                    continue;
                                existingViolations.Add(ispViol.Id, ispViol);
                            }

                            //если нарушение есть в базе - обновляем его данные
                            //и создаем нарушение акта проверки исполнения (если такого нет. Конечно нет. Но проверим на всякий)
                            // если нарушения нет - то создаем его и  нарушение акта проверки исполнения
                            foreach (var remViolation in roRemProxyList)
                            {
                                InspectionGjiViol thisInspGjiViol;
                                if (existingViolations.ContainsKey(remViolation.Id))
                                {
                                    thisInspGjiViol = existingViolations[remViolation.Id];
                                    existingViolations[remViolation.Id].DatePlanRemoval = remViolation.DatePlanRemoval;
                                    existingViolations[remViolation.Id].DateFactRemoval = remViolation.DateFactRemoval;
                                    existingViolations[remViolation.Id].Description = remViolation.ViolationDescription;
                                    this.InspectionGjiViolDomain.Update(existingViolations[remViolation.Id]);
                                }
                                else
                                {
                                    thisInspGjiViol = new InspectionGjiViol
                                    {
                                        Inspection = this.InspectionService.GetAll().FirstOrDefault(x => x.Id == inspection.Id),
                                        RealityObject = this.RealityObjectService.GetAll()
                                            .FirstOrDefault(x => x.Id == remViolation.IncpectionGJIRealityObject.RoId),
                                        Violation = this.ViolationGjiDomain.GetAll().FirstOrDefault(x => x.Id == remViolation.ViolationId),
                                        DatePlanRemoval = remViolation.DatePlanRemoval,
                                        DateFactRemoval = remViolation.DateFactRemoval,
                                        Description = remViolation.ViolationDescription
                                    };
                                    this.InspectionGjiViolDomain.Save(thisInspGjiViol);
                                }

                                var existingActRemViol = this.ActRemovalViolService.GetAll()
                                    .FirstOrDefault(x => x.Document.Id == actRem.Id && x.InspectionViolation.Id == remViolation.Id);
                                if (existingActRemViol == null)
                                    this.ActRemovalViolService.Save(new ActRemovalViolation
                                    {
                                        Document = actRem,
                                        InspectionViolation = thisInspGjiViol,
                                        DatePlanRemoval = remViolation.DatePlanRemoval,
                                        DateFactRemoval = remViolation.DateFactRemoval,
                                        DatePlanExtension = remViolation.DatePlanExtension
                                    });
                                else
                                {
                                    existingActRemViol.DatePlanRemoval = remViolation.DatePlanRemoval;
                                    existingActRemViol.DateFactRemoval = remViolation.DateFactRemoval;
                                    existingActRemViol.DatePlanExtension = remViolation.DatePlanExtension;
                                    this.ActRemovalViolService.Update(existingActRemViol);
                                }
                            }

                            if (actRemProxy.ActCheckWitnessesproxy.Length > 0)
                            {
                                foreach (var witnessProxy in actRemProxy.ActCheckWitnessesproxy)
                                {
                                    this.ActRemovalWitnessDomain.Save(new ActRemovalWitness()
                                    {
                                        ActRemoval = actRem,
                                        Fio = witnessProxy.Fio,
                                        Position = witnessProxy.Position
                                    }
                                    );
                                }
                            }

                        }
                    }

                    foreach (var doc in inspection.DocumentsGJI)
                    {
                        if (doc.TypeDocumentGji == TypeDocumentGji.Prescription)
                        {
                            var prescrProxy = request.DocumentsGJIData.Prescriptions.ToList().FirstOrDefault(x => x.Id == doc.Id);
                            var existingPrescr = documentList.FirstOrDefault(x => x.Id == doc.Id);
                            var prescrIsExists = existingPrescr != null;
                            if (prescrProxy == null)
                                continue;
                            var proxyPrescrViolationsIds = prescrProxy.RealityObjectViolations.Select(x => x.Id).ToList();

                            Prescription prescr;
                            if (prescrIsExists)
                            {
                                prescr = this.PrescriptionDomain.Get(existingPrescr.Id);
                            }
                            else
                            {
                                var parentPrescr = this.DocumentGjiService.GetAll().FirstOrDefault(x => x.Id == doc.ParentId);

                                if (parentPrescr.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                {
                                    var prescrRule = new ActCheckToPrescriptionRule();
                                    prescrRule.Container = Container;
                                    prescrRule.ChelyabinskPrescriptionDomain = this.Container.ResolveDomain<ChelyabinskPrescription>();
                                    prescrRule.ChelyabinskActCheckDomain = this.Container.ResolveDomain<ChelyabinskActCheck>();
                                    prescrRule.InspectionStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();
                                    prescrRule.InspectionViolDomain = this.Container.ResolveDomain<InspectionGjiViol>();
                                    prescrRule.PrescriptionViolDomain = this.Container.ResolveDomain<PrescriptionViol>();
                                    prescrRule.ViolationActionDomain = this.Container.ResolveDomain<ViolationActionsRemovGji>();
                                    prescrRule.ChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
                                    prescrRule.ActSurveyRoDomain = this.Container.ResolveDomain<ActSurveyRealityObject>();
                                    prescrRule.DocumentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
                                    prescrRule.PrescriptionDomain = this.Container.ResolveDomain<Prescription>();

                                    var resp = prescrRule.CreateDocument(parentPrescr);
                                    prescr = this.PrescriptionDomain.Get(this.GetDocId(resp));
                                }
                                else
                                {
                                    var prescrRule = new DomainS.ActRemovalToPrescriptionRule();
                                    prescrRule.Container = ApplicationContext.Current.Container;
                                    prescrRule.PrescriptionViolDomain = this.Container.ResolveDomain<PrescriptionViol>();
                                    prescrRule.InspectionStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();
                                    prescrRule.PrescriptionDomain = this.Container.ResolveDomain<Prescription>();
                                    prescrRule.DocumentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
                                    prescrRule.ActSurveyRoDomain = this.Container.ResolveDomain<ActSurveyRealityObject>();
                                    prescrRule.ChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
                                    prescrRule.InspectionViolDomain = this.Container.ResolveDomain<InspectionGjiViol>();
                                    prescrRule.ViolationActionDomain = this.Container.ResolveDomain<ViolationActionsRemovGji>();
                                    prescr = this.PrescriptionDomain.Get(this.GetDocId(
                                        prescrRule.CreateDocument(parentPrescr)
                                    ));
                                }
                            }

                            prescr.ObjectEditDate = DateTime.Now;
                            prescr.DocumentDate = doc.DocumentDate;
                            prescr.DocumentNumber = doc.DocumentNumber;
                            prescr.DocumentYear = ((DateTime)doc.DocumentDate).Year;

                            this.PrescriptionDomain.Update(prescr);


                            //Создаем нарушения
                            var roPrescrProxyList = prescrProxy.RealityObjectViolations.ToList();

                            var existingPrescrViolations = new Dictionary<long, InspectionGjiViol>();

                            //ищем все нарушения, которые пришли из мобилки и забиваем их в словарь
                            existingPrescrViolations = this.PrescriptionViolService.GetAll()
                                    .Where(x => proxyPrescrViolationsIds.Contains(x.InspectionViolation.Id))
                                    .Select(x => x.InspectionViolation)
                                    .ToDictionary(x => x.Id, y => y);

                            //если нарушение есть в базе - обновляем его данные
                            //и создаем нарушение предписания (если такого нет. Конечно нет. Но проверим на всякий)
                            // если нарушения нет - то создаем его и  нарушение предписания
                            foreach (var prescrViolation in roPrescrProxyList)
                            {
                                InspectionGjiViol thisInspGjiViol;
                                if (existingPrescrViolations.ContainsKey(prescrViolation.Id))
                                {
                                    thisInspGjiViol = existingPrescrViolations[prescrViolation.Id];
                                    existingPrescrViolations[prescrViolation.Id].DatePlanRemoval = prescrViolation.DatePlanRemoval;
                                    existingPrescrViolations[prescrViolation.Id].DateFactRemoval = prescrViolation.DateFactRemoval;
                                    existingPrescrViolations[prescrViolation.Id].Description = prescrViolation.ViolationDescription;
                                    this.InspectionGjiViolDomain.Update(existingPrescrViolations[prescrViolation.Id]);
                                }
                                else
                                {
                                    thisInspGjiViol = new InspectionGjiViol
                                    {
                                        Inspection = this.InspectionService.GetAll().FirstOrDefault(x => x.Id == inspection.Id),
                                        RealityObject = this.RealityObjectService.GetAll()
                                            .FirstOrDefault(x => x.Id == prescrViolation.IncpectionGJIRealityObject.RoId),
                                        Violation = this.ViolationGjiDomain.GetAll().FirstOrDefault(x => x.Id == prescrViolation.ViolationId),
                                        DatePlanRemoval = prescrViolation.DatePlanRemoval,
                                        DateFactRemoval = prescrViolation.DateFactRemoval,
                                        Description = prescrViolation.ViolationDescription,

                                    };
                                    this.InspectionGjiViolDomain.Save(thisInspGjiViol);
                                }

                                var existingPrescrViol = this.PrescriptionViolService.GetAll()
                                    .FirstOrDefault(x => x.Document.Id == prescr.Id && x.InspectionViolation.Id == prescrViolation.Id);
                                if (existingPrescrViol == null)
                                    this.PrescriptionViolService.Save(new PrescriptionViol
                                    {
                                        Document = prescr,
                                        InspectionViolation = thisInspGjiViol,
                                        DatePlanRemoval = prescrViolation.DatePlanRemoval,
                                        DateFactRemoval = prescrViolation.DateFactRemoval,
                                        DatePlanExtension = prescrViolation.DatePlanExtension,
                                        TypeViolationStage = TypeViolationStage.InstructionToRemove,
                                        Action = prescrViolation.Action
                                    });
                                else
                                {
                                    existingPrescrViol.DatePlanRemoval = prescrViolation.DatePlanRemoval;
                                    existingPrescrViol.DateFactRemoval = prescrViolation.DateFactRemoval;
                                    existingPrescrViol.DatePlanExtension = prescrViolation.DatePlanExtension;
                                    this.PrescriptionViolService.Update(existingPrescrViol);
                                }
                            }
                        }

                        else if (doc.TypeDocumentGji == TypeDocumentGji.Protocol)
                        {
                            var protProxy = request.DocumentsGJIData.Protocols.ToList().FirstOrDefault(x => x.Id == doc.Id);
                            var existingProt = documentList.FirstOrDefault(x => x.Id == doc.Id);
                            var protIsExists = existingProt != null;
                            if (protProxy == null)
                                continue;
                            var proxyProtViolationsIds = protProxy.RealityObjectViolations.Select(x => x.Id).ToList();

                            Protocol prot;
                            if (protIsExists)
                            {
                                prot = this.ProtocolDomain.Get(existingProt.Id);
                            }
                            else
                            {
                                var parentPrescr = this.DocumentGjiService.GetAll().FirstOrDefault(x => x.Id == doc.ParentId);

                                if (parentPrescr.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                {
                                    var protRule = new ActCheckToProtocolRule { Container = ApplicationContext.Current.Container };
                                    protRule.ProtocolDomain = this.ProtocolDomain;
                                    protRule.InspectionStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();
                                    protRule.DocumentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
                                    protRule.ActSurveyRoDomain = this.Container.ResolveDomain<ActSurveyRealityObject>();
                                    protRule.ChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
                                    protRule.InspectionViolDomain = this.Container.ResolveDomain<InspectionGjiViol>();
                                    protRule.ProtocolViolationDomain = this.Container.ResolveDomain<ProtocolViolation>();
                                    prot = this.ProtocolDomain.Get(this.GetDocId(
                                        protRule.CreateDocument(parentPrescr)
                                    ));
                                }
                                else
                                {
                                    var protRule = new ActRemovalToProtocolRule { Container = ApplicationContext.Current.Container };
                                    protRule.ProtocolDomain = this.ProtocolDomain;
                                    protRule.InspectionStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();
                                    protRule.DocumentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
                                    protRule.ActSurveyRoDomain = this.Container.ResolveDomain<ActSurveyRealityObject>();
                                    protRule.ChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
                                    protRule.InspectionViolDomain = this.Container.ResolveDomain<InspectionGjiViol>();
                                    protRule.ProtocolViolationDomain = this.Container.ResolveDomain<ProtocolViolation>();
                                    prot = this.ProtocolDomain.Get(this.GetDocId(
                                        protRule.CreateDocument(parentPrescr)
                                    ));
                                }
                            }

                            prot.ObjectEditDate = DateTime.Now;
                            prot.DocumentDate = doc.DocumentDate;
                            prot.DocumentNumber = doc.DocumentNumber;
                            prot.DocumentYear = ((DateTime)doc.DocumentDate).Year;
                            prot.Inspection = this.InspectionService.GetAll().FirstOrDefault(x => x.Id == inspection.Id);
                            prot.TypeDocumentGji = TypeDocumentGji.Protocol;
                            prot.Executant = protProxy.ExecutantId > 0 ? new ExecutantDocGji { Id = protProxy.ExecutantId } : null;
                            prot.Contragent = protProxy.ContragentProxy?.Id > 0 ? new Contragent { Id = protProxy.ContragentProxy.Id } : null;
                            prot.PhysicalPerson = protProxy.PhysicalPerson;
                            prot.PhysicalPersonInfo = protProxy.PhysicalPersonInfo;
                            prot.PhysicalPersonDocType = protProxy.PhysicalPersonDocTypeId > 0 ? new PhysicalPersonDocType { Id = protProxy.PhysicalPersonDocTypeId } : null;
                            prot.PhysicalPersonDocumentNumber = protProxy.PhysicalPersonDocumentNumber;
                            prot.PhysicalPersonDocumentSerial = protProxy.PhysicalPersonDocumentSerial;
                            prot.PhysicalPersonIsNotRF = protProxy.PhysicalPersonIsNotRF;
                            prot.DateToCourt = protProxy.DateToCourt;
                            prot.ToCourt = protProxy.ToCourt;
                            prot.Description = protProxy.Description;
                            prot.DateOfProceedings = protProxy.DateOfProceedings;
                            prot.HourOfProceedings = protProxy.HourOfProceedings;
                            prot.MinuteOfProceedings = protProxy.MinuteOfProceedings;
                            prot.PersonFollowConversion = protProxy.PersonFollowConversion;
                            prot.FormatPlace = protProxy.FormatPlace;
                            prot.FormatHour = protProxy.FormatHour;
                            prot.FormatMinute = protProxy.FormatMinute;
                            prot.NotifDeliveredThroughOffice = protProxy.NotifDeliveredThroughOffice;
                            prot.FormatDate = protProxy.FormatDate;
                            prot.NotifNumber = protProxy.NotifNumber;
                            prot.ProceedingCopyNum = protProxy.ProceedingCopyNum;
                            prot.ProceedingsPlace = protProxy.ProceedingsPlace;
                            prot.Remarks = protProxy.Remarks;
                            prot.UIN = protProxy.UIN;
                            this.ProtocolDomain.Update(prot);


                            //Создаем нарушения
                            var roprotProxyList = protProxy.RealityObjectViolations.ToList();

                            var existingProtViolations = new Dictionary<long, InspectionGjiViol>();

                            //ищем все нарушения, которые пришли из мобилки и забиваем их в словарь
                            existingProtViolations = this.ProtocolViolService.GetAll()
                                    .Where(x => proxyProtViolationsIds.Contains(x.InspectionViolation.Id))
                                    .Select(x => x.InspectionViolation)
                                    .ToDictionary(x => x.Id, y => y);

                            //если нарушение есть в базе - обновляем его данные
                            //и создаем нарушение предписания (если такого нет. Конечно нет. Но проверим на всякий)
                            // если нарушения нет - то создаем его и  нарушение предписания
                            foreach (var protViolation in roprotProxyList)
                            {
                                InspectionGjiViol thisInspGjiViol;
                                if (existingProtViolations.ContainsKey(protViolation.Id))
                                {
                                    thisInspGjiViol = existingProtViolations[protViolation.Id];
                                    existingProtViolations[protViolation.Id].DatePlanRemoval = protViolation.DatePlanRemoval;
                                    existingProtViolations[protViolation.Id].DateFactRemoval = protViolation.DateFactRemoval;
                                    existingProtViolations[protViolation.Id].Description = protViolation.ViolationDescription;
                                    this.InspectionGjiViolDomain.Update(existingProtViolations[protViolation.Id]);
                                }
                                else
                                {
                                    thisInspGjiViol = new InspectionGjiViol
                                    {
                                        Inspection = this.InspectionService.GetAll().FirstOrDefault(x => x.Id == inspection.Id),
                                        RealityObject = this.RealityObjectService.GetAll()
                                            .FirstOrDefault(x => x.Id == protViolation.IncpectionGJIRealityObject.RoId),
                                        Violation = this.ViolationGjiDomain.GetAll().FirstOrDefault(x => x.Id == protViolation.ViolationId),
                                        DatePlanRemoval = protViolation.DatePlanRemoval,
                                        DateFactRemoval = protViolation.DateFactRemoval,
                                        Description = protViolation.ViolationDescription
                                    };
                                    this.InspectionGjiViolDomain.Save(thisInspGjiViol);
                                }

                                var existingProtViol = this.PrescriptionViolService.GetAll()
                                    .FirstOrDefault(x => x.Document.Id == prot.Id && x.InspectionViolation.Id == protViolation.Id);
                                if (existingProtViol == null)
                                    this.ProtocolViolService.Save(new ProtocolViolation()
                                    {
                                        Document = prot,
                                        InspectionViolation = thisInspGjiViol,
                                        DatePlanRemoval = protViolation.DatePlanRemoval,
                                        DateFactRemoval = protViolation.DateFactRemoval,
                                        DatePlanExtension = protViolation.DatePlanExtension
                                    });
                                else
                                {
                                    existingProtViol.DatePlanRemoval = protViolation.DatePlanRemoval;
                                    existingProtViol.DateFactRemoval = protViolation.DateFactRemoval;
                                    existingProtViol.DatePlanExtension = protViolation.DatePlanExtension;
                                    this.ProtocolViolService.Update(existingProtViol);
                                }
                            }

                            //Добавляем статьи закона
                            protProxy.ArticleLawGjiProtProxyes.ForEach(x =>
                            {
                                this.ProtocolArticleLawDomain.Save(new ProtocolArticleLaw
                                {
                                    ArticleLaw = new ArticleLawGji { Id = x.ArticleLawGjiId },
                                    Protocol = prot,
                                    Description = x.Description
                                });
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RequestResult.DataNotFound;
            }

            return new RequestResult
            {
                Code = "ok",
                Message = "Получено",
                Name = "Александр"
            };
        }

        private long GetDocId(IDataResult result)
        {
            var rs = result.Data.ToString().Split(',');
            return long.Parse(rs[0].Substring(rs[0].IndexOf('=') + 2, rs[0].Length - rs[0].IndexOf('=') - 2));
        }

        public IncpectionGJIResponse GetInspections(string token, long operatorId)
        {
            try
            {
                var inspOperator = OperatorDomain.Get(operatorId);
                if (inspOperator == null || inspOperator.Inspector == null)
                {
                    return new IncpectionGJIResponse
                    {
                        RequestResult = RequestResult.InspectorUnknown,
                        DocumentsGJIData = null,
                        IncpectionsGJI = null
                    };
                }

                var inspector = inspOperator.Inspector;
                var inspectionList = this.DocumentInspectorDomain.GetAll()
                    .Where(x => x.Inspector == inspector)
                    .Where(x => x.DocumentGji.DocumentDate.HasValue && x.DocumentGji.DocumentDate.Value >= DateTime.Now.AddMonths(-4))
                    .Select(x => x.DocumentGji.Inspection.Id).Distinct().ToList();
                var actsWithResult = this.ActCheckRealityObjectDomain.GetAll()
                    .Where(x => inspectionList.Contains(x.ActCheck.Inspection.Id))
                    .Where(x => x.HaveViolation == Gkh.Enums.YesNoNotSet.No)
                    .Select(x => x.ActCheck.Id).Distinct().ToList();
                actsWithResult.AddRange(ActRemovalDomain.GetAll()
                    .Where(x => inspectionList.Contains(x.Inspection.Id))
                    .Where(x => x.TypeRemoval == Gkh.Enums.YesNoNotSet.Yes)
                    .Select(x => x.Id).Distinct().ToList()
                );

                //получаем решения c закрытыми актами по искомым проверкам, эти распоряжения будут исключены из передачи на планшет
                var decisionDocsWithActs = this.ParentChildrenDomain.GetAll()
                    .Where(x => x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck ||
                        x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval)
                    .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision)
                    .Where(x => x.Children.DocumentDate.HasValue && x.Children.DocumentDate.Value < DateTime.Now)
                    .Where(x => inspectionList.Contains(x.Children.Inspection.Id))
                    .Where(x => actsWithResult.Contains(x.Children.Id))
                    .Select(x => x.Parent.Id).Distinct().ToList();

                // получаем решения которые пойдут на мобильное приложение
                var decisions = this.DecisionDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue)
                    .Where(x => inspectionList.Contains(x.Inspection.Id))
                    .Where(x => !decisionDocsWithActs.Contains(x.Id)).ToList();

                //Списки прокси классов для респонса
                List<IncpectionGJIProxy> inspProxyList = new List<IncpectionGJIProxy>();
                List<DocumentsGJIData> documentsDataListList = new List<DocumentsGJIData>();
                List<DecisionProxy> decisionProxyList = new List<DecisionProxy>();
                List<ActCheckProxy> actCheckProxyList = new List<ActCheckProxy>();
                List<ActRemovalProxy> actRemovalProxyList = new List<ActRemovalProxy>();
                List<PrescriptionProxy> prescriptionProxyList = new List<PrescriptionProxy>();

                //бежим по проверкам в полученных решениях, которые предстоит отработать
                foreach (InspectionGji inpectionGji in decisions.Select(x => x.Inspection).Distinct().ToList())
                {
                    string baseInsp = GetBaseInspection(inpectionGji);
                    DateTime checkDate = inpectionGji.ObjectCreateDate.AddYears(1);
                    List<DocumentGJIProxy> documentsProxyList = new List<DocumentGJIProxy>();

                    //дома в проверке
                    List<IncpectionGJIRealityObject> InspectionROList = InspectionRODomain.GetAll()
                        .Where(x => x.Inspection == inpectionGji)
                        .Select(x => new IncpectionGJIRealityObject
                        {
                            Address = x.RealityObject.Address,
                            RoId = x.RealityObject.Id,
                            Area = x.RealityObject.AreaMkd.HasValue ? x.RealityObject.AreaMkd.Value : 0,
                            Id = x.Id,
                            MunicipalityName = x.RealityObject.Municipality.Name
                        }).ToList();

                    //бежим по всем решениям в проверке
                    foreach (Decision dec in decisions.Where(x => x.Inspection == inpectionGji).ToList())
                    {
                        //если проверка первичная - то есть по основанию, то есть не проверка исполнения предписания
                        if (dec.TypeDisposal == GkhGji.Enums.TypeDisposalGji.Base)
                        {
                            if (dec.TimeVisitStart.HasValue)
                            {
                                checkDate = dec.TimeVisitStart.Value < checkDate ? dec.TimeVisitStart.Value : checkDate;
                            }

                            //добавляем сведения о текущем распоряжении в общий список
                            decisionProxyList.Add(new DecisionProxy
                            {
                                DateStart = dec.DateStart,
                                BaseText = baseInsp,
                                Inspectors = GetInspectorsFromDocument(dec.Id),
                                IssuedDisposal = dec.IssuedDisposal != null
                                    ? new InspectorProxy
                                    { Id = dec.IssuedDisposal.Id, FIO = dec.IssuedDisposal.Fio, Position = dec.IssuedDisposal.Position }
                                    : null,
                                KindCheckGjiId = dec.KindCheck.Id,
                                ObjectVisitEnd = dec.ObjectVisitEnd,
                                ObjectVisitStart = dec.ObjectVisitStart,
                                //SurveyObjective = GetSurveyObjectivesForDisposal(dec.Id),
                                //SurveyPurpose = GetSurveyPurposesForDisposal(dec.Id),
                                TypeAgreementProsecutor = dec.TypeAgreementProsecutor,
                                Id = dec.Id,
                                TypeAgreementResult = dec.TypeAgreementResult,
                                AgreementResult = dec.ProcAprooveDate.HasValue
                                    ? $"Прокуратура Челябинской области, решение от {dec.ProcAprooveDate.Value.ToShortDateString()} N{dec.ProcAprooveNum}"
                                    : "",
                                TypeDisposalGji = dec.TypeDisposal,
                                PeriodCorrect = dec.PeriodCorrect,
                                KindKNDGJI = dec.KindKNDGJI
                            });

                            //добавляем документ в список документов проверки, он будет прикреплен к самой inspection
                            documentsProxyList.Add(new DocumentGJIProxy
                            {
                                Id = dec.Id,
                                DocumentNumber = dec.DocumentNumber,
                                DocumentDate = dec.DocumentDate.Value,
                                TypeDocumentGji = GkhGji.Enums.TypeDocumentGji.Decision,
                                ParentId = null
                            });

                            //проверяем наличие актов
                            var childrenAct = this.ParentChildrenDomain.GetAll()
                                .Where(x => x.Parent.Id == dec.Id)
                                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision)
                                .Where(x => x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck ||
                                    x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval)
                                .Select(x => x.Children).ToList();
                            if (childrenAct.Count > 0)
                            {
                                foreach (DocumentGji doc in childrenAct)
                                {
                                    //добавляем документ в список документов проверки, он будет прикреплен к самой inspection
                                    if (doc.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck)
                                    {
                                        var actCheck = ActCheckDomain.Get(doc.Id);

                                        //акт дочерний по отношению к распоряжению
                                        documentsProxyList.Add(new DocumentGJIProxy
                                        {
                                            Id = actCheck.Id,
                                            DocumentNumber = actCheck.DocumentNumber,
                                            DocumentDate = actCheck?.DocumentDate,
                                            TypeDocumentGji = GkhGji.Enums.TypeDocumentGji.ActCheck,
                                            ParentId = dec.Id
                                        });

                                        //добавляем сведения о акте в общий список
                                        actCheckProxyList.Add(new ActCheckProxy
                                        {
                                            Id = actCheck.Id,
                                            AcquaintedPerson = actCheck.AcquaintedPerson,
                                            AcquaintState = actCheck.AcquaintState.HasValue ? actCheck.AcquaintState.Value : AcquaintState.NotSet,
                                            ActCheckWitnessesproxy = GetActWinnesses(doc.TypeDocumentGji, doc.Id),
                                            ActCheckAnswerProxy = GetActCheckCLAnswers(doc.Id),
                                            Area = actCheck.Area,
                                            DocumentDateTime = GetDateTimeFromDates(actCheck.DocumentDate, actCheck.DocumentTime),
                                            DocumentPlace = actCheck.DocumentPlace,
                                            HasViolations = Gkh.Enums.YesNoNotSet.NotSet,
                                            IsEnded = false,
                                            RealityObjectViolations = null
                                        });
                                    }
                                    else if (doc.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval)
                                    {
                                        //этого не может быть, в этом случае ничего не добавляем
                                    }
                                }
                            }
                        }

                        //решение на проверку исполнения предписания
                        else if (dec.TypeDisposal == GkhGji.Enums.TypeDisposalGji.DocumentGji)
                        {
                            if (dec.TimeVisitStart.HasValue)
                            {
                                checkDate = dec.TimeVisitStart.Value < checkDate ? dec.TimeVisitStart.Value : checkDate;
                            }

                            //находим предписание
                            var prescriptiondoc = this.ParentChildrenDomain.GetAll()
                                .Where(x => x.Children.Id == dec.Id)
                                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                                .FirstOrDefault().Parent;
                            if (prescriptiondoc != null)
                            {
                                //добавляем сведения о текущем решении в общий список
                                decisionProxyList.Add(new DecisionProxy
                                {
                                    DateStart = dec.DateStart,
                                    BaseText =
                                        $"Проверка исполнения предписания {prescriptiondoc.DocumentNumber} от {(prescriptiondoc.DocumentDate.HasValue ? prescriptiondoc.DocumentDate.Value.ToShortDateString() : "")}",
                                    Inspectors = GetInspectorsFromDocument(dec.Id),
                                    IssuedDisposal = dec.IssuedDisposal != null
                                        ? new InspectorProxy
                                        { Id = dec.IssuedDisposal.Id, FIO = dec.IssuedDisposal.Fio, Position = dec.IssuedDisposal.Position }
                                        : null,
                                    KindCheckGjiId = dec.KindCheck.Id,
                                    ObjectVisitEnd = dec.ObjectVisitEnd,
                                    ObjectVisitStart = dec.ObjectVisitStart,
                                    //SurveyObjective = GetSurveyObjectivesForDisposal(dec.Id),
                                    //SurveyPurpose = GetSurveyPurposesForDisposal(dec.Id),
                                    TypeAgreementProsecutor = dec.TypeAgreementProsecutor,
                                    Id = dec.Id,
                                    AgreementResult = dec.ProcAprooveDate.HasValue
                                        ? $"Прокуратура Челябинской области, решение от {dec.ProcAprooveDate.Value.ToShortDateString()} N{dec.ProcAprooveNum}"
                                        : "",
                                    TypeAgreementResult = dec.TypeAgreementResult,
                                    TypeDisposalGji = dec.TypeDisposal
                                });

                                var prescription = this.PrescriptionDomain.Get(prescriptiondoc.Id);

                                //добавляем предписание в список документов проверки
                                documentsProxyList.Add(new DocumentGJIProxy
                                {
                                    Id = prescriptiondoc.Id,
                                    DocumentNumber = prescriptiondoc.DocumentNumber,
                                    DocumentDate = prescriptiondoc.DocumentDate.Value,
                                    TypeDocumentGji = TypeDocumentGji.Prescription,
                                    ParentId = null
                                });

                                //добавляем распоряжение с родительским предписанием в список документов проверки
                                documentsProxyList.Add(new DocumentGJIProxy
                                {
                                    Id = dec.Id,
                                    DocumentNumber = dec.DocumentNumber,
                                    DocumentDate = dec.DocumentDate.Value,
                                    TypeDocumentGji = TypeDocumentGji.Decision,
                                    ParentId = prescriptiondoc.Id
                                });

                                prescriptionProxyList.Add(new PrescriptionProxy
                                {
                                    Id = prescription.Id,
                                    Area = 0,
                                    ContragentProxy = prescription.Contragent != null
                                            ? new ContragentProxy
                                            {
                                                Id = prescription.Contragent.Id,
                                                Inn = prescription.Contragent.Inn,
                                                Name = prescription.Contragent.Name,
                                                Ogrn = prescription.Contragent.Ogrn
                                            }
                                            : null,
                                    DocumentPlace = "",
                                    PhysicalPerson = prescription.PhysicalPerson,
                                    PhysicalPersonInfo = prescription.PhysicalPersonInfo,
                                    RealityObjectViolations = GetPrescriptionViolation(prescription.Id)
                                });

                                //проверяем наличие актов
                                var childrenAct = this.ParentChildrenDomain.GetAll()
                                    .Where(x => x.Parent.Id == dec.Id)
                                    .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision)
                                    .Where(x => x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck ||
                                        x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval)
                                    .Select(x => x.Children).ToList();
                                if (childrenAct.Count > 0)
                                {
                                    foreach (DocumentGji doc in childrenAct)
                                    {
                                        //добавляем документ в список документов проверки, он будет прикреплен к самой inspection
                                        if (doc.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval)
                                        {
                                            var actRemoval = ActRemovalDomain.Get(doc.Id);

                                            //акт дочерний по отношению к распоряжению
                                            documentsProxyList.Add(new DocumentGJIProxy
                                            {
                                                Id = actRemoval.Id,
                                                DocumentNumber = actRemoval.DocumentNumber,
                                                DocumentDate = actRemoval.DocumentDate.Value,
                                                TypeDocumentGji = GkhGji.Enums.TypeDocumentGji.ActRemoval,
                                                ParentId = dec.Id
                                            });

                                            //добавляем сведения о акте в общий список
                                            actRemovalProxyList.Add(new ActRemovalProxy
                                            {
                                                Id = actRemoval.Id,
                                                AcquaintedPerson = actRemoval.AcquaintedWithDisposalCopy,
                                                AcquaintState = AcquaintState.NotSet,
                                                ActCheckWitnessesproxy = GetActWinnesses(doc.TypeDocumentGji, doc.Id),
                                                Area = actRemoval.Area,
                                                DocumentDateTime = GetDateTimeFromDates(actRemoval.DocumentDate, actRemoval.DocumentTime),
                                                DocumentPlace = actRemoval.DocumentPlace,
                                                IsRemooved = Gkh.Enums.YesNoNotSet.NotSet,
                                                IsEnded = false,
                                                RealityObjectViolations = null
                                            });
                                        }
                                        else if (doc.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck)
                                        {
                                            //Этого не может быть промежуток должен быть
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //такого не может быть, ничего не добавляем
                            }
                        }
                    }

                    //добавляем все в проверку
                    inspProxyList.Add(new IncpectionGJIProxy
                    {
                        BaseDocAttr = baseInsp,
                        CheckDate = checkDate,
                        ContragentProxy = inpectionGji.Contragent != null
                            ? new ContragentProxy
                            {
                                Id = inpectionGji.Contragent.Id,
                                Inn = inpectionGji.Contragent.Inn,
                                Name = inpectionGji.Contragent.Name,
                                Ogrn = inpectionGji.Contragent.Ogrn
                            }
                            : null,
                        DocumentsGJI = documentsProxyList.ToArray(),
                        Id = inpectionGji.Id,
                        InspectionNumber = inpectionGji.InspectionNumber,
                        KindKNDGJI = inpectionGji.KindKNDGJI,
                        Name = "Проверка",
                        PhysicalPerson = inpectionGji.PhysicalPerson,
                        PhysicalPersonInfo = inpectionGji.PhysicalPersonInfo,
                        RealityObjects = InspectionROList.ToArray(),
                        TypeBase = inpectionGji.TypeBase,
                        TypeJurPerson = inpectionGji.TypeJurPerson
                    });
                }

                //компануем данные
                var docdataresp = new DocumentsGJIData
                {
                    ActChecks = actCheckProxyList.ToArray(),
                    ActRemovals = actRemovalProxyList.ToArray(),
                    Decisions = decisionProxyList.ToArray(),
                    Prescriptions = prescriptionProxyList.ToArray()
                };

                return new IncpectionGJIResponse
                {
                    RequestResult = RequestResult.NoErrors,
                    DocumentsGJIData = docdataresp,
                    IncpectionsGJI = inspProxyList.ToArray()
                };
            }
            catch (Exception e)
            {
                return new IncpectionGJIResponse
                {
                    RequestResult = new RequestResult { Code = "404", Name = "Необработанное исключение", Message = e.Message },
                    DocumentsGJIData = null,
                    IncpectionsGJI = null
                };
            }
            finally
            {
            }
        }

        public GetReportResponse ActpPintedFormt(string Token, string reportCode, long docId)
        {
            if (!ValidateToken(Token))
            {
                return new GetReportResponse
                {
                    RequestResult = RequestResult.IncorrectToken
                };
            }
            var reports = this.Container.ResolveAll<IGkhBaseReport>();

            var report = reports.FirstOrDefault(x => x.Id == reportCode);
            if (report == null)
            {
                return new GetReportResponse
                {
                    RequestResult = RequestResult.DataNotFound
                };
            }
            var userParam = new UserParamsValues();
            userParam.AddValue("DocumentId", docId);
            report.SetUserParams(userParam);
            MemoryStream stream;
            var reportProvider = Container.Resolve<IGkhReportProvider>();
            if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
            {
                //Вот такой вот костыльный этот метод Все над опеределывать
                stream = (report as StimulReport).GetGeneratedReport();
                if (stream != null)
                {
                    return new GetReportResponse
                    {
                        RequestResult = RequestResult.NoErrors,
                        Base64String = ConvertToBase64(stream),
                        FileName = "report.docx"
                    };
                }
            }
            else
            {
                var reportParams = new ReportParams();
                report.PrepareReport(reportParams);

                // получаем Генератор отчета
                var generatorName = report.GetReportGenerator();

                stream = new MemoryStream();
                var generator = Container.Resolve<IReportGenerator>(generatorName);
                reportProvider.GenerateReport(report, stream, generator, reportParams);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(ms);
                StreamReader sr = new StreamReader(ms);
                string myStr = sr.ReadToEnd();
                var result = new GetReportResponse() { Base64String = myStr };
            }
            return null;
        }

        private static string ConvertToBase64(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                return Convert.ToBase64String(memoryStream.ToArray());
            }

            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }

        private string GetBaseInspection(InspectionGji inspection)
        {
            string result = string.Empty;
            switch (inspection.TypeBase)
            {
                case TypeBase.CitizenStatement:

                    var appcit = InspectionAppealCitsDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspection.Id).Where(x => x.AppealCits.DateFrom.HasValue).ToList();
                    if (appcit.Count > 0)
                    {
                        appcit.ForEach(x =>
                        {
                            result += $"Обращение № {x.AppealCits.NumberGji} от {x.AppealCits.DateFrom.Value.ToShortDateString()} \n";
                        });
                    }
                    else result = "Обращение не найдено";

                    break;

                case TypeBase.PlanJuridicalPerson:
                    result = "Проверка по плану мероприятий";
                    break;

                default:
                    result = "Не установленное основание";
                    break;
            }

            return result;
        }

        private RealityObjectViolation[] GetPrescriptionViolation(long id)
        {
            return PrescriptionViolService.GetAll()
                .Where(x => x.Document.Id == id)
                .Select(x => new RealityObjectViolation
                {
                    Id = x.InspectionViolation.Id,
                    IncpectionGJIRealityObject = GetInspectionRO(x.InspectionViolation.Id),
                    DateFactRemoval = x.DateFactRemoval,
                    DatePlanExtension = x.DatePlanExtension,
                    DatePlanRemoval = x.DatePlanRemoval,
                    ViolationId = x.InspectionViolation.Violation.Id
                }).ToArray();
        }

        private IncpectionGJIRealityObject GetInspectionRO(long id)
        {
            var inspViol = InspectionGjiViolDomain.Get(id);
            var relityObj = InspectionRODomain.GetAll()
                .Where(x => x.Inspection.Id == inspViol.Inspection.Id)
                .Where(x => x.RealityObject.Id == inspViol.RealityObject.Id)
                .FirstOrDefault();
            if (relityObj != null)
            {
                return new IncpectionGJIRealityObject
                {
                    Id = relityObj.Id,
                    RoId = relityObj.RealityObject.Id,
                    Address = relityObj.RealityObject.Address,
                    Area = relityObj.RealityObject.AreaMkd.HasValue ? relityObj.RealityObject.AreaMkd.Value : 0,
                    MunicipalityName = relityObj.RealityObject.Municipality.Name
                };
            }
            else return null;
        }

        private DateTime? GetDateTimeFromDates(DateTime? docDate, DateTime? docTime)
        {
            if (docDate.HasValue && docTime.HasValue)
            {
                return new DateTime(docDate.Value.Year, docDate.Value.Month, docDate.Value.Day, docTime.Value.Hour, docTime.Value.Minute, 0);
            }
            else if (docDate.HasValue)
            {
                return new DateTime(docDate.Value.Year, docDate.Value.Month, docDate.Value.Day, 12, 0, 0);
            }

            return null;
        }

        public IDomainService<ActCheckWitness> ActCheckWitnessDomain { get; set; }

        public IDomainService<ActRemovalWitness> ActRemovalWitnessDomain { get; set; }

        private ActCheckWitnessproxy[] GetActWinnesses(TypeDocumentGji typeDoc, long id)
        {
            List<ActCheckWitnessproxy> result = new List<ActCheckWitnessproxy>();
            switch (typeDoc)
            {
                case TypeDocumentGji.ActCheck:
                    result = ActCheckWitnessDomain.GetAll()
                        .Where(x => x.ActCheck.Id == id)
                        .Select(x => new ActCheckWitnessproxy
                        {
                            Id = x.Id,
                            Fio = x.Fio,
                            IsFamiliar = x.IsFamiliar,
                            Position = x.Position
                        }).ToList();
                    break;

                case TypeDocumentGji.ActSurvey:
                    result = ActRemovalWitnessDomain.GetAll()
                        .Where(x => x.ActRemoval.Id == id)
                        .Select(x => new ActCheckWitnessproxy
                        {
                            Id = x.Id,
                            Fio = x.Fio,
                            IsFamiliar = x.IsFamiliar,
                            Position = x.Position
                        }).ToList();
                    break;
            }

            return result.ToArray();
        }

        private ActCheckAnswerProxy[] GetActCheckCLAnswers(long docId)
        {
            List<ActCheckAnswerProxy> result = new List<ActCheckAnswerProxy>();

            result = ActCheckCLAnswerDomain.GetAll()
                        .Where(x => x.ActCheck.Id == docId)
                        .Select(x => new ActCheckAnswerProxy
                        {
                            Id = x.Id,
                            ActCheckId = x.ActCheck.Id,
                            ControlListQuestionId = x.ControlListQuestion.Id,
                            YesNoNotApplicable = SendYesNoNotApplicable(x.YesNoNotApplicable)
                        }).ToList();

            return result.ToArray();
        }

        private YesNoNotApplicable GetYesNoNotApplicable(short value)
        {
            switch (value)
            {
                case 10: return YesNoNotApplicable.Yes;
                case 20: return YesNoNotApplicable.No;
                case 30: return YesNoNotApplicable.NotApplicable;
                default: return YesNoNotApplicable.NotSet;
            }    
        }

        private short SendYesNoNotApplicable(YesNoNotApplicable value)
        {
            switch (value)
            {
                case YesNoNotApplicable.Yes: return 10;
                case YesNoNotApplicable.No: return 20;
                case YesNoNotApplicable.NotApplicable: return 30;
                default: return 0;
            }
        }

        //public IDomainService<DisposalSurveyObjective> DisposalSurveyObjectiveDomain { get; set; }

        //private string GetSurveyObjectivesForDisposal(long id)
        //{
        //    return DisposalSurveyObjectiveDomain.GetAll()
        //        .Where(x => x.Disposal.Id == id)
        //        .AggregateWithSeparator(x => x.SurveyObjective.Name, "; ");
        //}

        //public IDomainService<DisposalSurveyPurpose> DisposalSurveyPurposeDomain { get; set; }

        //private string GetSurveyPurposesForDisposal(long id)
        //{
        //    return DisposalSurveyPurposeDomain.GetAll()
        //        .Where(x => x.Disposal.Id == id)
        //        .AggregateWithSeparator(x => x.SurveyPurpose.Name, "; ");
        //}

        private InspectorProxy[] GetInspectorsFromDocument(long id)
        {
            return this.DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == id)
                .Select(x => new InspectorProxy
                {
                    Id = x.Inspector.Id,
                    FIO = x.Inspector.Fio,
                    Position = x.Inspector.Position
                }).ToArray();
        }

        private DictionaryGJI GetArtLaws()
        {
            var records = ArticleLawGjiDomain.GetAll()
                .Select(x => new DictionaryRecord
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Attr1 = x.Description,
                    Attr2 = x.Article,
                    ObjectEditDate = x.ObjectEditDate
                }).OrderBy(x => x.Name).ToArray();
            DictionaryGJI inspectorsDict = new DictionaryGJI
            {
                DictionaryCode = "3",
                DictionaryName = "Статьи закона",
                DictionaryRecords = records
            };

            return inspectorsDict;
        }

        private DictionaryGJI GetKindCheck()
        {
            var records = KindCheckGjiDomain.GetAll()
                .Select(x => new DictionaryRecord
                {
                    Id = x.Id,
                    Code = x.Code.ToString(),
                    Name = x.Name,
                    Attr1 = x.Description,
                    Attr2 = "",
                    ObjectEditDate = x.ObjectEditDate
                }).OrderBy(x => x.Name).ToArray();
            DictionaryGJI inspectorsDict = new DictionaryGJI
            {
                DictionaryCode = "4",
                DictionaryName = "Вид проверки",
                DictionaryRecords = records
            };

            return inspectorsDict;
        }

        private DictionaryGJI GetInspectors()
        {
            var records = InspectorDomain.GetAll()
                .Where(x => x.Active)
                .Select(x => new DictionaryRecord
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Fio,
                    Attr1 = Newtonsoft.Json.JsonConvert.SerializeObject(x),
                    Attr2 = ZonalInspInspService.GetAll().Where(y => y.Inspector.Id == x.Id)
                        .Select(t => t.ZonalInspection.NameGenetive)
                        .FirstOrDefault(),
                    ObjectEditDate = x.ObjectEditDate
                }).OrderBy(x => x.Name).ToArray();


            DictionaryGJI inspectorsDict = new DictionaryGJI
            {
                DictionaryCode = "1",
                DictionaryName = "Инспекторы",
                DictionaryRecords = records
            };

            return inspectorsDict;
        }

        private DictionaryGJI GetViolations()
        {
            var violations = ViolGroupService.GetAll()
                .Select(x => x.ViolationGji)
                .ToList();

            var records = violations
                .Select(x => new DictionaryRecord
                {
                    Id = x.Id,
                    Code = x.CodePin,
                    Name = x.Name,
                    Attr1 = x.NormativeDocNames,
                    Attr2 = x.Description,
                    ObjectEditDate = x.ObjectEditDate
                }).OrderBy(x => x.Attr1).ToArray();

            DictionaryGJI inspectorsDict = new DictionaryGJI
            {
                DictionaryCode = "2",
                DictionaryName = "Нарушения",
                DictionaryRecords = records
            };

            return inspectorsDict;
        }

        private DictionaryGJI GetActionsRemovViols()
        {
            var records = this.ActionViolDomain.GetAll()
                .Select(x => new DictionaryRecord
                {
                    Id = x.Id,

                    //Attr1 = string.Join(",",violActRemoLinks.GetAll().Where(y =>y.ActionsRemovViol.Id == x.Id).Select(t => t.ViolationGji.Id).ToArray()),
                    Name = x.Name,
                    ObjectEditDate = x.ObjectEditDate
                }).ToArray();

            records.ForEach(x =>
            {
                x.Attr1 = string.Join(",",
                    this.ViolActRemoLinks.GetAll().Where(y => y.ActionsRemovViol.Id == x.Id).Select(t => t.ViolationGji.Id).ToArray());
            });
            DictionaryGJI actionViolations = new DictionaryGJI
            {
                DictionaryCode = "5",
                DictionaryName = "Мероприятия по устранению нарушения",
                DictionaryRecords = records.Where(x => !string.IsNullOrEmpty(x.Attr1)).ToArray()
            };
            return actionViolations;
        }

        private DictionaryGJI GetExecutantDocGji()
        {
            var records = ExecutantDocGjiService.GetAll().Select(x =>
                new DictionaryRecord
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ObjectEditDate = x.ObjectEditDate
                }
            ).ToArray();

            DictionaryGJI executantDocsGji = new DictionaryGJI
            {
                DictionaryCode = "6",
                DictionaryName = "Типы исполнителя",
                DictionaryRecords = records
            };

            return executantDocsGji;
        }

        private DictionaryGJI GetPhysicalPersonDocType()
        {
            var records = PhysicalPersonDocTypService.GetAll().Select(x =>
                new DictionaryRecord
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ObjectEditDate = x.ObjectEditDate
                }
            ).ToArray();

            DictionaryGJI executantDocsGji = new DictionaryGJI
            {
                DictionaryCode = "7",
                DictionaryName = "Типы документа физ. лица",
                DictionaryRecords = records
            };

            return executantDocsGji;
        }

        private DictionaryGJI GetControlLists()
        {
            var records = ControlListDomain.GetAll().Select(x =>
                new DictionaryRecord
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ObjectEditDate = x.ObjectEditDate,
                    Attr1 = x.KindKNDGJI.ToString()
                }
            ).ToArray();

            DictionaryGJI controlListsGji = new DictionaryGJI
            {
                DictionaryCode = "8",
                DictionaryName = "Проверочные листы",
                DictionaryRecords = records
            };

            return controlListsGji;
        }

        private DictionaryGJI GetControlListQuestions()
        {
            var records = ControlListQuestionDomain.GetAll().Select(x =>
                new DictionaryRecord
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Attr1 = x.ControlList.Id.ToString(),
                    Attr2 = x.NPDName,
                    Attr3 = x.Description,
                    ObjectEditDate = x.ObjectEditDate
                }
            ).ToArray();

            DictionaryGJI controlListQuestionsGji = new DictionaryGJI
            {
                DictionaryCode = "9",
                DictionaryName = "Вопросы к проверочному листу",
                DictionaryRecords = records
            };

            return controlListQuestionsGji;
        }

        private DictionaryGJI GetFeatureViolGji()
        {
            var features = ViolGroupService.GetAll()
                .Select(x => x.FeatureViolGji)
                .DistinctBy(x => x.Id)
                .ToList();

            var records = features
                .Select(x => new DictionaryRecord
                {
                    Id = x.Id,
                    Code = x.Code ?? "",
                    Name = x.Name ?? "FeatureViolGji",
                    Attr1 = x.FullName ?? "",
                    Attr2 = x.QuestionCode ?? "",
                    ObjectEditDate = x.ObjectEditDate
                })
                .ToArray();

            DictionaryGJI featureViolsDict = new DictionaryGJI
            {
                DictionaryCode = "10",
                DictionaryName = "Группы нарушений",
                DictionaryRecords = records
            };

            return featureViolsDict;
        }

        private DictionaryGJI GetViolGroups()
        {
            var records = ViolGroupService.GetAll()
                .Select(x => new DictionaryRecord
                {
                    Id = x.Id,
                    Attr1 = x.ViolationGji != null ? x.ViolationGji.Id.ToString() : "0",
                    Attr2 = x.FeatureViolGji != null ? x.FeatureViolGji.Id.ToString() : "0",
                    ObjectEditDate = x.ObjectEditDate
                })
                .ToArray();

            DictionaryGJI violGroups = new DictionaryGJI
            {
                DictionaryCode = "11",
                DictionaryName = "Нарушения к группам",
                DictionaryRecords = records
            };

            return violGroups;
        }

        private string GetAccessToken()
        {
            var token = $"{DateTime.Now.ToString("dd.MM.yyyy")}_ANV_6966644";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            string srt = Convert.ToBase64String(hash);
            return Convert.ToBase64String(hash);
        }

        private bool ValidateToken(string check_token)
        {
            return this.GetAccessToken() == check_token;
        }
    }
}