namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Приказ на проверку предписания' из документа 'Предписание'
    /// </summary>
    public class TomskPrescriptionToDisposalRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DisposalTypeSurvey> TypeSurveyDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tomsk"; }
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
            var inspectorIds = this.DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
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

            #region преносим типы обследования
            var listSurveysList = new List<DisposalTypeSurvey>();
 
            //получаем по предписанию родительское распоряжение
            var parentDisposalId = ChildrenDomain.GetAll()
                                    .Where(x => x.Children.Id == document.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                                    .Select(x => x.Parent.Id)
                                    .FirstOrDefault();


            if (parentDisposalId > 0)
            {
                var parentDisposalTypeSurveyIds = TypeSurveyDomain.GetAll()
                        .Where(x => x.Disposal.Id == parentDisposalId)
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
            }
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.DisposalDomain.Save(disposal);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listSurveysList.ForEach(x => TypeSurveyDomain.Save(x));

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

        public IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }
    }
}
