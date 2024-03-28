namespace Bars.GkhGji.Regions.Tatarstan.StateChange.PreventiveAction
{
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Castle.Windsor;
    using System.Linq;

    using Bars.GkhGji.Entities;

    public class VisitSheetDocNumberValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string Id => "gji_document_visit_sheet_doc_number_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка возможности формирования номера листа визита";

        /// <inheritdoc />
        public string TypeId => "gji_document_visit_sheet";

        /// <inheritdoc />
        public string Description => "Данное правило проверяет формирование номера листа визита в соответствии с правилами РТ";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is VisitSheet visitSheet)
            {
                var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
                var preventiveActionDomain = this.Container.ResolveDomain<PreventiveAction>();
                var visitSheetTaskDomain = this.Container.ResolveDomain<VisitSheet>();

                using (this.Container.Using(documentGjiChildrenDomain,
                    preventiveActionDomain, visitSheetTaskDomain))
                {
                    if (!visitSheet.DocumentDate.HasValue ||
                        !visitSheet.VisitDateEnd.HasValue ||
                        !visitSheet.VisitTimeStart.HasValue ||
                        !visitSheet.VisitTimeEnd.HasValue)
                    {
                        return ValidateResult.No("Невозможно сформировать номер, " +
                            "поскольку имеются незаполненные обязательные поля.");
                    }

                    var preventiveAction = preventiveActionDomain.GetAll()
                        .FirstOrDefault(x => x.Inspection.Id == visitSheet.Inspection.Id);

                    var relatedVisitSheets = visitSheetTaskDomain.GetAll()
                        .Join(documentGjiChildrenDomain.GetAll(),
                            x => x.Id,
                            y => y.Children.Id,
                            (x, y) => new
                            {
                                VisitSheet = x,
                                y.Parent
                            })
                        .Join(preventiveActionDomain.GetAll(),
                            x => x.Parent.Id,
                            y => y.Id,
                            (x, y) => new
                            {
                                x.VisitSheet,
                                PreventiveAction = y
                            })
                        .Where(x => x.PreventiveAction.Inspection.Id == preventiveAction.Inspection.Id)
                        .Where(x =>
                            x.VisitSheet.Inspection.TypeBase == TypeBase.PreventiveAction &&
                            x.PreventiveAction.Inspection.TypeBase == TypeBase.ActionIsolated)
                        .Where(x => x.VisitSheet.Id != visitSheet.Id &&
                            x.PreventiveAction.Municipality == preventiveAction.Municipality &&
                            x.VisitSheet.DocumentNum.HasValue &&
                            x.VisitSheet.DocumentNumber != string.Empty)
                        .Select(x => x.VisitSheet);

                    int? visitSheetSerialNum = null;
                    if (relatedVisitSheets.Any())
                    {
                        // Если найденный документ единственный, то его подномер = null => проставляем новому подномер "1"
                        visitSheetSerialNum = relatedVisitSheets.SafeMax(x => x.DocumentSubNum) + 1 ?? 1;
                    }

                    var numPostfix = visitSheetSerialNum.HasValue
                        ? $"{preventiveAction.DocumentNum}/{visitSheetSerialNum}"
                        : preventiveAction.DocumentNum.ToString();
                    visitSheet.DocumentNum = preventiveAction.DocumentNum;
                    visitSheet.DocumentSubNum = visitSheetSerialNum;
                    visitSheet.DocumentNumber = $"{preventiveAction.Municipality.Code}-{numPostfix}";
                }
            }

            return ValidateResult.Yes();
        }
    }
}