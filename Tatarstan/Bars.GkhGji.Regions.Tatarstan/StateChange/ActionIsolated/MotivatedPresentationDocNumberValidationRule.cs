namespace Bars.GkhGji.Regions.Tatarstan.StateChange.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    public class MotivatedPresentationDocNumberValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string Id => "gji_document_motivatedpresentation_doc_number_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка возможности формирования номера мотивированного представления";

        /// <inheritdoc />
        public string TypeId => "gji_document_motivatedpresentation";

        /// <inheritdoc />
        public string Description => "Данное правило проверяет формирование номера мотивированного представления в соответствии с правилами РТ";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is MotivatedPresentation motivatedPresentation)
            {
                var documentGjiInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
                var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();
                var visitSheetDomain = this.Container.ResolveDomain<VisitSheet>();
                var preventiveActionDomain = this.Container.ResolveDomain<PreventiveAction>();
                var motivatedPresentationActionDomain = this.Container.ResolveDomain<MotivatedPresentation>();
                var docGjiDomain = this.Container.Resolve<IDomainService<DocumentGji>>();

                using (this.Container.Using(documentGjiInspectorDomain, taskActionIsolatedDomain,
                           motivatedPresentationActionDomain, visitSheetDomain, docGjiDomain, preventiveActionDomain))
                {
                    var hasInspector = documentGjiInspectorDomain.GetAll()
                        .Any(x => x.DocumentGji.Id == motivatedPresentation.Id);

                    if (!motivatedPresentation.DocumentDate.HasValue ||
                        motivatedPresentation.CreationPlace.IsNull() ||
                        motivatedPresentation.IssuedMotivatedPresentation.IsNull() ||
                        motivatedPresentation.ResponsibleExecution.IsNull() ||
                        !hasInspector)
                    {
                        return ValidateResult.No("Невозможно сформировать номер, " +
                            "поскольку имеются незаполненные обязательные поля.");
                    }

                    var taskActionIsolated = taskActionIsolatedDomain.GetAll()
                        .FirstOrDefault(x => x.Inspection.Id == motivatedPresentation.Inspection.Id);
                    var visitSheet = visitSheetDomain.GetAll()
                        .SingleOrDefault(x => x.Inspection.Id == motivatedPresentation.Inspection.Id);

                    PreventiveAction preventiveAction = null;
                    IQueryable<MotivatedPresentation> relatedMotivatedPresentations = null;
                    
                    if (taskActionIsolated != null)
                    {
                        if (!taskActionIsolated.DocumentNum.HasValue)
                        {
                            ValidateResult.No("Родительскому документу не присвоен номер");
                        }
                        
                        relatedMotivatedPresentations = motivatedPresentationActionDomain.GetAll()
                            .Join(docGjiDomain.GetAll(),
                                x => x.Id,
                                y => y.Id,
                                (x, y) => new
                                {
                                    MotivatedPresentation = x,
                                    y.Inspection
                                })
                            .Join(docGjiDomain.GetAll(),
                                x => x.Inspection,
                                y => y.Inspection,
                                (x, y) => new
                                {
                                    x.MotivatedPresentation,
                                    y.Id
                                })
                            .Join(taskActionIsolatedDomain.GetAll(),
                                x => x.Id,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.MotivatedPresentation,
                                    TaskActionIsolated = y
                                })
                            .Where(x => x.TaskActionIsolated.Inspection.Id == taskActionIsolated.Inspection.Id)
                            .Where(x =>
                                x.TaskActionIsolated.Inspection.TypeBase == TypeBase.ActionIsolated &&
                                x.MotivatedPresentation.Inspection.TypeBase == TypeBase.ActionIsolated)
                            .Where(x => x.MotivatedPresentation.Id != motivatedPresentation.Id &&
                                x.TaskActionIsolated.Municipality == taskActionIsolated.Municipality &&
                                x.MotivatedPresentation.DocumentNum.HasValue &&
                                x.MotivatedPresentation.DocumentNumber != string.Empty)
                            .Select(x => x.MotivatedPresentation);
                    }
                    else if (visitSheet != null)
                    { 
                        if (!visitSheet.DocumentNum.HasValue)
                        {
                            ValidateResult.No("Родительскому документу не присвоен номер");
                        }
                        
                        preventiveAction = preventiveActionDomain.GetAll()
                            .SingleOrDefault(x => x.Inspection.Id == visitSheet.Inspection.Id);
                        
                        relatedMotivatedPresentations = motivatedPresentationActionDomain.GetAll()
                            .Join(docGjiDomain.GetAll(),
                                x => x.Id,
                                y => y.Id,
                                (x, y) => new
                                {
                                    MotivatedPresentation = x,
                                    y.Inspection,
                                    y.TypeDocumentGji
                                })
                            .Where(x => x.TypeDocumentGji == TypeDocumentGji.VisitSheet)
                            .Where(x => x.Inspection.Id == visitSheet.Inspection.Id)
                            .Where(x =>
                                x.Inspection.TypeBase == TypeBase.PreventiveAction &&
                                x.MotivatedPresentation.Inspection.TypeBase == TypeBase.PreventiveAction)
                            .Where(x => x.MotivatedPresentation.Id != motivatedPresentation.Id &&
                                x.MotivatedPresentation.DocumentNum.HasValue &&
                                x.MotivatedPresentation.DocumentNumber != string.Empty)
                            .Select(x => x.MotivatedPresentation);
                    }
                    else
                    {
                        return ValidateResult.No("Не найдено ни одного родителя для документа");
                    }
                    
                    int? motivatedPresentationSerialNum = null;
                    if (relatedMotivatedPresentations.Any())
                    {
                        // Если найденный документ единственный, то его подномер = null => проставляем новому подномер "1"
                        motivatedPresentationSerialNum = relatedMotivatedPresentations.SafeMax(x => x.DocumentSubNum) + 1 ?? 1;
                    }

                    var numPostfix = motivatedPresentationSerialNum.HasValue
                        ? $"{taskActionIsolated?.DocumentNum ?? visitSheet.DocumentNum}/{motivatedPresentationSerialNum}"
                        : taskActionIsolated?.DocumentNum.ToString() ?? visitSheet.DocumentNum.ToString();
                    motivatedPresentation.DocumentNum = taskActionIsolated?.DocumentNum ?? visitSheet.DocumentNum;
                    motivatedPresentation.DocumentSubNum = motivatedPresentationSerialNum;
                    motivatedPresentation.DocumentNumber = $"{taskActionIsolated?.Municipality.Code ?? preventiveAction.Municipality.Code}-{numPostfix}";
                }
            }

            return ValidateResult.Yes();
        }
    }
}