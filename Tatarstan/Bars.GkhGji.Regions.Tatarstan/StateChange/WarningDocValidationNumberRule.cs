using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Modules.States;
using Bars.GkhGji.Entities;
using Castle.Windsor;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public class WarningDocValidationNumberRule : IRuleChangeStatus
    {
        public string Id => "warning_doc_validation_rule";

        public string Name => "Проверка возможности формирования номера предостережение";

        public string TypeId => "gji_document_warning";

        public string Description => "Данное правило проверяет формирование номера предостережения в соответствии с правилами РТ";

        public IDomainService<WarningDoc> WarningDocDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocChildDomain { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is WarningDoc warningDoc && string.IsNullOrEmpty(warningDoc.DocumentNumber))
            {
                if (warningDoc.DocumentDate == null
                    || warningDoc.CompilationPlace == null
                    || warningDoc.ActionStartDate == null)
                {
                    return ValidateResult.No("Невозможно сформировать номер, поскольку имеются незаполненные обязательные поля.");
                }

                switch (warningDoc.Inspection.TypeBase)
                {
                    case TypeBase.PlanJuridicalPerson:
                    case TypeBase.ProsecutorsClaim:
                    case TypeBase.DisposalHead:
                    case TypeBase.CitizenStatement:
                    case TypeBase.InspectionActionIsolated:
                        var actCheck = this.DocChildDomain.GetAll()
                            .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                            .Where(x => x.Children.Id == warningDoc.Id)
                            .Select(x => x.Parent)
                            .First();

                        if (string.IsNullOrEmpty(actCheck.DocumentNumber))
                        {
                            return ValidateResult.No(
                                $"Изменение статуса запрещено. Номер не может быть присвоен, потому что у предыдущего документа \"aкт проверки\" от {actCheck.DocumentDate} нет номера.");
                        }

                        this.SetInspectionBasedDocNumber(actCheck, warningDoc);
                        break;
                    
                    case TypeBase.ActionIsolated:
                        var motivatedPresentation = this.DocChildDomain.GetAll()
                            .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.MotivatedPresentation)
                            .Where(x => x.Children.Id == warningDoc.Id)
                            .Select(x => x.Parent)
                            .First();

                        if (string.IsNullOrEmpty(motivatedPresentation.DocumentNumber))
                        {
                            return ValidateResult.No(
                                $"Изменение статуса запрещено. Номер не может быть присвоен, потому что у предыдущего документа \"мотивированное представление\" от {motivatedPresentation.DocumentDate} нет номера.");
                        }

                        SetActionIsolatedBasedDocNumber(motivatedPresentation, warningDoc);
                        break;
                    
                    case TypeBase.GjiWarning:
                        var mo = this.InspectionRoDomain.GetAll()
                            .Where(x => x.Inspection.Id == warningDoc.Inspection.Id)
                            .Select(x => new
                            {
                                x.RealityObject.Municipality.Id,
                                x.RealityObject.Municipality.Code
                            })
                            .FirstOrDefault();

                        if (mo == null)
                        {
                            return ValidateResult.No("Невозможно сформировать номер, поскольку не удалось определить муниципальное образование.");
                        }

                        this.SetWarningBasedDocNumber(mo.Code, warningDoc);
                        break;
                }

                this.WarningDocDomain.Update(warningDoc);
            }
            
            return ValidateResult.Yes();
        }

        private void SetInspectionBasedDocNumber(DocumentGji parentDoc, WarningDoc warningDoc)
        {
            warningDoc.DocumentNumber = parentDoc.DocumentNumber;
            warningDoc.DocumentNum = parentDoc.DocumentNum;
        }

        private void SetActionIsolatedBasedDocNumber(DocumentGji parentDoc, WarningDoc warningDoc)
        {
            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();

            using (this.Container.Using(taskActionIsolatedDomain))
            {
                var isObservationAction = taskActionIsolatedDomain.GetAll()
                    .FirstOrDefault(x => x.Inspection == parentDoc.Inspection)?.KindAction == KindAction.Observation;

                warningDoc.DocumentNumber = $"{parentDoc.DocumentNumber}{(isObservationAction ? $"-НОТ" : string.Empty)}";
                warningDoc.DocumentNum = parentDoc.DocumentNum;
            }
        }

        private void SetWarningBasedDocNumber(string moCode, WarningDoc warningDoc)
        {
            var documentNum = (WarningDocDomain.GetAll()
                .Join(this.InspectionRoDomain.GetAll(),
                    x => x.Inspection.Id,
                    x => x.Inspection.Id,
                    (x, y) => new
                    {
                        x.Inspection.TypeBase,
                        x.DocumentNum,
                        y.RealityObject.Municipality.Code
                    })
                .Where(x => x.TypeBase == TypeBase.GjiWarning)
                .Where(x => x.Code == moCode)
                .Select(x => x.DocumentNum)
                .Max() ?? 0) + 1;

            warningDoc.DocumentNumber = $"{moCode}-{documentNum}-ОГ";
            warningDoc.DocumentNum = documentNum;
        }
    }
}
