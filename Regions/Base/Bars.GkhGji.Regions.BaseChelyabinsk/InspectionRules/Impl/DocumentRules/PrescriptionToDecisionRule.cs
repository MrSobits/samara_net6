namespace Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Rules;
    using Bars.GkhGji.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Решение на проверку предписания' из документа 'Предписание'
    /// </summary>
    public class PrescriptionToDecisionRule : IDocumentGjiRule
    {
        private const string SurveySubjectForDisposalCode = "III.";

        public IWindsorContainer Container { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "PrescriptionToDecisionRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Решение на проверку предписания' из документа 'Предписание'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Decision"; }
        }

        public virtual string ResultName
        {
            get { return "Решение на проверку предписания"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Prescription; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Decision; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // не ожидаем никаких входных параметров
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var DisposalDomain = this.Container.Resolve<IDomainService<Decision>>();
            var DisposalBaseDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var InspectionStageDomain = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var SurveySubjectDomain = this.Container.Resolve<IDomainService<SurveySubject>>();
            var DecisionInspectionReasonDomain = this.Container.Resolve<IDomainService<DecisionInspectionReason>>();
            var InspectionReasonDomain = this.Container.Resolve<IDomainService<InspectionReason>>();
            var DisposalVerificationSubjectDomain = this.Container.Resolve<IDomainService<DecisionVerificationSubject>>();
            var SurveyPurposeDomain = this.Container.Resolve<IDomainService<SurveyPurpose>>();
            var SurveyObjectiveDomain = this.Container.Resolve<IDomainService<SurveyObjective>>();
            var DecisionControlSubjectsDomain = this.Container.Resolve<IDomainService<DecisionControlSubjects>>();
          
            var DecisionAdminRegulationDomain = this.Container.Resolve<IDomainService<DecisionAdminRegulation>>();
            var DisposalAdminRegulationDomain = this.Container.Resolve<IDomainService<DisposalAdminRegulation>>();
            var DecisionProvidedDocDomain = this.Container.Resolve<IDomainService<DecisionProvidedDoc>>();
            var DisposalProvidedDocDomain = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var ProvidedDocDomain = this.Container.Resolve<IDomainService<ProvidedDocGji>>();

            try
            {
                #region Формируем этап проверки
                var stageMaxPosition = InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

                // Создаем Этап Проверки (Который показывается слева в дереве)
                var stage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                    TypeStage = TypeStage.DecisionPrescription
                };
                #endregion

                #region Формируем распоряжение распоряжение
                var disposal = new Decision()
                {
                    Inspection = document.Inspection,
                    TypeDisposal = TypeDisposalGji.DocumentGji,
                    TypeDocumentGji = TypeDocumentGji.Decision,
                    TypeAgreementProsecutor = TypeAgreementProsecutor.NotSet,
                    TypeAgreementResult = TypeAgreementResult.NotSet,
                    Stage = stage
                };
                #endregion

                #region Формируем связь с родителем
                var parentChildren = new DocumentGjiChildren
                {
                    Parent = document,
                    Children = disposal
                };
                #endregion

                #region Проставляем вид проверки
                var kindCheck = DisposalDomain.GetAll()
                     .FirstOrDefault(x => x.Inspection == document.Inspection)?.KindCheck;
                if (kindCheck != null)
                {
                    disposal.KindCheck = kindCheck;
                }
                else
                {
                    kindCheck = DisposalBaseDomain.GetAll()
                     .FirstOrDefault(x => x.Inspection == document.Inspection)?.KindCheck;
                    if (kindCheck != null)
                    {
                        disposal.KindCheck = kindCheck;
                    }
                }
                #endregion

                #region Забираем инспекторов из основания и переносим в Распоряжение
                var listInspectors = new List<DocumentGjiInspector>();
                var inspectorIds = DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

                foreach (var inspector in inspectorIds)
                {
                    var newInspector = new DocumentGjiInspector
                    {
                        DocumentGji = disposal,
                        Inspector = new Inspector {Id = inspector}
                    };

                    listInspectors.Add(newInspector);
                }
                #endregion

                #region преносим типы обследования, цели и задачи
                List<DecisionAdminRegulation> dispAdminRegulationList = new List<DecisionAdminRegulation>();
                List<DecisionControlSubjects> contrSubjList = new List<DecisionControlSubjects>();
                List<DecisionProvidedDoc> disposalProvidedDocs = new List<DecisionProvidedDoc>();

                //получаем по предписанию родительское распоряжение
                var parentDisposal = Utils.GetParentDocumentByType(ChildrenDomain, document, TypeDocumentGji.Decision);
                if (parentDisposal != null)
                {
                    //Административные регламенты
                    dispAdminRegulationList = DecisionAdminRegulationDomain.GetAll()
                        .Where(x => x.Decision.Id == parentDisposal.Id)
                        .Select(x => new DecisionAdminRegulation
                        {
                            Decision = disposal,
                            AdminRegulation = x.AdminRegulation,
                        })
                        .ToList();

                    //предоставляемые документы
                    disposalProvidedDocs = DecisionProvidedDocDomain.GetAll()
                         .Where(x => x.Decision.Id == parentDisposal.Id)
                        .Select(x => new DecisionProvidedDoc
                        {
                            Decision = disposal,
                            ProvidedDoc = x.ProvidedDoc,
                            Description = x.Description
                        })
                        .ToList();

                }
                else // похоже что основание было распорягой
                {
                    parentDisposal = Utils.GetParentDocumentByType(ChildrenDomain, document, TypeDocumentGji.Disposal);
                    if (parentDisposal != null)
                    {
                        //Административные регламенты
                        dispAdminRegulationList = DisposalAdminRegulationDomain.GetAll()
                            .Where(x => x.Disposal.Id == parentDisposal.Id)
                            .Select(x => new DecisionAdminRegulation
                            {
                                Decision = disposal,
                                AdminRegulation = x.AdminRegulation,
                            })
                            .ToList();
                        //Субъекты проверки
                        contrSubjList = DecisionControlSubjectsDomain.GetAll()
                           .Where(x => x.Decision.Id == parentDisposal.Id)
                           .Select(x => new DecisionControlSubjects
                           {
                               Decision = disposal,
                               Contragent = x.Contragent,
                               PersonInspection = x.PersonInspection,
                               PhysicalPerson = x.PhysicalPerson,
                               PhysicalPersonPosition = x.PhysicalPersonPosition
                           })
                           .ToList();



                        //предоставляемые документы
                        disposalProvidedDocs = DisposalProvidedDocDomain.GetAll()
                             .Where(x => x.Disposal.Id == parentDisposal.Id)
                            .Select(x => new DecisionProvidedDoc
                            {
                                Decision = disposal,
                                ProvidedDoc = x.ProvidedDoc,
                                Description = x.Description
                            })
                            .ToList();
                    }

                }
                #endregion

                //типы обследования
                var inspectionReason = InspectionReasonDomain.GetAll()
                    .Where(x => x.Code == "05")
                    .Select(x => new DecisionInspectionReason
                    {
                        Decision = disposal,
                        InspectionReason = x,
                        Description = $"Предписание об устранении нарушений обязательных требований №{document.DocumentNumber} от {document.DocumentDate.Value.ToString("dd.MM.yyyy")}",
                    }).ToList();

                #region Находим предмет проверки
                var surveySubject = SurveySubjectDomain.GetAll()
                    .FirstOrDefault(x => x.Name.Contains("выполнение предписаний"));
                List<DecisionVerificationSubject> subjects = new List<DecisionVerificationSubject>();
                if (surveySubject != null)
                {
                    subjects.Add(new DecisionVerificationSubject
                    {
                        Decision = disposal,
                        SurveySubject = surveySubject
                    });
                }
        
                #endregion

                #region Сохранение
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        InspectionStageDomain.Save(stage);
                        DisposalDomain.Save(disposal);
                        ChildrenDomain.Save(parentChildren);
                        listInspectors.ForEach(DocumentInspectorDomain.Save);
                        dispAdminRegulationList.ForEach(DecisionAdminRegulationDomain.Save);
                        contrSubjList.ForEach(DecisionControlSubjectsDomain.Save);
                        disposalProvidedDocs.ForEach(DecisionProvidedDocDomain.Save);
                        inspectionReason.ForEach(DecisionInspectionReasonDomain.Save);
                        subjects.ForEach(DisposalVerificationSubjectDomain.Save);

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
                #endregion

                return new BaseDataResult(new {documentId = disposal.Id, inspectionId = document.Inspection.Id});
            }
            finally
            {
                this.Container.Release(DisposalDomain);
                this.Container.Release(InspectionStageDomain);
                this.Container.Release(DocumentInspectorDomain);
                this.Container.Release(ChildrenDomain);
                this.Container.Release(DecisionControlSubjectsDomain);
                this.Container.Release(SurveySubjectDomain);
                this.Container.Release(DisposalVerificationSubjectDomain);
                this.Container.Release(DisposalProvidedDocDomain);
                this.Container.Release(DecisionProvidedDocDomain);
                this.Container.Release(InspectionReasonDomain);
                this.Container.Release(DisposalAdminRegulationDomain);
                this.Container.Release(DecisionAdminRegulationDomain);
            }
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }
    }
}