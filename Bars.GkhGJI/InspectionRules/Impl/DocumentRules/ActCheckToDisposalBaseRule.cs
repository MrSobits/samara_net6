namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Распоряжения (основного)' из документа 'Акт проверки'
    /// </summary>
    public class ActCheckToDisposalBaseRule : IDocumentGjiRule
    {
        #region injection
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<KindCheckGji> KindCheckDomain { get; set; }

        public IDomainService<KindCheckRuleReplace> KindCheckRuleReplaceDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }
        #endregion

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "ActCheckToDisposalBaseRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Распоряжения' из документа 'Акт проверки'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Disposal"; }
        }

        public string ResultName
        {
            get { return string.Format("{0}", DisposalTextService.SubjectiveCase); }
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
            // не ожидаем никаких параметров
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем распоряжение распоряжение на проверку предписания
            var disposal = new Disposal()
            {
                Inspection = document.Inspection,
                TypeDisposal = TypeDisposalGji.Base,
                TypeDocumentGji = TypeDocumentGji.Disposal,
                TypeAgreementProsecutor = TypeAgreementProsecutor.NotSet,
                TypeAgreementResult = TypeAgreementResult.NotSet
            };
            #endregion

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
            
            disposal.Stage = stage;
            #endregion

            #region Проставляем вид проверки
            var rules = Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

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

            #region Формируем инспекторов (берем из предписаний, которые выбрал пользователь)
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var inspector in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = disposal,
                    Inspector = new Inspector { Id = inspector }
                });
            }
            #endregion

            #region формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = disposal
            };
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.DisposalDomain.Save(disposal);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    ChildrenDomain.Save(parentChildren);

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

        public virtual IDataResult ValidationRule(DocumentGji document)
        {

            if (ActCheckRoDomain.GetAll().Where(x => x.ActCheck.Id == document.Id).Any(x => x.HaveViolation == YesNoNotSet.NotSet || x.HaveViolation == YesNoNotSet.No))
            {
                return new BaseDataResult(false, "Приказ из акта можно сформирвоать только в случае документарной проверки и если нарушения выявлены");
            }

            // Если проверка  Плановая документарная или внеплановая дкоументарная
            // то можно из Акта формировать еще один приказ.
            // иначе нельзя
            var disposal = DisposalDomain.GetAll()
                              .FirstOrDefault(
                                  x =>
                                  x.Inspection.Id == document.Inspection.Id && x.TypeDisposal == TypeDisposalGji.Base);

            if (disposal != null && disposal.KindCheck != null && (disposal.KindCheck.Code == TypeCheck.PlannedDocumentation || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation))
            {
                return new BaseDataResult();
            }

            return new BaseDataResult(false, "Приказ из акта можно сформирвоать только в случае документарной проверки и если нарушения выявлены");
        }
    }
}
