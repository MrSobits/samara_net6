namespace Bars.GkhGji.Regions.Habarovsk.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Habarovsk.Entities;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;
    using GkhGji.InspectionRules;

    /// <summary>
    /// Правило создания из основания плановой проверки ЮЛ документа Распоряжения
    /// </summary>
    public class BaseOMSUToDecisionRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IDomainService<Decision> DecisionDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<BaseOMSU> BaseOMSUDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<KindCheckGji> KindCheckDomain { get; set; }

        public IDomainService<KindCheckRuleReplace> KindCheckRuleReplaceDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "BaseOMSUDecisionRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа решения из основания плановой проверки ОМСУ"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Decision"; }
        }

        public string ResultName
        {
            get { return "Решение"; }
        }

        public TypeBase TypeInspectionInitiator
        {
            get { return TypeBase.PlanOMSU; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Decision; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // обработка пользовательских параметров не требуется
        }

        public virtual IDataResult CreateDocument(InspectionGji inspection)
        {
            var baseOMSU = BaseOMSUDomain.Get(inspection.Id);
            #region Формируем распоряжение распоряжение
            var decision = new Decision()
            {
                Inspection = inspection,
                TypeDisposal = TypeDisposalGji.Base,
                TypeBase = TypeBase.PlanOMSU,
                TypeDocumentGji = TypeDocumentGji.Decision,
                TypeAgreementProsecutor = TypeAgreementProsecutor.NotSet,
                TypeAgreementResult = TypeAgreementResult.NotSet,
                DateStart = baseOMSU.DateStart,
                

            };
            if (baseOMSU.DateStart.HasValue && baseOMSU.CountDays.HasValue)
            {
                decision.DateEnd = baseOMSU.DateStart.Value.AddDays(baseOMSU.CountDays.Value);
            }
           
            #endregion

            #region Формируем этап проверки
            var stageMaxPosition = InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

            // Создаем Этап Проверки (Который показывается слева в дереве)
            var stage = new InspectionGjiStage
            {
                Inspection = inspection,
                Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                TypeStage = TypeStage.Decision
            };

            decision.Stage = stage;
            #endregion

            #region Проставляем вид проверки
            decision.KindCheck = GetKindCheck();
            #endregion

            #region Забираем инспекторов из основания и переносим в Распоряжение
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = InspectionInspectorDomain.GetAll().Where(x => x.Inspection.Id == inspection.Id)
                .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var inspector in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = decision,
                    Inspector = new Inspector { Id = inspector }
                });
            }
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.DecisionDomain.Save(decision);

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

            return new BaseDataResult(new { documentId = decision.Id, inspectionId = inspection.Id });
        }

        public IDataResult ValidationRule(InspectionGji inspection)
        {
            /*
             тут проверяем, если у проверки уже есть дкоумент Распоряжение, то нельзя больше создавать
            */
            if (inspection != null)
            {
                if (this.DecisionDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, "По плановой проверке ОМСУ уже создано решение");
                }
            }

            return new BaseDataResult();
        }

        private KindCheckGji GetKindCheck()
        {
            var serviceKindCheck = Container.Resolve<IDomainService<KindCheckGji>>();

            return serviceKindCheck.GetAll().FirstOrDefault(x=> x.Code == TypeCheck.PlannedDocumentation);
        }
    }
}
