using Castle.Core.Internal;

namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.StateChange;

    public abstract class DocGjiValidationNumberRuleStavropol : BaseDocValidationRule
    {
        public override ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = base.Validate(statefulEntity, oldState, newState);

            if (result.Success && statefulEntity is DocumentGji)
            {
                var document = statefulEntity as DocumentGji;

                result = Action(document);
            }

            return result;
        }

        protected virtual ValidateResult Action(DocumentGji document)
        {
            //сквозная нумерация
            // Номер формируется для документов: распоряжений, предписаний, протоколов, постановлений, постановлений прокуратуры, представлений 
            // путем получения максимального номера за текущий год + 1
           
            // Год берется из даты документа
            document.DocumentYear = document.DocumentDate.HasValue ? document.DocumentDate.Value.Year : (int?)null;

            var documentService = Container.Resolve<IDomainService<DocumentGji>>();

            var nextNum = documentService.GetAll()
                .Where(x => x.DocumentYear == document.DocumentYear
                    && x.Id != document.Id
                    && x.TypeDocumentGji == document.TypeDocumentGji)
                .Select(x => x.DocumentNum)
                .Max();

            document.DocumentNum = nextNum.HasValue ? nextNum.Value + 1 : 1;
            document.DocumentNumber = document.DocumentNum.ToString();

            return ValidateResult.Yes();
        }

        protected ValidateResult GetActDocumentNumber(DocumentGji document, TypeDocumentGji typeParent = TypeDocumentGji.Disposal)
        {
            var documentService = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var inspectorService = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            string parentNum = null;
            string inspectorNum = null;

            var parent = documentService.GetAll()
                .Where(x => x.Children.Id == document.Id && x.Parent.TypeDocumentGji == typeParent)
                .Select(x => x.Parent)
                .FirstOrDefault();

            if (parent != null)
            {
                var lastNum = documentService.GetAll()
                .Where(x => x.Parent.Id == parent.Id
                            && x.Children.TypeDocumentGji == document.TypeDocumentGji
                            && x.Children.Id != document.Id)
                .Select(x => x.Children.DocumentNum)
                .Max();

                var nextNum = lastNum.HasValue ? lastNum.Value + 1 : 1;

                parentNum = parent.DocumentNum.ToString();

                if (parentNum.IsNullOrEmpty())
                {
                    return new ValidateResult { Success = false, Message = "Невозможно сформировать номер, поскольку не присвоен номер родительскому документу!" };
                }

                document.DocumentNum = nextNum;

                var inspector = inspectorService.GetAll()
                    .Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector).FirstOrDefault();

                if (inspector != null) inspectorNum = inspector.Code;
            }

            document.DocumentNumber = parentNum + "-" + inspectorNum + "/" + document.DocumentNum;

            return ValidateResult.Yes();
        }

    }
}