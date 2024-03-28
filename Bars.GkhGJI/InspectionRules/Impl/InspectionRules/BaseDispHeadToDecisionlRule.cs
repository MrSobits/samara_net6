namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания из основания проверки по поручению руководства документа Распоряжения
    /// </summary>
    public class BaseDispHeadToDecisionlRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IDomainService<Decision> DisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<BaseDispHead> BaseDispHeadDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "BaseDispHeadToDecisionlRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа решения из основания проверки по поручению руководства"; }
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
            get { return TypeBase.DisposalHead; }
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
            var smevRule = Container.ResolveAll<ISMEVRule>();

            try
            {
                var rule = smevRule.FirstOrDefault(x => x.Id == "BaseSMEVInspectionRule");
                if (rule != null)
                {
                    rule.SendRequests(inspection);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(smevRule);
            }
            #region Формируем распоряжение распоряжение
            var decision = new Decision()
            {
                Inspection = inspection,
                TypeDisposal = TypeDisposalGji.Base,
                TypeBase = TypeBase.DisposalHead,
                TypeDocumentGji = TypeDocumentGji.Decision,
                TypeAgreementProsecutor = TypeAgreementProsecutor.NotSet,
                TypeAgreementResult = TypeAgreementResult.NotSet
            };


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
            var rules = Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

            decision.KindCheck = GetKindCheck(inspection);
            #endregion

            #region Забираем инспекторов из основания и переносим в Распоряжение

            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = InspectionInspectorDomain.GetAll().Where(x => x.Inspection.Id == inspection.Id)
                .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var inspector in inspectorIds)
            {
                var newInspector = new DocumentGjiInspector
                {
                    DocumentGji = decision,
                    Inspector = new Inspector { Id = inspector }
                };

                listInspectors.Add(newInspector);

            }

            // Для распоряжения по проверке руководителя переносим также и инспектора из оснвоания 
            var head = BaseDispHeadDomain
                            .GetAll()
                            .Where(x => x.Id == inspection.Id)
                            .Select(x => x.Head)
                            .FirstOrDefault();

            decision.IssuedDisposal = head;
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.DisposalDomain.Save(decision);

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
        private KindCheckGji GetKindCheck(InspectionGji insp)
        {
            var serviceKindCheck = Container.Resolve<IDomainService<KindCheckGji>>();

            return serviceKindCheck.GetAll().FirstOrDefault();
        }

        public IDataResult ValidationRule(InspectionGji inspection)
        {
            /*
             тут проверяем, если у проверки уже есть дкоумент Распоряжение, то нельзя больше создавать
            */
            if (inspection != null)
            {
                if (this.DisposalDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, "По плановой проверке юр. лиц уже создано распоряжение");
                }

                if (!this.InspectionGjiRealityObjectDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, "Отсутствуют проверяемые дома");
                }
            }

            return new BaseDataResult();
        }
    }
}
