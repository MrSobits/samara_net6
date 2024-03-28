namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа Распоряжения из основания проверки по обращению граждан
    /// </summary>
    public class BaseStatementToDisposalRule : BaseStatementToDisposalBaseRule<Disposal>
    {
    }

    /// <summary>
    /// Базовый класс с функционалом для создания "Распоряжения" (или наследованного от него документа)
    /// на основе проверки по обращению граждан
    /// </summary>
    public class BaseStatementToDisposalBaseRule<T> : IInspectionGjiRule
        where T : Disposal, new()
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IRepository<Disposal> DisposalRepository { get; set; }

        public IDomainService<T> DisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<InspectionAppealCits> StatementDomain { get; set; }

        public IDomainService<KindCheckRuleReplace> KindCheckRuleReplaceDomain { get; set; }

        public IDomainService<KindCheckGji> KindCheckGjiDomain { get; set; }

        public IDomainService<InspectionGjiDocumentGji> InspectionDocDomain { get; set; }

        public IDomainService<MotivationConclusion> MotivationConclusionDomain { get; set; }

        public string CodeRegion => "Tat";

        public virtual string Id => "BaseStatementToDisposalRule";

        public string Description => "Правило создание документа Распоряжения из основания проверки по обращению";

        public string ActionUrl => "B4.controller.Disposal";

        public virtual string ResultName => this.DisposalTextService.SubjectiveCase;

        public TypeBase TypeInspectionInitiator => TypeBase.CitizenStatement;

        public virtual TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Disposal;

        protected virtual TypeStage InspectionTypeStageResult => TypeStage.Disposal;

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
            var stageMaxPosition = this.InspectionStageDomain.GetAll()
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
            var rules = this.Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

            using (this.Container.Using(rules))
            {
                foreach (var rule in rules)
                {
                    if (rule.Validate(disposal))
                    {
                        var replace = this.KindCheckRuleReplaceDomain.GetAll()
                            .FirstOrDefault(x => x.RuleCode == rule.Code);

                        disposal.KindCheck = replace != null
                            ? this.KindCheckGjiDomain.GetAll().FirstOrDefault(x => x.Code == replace.Code)
                            : this.KindCheckGjiDomain.GetAll().FirstOrDefault(x => x.Code == rule.DefaultCode);
                    }
                }
            }
            #endregion

            #region Забираем инспекторов из основания и переносим в Распоряжение
            var listInspectors = new List<DocumentGjiInspector>();
            ISet<long> inspectorIds;
            long? headId = null;
            long? executantId = null;
            if ((inspection as BaseStatement).RequestType == BaseStatementRequestType.MotivationConclusion)
            {
                inspectorIds = this.InspectionDocDomain.GetAll()
                    .Where(x => x.Inspection == inspection)
                    .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.MotivationConclusion)
                    .Join(this.InspectionInspectorDomain.GetAll(),
                        x => x.Document.Inspection,
                        x => x.Inspection,
                        (doc, insp) => (long?)insp.Inspector.Id)
                    .AsEnumerable()
                    .Where(x => x.HasValue)
                    .Select(x => x.Value)
                    .ToHashSet();

                var inspectorInfo = this.InspectionDocDomain.GetAll()
                    .Where(x => x.Inspection == inspection)
                    .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.MotivationConclusion)
                    .Join(this.MotivationConclusionDomain.GetAll(),
                        i => i.Document,
                        d => d,
                        (insp, doc) => new
                        {
                            Autor = (long?)doc.Autor.Id,
                            Executant = (long?)doc.Executant.Id
                        })
                    .AsEnumerable()
                    .OrderByDescending(x => x.Autor ?? 0)
                    .FirstOrDefault(x => x.Autor.HasValue && x.Executant.HasValue || x.Autor.HasValue || x.Executant.HasValue);

                headId = inspectorInfo?.Autor;
                executantId = inspectorInfo?.Executant;
            }
            else
            {
                inspectorIds = this.InspectionInspectorDomain.GetAll()
                    .Where(x => x.Inspection == inspection)
                    .Where(x => (long?)x.Inspector.Id != null)
                    .Select(x => x.Inspector.Id)
                    .ToHashSet();

                var inspectorInfo = this.StatementDomain.GetAll()
                    .Where(x => x.Inspection == inspection)
                    .Select(x => new
                    {
                        Autor = (long?)x.AppealCits.Surety.Id,
                        Executant = (long?)x.AppealCits.Executant.Id
                    })
                    .AsEnumerable()
                    .OrderByDescending(x => x.Autor ?? 0)
                    .FirstOrDefault(x => x.Autor.HasValue && x.Executant.HasValue || x.Autor.HasValue || x.Executant.HasValue);

                headId = inspectorInfo?.Autor;
                executantId = inspectorInfo?.Executant;
            }

            foreach (var inspector in inspectorIds)
            {
                var newInspector = new DocumentGjiInspector
                {
                    DocumentGji = disposal,
                    Inspector = new Inspector { Id = inspector }
                };

                listInspectors.Add(newInspector);
            }

            disposal.IssuedDisposal = headId.HasValue ? new Inspector { Id = headId.Value } : null;
            disposal.ResponsibleExecution = executantId.HasValue ? new Inspector { Id = executantId.Value } : null;
            #endregion

            #region Сохранение
            using (var tr = this.Container.Resolve<IDataTransaction>())
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