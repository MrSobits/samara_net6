namespace Bars.GkhGji.InspectionRules
{
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
    /// Правило создания документа Распоряжения для основания отопительного сезона
    /// </summary>
    public class BaseHeatSeasonToDisposalRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "BaseHeatSeasonToDisposalRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа Распоряжения для основания отопительного сезона"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Disposal"; }
        }

        public string ResultName
        {
            get { return DisposalTextService.SubjectiveCase; }
        }

        public TypeBase TypeInspectionInitiator
        {
            get { return TypeBase.HeatingSeason; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Disposal; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // обработка пользовательских параметров не требуется
        }

        public IDataResult CreateDocument(InspectionGji inspection)
        {
            #region Формируем распоряжение распоряжение
            var disposal = new Disposal()
            {
                Inspection = inspection,
                TypeDisposal = TypeDisposalGji.Base,
                TypeDocumentGji = TypeDocumentGji.Disposal,
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
            var inspectorIds = InspectionInspectorDomain.GetAll().Where(x => x.Inspection.Id == inspection.Id)
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

            return new BaseDataResult(new { documentId = disposal.Id, inspectionId = inspection.Id });
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
            }

            return new BaseDataResult();
        }
    }
}
