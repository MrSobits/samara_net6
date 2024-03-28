namespace Bars.GkhGji.Regions.Nso.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;
    using DomainService;
    using Entities;
    using Entities.Disposal;
    using GkhGji.Entities.Dict;

	/// <summary>
    /// Правило создание документа 'Приказ на проверку предписания' из документа 'Предписание'
    /// </summary>
    public class PrescriptionToDisposalRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "PrescriptionToDisposalRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Приказ на проверку предписания' из документа 'Предписание'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Disposal"; }
        }

        public string ResultName
        {
            get { return "Приказ на проверку предписания"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Prescription; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Disposal; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // не ожидаем никаких входных параметров
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            
            var DisposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var InspectionStageDomain = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var TypeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var SurveySubjectDomain = Container.Resolve<IDomainService<SurveySubject>>();
            var DisposalVerificationSubjectDomain = Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            var DisposalSurveyPurposeDomain = Container.Resolve<IDomainService<DisposalSurveyPurpose>>();
            var DisposalSurveyObjectiveDomain = Container.Resolve<IDomainService<DisposalSurveyObjective>>();
            var DisposalVerificationSubjectService = Container.Resolve<IDisposalVerificationSubjectService>();

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
                var rules = Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

                foreach (var rule in rules)
                {
                    if (rule.Validate(disposal))
                    {
                        var replace = Container.Resolve<IDomainService<KindCheckRuleReplace>>().GetAll()
                                         .FirstOrDefault(x => x.RuleCode == rule.Code);

                        var serviceKindCheck = Container.Resolve<IDomainService<KindCheckGji>>();

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

				//получаем по предписанию родительское распоряжение
				var parentDisposal = Utils.Utils.GetParentDocumentByType(ChildrenDomain, document, TypeDocumentGji.Disposal);
				if (parentDisposal != null)
                {
					//типы обследования
                    var parentDisposalTypeSurveyIds = TypeSurveyDomain.GetAll()
                            .Where(x => x.Disposal.Id == parentDisposal.Id)
                            .Select(x => x.TypeSurvey.Id)
                            .Distinct()
                            .ToList();

                    parentDisposalTypeSurveyIds.ForEach(x =>
                    {
                        listSurveysList.Add(new DisposalTypeSurvey
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

					parentDisposalSurveyPurposeIds.ForEach(x =>
					{
						listSurveyPurposes.Add(new DisposalSurveyPurpose
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

					parentDisposalSurveyObjectiveIds.ForEach(x =>
					{
						listSurveyObjectives.Add(new DisposalSurveyObjective
						{
							Disposal = disposal,
							SurveyObjective = new SurveyObjective { Id = x }
						});
					});
				}

				#endregion

				#region Находим предмет проверки

				var surveySubject = SurveySubjectDomain.GetAll()
				   .FirstOrDefault(x => x.Code == SurveySubjectForDisposalCode && x.Relevance == SurveySubjectRelevance.Actual);

				#endregion

				#region Сохранение
				using (var tr = Container.Resolve<IDataTransaction>())
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

	                    if (surveySubject != null)
	                    {
							DisposalVerificationSubjectService.AddSurveySubjects(disposal.Id, new[] { surveySubject.Id });
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
                Container.Release(DisposalDomain);
                Container.Release(InspectionStageDomain);
                Container.Release(DocumentInspectorDomain);
                Container.Release(ChildrenDomain);
                Container.Release(TypeSurveyDomain);
                Container.Release(SurveySubjectDomain);
                Container.Release(DisposalVerificationSubjectDomain);
                Container.Release(DisposalSurveyPurposeDomain);
                Container.Release(DisposalSurveyObjectiveDomain);
                Container.Release(DisposalVerificationSubjectService);
            }
        }

        public IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }

		private const string SurveySubjectForDisposalCode = "III.";
    }
}
