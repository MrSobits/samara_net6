namespace Bars.GkhGji.Regions.Voronezh.InspectionRules.DocumentRules
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules;
    using Bars.GkhGji.Rules;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.GkhGji.Regions.Voronezh.Entities;

    public class VoronezhPrescriptionToDisposalRule : PrescriptionToDisposalRule
    {

        private const string SurveySubjectForDisposalCode = "III.";
        public override string ResultName
        {
            get { return "Приказ на проверку предписания"; }
        }

        private bool CheckCourtPractice(long docId)
        {
            var CpDomain = this.Container.Resolve<IDomainService<CourtPractice>>();
            var cpByDoc = CpDomain.GetAll().FirstOrDefault(x => x.DocumentGji != null && x.DocumentGji.Id == docId);
            if (cpByDoc != null)
            {
                return cpByDoc.InterimMeasures;
            }
            else return false;
        }

        public override IDataResult CreateDocument(DocumentGji document)
        {
            if (CheckCourtPractice(document.Id))
            {
                return new BaseDataResult(false, "Приняты обеспечительные меры. Действие предписания приостановлено судом. Формирование приказа невозможно!");
            }
            var DisposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var InspectionStageDomain = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var TypeSurveyDomain = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var SurveySubjectDomain = this.Container.Resolve<IDomainService<SurveySubject>>();
            var DisposalVerificationSubjectDomain = this.Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            var DisposalSurveyPurposeDomain = this.Container.Resolve<IDomainService<DisposalSurveyPurpose>>();
            var DisposalSurveyObjectiveDomain = this.Container.Resolve<IDomainService<DisposalSurveyObjective>>();
            var DisposalVerificationSubjectService = this.Container.Resolve<IDisposalVerificationSubjectService>();
            var dispInsfFoundationCheckDomain = this.Container.Resolve<IDomainService<DisposalInspFoundationCheck>>();
            var DisposalAdminRegulationDomain = this.Container.Resolve<IDomainService<DisposalAdminRegulation>>();
            var DisposalProvidedDocDomain = this.Container.Resolve<IDomainService<ChelyabinskDisposalProvidedDoc>>();

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
                    TypeStage = TypeStage.DisposalPrescription
                };
                #endregion

                #region Формируем распоряжение распоряжение
                var disposal = new Disposal()
                {
                    Inspection = document.Inspection,
                    TypeDisposal = TypeDisposalGji.DocumentGji,
                    TypeDocumentGji = TypeDocumentGji.Disposal,
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
                var rules = this.Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

                foreach (var rule in rules)
                {
                    if (rule.Validate(disposal))
                    {
                        var replace = this.Container.Resolve<IDomainService<KindCheckRuleReplace>>().GetAll()
                            .FirstOrDefault(x => x.RuleCode == rule.Code);

                        var serviceKindCheck = this.Container.Resolve<IDomainService<KindCheckGji>>();

                        disposal.KindCheck = replace != null
                            ? serviceKindCheck.GetAll().FirstOrDefault(x => x.Code == replace.Code)
                            : serviceKindCheck.GetAll().FirstOrDefault(x => x.Code == rule.DefaultCode);
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
                        Inspector = new Inspector { Id = inspector }
                    };

                    listInspectors.Add(newInspector);
                }
                #endregion

                #region преносим типы обследования, цели и задачи
                var listSurveysList = new List<DisposalTypeSurvey>();
                var listSurveyPurposes = new List<DisposalSurveyPurpose>();
                var listSurveyObjectives = new List<DisposalSurveyObjective>();
                var listNPANewDisposal = new List<DisposalInspFoundationCheck>();
                var listAdminRegulationDisposal = new List<DisposalAdminRegulation>();
                var listDisposalProvidedDoc = new List<ChelyabinskDisposalProvidedDoc>();

                //получаем по предписанию родительское распоряжение
                var parentDisposal = Utils.GetParentDocumentByType(ChildrenDomain, document, TypeDocumentGji.Disposal);
                if (parentDisposal != null)
                {
                    //Административные регламенты
                    var dispAdminRegulationList = DisposalAdminRegulationDomain.GetAll()
                        .Where(x => x.Disposal.Id == parentDisposal.Id)
                        .Select(x => x.AdminRegulation.Id)
                        .Distinct()
                        .ToList();
                    //НПА проверки 
                    var npaDisposalList = dispInsfFoundationCheckDomain.GetAll()
                        .Where(x => x.Disposal.Id == parentDisposal.Id)
                        .Select(x => x.InspFoundationCheck.Id)
                        .Distinct()
                        .ToList();

                    //типы обследования
                    var parentDisposalTypeSurveyIds = TypeSurveyDomain.GetAll()
                        .Where(x => x.Disposal.Id == parentDisposal.Id)
                        .Select(x => x.TypeSurvey.Id)
                        .Distinct()
                        .ToList();

                    //предоставляемые документы
                    var disposalProvidedDocs = DisposalProvidedDocDomain.GetAll()
                         .Where(x => x.Disposal.Id == parentDisposal.Id)
                        .Select(x => new
                        {
                            x.ProvidedDoc.Id,
                            x.Order
                        })
                        .Distinct()
                        .ToList();

                    // предмет проверки исполнение предписания DisposalVerificationSubject



                    parentDisposalTypeSurveyIds.ForEach(
                        x =>
                        {
                            listSurveysList.Add(
                                new DisposalTypeSurvey
                                {
                                    Disposal = disposal,
                                    TypeSurvey = new TypeSurveyGji { Id = x }
                                });
                        });

                    //цели проверки
                    var parentDisposalSurveyPurposeIds = DisposalSurveyPurposeDomain.GetAll()
                        .Where(x => x.Disposal.Id == parentDisposal.Id)
                        .Select(x => x.SurveyPurpose.Id)
                        .Distinct()
                        .ToList();

                    parentDisposalSurveyPurposeIds.ForEach(
                        x =>
                        {
                            listSurveyPurposes.Add(
                                new DisposalSurveyPurpose
                                {
                                    Disposal = disposal,
                                    SurveyPurpose = new SurveyPurpose { Id = x }
                                });
                        });

                    //задачи проверки
                    var parentDisposalSurveyObjectiveIds = DisposalSurveyObjectiveDomain.GetAll()
                        .Where(x => x.Disposal.Id == parentDisposal.Id)
                        .Select(x => x.SurveyObjective.Id)
                        .Distinct()
                        .ToList();

                    parentDisposalSurveyObjectiveIds.ForEach(
                        x =>
                        {
                            listSurveyObjectives.Add(
                                new DisposalSurveyObjective
                                {
                                    Disposal = disposal,
                                    SurveyObjective = new SurveyObjective { Id = x }
                                });
                        });
                    //Административные регламенты 
                    dispAdminRegulationList.ForEach(
                    x =>
                    {
                        listAdminRegulationDisposal.Add(
                            new DisposalAdminRegulation
                            {
                                Disposal = disposal,
                                AdminRegulation = new Gkh.Entities.Dicts.NormativeDoc { Id = x }
                            });
                    });

                    //НПА проверки
                    npaDisposalList.ForEach(
                        x =>
                        {
                            listNPANewDisposal.Add(
                                new DisposalInspFoundationCheck
                                {
                                    Disposal = disposal,
                                    InspFoundationCheck = new Gkh.Entities.Dicts.NormativeDoc { Id = x }
                                });
                        });
                    //предоставляемые документы
                    disposalProvidedDocs.ForEach(
                  x =>
                  {
                      listDisposalProvidedDoc.Add(
                          new ChelyabinskDisposalProvidedDoc
                          {
                              Disposal = disposal,
                              Order = x.Order,
                              ProvidedDoc = new ProvidedDocGji { Id = x.Id }
                          });
                  });
                }
                #endregion

                #region Находим предмет проверки
                var surveySubject = SurveySubjectDomain.GetAll()
                    .FirstOrDefault(x => x.Code == SurveySubjectForDisposalCode && x.Relevance == SurveySubjectRelevance.Actual);
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
                        listSurveysList.ForEach(TypeSurveyDomain.Save);
                        listSurveyPurposes.ForEach(DisposalSurveyPurposeDomain.Save);
                        listSurveyObjectives.ForEach(DisposalSurveyObjectiveDomain.Save);
                        listSurveysList.ForEach(TypeSurveyDomain.Save);
                        listNPANewDisposal.ForEach(dispInsfFoundationCheckDomain.Save);
                        listAdminRegulationDisposal.ForEach(DisposalAdminRegulationDomain.Save);
                        listDisposalProvidedDoc.ForEach(DisposalProvidedDocDomain.Save);

                        if (surveySubject != null)
                        {
                            DisposalVerificationSubjectService.AddSurveySubjects(disposal.Id, new[] { surveySubject.Id });
                        }
                        else
                        {
                            try
                            {
                                surveySubject = SurveySubjectDomain.GetAll()
                                     .FirstOrDefault(x => x.Code == "3");
                                if (surveySubject != null)
                                {
                                    DisposalVerificationSubjectService.AddSurveySubjects(disposal.Id, new[] { surveySubject.Id });
                                }
                            }
                            catch
                            {

                            }
                        }

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
                #endregion

                return new BaseDataResult(new { documentId = disposal.Id, inspectionId = document.Inspection.Id });
            }
            finally
            {
                this.Container.Release(DisposalDomain);
                this.Container.Release(InspectionStageDomain);
                this.Container.Release(DocumentInspectorDomain);
                this.Container.Release(ChildrenDomain);
                this.Container.Release(TypeSurveyDomain);
                this.Container.Release(SurveySubjectDomain);
                this.Container.Release(DisposalVerificationSubjectDomain);
                this.Container.Release(DisposalSurveyPurposeDomain);
                this.Container.Release(DisposalSurveyObjectiveDomain);
                this.Container.Release(DisposalVerificationSubjectService);
            }
        }
    }
}