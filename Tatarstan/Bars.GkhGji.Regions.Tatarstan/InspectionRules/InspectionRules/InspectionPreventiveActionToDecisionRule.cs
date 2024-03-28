namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа 'Решение' из основании проверки 'Проверка по профилактическому мероприятию'
    /// </summary>
    public class InspectionPreventiveActionToDecisionRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }
        
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<TatarstanDecision> DecisionDomain { get; set; }
        
        public IRepository<TatarstanDisposal> TatDisposalRepository { get; set; }

        public IDomainService<KindCheckGji> KindCheckGjiDomain { get; set; }
        
        public IGjiValidityDocPeriodService GjiValidityDocPeriodService { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => "InspectionPreventiveActionToDecisionRule";

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Решение' из основании проверки 'Проверка по профилактическому мероприятию'";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.Disposal";

        /// <inheritdoc />
        public string ResultName => "Решение";

        /// <inheritdoc />
        public TypeBase TypeInspectionInitiator => TypeBase.InspectionPreventiveAction;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Decision;
        
        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(InspectionGji inspection)
        {
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
                TypeStage = TypeStage.Decision
            };

            #endregion
            
            #region Фомриурем документ
            var decision = new TatarstanDecision
            {
                Inspection = inspection,
                TypeDisposal = TypeDisposalGji.Base,
                TypeDocumentGji = TypeDocumentGji.Decision,
                Stage = stage,
                TypeAgreementProsecutor = TypeAgreementProsecutor.NotSet,
                TypeAgreementResult = TypeAgreementResult.NotSet,
            };
            #endregion
            
            #region Проставляем вид проверки
            var rules = this.Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

            foreach (var rule in rules)
            {
                if (rule.Validate(decision))
                {
                    var replace = this.Container.Resolve<IDomainService<KindCheckRuleReplace>>().GetAll()
                        .FirstOrDefault(x => x.RuleCode == rule.Code);
                    
                    decision.KindCheck = replace != null
                        ? this.KindCheckGjiDomain.GetAll().FirstOrDefault(x => x.Code == replace.Code)
                        : this.KindCheckGjiDomain.GetAll().FirstOrDefault(x => x.Code == rule.DefaultCode);
                }
            }
            #endregion

            #region Сохраняем
            
            this.Container.InTransaction(() =>
            {
                this.InspectionStageDomain.Save(stage);
                this.DecisionDomain.Save(decision);
            });

            #endregion

            return new BaseDataResult(new { documentId = decision.Id, typeDocument = this.TypeDocumentResult, inspectionId = inspection.Id });
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(InspectionGji inspection)
        {
            if (inspection != null)
            {
                if (this.TatDisposalRepository.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, $"Из 'Проверка по профилактическому мероприятию' уже создано '{this.ResultName}'");
                }

                return this.GjiValidityDocPeriodService.DocPeriodValidation(inspection.CheckDate, this.TypeDocumentResult);
            }

            return new BaseDataResult();
        }
    }
}