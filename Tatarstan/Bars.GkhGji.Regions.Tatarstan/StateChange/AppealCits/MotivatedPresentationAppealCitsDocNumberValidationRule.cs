namespace Bars.GkhGji.Regions.Tatarstan.StateChange.AppealCits
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;

    using Castle.Windsor;

    /// <summary>
    /// Правило для <see cref="MotivatedPresentationAppealCits"/>
    /// </summary>
    public class MotivatedPresentationAppealCitsDocNumberValidationRule : IRuleChangeStatus
    {
        private readonly IWindsorContainer container;

        /// <inheritdoc />
        public string Id => "gji_document_motivatedpresentation_appealcits_doc_number_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка возможности формирования номера мотивированному представлению по обращению гражданина";

        /// <inheritdoc />
        public string TypeId => "gji_document_motivatedpresentation_appealcits";

        /// <inheritdoc />
        public string Description => "Данное правило проверяет формирование номера мотивированному представлению по обращению гражданина в соответствии с правилами РТ";

        public MotivatedPresentationAppealCitsDocNumberValidationRule(IWindsorContainer container)
        {
            this.container = container;
        }
        
        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is MotivatedPresentationAppealCits motivatedPresentationAppealCits)
            {
                var documentGjiInspectorDomain = this.container.ResolveDomain<DocumentGjiInspector>();
                var motivatedPresentationAppealCitsDomain = this.container.ResolveDomain<MotivatedPresentationAppealCits>();

                using (this.container.Using(documentGjiInspectorDomain, motivatedPresentationAppealCitsDomain))
                {
                    var hasInspector = documentGjiInspectorDomain.GetAll()
                        .Any(x => x.DocumentGji.Id == motivatedPresentationAppealCits.Id);

                    if (!motivatedPresentationAppealCits.DocumentDate.HasValue ||
                        motivatedPresentationAppealCits.Official.IsNull() ||
                        !motivatedPresentationAppealCits.ResultType.HasValue ||
                        !hasInspector)
                    {
                        return ValidateResult.No("Невозможно сформировать номер, " +
                            "поскольку имеются незаполненные обязательные поля.");
                    }

                    var documentNum = (motivatedPresentationAppealCitsDomain.GetAll()
                        .Where(x => x.Id != motivatedPresentationAppealCits.Id)
                        .Select(x => x.DocumentNum)
                        .Max() ?? 0) + 1;

                    motivatedPresentationAppealCits.DocumentNum = documentNum;
                    motivatedPresentationAppealCits.DocumentNumber = documentNum.ToString();
                }
            }

            return ValidateResult.Yes();
        }
    }
}