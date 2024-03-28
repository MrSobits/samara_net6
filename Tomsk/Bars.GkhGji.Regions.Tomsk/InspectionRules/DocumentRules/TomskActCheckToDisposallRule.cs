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
    /// Правило создание документа 'Приказ' из документа 'Акт проверки (нарушения выявлены)'
    /// </summary>
    public class TomskActCheckToDisposalRule : IDocumentGjiRule
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

        public IDomainService<ActCheckViolation> ActCheckViolationDomain { get; set; }

        public IDomainService<KindCheckRuleReplace> KindCheckRuleReplaceDomain { get; set; }

        public IDomainService<KindCheckGji> KindCheckDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "TomskActCheckToDisposalRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Приказ ' из документа 'Акта проверки'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Disposal"; }
        }

        public string ResultName
        {
            get { return "Приказ"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.ActCheck; }
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
                TypeStage = TypeStage.Disposal
            };
            #endregion

            #region Формируем распоряжение распоряжение
            var disposal = new Disposal()
            {
                Inspection = document.Inspection,
                TypeDisposal = TypeDisposalGji.Base,
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
            var rules = Container.ResolveAll<IKindCheckRule>()
                        .OrderBy(x => x.Priority);

            foreach (var rule in rules)
            {
                if (rule.Validate(disposal))
                {
                    var replace = KindCheckRuleReplaceDomain.GetAll()
                                     .FirstOrDefault(x => x.RuleCode == rule.Code);

                    disposal.KindCheck = replace != null
                                           ? KindCheckDomain.GetAll().FirstOrDefault(x => x.Code == replace.Code)
                                           : KindCheckDomain.GetAll().FirstOrDefault(x => x.Code == rule.DefaultCode);
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

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.DisposalDomain.Save(disposal);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

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

            if (ChildrenDomain.GetAll().Any(x => x.Parent.Id == document.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Disposal))
            {
                return new BaseDataResult(
                    false,
                    "Невозможно сформировать Приказ, поскольку из Акта проверки уже приказ сформирован");
            }

            // В акте можно формировать протокол, только если есть нарушения  
            if (!this.ActCheckViolationDomain.GetAll().Any(x => x.Document.Id == document.Id))
            {
                return new BaseDataResult(false, "Для Акта (нарушения не выявлены) нельзя сформировать Приказ");
            }


            return new BaseDataResult();
        }
    }
}
