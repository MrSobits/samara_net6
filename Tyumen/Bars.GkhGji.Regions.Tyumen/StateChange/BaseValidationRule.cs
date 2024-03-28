using System.Linq;
using Castle.Windsor;
using Bars.B4;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;

using ValidateResult = Bars.B4.Modules.States.ValidateResult;

namespace Bars.GkhGji.Regions.Tyumen.StateChange
{
    public abstract class BaseValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string TypeId { get; }

        public abstract string Description { get; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is DocumentGji)
            {
                var document = statefulEntity as DocumentGji;

                if (!document.DocumentDate.HasValue)
                {
                    return ValidateResult.No("Невозможно сформировать номер, поскольку дата документа не указана");
                }

                var parentDocument = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Children.Id == document.Id && !x.Parent.State.FinalState)
                    .Select(x => new { x.Parent.TypeDocumentGji, x.Parent.DocumentDate })
                    .FirstOrDefault();

                if (parentDocument != null)
                {
                    return ValidateResult.No(string.Format(
                        "Необходимо зарегистрировать родительский документ {0} от {1}",
                        parentDocument.TypeDocumentGji.GetEnumMeta().Display,
                        parentDocument.DocumentDate.ToDateTime().ToShortDateString()));
                }

                Action(document);
            }

            return ValidateResult.Yes();
        }

        protected abstract void Action(DocumentGji document);
    }
}