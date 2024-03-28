namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа Распоряжения из основания проверки по мероприятиям без взаимодействия с контролируемыми лицами
    /// </summary>
    public class InspectionActionIsolatedToDisposalRule : InspectionActionIsolatedToDisposalBaseRule<TatarstanDisposal>
    {
    }

    /// <summary>
    /// Базовый класс с функционалом для создания "Распоряжения" (или наследованного от него документа)
    /// на основе проверки по мероприятиям без взаимодействия с контролируемыми лицами
    /// </summary>
    /// <typeparam name="T">Тип документа "Распоряжение" или наследованнй от него тип</typeparam>
    public class InspectionActionIsolatedToDisposalBaseRule<T> : IInspectionGjiRule
        where T : TatarstanDisposal, new()
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IRepository<TatarstanDisposal> TatDisposalRepository { get; set; }

        public IDomainService<T> TatDisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<KindCheckRuleReplace> KindCheckRuleReplaceDomain { get; set; }

        public IDomainService<KindCheckGji> KindCheckGjiDomain { get; set; }

        public IGjiValidityDocPeriodService GjiValidityDocPeriodService { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public virtual string Id => "InspectionActionIsolatedToDisposalRule";

        /// <inheritdoc />
        public string Description => $"Правило создания документа '{this.ResultName}' из основания проверки КНМ без взаимодействия";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.Disposal";

        /// <inheritdoc />
        public virtual string ResultName => DisposalTextService.SubjectiveCase;

        /// <inheritdoc />
        public TypeBase TypeInspectionInitiator => TypeBase.InspectionActionIsolated;

        /// <inheritdoc />
        public virtual TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Disposal;

        protected virtual TypeStage InspectionTypeStageResult => TypeStage.Disposal;

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // обработка пользовательских параметров не требуется
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(InspectionGji inspection)
        {
            #region Формируем решение
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

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);
                    this.TatDisposalDomain.Save(disposal);
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

        /// <inheritdoc />
        public IDataResult ValidationRule(InspectionGji inspection)
        {
            /*
             тут проверяем, если у проверки уже есть документ с типом TypeDocumentResult, то нельзя больше создавать
            */
            if (inspection != null)
            {
                if (this.TatDisposalRepository.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, $"Из основания проверки КНМ без взаимодействия уже создано '{this.ResultName}'");
                }

                return this.GjiValidityDocPeriodService.DocPeriodValidation(inspection.CheckDate, this.TypeDocumentResult);
            }

            return new BaseDataResult();
        }
    }
}