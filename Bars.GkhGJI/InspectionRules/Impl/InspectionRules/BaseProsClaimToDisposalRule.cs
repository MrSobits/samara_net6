namespace Bars.GkhGji.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа Распоряжение для проверки по требованию прокуратуры
    /// </summary>
    public class BaseProsClaimToDisposalRule : BaseProsClaimToDisposalBaseRule<Disposal>
    {
    }

    /// <summary>
    /// Базовый класс с функционалом для создания "Распоряжения" (или наследованного от него документа)
    /// на основе проверки по обращению граждан
    /// </summary>
    public class BaseProsClaimToDisposalBaseRule<T> : IInspectionGjiRule
        where T : Disposal, new()
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IRepository<Disposal> DisposalRepository { get; set; }

        public IDomainService<T> DisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<KindCheckGji> KindCheckGjiDomain { get; set; }

        public IDomainService<KindCheckRuleReplace> KindCheckRuleReplaceDomain { get; set; }

        public string CodeRegion => "Tat";

        public virtual string Id => "BaseProsClaimToDisposalRule";

        public string Description => $"Правило создание документа '{this.ResultName}' по основанию Проверки по требованию прокуратуры";

        public string ActionUrl => "B4.controller.Disposal";

        public virtual string ResultName => DisposalTextService.SubjectiveCase;

        public TypeBase TypeInspectionInitiator => TypeBase.ProsecutorsClaim;

        public virtual TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Disposal;

        protected virtual TypeStage InspectionTypeStageResult => TypeStage.Disposal;

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // обработка пользовательских параметров не требуется
        }

        public virtual IDataResult CreateDocument(InspectionGji inspection)
        {
            #region Формируем распоряжение распоряжение
            var disposal = new T
            {
                Inspection = inspection,
                TypeDisposal = TypeDisposalGji.Base,
                TypeDocumentGji = this.TypeDocumentResult,
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
                TypeStage = this.InspectionTypeStageResult
            };

            disposal.Stage = stage;
            #endregion

            #region Проставляем вид проверки
            var rules = Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

            using (this.Container.Using(rules))
            {
                foreach (var rule in rules)
                {
                    if (rule.Validate(disposal))
                    {
                        var replace = KindCheckRuleReplaceDomain.GetAll()
                            .FirstOrDefault(x => x.RuleCode == rule.Code);

                        disposal.KindCheck = replace != null
                            ? KindCheckGjiDomain.GetAll().FirstOrDefault(x => x.Code == replace.Code)
                            : KindCheckGjiDomain.GetAll().FirstOrDefault(x => x.Code == rule.DefaultCode);
                    }
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

            return new BaseDataResult(new { documentId = disposal.Id, typeDocument = this.TypeDocumentResult, inspectionId = inspection.Id });
        }

        public virtual IDataResult ValidationRule(InspectionGji inspection)
        {
            /*
             тут проверяем, если у проверки уже есть дкоумент Распоряжение, то нельзя больше создавать
            */
            if (inspection != null)
            {
                if (this.DisposalRepository.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, "По плановой проверке юр. лиц уже создано распоряжение");
                }
            }

            return new BaseDataResult();
        }
    }
}